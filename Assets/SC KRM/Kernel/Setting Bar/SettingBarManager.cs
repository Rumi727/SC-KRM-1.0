using SCKRM.Input;
using SCKRM.Tool;
using SCKRM.UI.StatusBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.SideBar
{
    [AddComponentMenu(""), RequireComponent(typeof(RectTransform), typeof(RectTransformInfo))]
    public class SettingBarManager : UI
    {
        public static SettingBarManager instance { get; private set; }

        static bool _isNoticeBarShow;
        public static bool isSettingBarShow
        {
            get => _isNoticeBarShow;
            set
            {
                InputManager.SetInputLock("settingbar", value);
                _isNoticeBarShow = value;
            }
        }



        #region variable
        [SerializeField] RectTransform _viewPort;
        public RectTransform viewPort => _viewPort;

        [SerializeField] RectTransformInfo _content;
        public RectTransformInfo content => _content;

        [SerializeField] RectTransform _scrollBarRectTransform;
        public RectTransform scrollBarRectTransform => _scrollBarRectTransform;

        [SerializeField] Scrollbar _scrollBar;
        public Scrollbar scrollBar => _scrollBar;
        #endregion

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                if (isSettingBarShow)
                {
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(0, rectTransform.anchoredPosition.y), 0.2f * Kernel.fpsUnscaledDeltaTime);

                    if (InputManager.GetKeyDown("gui.back", "all") || InputManager.GetKeyDown("gui.home", "all"))
                    {
                        isSettingBarShow = false;
                        StatusBarManager.Tab();
                    }
                }
                else
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(-rectTransform.sizeDelta.x, rectTransform.anchoredPosition.y), 0.2f * Kernel.fpsUnscaledDeltaTime);



                StatusBarManager taskBarManager = StatusBarManager.instance;
                if (StatusBarManager.SaveData.bottomMode)
                {
                    rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, taskBarManager.rectTransform.sizeDelta.y + taskBarManager.rectTransform.anchoredPosition.y);
                    rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 0);
                }
                else
                {
                    rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0);
                    rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -(taskBarManager.rectTransform.sizeDelta.y - taskBarManager.rectTransform.anchoredPosition.y));
                }

                

                if (content.localSize.y > rectTransformInfo.localSize.y)
                {
                    scrollBar.interactable = true;

                    scrollBarRectTransform.anchoredPosition = scrollBarRectTransform.anchoredPosition.Lerp(Vector2.zero, 0.2f * Kernel.fpsUnscaledDeltaTime);
                    viewPort.offsetMin = viewPort.offsetMin.Lerp(new Vector2(scrollBarRectTransform.sizeDelta.x, 0), 0.2f * Kernel.fpsUnscaledDeltaTime);
                }
                else
                {
                    scrollBar.interactable = false;

                    scrollBarRectTransform.anchoredPosition = scrollBarRectTransform.anchoredPosition.Lerp(new Vector2(-scrollBarRectTransform.sizeDelta.x, 0), 0.2f * Kernel.fpsUnscaledDeltaTime);
                    viewPort.offsetMin = viewPort.offsetMin.Lerp(Vector2.zero, 0.2f * Kernel.fpsUnscaledDeltaTime);
                }
            }
        }
    }
}