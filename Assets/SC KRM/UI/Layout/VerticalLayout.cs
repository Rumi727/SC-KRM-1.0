using SCKRM.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform), typeof(RectTransformInfo))]
    [AddComponentMenu("커널/UI/Layout/수직 레이아웃")]
    public sealed class VerticalLayout : MonoBehaviour
    {
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


        [SerializeField] RectOffset _padding = new RectOffset();
        public RectOffset padding { get => _padding; set => _padding = value; }
        [SerializeField, Min(0)] float _spacing;
        public float spacing { get => _spacing; set => _spacing = value; }
        [SerializeField] bool _lerp = true;
        public bool lerp { get => _lerp; set => _lerp = value; }


        public RectTransform[] childRectTransforms { get; private set; }
        public VerticalLayoutSetting[] childVerticalLayoutSettings { get; private set; }



        [System.NonSerialized] int tempChildCount = -1;
        void Update()
        {
            float localSizeX = rectTransformInfo.localSize.x;

            {
#if UNITY_EDITOR
                if (tempChildCount != transform.childCount || !Application.isPlaying)
                {
                    SetChild();
                    tempChildCount = transform.childCount;
                }
#else
                if (tempChildCount != transform.childCount)
                {
                    SetChild();
                    tempChildCount = transform.childCount;
                }
#endif
                for (int i = 0; i < childRectTransforms.Length; i++)
                {
                    RectTransform rectTransform = childRectTransforms[i];
                    if (i != rectTransform.GetSiblingIndex())
                        SetChild();
                }

                void SetChild()
                {
                    childRectTransforms = new RectTransform[transform.childCount];
                    childVerticalLayoutSettings = new VerticalLayoutSetting[childRectTransforms.Length];
                    for (int i = 0; i < childRectTransforms.Length; i++)
                    {
                        Transform childTransform = transform.GetChild(i);
                        childRectTransforms[i] = childTransform.GetComponent<RectTransform>();
                        childVerticalLayoutSettings[i] = childTransform.GetComponent<VerticalLayoutSetting>();
                    }
                }
            }

            if (childRectTransforms == null || childRectTransforms.Length <= 0)
                return;

            bool center = false;
            bool down = false;
            float y = 0;
            for (int i = 0; i < childRectTransforms.Length; i++)
            {
                RectTransform childRectTransform = childRectTransforms[i];
                if (childRectTransform == null)
                    continue;
                else if (!childRectTransform.gameObject.activeSelf)
                    continue;
                else if (childRectTransform.sizeDelta.x == 0 || childRectTransform.sizeDelta.y == 0)
                    continue;

                VerticalLayoutSetting taskBarLayoutSetting = childVerticalLayoutSettings[i];
                if (taskBarLayoutSetting != null)
                {
                    if (taskBarLayoutSetting.custom)
                        continue;

                    if (!down && taskBarLayoutSetting.mode == VerticalLayoutSetting.Mode.down)
                    {
                        down = true;
                        y = 0;
                    }
                    else if (!center && taskBarLayoutSetting.mode == VerticalLayoutSetting.Mode.center)
                    {
                        center = true;

                        y = 0;
                        y += (childRectTransform.sizeDelta.y - (_padding.top - _padding.bottom) - spacing) * 0.5f;
                        for (int j = i; j < childRectTransforms.Length; j++)
                        {
                            RectTransform rectTransform2 = childRectTransforms[j];
                            if (!rectTransform2.gameObject.activeSelf)
                                continue;
                            if (rectTransform2 == null)
                                continue;
                            else if (!rectTransform2.gameObject.activeSelf)
                                continue;
                            else if (rectTransform2.sizeDelta.x == 0 || rectTransform2.sizeDelta.y == 0)
                                continue;

                            VerticalLayoutSetting taskBarLayoutSetting2 = childVerticalLayoutSettings[j];
                            if (taskBarLayoutSetting2 != null && taskBarLayoutSetting2.mode == VerticalLayoutSetting.Mode.down)
                                break;

                            y -= rectTransform2.sizeDelta.y * 0.5f;
                        }
                    }
                }

                if (down)
                {
                    childRectTransform.anchorMin = new Vector2(0.5f, 0);
                    childRectTransform.anchorMax = new Vector2(0.5f, 0);
                    childRectTransform.pivot = new Vector2(0.5f, 0);
#if UNITY_EDITOR
                    if (!Application.isPlaying || !lerp)
#else
                    if (!lerp)
#endif
                        childRectTransform.anchoredPosition = new Vector2((padding.left - padding.right) * 0.5f, y + padding.bottom);
                    else
                        childRectTransform.anchoredPosition = childRectTransform.anchoredPosition.Lerp(new Vector2((padding.left - padding.right) * 0.5f, y + padding.bottom), 0.2f * Kernel.fpsDeltaTime);

                    y += childRectTransform.sizeDelta.y + spacing;
                }
                else if (center)
                {
                    childRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    childRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    childRectTransform.pivot = new Vector2(0.5f, 0.5f);
#if UNITY_EDITOR
                    if (!Application.isPlaying || !lerp)
#else
                    if (!lerp)
#endif
                        childRectTransform.anchoredPosition = new Vector2((padding.left - padding.right) * 0.5f, y);
                    else
                        childRectTransform.anchoredPosition = childRectTransform.anchoredPosition.Lerp(new Vector2((padding.left - padding.right) * 0.5f, y), 0.2f * Kernel.fpsDeltaTime);

                    y += childRectTransform.sizeDelta.y + spacing;
                }
                else
                {
                    childRectTransform.anchorMin = new Vector2(0.5f, 1);
                    childRectTransform.anchorMax = new Vector2(0.5f, 1);
                    childRectTransform.pivot = new Vector2(0.5f, 1);
#if UNITY_EDITOR
                    if (!Application.isPlaying || !lerp)
#else
                    if (!lerp)
#endif
                        childRectTransform.anchoredPosition = new Vector2((padding.left - padding.right) * 0.5f, y - padding.top);
                    else
                        childRectTransform.anchoredPosition = childRectTransform.anchoredPosition.Lerp(new Vector2((padding.left - padding.right) * 0.5f, y - padding.top), 0.2f * Kernel.fpsDeltaTime);

                    y -= childRectTransform.sizeDelta.y + spacing;
                }

                childRectTransform.sizeDelta = new Vector3(localSizeX - padding.left - padding.right, childRectTransform.sizeDelta.y);
            }
        }
    }
}