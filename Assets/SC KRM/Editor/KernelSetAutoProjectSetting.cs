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

namespace SCKRM.Editor
{
    [InitializeOnLoad]
    public class KernelSetAutoProjectSetting
    {
        static bool sceneListChangedEnable = true;
        static bool hierarchyChangedEnable = true;

        static KernelSetAutoProjectSetting()
        {
            PlayerSettings.allowFullscreenSwitch = false;
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

                    EditorSceneManager.OpenScene($"{PathTool.Combine(SplashScreen.Data.splashScreenPath, SplashScreen.Data.splashScreenName)}.unity");
                    HierarchyChanged();

                    string splashScenePath = SceneManager.GetActiveScene().path;

                    bool exists = false;
                    List<EditorBuildSettingsScene> buildScenes = EditorBuildSettings.scenes.ToList();
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

                    sceneListChangedEnable = true;

                    if (!EditorBuildSettings.scenes[0].enabled)
                        buildScenes.RemoveAt(0);

                    EditorBuildSettings.scenes = buildScenes.ToArray();

                    EditorSceneManager.OpenScene(activeScenePath);
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

        public static void HierarchyChanged()
        {
            if (Application.isPlaying)
                return;

            try
            {
                if (hierarchyChangedEnable)
                {
                    hierarchyChangedEnable = false;
                    
                    if (SceneManager.GetActiveScene().path == $"{PathTool.Combine(SplashScreen.Data.splashScreenPath, SplashScreen.Data.splashScreenName)}.unity")
                    {
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
                    }
                    else
                    {
                        Kernel kernel = UnityEngine.Object.FindObjectOfType<Kernel>(true);
                        if (kernel != null)
                            UnityEngine.Object.DestroyImmediate(kernel.gameObject);
                    }

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

                        if (canvas.GetComponent<UIManager>() == null)
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