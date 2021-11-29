using SCKRM.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class HorizontalLayout : MonoBehaviour
    {
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

        public RectTransform[] buttonRectTransforms { get; private set; }
        public HorizontalLayoutSetting[] buttonHorizontalLayoutSettings { get; private set; }

        [System.NonSerialized] int tempChildCount = -1;

        void Update()
        {
            if (tempChildCount != transform.childCount)
            {
                buttonRectTransforms = new RectTransform[transform.childCount];
                buttonHorizontalLayoutSettings = new HorizontalLayoutSetting[buttonRectTransforms.Length];
                for (int i = 0; i < buttonRectTransforms.Length; i++)
                {
                    Transform buttonTransform = transform.GetChild(i);
                    buttonRectTransforms[i] = buttonTransform.GetComponent<RectTransform>();
                    buttonHorizontalLayoutSettings[i] = buttonTransform.GetComponent<HorizontalLayoutSetting>();
                }

                tempChildCount = transform.childCount;
            }

            if (buttonRectTransforms == null || buttonRectTransforms.Length <= 0)
                return;

            bool center = false;
            bool right = false;
            float x = 0;
            for (int i = 0; i < buttonRectTransforms.Length; i++)
            {
                RectTransform childRectTransform = buttonRectTransforms[i];
                if (childRectTransform == null)
                    continue;
                else if (!childRectTransform.gameObject.activeSelf)
                    continue;

                HorizontalLayoutSetting taskBarLayoutSetting = buttonHorizontalLayoutSettings[i];
                if (taskBarLayoutSetting != null)
                {
                    if (!center && taskBarLayoutSetting.mode == HorizontalLayoutSetting.Mode.center)
                    {
                        center = true;

                        x = 0;
                        x += childRectTransform.sizeDelta.x * 0.5f;
                        for (int j = i; j < buttonRectTransforms.Length; j++)
                        {
                            RectTransform rectTransform2 = buttonRectTransforms[j];
                            if (!rectTransform2.gameObject.activeSelf)
                                continue;

                            HorizontalLayoutSetting taskBarLayoutSetting2 = buttonHorizontalLayoutSettings[j];
                            if (taskBarLayoutSetting2 != null && taskBarLayoutSetting2.mode == HorizontalLayoutSetting.Mode.right)
                                break;

                            x -= rectTransform2.sizeDelta.x * 0.5f;
                        }
                    }
                    if (!right && taskBarLayoutSetting.mode == HorizontalLayoutSetting.Mode.right)
                    {
                        right = true;
                        x = 0;
                    }
                }

                if (right)
                {
                    childRectTransform.anchorMin = new Vector2(1, 1);
                    childRectTransform.anchorMax = new Vector2(1, 1);
                    childRectTransform.pivot = new Vector2(1, 1);
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        childRectTransform.anchoredPosition = new Vector2(x, 0);
                    else
                        childRectTransform.anchoredPosition = childRectTransform.anchoredPosition.Lerp(new Vector2(x, 0), 0.2f * Kernel.fpsDeltaTime);
#else
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(x, 0), 0.2f * Kernel.fpsDeltaTime);
#endif
                    x -= childRectTransform.sizeDelta.x;
                }
                else if (center)
                {
                    childRectTransform.anchorMin = new Vector2(0.5f, 1);
                    childRectTransform.anchorMax = new Vector2(0.5f, 1);
                    childRectTransform.pivot = new Vector2(0.5f, 1);
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        childRectTransform.anchoredPosition = new Vector2(x, 0);
                    else
                        childRectTransform.anchoredPosition = childRectTransform.anchoredPosition.Lerp(new Vector2(x, 0), 0.2f * Kernel.fpsDeltaTime);
#else
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(x, 0), 0.2f * Kernel.fpsDeltaTime);
#endif
                    x += childRectTransform.sizeDelta.x;
                }
                else
                {
                    childRectTransform.anchorMin = new Vector2(0, 1);
                    childRectTransform.anchorMax = new Vector2(0, 1);
                    childRectTransform.pivot = new Vector2(0, 1);
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        childRectTransform.anchoredPosition = new Vector2(x, 0);
                    else
                        childRectTransform.anchoredPosition = childRectTransform.anchoredPosition.Lerp(new Vector2(x, 0), 0.2f * Kernel.fpsDeltaTime);
#else
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(x, 0), 0.2f * Kernel.fpsDeltaTime);
#endif
                    x += childRectTransform.sizeDelta.x;
                }

                childRectTransform.sizeDelta = new Vector3(childRectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
            }
        }
    }
}