using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI.Layout
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("커널/UI/Layout/수평 레이아웃")]
    public sealed class HorizontalLayout : LayoutChildSetting<HorizontalLayoutSetting>
    {
        [SerializeField] RectOffset _padding = new RectOffset();
        public RectOffset padding { get => _padding; set => _padding = value; }

        DrivenRectTransformTracker tracker;

        protected override void OnDisable() => tracker.Clear();

        protected override void SizeUpdate()
        {
            if (childRectTransforms == null)
                return;

            tracker.Clear();

            bool center = false;
            bool right = false;
            float x = 0;
            for (int i = 0; i < childRectTransforms.Count; i++)
            {
                RectTransform childRectTransform = childRectTransforms[i];
                if (childRectTransform == null)
                    continue;
                else if (!childRectTransform.gameObject.activeSelf)
                    continue;

                tracker.Add(this, childRectTransform, DrivenTransformProperties.AnchoredPosition3D | DrivenTransformProperties.SizeDeltaY | DrivenTransformProperties.Anchors | DrivenTransformProperties.Pivot);

                HorizontalLayoutSetting taskBarLayoutSetting = childSettingComponents[i];
                if (taskBarLayoutSetting != null)
                {
                    if (!right && taskBarLayoutSetting.mode == HorizontalLayoutSetting.Mode.right)
                    {
                        right = true;
                        x = 0;
                    }
                    else if (!center && taskBarLayoutSetting.mode == HorizontalLayoutSetting.Mode.center)
                    {
                        center = true;

                        x = 0;
                        x += (childRectTransform.sizeDelta.x + (padding.left - padding.right) - spacing) * 0.5f;
                        for (int j = i; j < childRectTransforms.Count; j++)
                        {
                            RectTransform rectTransform2 = childRectTransforms[j];
                            if (rectTransform2 == null)
                                continue;
                            else if (!rectTransform2.gameObject.activeSelf)
                                continue;
                            else if (rectTransform2.sizeDelta.x == 0)
                                continue;

                            HorizontalLayoutSetting taskBarLayoutSetting2 = childSettingComponents[j];
                            if (taskBarLayoutSetting2 != null && taskBarLayoutSetting2.mode == HorizontalLayoutSetting.Mode.right)
                                break;

                            x -= rectTransform2.sizeDelta.x * 0.5f;
                        }
                    }
                }

                if (right)
                {
                    childRectTransform.anchorMin = new Vector2(1, 0);
                    childRectTransform.anchorMax = new Vector2(1, 1);
                    childRectTransform.pivot = new Vector2(1, 0.5f);
#if UNITY_EDITOR
                    if (!Application.isPlaying || !lerp)
#else
                    if (!lerp)
#endif
                        childRectTransform.anchoredPosition = new Vector2(x - padding.right, -(padding.top - padding.bottom) * 0.5f);
                    else
                        childRectTransform.anchoredPosition = childRectTransform.anchoredPosition.Lerp(new Vector2(x - padding.right, -(padding.top - padding.bottom) * 0.5f), lerpValue * Kernel.fpsUnscaledDeltaTime);

                    x -= childRectTransform.sizeDelta.x + spacing;
                }
                else if (center)
                {
                    childRectTransform.anchorMin = new Vector2(0.5f, 0);
                    childRectTransform.anchorMax = new Vector2(0.5f, 1);
                    childRectTransform.pivot = new Vector2(0.5f, 0.5f);
#if UNITY_EDITOR
                    if (!Application.isPlaying || !lerp)
#else
                    if (!lerp)
#endif
                        childRectTransform.anchoredPosition = new Vector2(x, -(padding.top - padding.bottom) * 0.5f);
                    else
                        childRectTransform.anchoredPosition = childRectTransform.anchoredPosition.Lerp(new Vector2(x, -(padding.top - padding.bottom) * 0.5f), lerpValue * Kernel.fpsUnscaledDeltaTime);

                    x += childRectTransform.sizeDelta.x + spacing;
                }
                else
                {
                    childRectTransform.anchorMin = new Vector2(0, 0);
                    childRectTransform.anchorMax = new Vector2(0, 1);
                    childRectTransform.pivot = new Vector2(0, 0.5f);
#if UNITY_EDITOR
                    if (!Application.isPlaying || !lerp)
#else
                    if (!lerp)
#endif
                        childRectTransform.anchoredPosition = new Vector2(x + padding.left, -(padding.top - padding.bottom) * 0.5f);
                    else
                        childRectTransform.anchoredPosition = childRectTransform.anchoredPosition.Lerp(new Vector2(x + padding.left, -(padding.top - padding.bottom) * 0.5f), lerpValue * Kernel.fpsUnscaledDeltaTime);

                    x += childRectTransform.sizeDelta.x + spacing;
                }

                childRectTransform.offsetMin = new Vector2(childRectTransform.offsetMin.x, padding.bottom);
                childRectTransform.offsetMax = new Vector2(childRectTransform.offsetMax.x, -padding.top);
            }
        }
    }
}