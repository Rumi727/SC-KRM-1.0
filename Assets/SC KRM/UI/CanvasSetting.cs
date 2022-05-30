using SCKRM.UI.StatusBar;
using System;
using UnityEngine;

namespace SCKRM.UI
{
    [ExecuteAlways]
    [AddComponentMenu("커널/UI/캔버스 설정")]
    public sealed class CanvasSetting : UI
    {
        [SerializeField] bool _customSetting; public bool customSetting { get => _customSetting; set => _customSetting = value; }
        [SerializeField] bool _customGuiSize; public bool customGuiSize { get => _customGuiSize; set => _customGuiSize = value; }
        [SerializeField] bool _worldRenderMode; public bool worldRenderMode { get => _worldRenderMode; set => _worldRenderMode = value; }
        [SerializeField] float _planeDistance; public float planeDistance { get => _planeDistance; set => _planeDistance = value; }

        [NonSerialized] Canvas _canvas; public Canvas canvas => _canvas = this.GetComponentFieldSave(_canvas, ComponentTool.GetComponentMode.destroyIfNull);

        [SerializeField, HideInInspector] RectTransform safeScreen;
        DrivenRectTransformTracker tracker;

        protected override void OnEnable() => Canvas.preWillRenderCanvases += Refresh;
        protected override void OnDisable()
        {
            tracker.Clear();
            Canvas.preWillRenderCanvases -= Refresh;
        }

        void Refresh()
        {
            if (canvas == null)
                return;

            if (!customGuiSize)
#if UNITY_EDITOR
            {
                if (Application.isPlaying)
                    canvas.scaleFactor = UIManager.currentGuiSize;
                else
                    canvas.scaleFactor = 1;
            }
#else
            canvas.scaleFactor = UIManager.currentGuiSize;
#endif

            if (!customSetting)
            {
                if (!worldRenderMode)
                {
#if UNITY_EDITOR
                    if (Application.isPlaying)
#endif
                    {
                        RectTransform taskBarManager = StatusBarManager.instance.rectTransform;

                        float guiSize = 1;
                        if (customGuiSize)
                            guiSize = UIManager.currentGuiSize / canvas.scaleFactor;

                        if (StatusBarManager.cropTheScreen && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                        {
                            SafeScreenSetting();

                            if (StatusBarManager.SaveData.bottomMode)
                                safeScreen.offsetMin = new Vector2(safeScreen.offsetMin.x, (taskBarManager.rect.size.y + taskBarManager.anchoredPosition.y) * guiSize);
                            else
                                safeScreen.offsetMax = new Vector2(0, -(taskBarManager.rect.size.y - taskBarManager.anchoredPosition.y) * guiSize);
                        }
                        else
                            SafeScreenDestroy();
                    }
#if UNITY_EDITOR
                    else
                    {
                        tracker.Clear();

                        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                            SafeScreenSetting();
                        else
                            SafeScreenDestroy();
                    }
#endif
                }
                else
                {
                    SafeScreenDestroy();
                    WorldRenderCamera();
                }
            }
        }

        void SafeScreenSetting()
        {
            if (safeScreen == null)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                {
                    if (Kernel.emptyRectTransform == null)
                        return;

                    safeScreen = Instantiate(Kernel.emptyRectTransform, transform.parent);
                }
#if UNITY_EDITOR
                else
#endif
                    safeScreen = new GameObject().AddComponent<RectTransform>();

                safeScreen.name = "Safe Screen";
            }

            tracker.Clear();
            tracker.Add(this, safeScreen, DrivenTransformProperties.All);

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

        void SafeScreenDestroy()
        {
            if (safeScreen == null)
                return;

            int childCount = safeScreen.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform childtransform = safeScreen.GetChild(i);
                if (childtransform != safeScreen)
                {
                    childtransform.SetParent(transform);

                    i--;
                    childCount--;
                }
            }

            DestroyImmediate(safeScreen.gameObject);
        }

        void WorldRenderCamera()
        {
            tracker.Clear();
            tracker.Add(this, rectTransform, DrivenTransformProperties.All);


            UnityEngine.Camera camera = canvas.worldCamera;
            if (camera == null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                return;
            }
            else
                canvas.renderMode = RenderMode.WorldSpace;



            transform.rotation = camera.transform.rotation;
            transform.position = camera.transform.position + (transform.forward * planeDistance);
            


            float width = camera.pixelWidth * (1 / UIManager.currentGuiSize);
            float height = camera.pixelHeight * (1 / UIManager.currentGuiSize);

            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.pivot = Vector2.one * 0.5f;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;



            float screenX, screenY;

            if (camera.orthographic)
            {
                screenY = camera.orthographicSize * 2;
                screenX = screenY / height * width;
            }
            else
            {
                screenY = Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * 2.0f * planeDistance;
                screenX = screenY / height * width;
            }

            transform.localScale = new Vector3(screenX / width, screenY / height, screenX / width);
        }
    }
}