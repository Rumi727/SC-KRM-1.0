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
using System.Reflection;
using System.IO;
using SCKRM.Splash;
using SCKRM.SaveLoad;
using SCKRM.ProjectSetting;
using Cysharp.Threading.Tasks;

namespace SCKRM.Editor
{
    [InitializeOnLoad]
    public static class SCKRMSetting
    {
        static bool sceneListChangedEnable = true;
        static bool hierarchyChangedEnable = true;

        static SCKRMSetting()
        {
            PlayerSettings.allowFullscreenSwitch = false;
            AudioListener.volume = 0.5f;

            EditorBuildSettings.sceneListChanged += () => { SceneListChanged(true); };
            EditorApplication.hierarchyChanged += () => { HierarchyChanged(true); };
        }

        public static SaveLoadClass splashProjectSetting = null;
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
                    if (!EditorBuildSettings.scenes[0].enabled)
                        buildScenes.RemoveAt(0);

                    for (int i = 0; i < buildScenes.Count; i++)
                    {
                        EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
                        if (splashScenePath == scene.path)
                        {
                            if (i != 0)
                                buildScenes.Move(i, 0);

                            exists = true;
                            break;
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
                sceneListChangedEnable = true;
                Debug.LogException(e);
                Debug.LogWarning($"{SplashScreen.Data.splashScreenName} 씬이 없는것같습니다 씬을 추가해주세요");
            }
            catch (Exception e)
            {
                sceneListChangedEnable = true;
                EditorSceneManager.OpenScene(activeScenePath);
                Debug.LogException(e);
            }
        }

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
                    hierarchyChangedEnable = false;
                    
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

                    UnityEngine.Camera[] cameras = UnityEngine.Object.FindObjectsOfType<UnityEngine.Camera>(true);
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

                    Canvas[] canvass = UnityEngine.Object.FindObjectsOfType<Canvas>(true);
                    for (int i = 0; i < canvass.Length; i++)
                    {
                        Canvas canvas = canvass[i];
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

                        if (canvasSetting != null && !canvasSetting.customSetting)
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

                    if (sceneDirty)
                        EditorSceneManager.MarkSceneDirty(activeScene);

                    hierarchyChangedEnable = true;
                }
            }
            catch (Exception e)
            {
                hierarchyChangedEnable = true;
                Debug.LogException(e);
            }
        }
    }
}