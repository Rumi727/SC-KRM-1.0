using SCKRM.UI.Layout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    [ExecuteAlways]
    [AddComponentMenu("커널/UI/선택한 Rect Transform의 크기 따라가기")]
    [RequireComponent(typeof(RectTransform))]
    public sealed class SetSizeAsTargetRectTransform : UIAniLayout
    {
        [SerializeField] RectTransform _targetRectTransform;
        public RectTransform targetRectTransform { get => _targetRectTransform; set => _targetRectTransform = value; }

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



        DrivenRectTransformTracker tracker;


        //protected override void OnEnable() => tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDelta);
        protected override void OnDisable() => tracker.Clear();



        public override void LayoutRefresh()
        {
            if (targetRectTransform == null)
                return;

            size = new Vector2(targetRectTransform.sizeDelta.x * targetRectTransform.localScale.x, targetRectTransform.sizeDelta.y * targetRectTransform.localScale.y) + offset;
            if (max.x <= 0)
                size.x = size.x.Clamp(min.x);
            else
                size.x = size.x.Clamp(min.x, max.x);
            if (max.y <= 0)
                size.y = size.y.Clamp(min.y);
            else
                size.y = size.y.Clamp(min.y, max.y);
        }

        Vector2 size;
        public override void SizeUpdate(bool useAni = true)
        {
            tracker.Clear();

            if (xSize)
                tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);
            if (ySize)
                tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);

#if UNITY_EDITOR
            if (!lerp || !useAni || !Application.isPlaying)
#else
            if (!lerp || !useAni)
#endif
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