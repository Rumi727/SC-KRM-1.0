using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using SCKRM.Camera;
using SCKRM.UI;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using SCKRM.Splash;
using SCKRM.SaveLoad;
using SCKRM.ProjectSetting;
using SCKRM.Renderer;
using System.IO;
using UnityEditorInternal;

namespace SCKRM.Editor
{
    [InitializeOnLoad]
    public static class SCKRMSetting
    {
        static SCKRMSetting()
        {
            PlayerSettings.allowFullscreenSwitch = false;
            AudioListener.volume = 0.5f;

            EditorBuildSettings.sceneListChanged += () => { SceneListChanged(true); };
            EditorApplication.hierarchyChanged += () => { HierarchyChanged(true); };

            EditorApplication.update += Update;

            File.WriteAllText(PathTool.Combine(Directory.GetCurrentDirectory(), "SC-KRM-Version"), Kernel.sckrmVersion.ToString());
        }

        static void Update()
        {
            if (EditorBuildSettings.scenes.Length <= 0)
            {
                SceneListChanged(true);
                HierarchyChanged(false);
            }

            EditorApplication.update -= Update;
        }



        [MenuItem("SC KRM/Show control panel")]
        public static void ShowWindow() => EditorWindow.GetWindow<SCKRMWindowEditor>(false, "SC KRM");

        [MenuItem("SC KRM/All Rerender")]
        public static void AllRerender()
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            CustomAllRenderer[] customAllRenderers;

            if (prefabStage == null)
                customAllRenderers = UnityEngine.Object.FindObjectsOfType<CustomAllRenderer>();
            else
                customAllRenderers = prefabStage.FindComponentsOfType<CustomAllRenderer>();

            for (int i = 0; i < customAllRenderers.Length; i++)
            {
                CustomAllRenderer customAllRenderer = customAllRenderers[i];
                customAllRenderer.Refresh();
            }
        }



        static bool sceneListChangedEnable = true;
        static SaveLoadClass splashProjectSetting = null;
        public static void SceneListChanged(bool autoLoad)
        {
            if (Kernel.isPlaying)
                return;

            string activeScenePath = SceneManager.GetActiveScene().path;
            try
            {
                if (sceneListChangedEnable)
                {
                    if (autoLoad)
                    {
                        if (splashProjectSetting == null)
                            SaveLoadManager.Initialize<ProjectSettingSaveLoadAttribute>(typeof(SplashScreen.Data), out splashProjectSetting);

                        SaveLoadManager.Load(splashProjectSetting, Kernel.projectSettingPath);
                    }

                    sceneListChangedEnable = false;

                    EditorSceneManager.OpenScene($"{PathTool.Combine(SplashScreen.Data.splashScreenPath, SplashScreen.Data.splashScreenName)}.unity");
                    HierarchyChanged(false);
                    EditorSceneManager.SaveOpenScenes();

                    string splashScenePath = SceneManager.GetActiveScene().path;

                    bool exists = false;
                    List<EditorBuildSettingsScene> buildScenes = EditorBuildSettings.scenes.ToList();

                    if (buildScenes.Count > 0)
                    {
                        if (!buildScenes[0].enabled)
                            buildScenes.RemoveAt(0);

                        for (int i = 0; i < buildScenes.Count; i++)
                        {
                            EditorBuildSettingsScene scene = buildScenes[i];
                            if (splashScenePath == scene.path)
                            {
                                if (i != 0)
                                    buildScenes.Move(i, 0);

                                exists = true;
                                break;
                            }
                        }
                    }

                    if (!exists)
                        buildScenes.Insert(0, new EditorBuildSettingsScene() { path = splashScenePath, enabled = true });

                    EditorBuildSettings.scenes = buildScenes.ToArray();
                    EditorSceneManager.OpenScene(activeScenePath);

                    sceneListChangedEnable = true;
                }
            }
            catch (ArgumentException e)
            {
                Debug.LogException(e);
                Debug.LogWarning($"{SplashScreen.Data.splashScreenName} 씬이 없는것같습니다 씬을 추가해주세요");
            }
            finally
            {
                EditorSceneManager.OpenScene(activeScenePath);
                sceneListChangedEnable = true;
            }
        }

