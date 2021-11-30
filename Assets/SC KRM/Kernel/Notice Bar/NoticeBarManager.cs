using SCKRM.Input;
using SCKRM.Tool;
using SCKRM.UI.TaskBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.NoticeBar
{
    [AddComponentMenu(""), RequireComponent(typeof(RectTransform), typeof(RectTransformSize))]
    public class NoticeBarManager : MonoBehaviour
    {
        public static NoticeBarManager instance { get; private set; }

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

        [SerializeField, HideInInspector] RectTransformSize _rectTransformSize;
        public RectTransformSize rectTransformSize
        {
            get
            {
                if (_rectTransformSize == null)
                    _rectTransformSize = GetComponent<RectTransformSize>();

                return _rectTransformSize;
            }
        }


        static bool _isNoticeBarShow;
        public static bool isNoticeBarShow
        {
            get => _isNoticeBarShow;
            set
            {
                InputManager.SetInputLock("noticebar", value);
                _isNoticeBarShow = value;
            }
        }



        #region variable
        [SerializeField] RectTransformSize _contentRectTransformSize;
        public RectTransformSize contentRectTransformSize { get => _contentRectTransformSize; }

        [SerializeField] RectTransform _viewPort;
        public RectTransform viewPort { get => _viewPort; }

        [SerializeField] RectTransform _content;
        public RectTransform content { get => _content; }

        [SerializeField] RectTransform _scrollBarRectTransform;
        public RectTransform scrollBarRectTransform { get => _scrollBarRectTransform; }

        [SerializeField] Scrollbar _scrollBar;
        public Scrollbar scrollBar { get => _scrollBar; }

        [SerializeField] RectTransform _scrollBarHandleShow;
        public RectTransform scrollBarHandleShow { get => _scrollBarHandleShow; }
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
                if (isNoticeBarShow)
                {
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(0, rectTransform.anchoredPosition.y), 0.2f * Kernel.fpsDeltaTime);

                    if (InputManager.GetKeyDown("gui.back", "taskbar", "noticebar") || InputManager.GetKeyDown("gui.home", "taskbar", "noticebar"))
                    {
                        isNoticeBarShow = false;
                        TaskBarManager.Tab();
                    }
                }
                else
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(rectTransform.sizeDelta.x, rectTransform.anchoredPosition.y), 0.2f * Kernel.fpsDeltaTime);



                TaskBarManager taskBarManager = TaskBarManager.instance;
                rectTransformSize.offset = new Vector2(0, -(taskBarManager.rectTransform.sizeDelta.y - taskBarManager.rectTransform.anchoredPosition.y));


                if (TaskBarManager.SaveData.topMode)
                    rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, (taskBarManager.rectTransform.sizeDelta.y - taskBarManager.rectTransform.anchoredPosition.y) * 0.5f);
                else
                    rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -(taskBarManager.rectTransform.sizeDelta.y - taskBarManager.rectTransform.anchoredPosition.y) * 0.5f);



                if (content.sizeDelta.y > rectTransform.sizeDelta.y)
                {
                    scrollBarHandleShow.anchorMin = scrollBar.handleRect.anchorMin;
                    scrollBarHandleShow.anchorMax = scrollBar.handleRect.anchorMax;

                    scrollBar.interactable = true;

                    scrollBarRectTransform.anchoredPosition = scrollBarRectTransform.anchoredPosition.Lerp(Vector2.zero, 0.2f * Kernel.fpsDeltaTime);
                    viewPort.offsetMax = viewPort.offsetMax.Lerp(new Vector2(-scrollBarRectTransform.sizeDelta.x, 0), 0.2f * Kernel.fpsDeltaTime);
                    contentRectTransformSize.offset = contentRectTransformSize.offset.Lerp(new Vector2(-scrollBarRectTransform.sizeDelta.x, 0), 0.2f * Kernel.fpsDeltaTime);
                }
                else
                {
                    scrollBar.interactable = false;

                    scrollBarRectTransform.anchoredPosition = scrollBarRectTransform.anchoredPosition.Lerp(new Vector2(scrollBarRectTransform.sizeDelta.x, 0), 0.2f * Kernel.fpsDeltaTime);
                    viewPort.offsetMax = viewPort.offsetMax.Lerp(Vector2.zero, 0.2f * Kernel.fpsDeltaTime);
                    contentRectTransformSize.offset = contentRectTransformSize.offset.Lerp(Vector2.zero, 0.2f * Kernel.fpsDeltaTime);
                }
            }
        }
    }
}