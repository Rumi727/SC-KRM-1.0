using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Threads;
using SCKRM.Tool;
using SCKRM.UI.StatusBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.SideBar
{
    [AddComponentMenu(""), RequireComponent(typeof(RectTransform), typeof(RectTransformInfo))]
    public class SideBarManager : MonoBehaviour
    {
        public static SideBarManager instance { get; private set; }

        [SerializeField, HideInInspector] RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();

                return _rectTransform;
            }
        }

        [SerializeField, HideInInspector] RectTransformInfo _rectTransformInfo;
        public RectTransformInfo rectTransformInfo
        {
            get
            {
                if (_rectTransformInfo == null)
                    _rectTransformInfo = GetComponent<RectTransformInfo>();

                return _rectTransformInfo;
            }
        }


        static bool _isSideBarShow;
        public static bool isSideBarShow
        {
            get => _isSideBarShow;
            set
            {
                InputManager.SetInputLock("sidebar", value);
                _isSideBarShow = value;
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

            NoticeManager.noticeAdd += () =>
            {
                isSideBarShow = true;
                SettingBarManager.isSettingBarShow = false;
            };
        }

        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                if (isSideBarShow)
                {
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(0, rectTransform.anchoredPosition.y), 0.2f * Kernel.fpsUnscaledDeltaTime);

                    if (InputManager.GetKeyDown("gui.back", "all") || InputManager.GetKeyDown("gui.home", "all"))
                    {
                        isSideBarShow = false;
                        StatusBarManager.Tab();
                    }
                }
                else
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(rectTransform.sizeDelta.x, rectTransform.anchoredPosition.y), 0.2f * Kernel.fpsUnscaledDeltaTime);



                StatusBarManager taskBarManager = StatusBarManager.instance;
                if (StatusBarManager.SaveData.bottomMode)
                {
                    rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, taskBarManager.rectTransform.sizeDelta.y - taskBarManager.rectTransform.anchoredPosition.y);
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
                    viewPort.offsetMax = viewPort.offsetMax.Lerp(new Vector2(-scrollBarRectTransform.sizeDelta.x, 0), 0.2f * Kernel.fpsUnscaledDeltaTime);
                }
                else
                {
                    scrollBar.interactable = false;

                    scrollBarRectTransform.anchoredPosition = scrollBarRectTransform.anchoredPosition.Lerp(new Vector2(scrollBarRectTransform.sizeDelta.x, 0), 0.2f * Kernel.fpsUnscaledDeltaTime);
                    viewPort.offsetMax = viewPort.offsetMax.Lerp(Vector2.zero, 0.2f * Kernel.fpsUnscaledDeltaTime);
                }
            }
        }
    }
}