        static bool hierarchyChangedEnable = true;
        public static void HierarchyChanged(bool autoLoad)
        {
            if (Kernel.isPlaying)
                return;

            try
            {
                if (hierarchyChangedEnable)
                {
                    bool sceneDirty = false;
                    if (autoLoad)
                    {
                        if (splashProjectSetting == null)
                            SaveLoadManager.Initialize<ProjectSettingSaveLoadAttribute>(typeof(SplashScreen.Data), out splashProjectSetting);

                        SaveLoadManager.Load(splashProjectSetting, Kernel.projectSettingPath);
                    }

                    Scene activeScene = SceneManager.GetActiveScene();
                    PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                    hierarchyChangedEnable = false;

                    #region Kernel
                    if (activeScene.path == $"{PathTool.Combine(SplashScreen.Data.splashScreenPath, SplashScreen.Data.splashScreenName)}.unity")
                    {
                        Kernel kernel = UnityEngine.Object.FindObjectOfType<Kernel>(true);
                        string kernelPrefabPath = PathTool.Combine(SplashScreen.Data.kernelObjectPath, SplashScreen.Data.kernelObjectName) + ".prefab";
                        Kernel kernelPrefab = AssetDatabase.LoadAssetAtPath<Kernel>(kernelPrefabPath);
                        if (kernelPrefab == null)
                            throw new NullObjectException(SplashScreen.Data.kernelObjectPath, SplashScreen.Data.kernelObjectName);

                        if (kernel == null)
                        {
                            PrefabUtility.InstantiatePrefab(kernelPrefab);
                            sceneDirty = true;
                        }
                        else if (PrefabUtility.GetPrefabAssetType(kernel) == PrefabAssetType.NotAPrefab || PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(kernel) != kernelPrefabPath)
                        {
                            UnityEngine.Object.DestroyImmediate(kernel.gameObject);
                            PrefabUtility.InstantiatePrefab(kernelPrefab);

                            sceneDirty = true;
                        }
                        else if (!kernel.enabled || !kernel.gameObject.activeSelf)
                        {
                            UnityEngine.Object.DestroyImmediate(kernel.gameObject);
                            sceneDirty = true;
                        }
                    }
                    else
                    {
                        Kernel kernel = UnityEngine.Object.FindObjectOfType<Kernel>(true);
                        if (kernel != null)
                        {
                            UnityEngine.Object.DestroyImmediate(kernel.gameObject);
                            sceneDirty = true;
                        }
                    }
                    #endregion

                    #region Camera Setting
                    UnityEngine.Camera[] cameras;
                    if (prefabStage != null)
                        cameras = prefabStage.FindComponentsOfType<UnityEngine.Camera>();
                    else
                        cameras = UnityEngine.Object.FindObjectsOfType<UnityEngine.Camera>(true);

                    for (int i = 0; i < cameras.Length; i++)
                    {
                        UnityEngine.Camera camera = cameras[i];
                        CameraSetting cameraSetting = camera.GetComponent<CameraSetting>();
                        if (camera.GetComponent<CameraSetting>() == null)
                        {
                            camera.gameObject.AddComponent<CameraSetting>();
                            sceneDirty = true;
                        }
                        else if (!cameraSetting.enabled)
                        {
                            UnityEngine.Object.DestroyImmediate(cameraSetting);
                            sceneDirty = true;
                        }
                    }
                    #endregion

                    #region Canvas Setting
                    Canvas[] canvases;
                    if (prefabStage != null)
                        canvases = prefabStage.FindComponentsOfType<Canvas>();
                    else
                        canvases = UnityEngine.Object.FindObjectsOfType<Canvas>(true);

                    for (int i = 0; i < canvases.Length; i++)
                    {
                        Canvas canvas = canvases[i];
                        CanvasSetting canvasSetting = canvas.GetComponent<CanvasSetting>();

                        if (canvas.GetComponent<UIManager>() == null)
                        {
                            if (canvasSetting == null)
                            {
                                canvas.gameObject.AddComponent<CanvasSetting>();
                                sceneDirty = true;
                            }
                            else if (!canvasSetting.enabled)
                            {
                                UnityEngine.Object.DestroyImmediate(canvasSetting);
                                sceneDirty = true;
                            }
                        }

                        if (canvasSetting != null && !canvasSetting.customSetting && !canvasSetting.customGuiSize)
                        {
                            CanvasScaler[] canvasScalers = canvas.GetComponents<CanvasScaler>();
                            for (int j = 0; j < canvasScalers.Length; j++)
                            {
                                CanvasScaler canvasScaler = canvasScalers[j];
                                if (canvasScaler != null)
                                {
                                    UnityEngine.Object.DestroyImmediate(canvasScaler);
                                    sceneDirty = true;
                                }
                            }
                        }
                    }
                    #endregion

                    #region Rect Transform Tool
                    Transform[] transforms;
                    if (prefabStage != null)
                        transforms = prefabStage.FindComponentsOfType<Transform>();
                    else
                        transforms = UnityEngine.Object.FindObjectsOfType<Transform>(true);

                    for (int i = 0; i < transforms.Length; i++)
                    {
                        Transform transform = transforms[i];
                        RectTransform rectTransform = transform.gameObject.GetComponent<RectTransform>();
                        RectTransformTool rectTransformTool = transform.GetComponent<RectTransformTool>();

                        if (rectTransform != null)
                        {
                            if (rectTransformTool == null)
                            {
                                PrefabAssetType prefabAssetType = PrefabUtility.GetPrefabAssetType(rectTransform.gameObject);
                                if (prefabAssetType == PrefabAssetType.NotAPrefab)
                                {
                                    RectTransformTool addedRectTransformTool = rectTransform.gameObject.AddComponent<RectTransformTool>();

                                    int length = rectTransform.GetComponents<Component>().Length;
                                    for (int j = 0; j < length - 2; j++)
                                        ComponentUtility.MoveComponentUp(addedRectTransformTool);

                                    sceneDirty = true;
                                }
                                else if (prefabAssetType != PrefabAssetType.MissingAsset)
                                {
                                    List<RemovedComponent> removedComponents = PrefabUtility.GetRemovedComponents(rectTransform.gameObject);
                                    bool revert = false;
                                    for (int j = 0; j < removedComponents.Count; j++)
                                    {
                                        RemovedComponent removedComponent = removedComponents[j];
                                        if (removedComponent.assetComponent.GetType() == typeof(RectTransformTool))
                                        {
                                            removedComponent.Revert();
                                            revert = true;
                                        }
                                    }

                                    if (!revert)
                                    {
                                        RectTransform original = PrefabUtility.GetCorrespondingObjectFromOriginalSource(rectTransform);
                                        RectTransformTool addedRectTransformTool = original.gameObject.AddComponent<RectTransformTool>();

                                        int length = original.GetComponents<Component>().Length;
                                        for (int j = 0; j < length - 2; j++)
                                            ComponentUtility.MoveComponentUp(addedRectTransformTool);

                                        EditorUtility.SetDirty(rectTransform.gameObject);
                                        EditorUtility.SetDirty(addedRectTransformTool);
                                    }

                                    sceneDirty = true;
                                }
                            }
                            else if (!rectTransformTool.enabled)
                            {
                                UnityEngine.Object.DestroyImmediate(rectTransformTool);
                                sceneDirty = true;
                            }
                        }
                        else if (rectTransformTool != null)
                        {
                            UnityEngine.Object.DestroyImmediate(rectTransformTool);
                            sceneDirty = true;
                        }
                    }
                    #endregion

                    if (sceneDirty)
                        EditorSceneManager.MarkSceneDirty(activeScene);
                }
            }
            finally
            {
                hierarchyChangedEnable = true;
            }
        }
    }
}