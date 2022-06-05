using SCKRM.UI.Layout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    [ExecuteAlways]
    [AddComponentMenu("커널/UI/선택한 Rect Transform들의 크기 따라가기")]
    [RequireComponent(typeof(RectTransform))]
    public sealed class SetSizeAsTargetsRectTransform : UIAniLayout
    {
        [SerializeField] RectTransform[] _targetRectTransforms;
        public RectTransform[] targetRectTransforms { get => _targetRectTransforms; set => _targetRectTransforms = value; }

        [SerializeField] bool _xSize = false;
        public bool xSize { get => _xSize; set => _xSize = value; }
        [SerializeField] bool _ySize = false;
        public bool ySize { get => _ySize; set => _ySize = value; }

        [SerializeField] Vector2 _offset = Vector2.zero;
        public Vector2 offset { get => _offset; set => _offset = value; }

        [SerializeField, Min(0)] Vector2 _min = Vector2.zero;
        public Vector2 min { get => _min; set => _min = value; }
        [SerializeField, Min(0)] Vector2 _max = Vector2.zero;
        public Vector2 max { get => _max; set => _max = value; }

        [SerializeField] bool _reversal = false;
        public bool reversal { get => _reversal; set => _reversal = value; }



        DrivenRectTransformTracker tracker;


        //protected override void OnEnable() => tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDelta);
        protected override void OnDisable()
        {
            if (!Kernel.isPlaying)
                tracker.Clear();
        }

        public override void LayoutRefresh()
        {
            if (targetRectTransforms == null)
                return;

            size = Vector2.zero;

            for (int i = 0; i < targetRectTransforms.Length; i++)
            {
                RectTransform targetRectTransform = targetRectTransforms[i];
                if (targetRectTransform == null)
                    continue;

                Vector2 targetSize = targetRectTransform.rect.size;
                size += new Vector2(targetSize.x * targetRectTransform.localScale.x, targetSize.y * targetRectTransform.localScale.y) + offset;
            }

            if (max.x <= 0)
                size.x = size.x.Clamp(min.x);
            else
                size.x = size.x.Clamp(min.x, max.x);
            if (max.y <= 0)
                size.y = size.y.Clamp(min.y);
            else
                size.y = size.y.Clamp(min.y, max.y);

            if (reversal)
                size = parentRectTransform.rect.size - size;
        }

        Vector2 size;
        public override void SizeUpdate(bool useAni = true)
        {
            if (!Kernel.isPlaying)
            {
                tracker.Clear();

                if (xSize)
                    tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);
                if (ySize)
                    tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);
            }

            if (!lerp || !useAni || !Kernel.isPlaying)
            {
                if (xSize && !ySize)
                    rectTransform.sizeDelta = new Vector2(size.x, rectTransform.sizeDelta.y);
                else if (!xSize && ySize)
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, size.y);
                else if (xSize && ySize)
                    rectTransform.sizeDelta = size;
            }
            else
            {
                if (xSize && !ySize)
                    rectTransform.sizeDelta = rectTransform.sizeDelta.Lerp(new Vector2(size.x, rectTransform.sizeDelta.y), lerpValue * Kernel.fpsUnscaledDeltaTime);
                else if (!xSize && ySize)
                    rectTransform.sizeDelta = rectTransform.sizeDelta.Lerp(new Vector2(rectTransform.sizeDelta.x, size.y), lerpValue * Kernel.fpsUnscaledDeltaTime);
                else if (xSize && ySize)
                    rectTransform.sizeDelta = rectTransform.sizeDelta.Lerp(size, lerpValue * Kernel.fpsUnscaledDeltaTime);
            }
        }
    }
}