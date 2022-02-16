using SCKRM.UI.StatusBar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SCKRM.UI
{
    [ExecuteAlways]
    [AddComponentMenu("커널/UI/캔버스 설정")]
    public sealed class CanvasSetting : UI
    {
        [SerializeField] bool _customSetting; public bool customSetting { get => _customSetting; set => _customSetting = value; }
        [SerializeField] bool _customGuiSize; public bool customGuiSize { get => _customGuiSize; set => _customGuiSize = value; }
        [SerializeField] bool _worldRenderMode; public bool worldRenderMode { get => _worldRenderMode; set => _worldRenderMode = value; }

        [NonSerialized] Canvas _canvas; public Canvas canvas
        {
            get
            {
                if (_canvas == null)
                {
                    _canvas = GetComponent<Canvas>();
                    if (_canvas == null)
                        DestroyImmediate(this);
                }

                return _canvas;
            }
        }

        void Update()
        {
            if (canvas == null)
                return;

            if (!customGuiSize)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                    canvas.scaleFactor = Kernel.guiSize;
                else
                    canvas.scaleFactor = 1;
            }
#else
                if (canvas == null)
                    canvas = GetComponent<Canvas>();
                
                canvas.scaleFactor = Kernel.guiSize;
            }
#endif

            if (!customSetting && !worldRenderMode)
            {
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
                canvas.worldCamera = UnityEngine.Camera.main;

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

                float width = Screen.width * (1 / Kernel.guiSize);
                float height;
#if UNITY_EDITOR
                if (Application.isPlaying && StatusBarManager.cropTheScreen)
#else
                if (StatusBarManager.cropTheScreen)
#endif
                    height = Screen.height * (1 / Kernel.guiSize) + (StatusBarManager.instance.rectTransform.anchoredPosition.y - StatusBarManager.instance.rectTransform.sizeDelta.y);
                else
                    height = Screen.height * (1 / Kernel.guiSize);

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