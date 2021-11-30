using SCKRM.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI
{
    [ExecuteAlways]
    [AddComponentMenu("커널/UI/자식들의 Rect Transform 크기 따라가기")]
    [RequireComponent(typeof(RectTransform))]
    public class ChildRectTransformSize : MonoBehaviour
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

        [SerializeField] Mode _mode = Mode.None;
        public Mode mode { get => _mode; set => _mode = value; }

        [SerializeField] Vector2 _offset = Vector2.zero;
        public Vector2 offset { get => _offset; set => _offset = value; }
        [SerializeField] float _spacing = 0;
        public float spacing { get => _spacing; set => _spacing = value; }
        [SerializeField] bool _lerp = false;
        public bool lerp { get => _lerp; set => _lerp = value; }


        public RectTransform[] childRectTransforms { get; private set; }


        [System.NonSerialized] int tempChildCount = -1;
        void Update()
        {
            if (tempChildCount != transform.childCount || !Application.isPlaying)
            {
                childRectTransforms = new RectTransform[transform.childCount];
                for (int i = 0; i < childRectTransforms.Length; i++)
                {
                    Transform childTransform = transform.GetChild(i);
                    childRectTransforms[i] = childTransform.GetComponent<RectTransform>();
                }

                tempChildCount = transform.childCount;
            }

            float x = 0;
            float y = 0;
            for (int i = 0; i < childRectTransforms.Length; i++)
            {
                RectTransform childRectTransform = childRectTransforms[i];
                if (childRectTransform == null)
                {
                    spacingCancel();
                    continue;
                }
                else if (!childRectTransform.gameObject.activeSelf)
                {
                    spacingCancel();
                    continue;
                }
                else if (childRectTransform.sizeDelta.x == 0 || childRectTransform.sizeDelta.y == 0)
                {
                    spacingCancel();
                    continue;
                }

                x += childRectTransform.sizeDelta.x + spacing;
                y += childRectTransform.sizeDelta.y + spacing;

                spacingCancel();

                void spacingCancel()
                {
                    if (i == childRectTransforms.Length - 1)
                    {
                        x -= spacing;
                        y -= spacing;
                    }
                }
            }

#if UNITY_EDITOR
            if (!lerp || !Application.isPlaying)
#else
            if (!lerp)
#endif
            {
                if (mode == Mode.XSize)
                    rectTransform.sizeDelta = new Vector2(x + offset.x, rectTransform.sizeDelta.y);
                else if (mode == Mode.YSize)
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, y + offset.y);
            }
            else
            {
                if (mode == Mode.XSize)
                    rectTransform.sizeDelta = rectTransform.sizeDelta.Lerp(new Vector2(x + offset.x, rectTransform.sizeDelta.y), 0.2f * Kernel.fpsDeltaTime);
                else if (mode == Mode.YSize)
                    rectTransform.sizeDelta = rectTransform.sizeDelta.Lerp(new Vector2(rectTransform.sizeDelta.x, y + offset.y), 0.2f * Kernel.fpsDeltaTime);
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