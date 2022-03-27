using SCKRM.Input;
using SCKRM.UI.StatusBar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

        [SerializeField, HideInInspector] RectTransform safeScreen;
        void Update()
        {
            if (canvas == null)
                return;

            if (!customGuiSize)
#if UNITY_EDITOR
            {
                if (Application.isPlaying)
                    canvas.scaleFactor = Kernel.guiSize;
                else
                    canvas.scaleFactor = 1;
            }
#else
            canvas.scaleFactor = Kernel.guiSize;
#endif

            if (!customSetting)
            {
                if (!worldRenderMode)
                {
                    SafeScreenSetting();
#if UNITY_EDITOR
                    if (Application.isPlaying)
#endif
                    {
                        RectTransform taskBarManager = StatusBarManager.instance.rectTransform;

                        float guiSize = 1;
                        if (customGuiSize)
                            guiSize = Kernel.guiSize / canvas.scaleFactor;

                        if (StatusBarManager.cropTheScreen && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                        {
                            if (StatusBarManager.SaveData.bottomMode)
                                safeScreen.offsetMin = new Vector2(safeScreen.offsetMin.x, (taskBarManager.rect.size.y + taskBarManager.anchoredPosition.y) * guiSize);
                            else
                                safeScreen.offsetMax = new Vector2(0, -(taskBarManager.rect.size.y - taskBarManager.anchoredPosition.y) * guiSize);
                        }
                    }
                }
                else
                    WorldRenderCamera();
            }
        }

        void SafeScreenSetting()
        {
            if (safeScreen == null)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                    safeScreen = Instantiate(Kernel.emptyRectTransform, transform.parent);
                else
                    safeScreen = new GameObject().AddComponent<RectTransform>();
#else
                safeScreen = Instantiate(Kernel.emptyRectTransform, transform.parent);
#endif

                safeScreen.name = "Safe Screen";
            }

            if (safeScreen.parent != transform)
                safeScreen.SetParent(transform);

            safeScreen.anchorMin = Vector2.zero;
            safeScreen.anchorMax = Vector2.one;

            safeScreen.offsetMin = Vector2.zero;
            safeScreen.offsetMax = Vector2.zero;

            safeScreen.pivot = Vector2.zero;

            safeScreen.localEulerAngles = Vector3.zero;
            safeScreen.localScale = Vector3.one;

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform childtransform = transform.GetChild(i);
                if (childtransform != safeScreen)
                {
                    childtransform.SetParent(safeScreen);

                    i--;
                    childCount--;
                }
            }
        }

        void WorldRenderCamera()
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
                height = Screen.height * (1 / Kernel.guiSize) + (StatusBarManager.instance.rectTransform.anchoredPosition.y - StatusBarManager.instance.rectTransform.rect.size.y);
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