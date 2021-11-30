using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using SCKRM.Camera;
using SCKRM.UI;
using UnityEngine.UI;
using SCKRM.Tool;

namespace SCKRM.Editor
{
    [InitializeOnLoad]
    public class KernelSetAutoProjectSetting
    {
        static bool sceneListChangedEnable = true;
        static bool hierarchyChangedEnable = true;

        static KernelSetAutoProjectSetting()
        {
            EditorBuildSettings.sceneListChanged += SceneListChanged;
            EditorApplication.hierarchyChanged += HierarchyChanged;
        }

        static void SceneListChanged()
        {
            try
            {
                if (sceneListChangedEnable)
                {
                    sceneListChangedEnable = false;

                    string activeScenePath = SceneManager.GetActiveScene().path;
                    EditorSceneManager.OpenScene("Assets/SC KRM/Splash Screen/Splash Screen.unity");

                    Scene splashScene = SceneManager.GetSceneByName("Splash Screen");
                    if (splashScene == null)
                        throw new NullSceneException("Splash Screen");

                    bool exists = false;
                    for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                    {
                        EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
                        if (splashScene.path == scene.path)
                        {
                            if (i != 0)
                                EditorBuildSettings.scenes = EditorBuildSettings.scenes.Change(0, i);

                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                        EditorBuildSettings.scenes = EditorBuildSettings.scenes.Insert(0, new EditorBuildSettingsScene() { path = splashScene.path, enabled = true });

                    sceneListChangedEnable = true;

                    if (!EditorBuildSettings.scenes[0].enabled)
                        EditorBuildSettings.scenes = EditorBuildSettings.scenes.RemoveAt(0);

                    EditorSceneManager.OpenScene(activeScenePath);
                }
            }
            catch (ArgumentException)
            {
                sceneListChangedEnable = true;
                Debug.LogError("Assets/SC KRM/Splash Screen/Splash Screen.unity가 없는것같습니다 씬을 추가해주세요");
            }
            catch (Exception e)
            {
                sceneListChangedEnable = true;
                Debug.LogError(e.Message);
            }
        }

        static void HierarchyChanged()
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
                            CanvasSetting cameraSetting = canvas.GetComponent<CanvasSetting>();
                            if (canvasSetting == null)
                                canvas.gameObject.AddComponent<CanvasSetting>();
                            else if (!cameraSetting.enabled)
                                UnityEngine.Object.DestroyImmediate(cameraSetting);
                        }

                        if (canvasSetting != null && !canvasSetting.customRenderMode)
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