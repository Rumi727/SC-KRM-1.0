using HSVPicker;
using SCKRM.Input;
using SCKRM.Renderer;
using SCKRM.Tool;
using SCKRM.UI.Layout;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.UI
{
    [AddComponentMenu("커널/UI/컬러 피커 버튼")]
    public class ColorPickerButton : UIAni, IPointerEnterHandler, IPointerExitHandler
    {
        bool _isShow = false;
        public bool isShow
        {
            get => _isShow;
            set
            {
                if (value)
                    Toggle();
                else
                    Hide();
            }
        }

        [SerializeField] RectTransform colorPickerMask;
        [SerializeField] RectTransform colorPickerRectTransform;
        [SerializeField] ColorPicker colorPicker;

        bool pointer;
        bool mouseDrag = false;
        Vector2 tempMousePos;


        protected override void SizeUpdate()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
            {
                if (!mouseDrag)
                {
                    mouseDrag = UnityEngine.Input.GetMouseButton(0) && tempMousePos != InputManager.mousePosition;
                    tempMousePos = InputManager.mousePosition;
                }
                
                if (!isShow)
                {
                    if (lerp)
                        colorPickerMask.sizeDelta = colorPickerMask.sizeDelta.Lerp(new Vector2(colorPickerMask.sizeDelta.x, colorPickerMask.anchoredPosition.y), lerpValue * Kernel.fpsUnscaledDeltaTime);
                    else
                        colorPickerMask.sizeDelta = new Vector2(colorPickerMask.sizeDelta.x, 0);

                    if (colorPickerMask.gameObject.activeSelf && colorPickerMask.sizeDelta.y < colorPickerMask.anchoredPosition.y + 0.01f)
                        colorPickerMask.gameObject.SetActive(false);
                }
                else if (!pointer && !mouseDrag && UnityEngine.Input.GetMouseButtonUp(0))
                    Hide();
                else if (UnityEngine.Input.GetMouseButtonUp(0))
                    mouseDrag = false;

                if (isShow)
                {
                    if (lerp)
                        colorPickerMask.sizeDelta = colorPickerMask.sizeDelta.Lerp(new Vector2(colorPickerMask.sizeDelta.x, colorPickerRectTransform.sizeDelta.y), lerpValue * Kernel.fpsUnscaledDeltaTime);
                    else
                        colorPickerMask.sizeDelta = new Vector2(colorPickerMask.sizeDelta.x, colorPickerRectTransform.sizeDelta.y);

                    if (!colorPickerMask.gameObject.activeSelf)
                        colorPickerMask.gameObject.SetActive(true);
                }
            }
        }

        protected override void OnEnable() => Hide();
        protected override void OnDisable() => Hide();

        public void Toggle()
        {
            if (isShow)
            {
                Hide();
                return;
            }

            UIManager.BackEventAdd(Hide, true);
            UIManager.homeEvent += Hide;

            _isShow = true;

            if (!lerp)
                SizeUpdate();
        }

        public void Hide()
        {
            if (!isShow)
                return;

            UIManager.BackEventRemove(Hide, true);
            UIManager.homeEvent -= Hide;

            mouseDrag = false;

            _isShow = false;

            if (!lerp)
                SizeUpdate();
        }

        public void OnPointerEnter(PointerEventData eventData) => pointer = true;

        public void OnPointerExit(PointerEventData eventData) => pointer = false;
    }
}