using SCKRM.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    [ExecuteAlways]
    [AddComponentMenu("커널/UI/선택한 Rect Transform의 크기 따라가기")]
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformSize : MonoBehaviour
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


        [SerializeField] RectTransform _targetRectTransform;
        public RectTransform targetRectTransform { get => _targetRectTransform; set => _targetRectTransform = value; }

        [SerializeField] bool _xSize = false;
        public bool xSize { get => _xSize; set => _xSize = value; }
        [SerializeField] bool _ySize = false;
        public bool ySize { get => _ySize; set => _ySize = value; }

        [SerializeField] Vector2 _offset = Vector2.zero;
        public Vector2 offset { get => _offset; set => _offset = value; }

        [SerializeField] bool _lerp = false;
        public bool lerp { get => _lerp; set => _lerp = value; }

        void Update()
        {
            if (targetRectTransform == null)
                return;

            if (!lerp || Application.isPlaying)
            {
                if (xSize && !ySize)
                    rectTransform.sizeDelta = new Vector2((targetRectTransform.sizeDelta.x * targetRectTransform.localScale.x) + offset.x, rectTransform.sizeDelta.y);
                else if (!xSize && ySize)
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (targetRectTransform.sizeDelta.y * targetRectTransform.localScale.y) + offset.y);
                else if (xSize && ySize)
                    rectTransform.sizeDelta = new Vector2(targetRectTransform.sizeDelta.x * targetRectTransform.localScale.x, targetRectTransform.sizeDelta.y * targetRectTransform.localScale.y) + offset;
            }
            else
            {
                if (xSize && !ySize)
                    rectTransform.sizeDelta = rectTransform.sizeDelta.Lerp(new Vector2((targetRectTransform.sizeDelta.x * targetRectTransform.localScale.x) + offset.x, rectTransform.sizeDelta.y), 0.2f * Kernel.fpsDeltaTime);
                else if (!xSize && ySize)
                    rectTransform.sizeDelta = rectTransform.sizeDelta.Lerp(new Vector2(rectTransform.sizeDelta.x, (targetRectTransform.sizeDelta.y * targetRectTransform.localScale.y) + offset.y), 0.2f * Kernel.fpsDeltaTime);
                else if (xSize && ySize)
                    rectTransform.sizeDelta = rectTransform.sizeDelta.Lerp(new Vector2(targetRectTransform.sizeDelta.x * targetRectTransform.localScale.x, targetRectTransform.sizeDelta.y * targetRectTransform.localScale.y) + offset, 0.2f * Kernel.fpsDeltaTime);            }
        }
    }
}