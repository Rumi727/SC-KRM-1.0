using SCKRM.Object;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SCKRM.UI
{
    public class UI : MonoBehaviour
    {
        [NonSerialized] RectTransform _rectTransform; public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                    if (_rectTransform == null)
                        _rectTransform = gameObject.AddComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }
        [NonSerialized] RectTransformInfo _rectTransformInfo; public RectTransformInfo rectTransformInfo
        {
            get
            {
                if (_rectTransformInfo == null)
                {
                    _rectTransformInfo = GetComponent<RectTransformInfo>();
                    if (_rectTransformInfo == null)
                        _rectTransformInfo = gameObject.AddComponent<RectTransformInfo>();
                }

                return _rectTransformInfo;
            }
        }
    }

    public class UILayout : UIBehaviour
    {
        [NonSerialized] RectTransform _rectTransform; public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                    if (_rectTransform == null)
                        _rectTransform = gameObject.AddComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }
        [NonSerialized] RectTransformInfo _rectTransformInfo; public RectTransformInfo rectTransformInfo
        {
            get
            {
                if (_rectTransformInfo == null)
                {
                    _rectTransformInfo = GetComponent<RectTransformInfo>();
                    if (_rectTransformInfo == null)
                        _rectTransformInfo = gameObject.AddComponent<RectTransformInfo>();
                }

                return _rectTransformInfo;
            }
        }
    }

    public abstract class UIAni : UILayout
    {
        [SerializeField] bool _lerp = true;
        public bool lerp { get => _lerp; set => _lerp = value; }
        [SerializeField, Range(0, 1)] float _lerpValue = 0.2f;
        public float lerpValue { get => _lerpValue; set => _lerpValue = value; }

        protected virtual void Update() => LayoutRefresh();

        protected abstract void LayoutRefresh();
    }

    public class ObjectPoolingUI : ObjectPooling
    {
        [NonSerialized] RectTransform _rectTransform; public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                    if (_rectTransform == null)
                        _rectTransform = gameObject.AddComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }
        [NonSerialized] RectTransformInfo _rectTransformInfo; public RectTransformInfo rectTransformInfo
        {
            get
            {
                if (_rectTransformInfo == null)
                {
                    _rectTransformInfo = GetComponent<RectTransformInfo>();
                    if (_rectTransformInfo == null)
                        _rectTransformInfo = gameObject.AddComponent<RectTransformInfo>();
                }

                return _rectTransformInfo;
            }
        }

        public override void Remove()
        {
            base.Remove();

            if (rectTransform != null)
                rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}