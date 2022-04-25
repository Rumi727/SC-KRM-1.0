using SCKRM.Input;
using SCKRM.Tool;
using SCKRM.UI;
using SCKRM.UI.StatusBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SCKRM.UI.SideBar
{
    public class SideBarAni : UIAni
    {
        [SerializeField] string _showControlKey; public string showControlKey => _showControlKey;
        [SerializeField] string _inputLockName; public string inputLockName => _inputLockName;


        [SerializeField] UnityEvent _showEvent; public UnityEvent showEvent => showEvent;
        [SerializeField] UnityEvent _hideEvent; public UnityEvent hideEvent => hideEvent;



        bool _isShow;
        public bool isShow
        {
            get => _isShow;
            private set
            {
                if (value != _isShow)
                {
                    if (value)
                    {
                        SideBarManager.AllHide();

                        UIManager.BackEventAdd(Hide, true);
                        UIManager.homeEvent += Hide;

                        SideBarManager.showedSideBars.Add(this);
                    }
                    else
                    {
                        UIManager.BackEventRemove(Hide, true);
                        UIManager.homeEvent -= Hide;

                        SideBarManager.showedSideBars.Remove(this);
                    }

                    InputManager.SetInputLock(inputLockName, value);
                    _isShow = value;
                }
            }
        }

        [SerializeField] bool _right = false;
        public bool right { get => _right; set => _right = value; } 



        #region variable
        [SerializeField] RectTransform _viewPort;
        public RectTransform viewPort => _viewPort;

        [SerializeField] RectTransform _content;
        public RectTransform content => _content;

        [SerializeField] RectTransform _scrollBarParentRectTransform;
        public RectTransform scrollBarParentRectTransform => _scrollBarParentRectTransform;

        [SerializeField] Scrollbar _scrollBar;
        public Scrollbar scrollBar => _scrollBar;
        #endregion

        public void Show()
        {
            isShow = true;
            showEvent.Invoke();
        }

        public void Hide()
        {
            isShow = false;
            hideEvent.Invoke();
        }

        public void Toggle()
        {
            if (isShow)
                Hide();
            else
                Show();
        }

        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                if (!(showControlKey == null || showControlKey == "") && InputManager.GetKey(showControlKey, InputType.Down, "all"))
                    Toggle();

                if (isShow)
                {
                    if (!viewPort.gameObject.activeSelf)
                    {
                        viewPort.gameObject.SetActive(true);
                        scrollBarParentRectTransform.gameObject.SetActive(true);
                        graphic.enabled = true;
                    }
                }
                else
                {
                    if (this.right)
                    {
                        if (rectTransform.anchoredPosition.x >= rectTransform.rect.size.x - 0.01f)
                        {
                            if (viewPort.gameObject.activeSelf)
                            {
                                viewPort.gameObject.SetActive(false);
                                scrollBarParentRectTransform.gameObject.SetActive(false);
                                graphic.enabled = false;
                            }

                            return;
                        }
                    }
                    else
                    {
                        if (rectTransform.anchoredPosition.x <= (-rectTransform.rect.size.x) + 0.01f)
                        {
                            if (viewPort.gameObject.activeSelf)
                            {
                                viewPort.gameObject.SetActive(false);
                                scrollBarParentRectTransform.gameObject.SetActive(false);
                                graphic.enabled = false;
                            }

                            return;
                        }
                    }
                }


                int right;
                if (this.right)
                    right = 1;
                else
                    right = -1;



                if (lerp)
                {
                    if (isShow)
                        rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(0, rectTransform.anchoredPosition.y), lerpValue * Kernel.fpsUnscaledDeltaTime);
                    else
                        rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(right * rectTransform.rect.size.x, rectTransform.anchoredPosition.y), lerpValue * Kernel.fpsUnscaledDeltaTime);



                    if (content.rect.size.y > rectTransform.rect.size.y)
                    {
                        scrollBar.interactable = true;

                        scrollBarParentRectTransform.anchoredPosition = scrollBarParentRectTransform.anchoredPosition.Lerp(Vector2.zero, lerpValue * Kernel.fpsUnscaledDeltaTime);

                        if (this.right)
                            viewPort.offsetMax = viewPort.offsetMax.Lerp(new Vector2(-scrollBarParentRectTransform.rect.size.x, 0), lerpValue * Kernel.fpsUnscaledDeltaTime);
                        else
                            viewPort.offsetMin = viewPort.offsetMin.Lerp(new Vector2(scrollBarParentRectTransform.rect.size.x, 1), lerpValue * Kernel.fpsUnscaledDeltaTime);
                    }
                    else
                    {
                        scrollBar.interactable = false;

                        scrollBarParentRectTransform.anchoredPosition = scrollBarParentRectTransform.anchoredPosition.Lerp(new Vector2(right * scrollBarParentRectTransform.rect.size.x, 0), 0.2f * Kernel.fpsUnscaledDeltaTime);
                        viewPort.offsetMin = viewPort.offsetMin.Lerp(Vector2.zero, lerpValue * Kernel.fpsUnscaledDeltaTime);
                        viewPort.offsetMax = viewPort.offsetMax.Lerp(Vector2.zero, lerpValue * Kernel.fpsUnscaledDeltaTime);
                    }
                }
                else
                {
                    if (isShow)
                        rectTransform.anchoredPosition = new Vector2(0, rectTransform.anchoredPosition.y);
                    else
                        rectTransform.anchoredPosition = new Vector2(right * rectTransform.rect.size.x, rectTransform.anchoredPosition.y);



                    if (content.rect.size.y > rectTransform.rect.size.y)
                    {
                        scrollBar.interactable = true;

                        scrollBarParentRectTransform.anchoredPosition = Vector2.zero;

                        if (this.right)
                            viewPort.offsetMax = new Vector2(-scrollBarParentRectTransform.rect.size.x, 0);
                        else
                            viewPort.offsetMin = new Vector2(scrollBarParentRectTransform.rect.size.x, 1);
                    }
                    else
                    {
                        scrollBar.interactable = false;

                        scrollBarParentRectTransform.anchoredPosition = new Vector2(right * scrollBarParentRectTransform.rect.size.x, 0);
                        viewPort.offsetMin = Vector2.zero;
                        viewPort.offsetMax = Vector2.zero;
                    }
                }
            }
        }
    }
}
