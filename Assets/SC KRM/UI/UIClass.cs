using SCKRM.Object;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        [NonSerialized] Graphic _graphic; public Graphic graphic
        {
            get
            {
                if (_graphic == null)
                {
                    _graphic = GetComponent<Graphic>();
                    if (_graphic == null)
                        return null;
                }

                return _graphic;
            }
        }
    }

    public class ManagerUI<T> : Manager<T> where T : MonoBehaviour
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

        [NonSerialized] Graphic _graphic; public Graphic graphic
        {
            get
            {
                if (_graphic == null)
                {
                    _graphic = GetComponent<Graphic>();
                    if (_graphic == null)
                        return null;
                }

                return _graphic;
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

        [NonSerialized] Graphic _graphic; public Graphic graphic
        {
            get
            {
                if (_graphic == null)
                {
                    _graphic = GetComponent<Graphic>();
                    if (_graphic == null)
                        return null;
                }

                return _graphic;
            }
        }
    }

    public abstract class UIAni : UILayout
    {
        [SerializeField] bool _lerp = true;
        public bool lerp { get => _lerp; set => _lerp = value; }
        [SerializeField, Range(0, 1)] float _lerpValue = 0.2f;
        public float lerpValue { get => _lerpValue; set => _lerpValue = value; }

        /*//protected override void OnEnable() => update();

        //protected override void OnValidate() => update();

        //protected override void OnTransformParentChanged() => update();

        protected override void OnRectTransformDimensionsChange() => update();

        void update()
        {
#if UNITY_EDITOR
            if (!lerp || !Application.isPlaying)
#else
            if (!lerp)
#endif
            {
                LayoutRefresh();
                SizeUpdate();
            }
            else
            {
                Debug.Log("asdf");
                LayoutRefresh();
            }
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR
            if (lerp && Application.isPlaying)
#else
            if (lerp)
#endif
                SizeUpdate();
        }*/

        protected virtual void Update()
        {
            LayoutRefresh();
            SizeUpdate();
        }

        protected virtual void LayoutRefresh()
        {

        }

        protected abstract void SizeUpdate();
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