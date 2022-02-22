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
    [ExecuteAlways]
    public class Dropdown : UI, IPointerEnterHandler, IPointerExitHandler
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

        [SerializeField] string[] _options = new string[0];
        public string[] options { get => _options; set => _options = value; }

        [SerializeField] string[] _customLabel = new string[0];
        public string[] customLabel { get => _customLabel; set => _customLabel = value; }

        [SerializeField, Min(0)] int _value = 0;
        public int value { get => _value; set => _value = value; }

        [SerializeField] UnityEvent _onValueChanged = new UnityEvent();
        public UnityEvent onValueChanged { get => _onValueChanged; }

        [SerializeField] TMP_Text label;
        [SerializeField] RectTransform listRectTransform;
        [SerializeField] SetSizeAsTargetRectTransform listSetSizeAsTargetRectTransform;
        [SerializeField] DropdownItem template;
        [SerializeField] RectTransform viewport;
        [SerializeField] Transform content;
        [SerializeField] RectTransform scrollbar;
        [SerializeField] GameObject scrollbarHandle;

        bool pointer;
        bool mouseDrag = false;
        Vector2 tempMousePos;
        bool invokeLock = false;

        List<DropdownItem> dropdownItems = new List<DropdownItem>();

        void Update()
        {
            if (options.Length > 0)
            {
                value = value.Clamp(0, options.Length - 1);

                if (customLabel.Length > value)
                    label.text = customLabel[value];
                else
                    label.text = options[value];
            }
            else
            {
                value = 0;
                label.text = "";
            }

            if (options.Length >= 9)
            {
                if (!scrollbarHandle.activeSelf)
                    scrollbarHandle.SetActive(true);

                viewport.offsetMax = new Vector2(-scrollbar.sizeDelta.x, 0);
            }
            else
            {
                if (scrollbarHandle.activeSelf)
                    scrollbarHandle.SetActive(false);

                viewport.offsetMax = Vector2.zero;
            }

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
                    listRectTransform.sizeDelta = listRectTransform.sizeDelta.Lerp(new Vector2(listRectTransform.sizeDelta.x, listRectTransform.anchoredPosition.y), listSetSizeAsTargetRectTransform.lerpValue * Kernel.fpsUnscaledDeltaTime);
                else if (!pointer && !mouseDrag && UnityEngine.Input.GetMouseButtonUp(0))
                    Hide();
                else if (UnityEngine.Input.GetMouseButtonUp(0))
                    mouseDrag = false;
            }
        }

        void OnEnable() => Hide();
        void OnDisable() => Hide();

        public void Toggle()
        {
            if (isShow)
            {
                Hide();
                return;
            }

            KernelCanvas.kernelBackEventList.Add(Hide);
            KernelCanvas.homeEvent += Hide;

            _isShow = true;
            listSetSizeAsTargetRectTransform.enabled = true;

            for (int i = 1; i < content.childCount; i++)
            {
                GameObject gameObject = content.GetChild(i).gameObject;
                if (content.gameObject != gameObject)
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }

            invokeLock = true;
            dropdownItems.Clear();
            for (int i = 0; i < options.Length; i++)
            {
                string option = options[i];

                DropdownItem dropdownItem = Instantiate(template, content);
                dropdownItem.gameObject.SetActive(true);

                dropdownItem.gameObject.name = option;

                if (i < customLabel.Length)
                    dropdownItem.label.text = customLabel[i];
                else
                    dropdownItem.label.text = option;

                if (i == value)
                    dropdownItem.toggle.isOn = true;
                else
                    dropdownItem.toggle.isOn = false;

                RendererManager.Rerender(dropdownItem.renderers, false).Forget();

                dropdownItems.Add(dropdownItem);
            }
            invokeLock = false;
        }

        public void Hide()
        {
            if (!isShow)
                return;

            KernelCanvas.kernelBackEventList.Remove(Hide);
            KernelCanvas.homeEvent -= Hide;

            mouseDrag = false;

            _isShow = false;
            listSetSizeAsTargetRectTransform.enabled = false;

            for (int i = 0; i < dropdownItems.Count; i++)
                dropdownItems[i].toggle.interactable = false;
        }

        public void OnValueChangedInvoke(DropdownItem dropdownItem)
        {
            if (!invokeLock)
            {
                if (dropdownItem.toggle.isOn)
                {
                    value = dropdownItem.transform.GetSiblingIndex() - 1;
                    label.text = dropdownItem.label.text;
                }

                onValueChanged.Invoke();
                Hide();
            }
        }

        public void OnPointerEnter(PointerEventData eventData) => pointer = true;

        public void OnPointerExit(PointerEventData eventData) => pointer = false;
    }
}