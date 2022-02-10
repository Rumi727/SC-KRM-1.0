using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using SCKRM.Camera;
using SCKRM.UI;
using UnityEngine.UI;
using SCKRM.Tool;
using UnityEditorInternal;

namespace SCKRM.Editor
{
    [InitializeOnLoad]
    public class KernelSetAutoProjectSetting
    {
        static bool sceneListChangedEnable = true;
        static bool hierarchyChangedEnable = true;

        static KernelSetAutoProjectSetting()
        {
            AudioListener.volume = 0.5f;
            EditorBuildSettings.sceneListChanged += SceneListChanged;
            EditorApplication.hierarchyChanged += HierarchyChanged;
        }

        public static void SceneListChanged()
        {
            if (Application.isPlaying)
                return;

            string activeScenePath = SceneManager.GetActiveScene().path;
            try
            {
                if (sceneListChangedEnable)
                {
                    sceneListChangedEnable = false;

                    EditorSceneManager.OpenScene($"{PathTool.Combine(Kernel.Data.splashScreenPath, Kernel.Data.splashScreenName)}.unity");

                    string splashScenePath = SceneManager.GetActiveScene().path;

                    bool exists = false;
                    for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                    {
                        EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
                        if (splashScenePath == scene.path)
                        {
                            if (i != 0)
                                EditorBuildSettings.scenes = EditorBuildSettings.scenes.Move(i, 0);

                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                        EditorBuildSettings.scenes = EditorBuildSettings.scenes.Insert(0, new EditorBuildSettingsScene() { path = splashScenePath, enabled = true });

                    sceneListChangedEnable = true;

                    if (!EditorBuildSettings.scenes[0].enabled)
                        EditorBuildSettings.scenes = EditorBuildSettings.scenes.RemoveAt(0);

                    EditorSceneManager.OpenScene(activeScenePath);
                }
            }
            catch (ArgumentException e)
            {
                sceneListChangedEnable = true;
                Debug.LogError(e);
                Debug.LogWarning($"{Kernel.Data.splashScreenName} 씬이 없는것같습니다 씬을 추가해주세요");
            }
            catch (Exception e)
            {
                sceneListChangedEnable = true;
                EditorSceneManager.OpenScene(activeScenePath);
                Debug.LogError(e.Message);
            }
        }

        public static void HierarchyChanged()
        {
            if (Application.isPlaying)
                return;

            try
            {
                if (hierarchyChangedEnable)
                {
                    hierarchyChangedEnable = false;

                    Kernel kernel = UnityEngine.Object.FindObjectOfType<Kernel>(true);
                    if (kernel == null)
                    {
                        GameObject gameObject = Resources.Load<GameObject>("Kernel");
                        if (gameObject != null)
                            PrefabUtility.InstantiatePrefab(gameObject);
                        else
                            throw new NullResourceObjectException("Kernel");

                        hierarchyChangedEnable = true;
                    }
                    else if (!kernel.enabled)
                        UnityEngine.Object.DestroyImmediate(kernel.gameObject);
                    else if (!kernel.gameObject.activeSelf)
                        UnityEngine.Object.DestroyImmediate(kernel.gameObject);

                    UnityEngine.Camera[] cameras = UnityEngine.Object.FindObjectsOfType<UnityEngine.Camera>(true);
                    for (int i = 0; i < cameras.Length; i++)
                    {
                        UnityEngine.Camera camera = cameras[i];
                        CameraSetting cameraSetting = camera.GetComponent<CameraSetting>();
                        if (camera.GetComponent<CameraSetting>() == null)
                            camera.gameObject.AddComponent<CameraSetting>();
                        else if (!cameraSetting.enabled)
                            UnityEngine.Object.DestroyImmediate(cameraSetting);
                    }

                    Canvas[] canvass = UnityEngine.Object.FindObjectsOfType<Canvas>(true);
                    for (int i = 0; i < canvass.Length; i++)
                    {
                        Canvas canvas = canvass[i];
                        CanvasSetting canvasSetting = canvas.GetComponent<CanvasSetting>();

                        if (canvas.GetComponent<KernelCanvas>() == null)
                        {
                            if (canvasSetting == null)
                                canvas.gameObject.AddComponent<CanvasSetting>();
                            else if (!canvasSetting.enabled)
                                UnityEngine.Object.DestroyImmediate(canvasSetting);
                        }

                        if (canvasSetting != null && !canvasSetting.customSetting)
                        {
                            CanvasScaler[] canvasScalers = canvas.GetComponents<CanvasScaler>();
                            for (int j = 0; j < canvasScalers.Length; j++)
                            {
                                CanvasScaler canvasScaler = canvasScalers[j];
                                if (canvasScaler != null)
                                    UnityEngine.Object.DestroyImmediate(canvasScaler);
                            }
                        }
                    }

                    Transform[] transforms = UnityEngine.Object.FindObjectsOfType<Transform>(true);
                    for (int i = 0; i < transforms.Length; i++)
                    {
                        Transform transform = transforms[i];
                        RectTransform rectTransform = transform.gameObject.GetComponent<RectTransform>();
                        RectTransformInfo rectTransformSetting = transform.GetComponent<RectTransformInfo>();

                        if (rectTransform != null)
                        {
                            if (rectTransformSetting == null)
                            {
                                RectTransformInfo rectTransformSetting2 = rectTransform.gameObject.AddComponent<RectTransformInfo>();
                                if (PrefabUtility.GetPrefabAssetType(rectTransform.gameObject) == PrefabAssetType.NotAPrefab)
                                {
                                    int length = rectTransform.GetComponents<Component>().Length;
                                    for (int j = 0; j < length - 2; j++)
                                        ComponentUtility.MoveComponentUp(rectTransformSetting2);
                                }
                            }
                            else if (!rectTransformSetting.enabled)
                                UnityEngine.Object.DestroyImmediate(rectTransformSetting);
                        }
                        else if (rectTransformSetting != null)
                            UnityEngine.Object.DestroyImmediate(rectTransformSetting);
                    }

                    hierarchyChangedEnable = true;
                }
            }
            catch (Exception e)
            {
                hierarchyChangedEnable = true;
                Debug.LogError(e.Message);
            }
        }
    }
}