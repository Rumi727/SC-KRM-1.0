using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.Layout
{
    [ExecuteAlways]
    [AddComponentMenu("커널/UI/자식들의 Rect Transform 크기 따라가기")]
    [RequireComponent(typeof(RectTransform))]
    public sealed class SetSizeAsChildRectTransform : LayoutChild
    {
        [SerializeField] Mode _mode = Mode.None;
        public Mode mode { get => _mode; set => _mode = value; }

        [SerializeField] Vector2 _offset = Vector2.zero;
        public Vector2 offset { get => _offset; set => _offset = value; }

        [SerializeField, Min(0)] float _min = 0;
        public float min { get => _min; set => _min = value; }
        [SerializeField, Min(0)] float _max = 0;
        public float max { get => _max; set => _max = value; }



        DrivenRectTransformTracker tracker;



        protected override void OnDisable() => tracker.Clear();



        public override void LayoutRefresh()
        {
            base.LayoutRefresh();

            if (childRectTransforms == null)
                return;

            float x = 0;
            float y = 0;
            
            for (int i = 0; i < childRectTransforms.Count; i++)
            {
                RectTransform childRectTransform = childRectTransforms[i];
                if (childRectTransform == null)
                {
                    spacingCancel();
                    continue;
                }
                else if (ignore.Contains(childRectTransform))
                {
                    spacingCancel();
                    continue;
                }
                else if (!childRectTransform.gameObject.activeSelf)
                {
                    spacingCancel();
                    continue;
                }

                x += childRectTransform.sizeDelta.x + spacing;
                y += childRectTransform.sizeDelta.y + spacing;

                if (i == childRectTransforms.Count - 1)
                    spacingCancel();

                void spacingCancel()
                {
                    x -= spacing;
                    y -= spacing;
                }
            }

            xSize = new Vector2(x + offset.x, rectTransform.sizeDelta.y);
            ySize = new Vector2(rectTransform.sizeDelta.x, y + offset.y);
            if (max <= 0)
            {
                xSize.x = xSize.x.Clamp(min);
                ySize.y = ySize.y.Clamp(min);
            }
            else
            {
                xSize.x = xSize.x.Clamp(min, max);
                ySize.y = ySize.y.Clamp(min, max);
            }
        }

        Vector2 xSize;
        Vector2 ySize;
        public override void SizeUpdate(bool useAni = true)
        {
            tracker.Clear();
            if (mode == Mode.XSize)
                tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);
            else if (mode == Mode.YSize)
                tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);

#if UNITY_EDITOR
            if (!lerp || !Application.isPlaying || !useAni)
#else
            if (!lerp || !useAni)
#endif
            {
                if (mode == Mode.XSize)
                    rectTransform.sizeDelta = xSize;
                else if (mode == Mode.YSize)
                    rectTransform.sizeDelta = ySize;
            }
            else
            {
                if (mode == Mode.XSize)
                    rectTransform.sizeDelta = rectTransform.sizeDelta.Lerp(xSize, lerpValue * Kernel.fpsUnscaledDeltaTime);
                else if (mode == Mode.YSize)
                    rectTransform.sizeDelta = rectTransform.sizeDelta.Lerp(ySize, lerpValue * Kernel.fpsUnscaledDeltaTime);
            }
        }

        public enum Mode
        {
            None,
            XSize,
            YSize
        }
    }
}