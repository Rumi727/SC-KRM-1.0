using SCKRM.UI.TaskBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SCKRM.UI
{
    [ExecuteAlways]
    [AddComponentMenu("커널/UI/캔버스 설정")]
    [RequireComponent(typeof(Canvas))]
    public sealed class CanvasSetting : MonoBehaviour
    {
        [SerializeField] bool _customSetting; public bool customSetting { get => _customSetting; set => _customSetting = value; }
        [SerializeField] bool _customGuiSize; public bool customGuiSize { get => _customGuiSize; set => _customGuiSize = value; }
        [SerializeField] bool _worldRenderMode; public bool worldRenderMode { get => _worldRenderMode; set => _worldRenderMode = value; }

        public Canvas canvas { get; private set; }
        public RectTransform rectTransform { get; private set; }

        void Update()
        {
            if (!customGuiSize)
            {
#if UNITY_EDITOR
                if (canvas == null)
                    canvas = GetComponent<Canvas>();

                if (Application.isPlaying)
                    canvas.scaleFactor = Kernel.SaveData.guiSize;
                else
                    canvas.scaleFactor = 1;
            }
#else
                if (canvas == null)
                    canvas = GetComponent<Canvas>();
                
                canvas.scaleFactor = Kernel.SaveData.guiSize;
            }
#endif

            if (!customSetting && !worldRenderMode)
            {
                if (canvas == null)
                    canvas = GetComponent<Canvas>();
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();

                canvas.worldCamera = UnityEngine.Camera.main;

                if (canvas.worldCamera != null)
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                else
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            else if (!customSetting)
                WorldRenderCamera();
        }

        void WorldRenderCamera()
        {
            if (worldRenderMode)
            {
                if (canvas == null)
                    canvas = GetComponent<Canvas>();
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();

                canvas.worldCamera = UnityEngine.Camera.main;
                canvas.scaleFactor = Kernel.SaveData.guiSize;

                if (canvas.worldCamera == null)
                {
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    return;
                }

#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    if (transform.parent != UnityEngine.Camera.main.transform)
                        transform.SetParent(UnityEngine.Camera.main.transform);

                    canvas.renderMode = RenderMode.WorldSpace;

                    transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
                    transform.localEulerAngles = Vector3.zero;
                }
                else
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
#else
                if (transform.parent != UnityEngine.Camera.main.transform)
                    transform.SetParent(UnityEngine.Camera.main.transform);

                canvas.renderMode = RenderMode.WorldSpace;

                transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
                transform.localEulerAngles = Vector3.zero;
#endif

                float width = Screen.width * (1 / Kernel.SaveData.guiSize);
                float height;
#if UNITY_EDITOR
                if (Application.isPlaying && TaskBarManager.cropTheScreen)
                    height = Screen.height * (1 / Kernel.SaveData.guiSize) + (TaskBarManager.instance.rectTransform.anchoredPosition.y - TaskBarManager.instance.rectTransform.sizeDelta.y);
                else
                    height = Screen.height * (1 / Kernel.SaveData.guiSize);
#else
                if (TaskBarManager.cropTheScreen)
                    height = Screen.height * (1 / Kernel.SaveData.guiSize) + TaskBarManager.instance.rectTransform.sizeDelta.y;
                else
                    height = Screen.height * (1 / Kernel.SaveData.guiSize);
#endif

                rectTransform.sizeDelta = new Vector2(width, height);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);

                UnityEngine.Camera camera;
                if (canvas.worldCamera != null)
                    camera = canvas.worldCamera;
                else
                    camera = UnityEngine.Camera.main;



                float spriteX;
                float spriteY;

                float screenX;
                float screenY;

                if (camera.orthographic)
                {
                    spriteX = rectTransform.sizeDelta.x;
                    spriteY = rectTransform.sizeDelta.y;

                    screenY = camera.orthographicSize * 2;
                    screenX = screenY / height * width;
                }
                else
                {
                    spriteX = rectTransform.sizeDelta.x;
                    spriteY = rectTransform.sizeDelta.y;

                    screenY = Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * 2.0f * transform.localPosition.z;
                    screenX = screenY / height * width;
                }

                transform.localScale = new Vector3(screenX / spriteX, screenY / spriteY, screenX / spriteX);
            }
        }
    }
}