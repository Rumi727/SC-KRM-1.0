using SCKRM.Object;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.UI
{
    public interface IUI
    {
        public RectTransform parentRectTransform { get; }
        public RectTransform rectTransform { get; }

        public Graphic graphic { get; }
    }

    public class UI : UIBehaviour, IUI
    {
        [SerializeField] RectTransform _parentRectTransform; public RectTransform parentRectTransform
        {
            get
            {
                if (_parentRectTransform == null)
                {
                    RectTransform rectTransform = transform.parent as RectTransform;
                    if (rectTransform == null)
                        _parentRectTransform = transform.parent.gameObject.AddComponent<RectTransform>();
                }

                return _parentRectTransform;
            }
        }
        [SerializeField] RectTransform _rectTransform; public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    RectTransform rectTransform = transform as RectTransform;
                    if (rectTransform == null)
                        _rectTransform = gameObject.AddComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

        [SerializeField] Graphic _graphic; public Graphic graphic => _graphic = this.GetComponentFieldSave(_graphic, ComponentTool.GetComponentMode.none);
    }

    public class UIObjectPooling : UI, IObjectPooling
    {
        public string objectKey { get; set; }

        public bool isRemoved => !isActived;

        public bool isActived { get; private set; }
        bool IObjectPooling.isActived { get => isActived; set => isActived = value; }



        IRefresh[] _refreshableObjects;
        public IRefresh[] refreshableObjects => _refreshableObjects = this.GetComponentsInChildrenFieldSave(_refreshableObjects, true);



        /// <summary>
        /// Please put base.OnCreate() when overriding
        /// </summary>
        public virtual void OnCreate() => IObjectPooling.OnCreateDefault(transform, this);

        /// <summary>
        /// Please put base.Remove() when overriding
        /// </summary>
        public virtual bool Remove() => IObjectPooling.RemoveDefault(this, this);
    }

    public class UIManager<T> : UI where T : MonoBehaviour
    {
        public static T instance { get; private set; }



        protected static bool SingletonCheck(T manager)
        {
            if (instance != null && instance != manager)
            {
                DestroyImmediate(manager.gameObject);
                return false;
            }

            return (instance = manager) == manager;
        }
    }

    public abstract class UILayout : UI
    {
        protected override void Awake()
        {
            onTransformParentChangedMethodLock = true;
            LayoutUpdate();
            onTransformParentChangedMethodLock = false;
        }

        bool onTransformParentChangedMethodLock = false;
        protected override void OnTransformParentChanged()
        {
            if (onTransformParentChangedMethodLock || !isActiveAndEnabled)
                return;

            onTransformParentChangedMethodLock = true;
            LayoutUpdate();
            onTransformParentChangedMethodLock = false;
        }

        bool onRectTransformDimensionsChangeMethodLock = false;
        protected override void OnRectTransformDimensionsChange()
        {
            if (onRectTransformDimensionsChangeMethodLock || !isActiveAndEnabled)
                return;

            onRectTransformDimensionsChangeMethodLock = true;
            LayoutUpdate();
            onRectTransformDimensionsChangeMethodLock = false;
        }

        public abstract void LayoutUpdate();
    }

    public class UIAni : UI
    {
        [SerializeField] bool _lerp = true;
        public bool lerp { get => _lerp; set => _lerp = value; }
        [SerializeField, Range(0, 1)] float _lerpValue = 0.2f;
        public float lerpValue { get => _lerpValue; set => _lerpValue = value; }
    }

    public abstract class UIAniLayout : UIAni
    {
        //protected override void OnEnable() => update();

        //protected override void OnValidate() => update();

        protected override void Awake()
        {
            onTransformParentChangedMethodLock = true;
            Update();
            onTransformParentChangedMethodLock = false;
        }

        bool onTransformParentChangedMethodLock = false;
        protected override void OnTransformParentChanged()
        {
            if (onTransformParentChangedMethodLock || (lerp && lerpValue < 1) || !isActiveAndEnabled)
                return;

            onTransformParentChangedMethodLock = true;
            Update();
            onTransformParentChangedMethodLock = false;
        }

        bool onRectTransformDimensionsChangeMethodLock = false;
        protected override void OnRectTransformDimensionsChange()
        {
            if (onRectTransformDimensionsChangeMethodLock || (lerp && lerpValue < 1) || !isActiveAndEnabled)
                return;

            onRectTransformDimensionsChangeMethodLock = true;
            Update();
            onRectTransformDimensionsChangeMethodLock = false;
        }

        /*void update()
        {
#if UNITY_EDITOR
            if (!lerp || !Kernel.isPlaying)
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
            if (lerp && Kernel.isPlaying)
#else
            if (lerp)
#endif
                SizeUpdate();
        }*/

        protected virtual void Update()
        {
            onRectTransformDimensionsChangeMethodLock = true;

            LayoutRefresh();
            SizeUpdate();

            onRectTransformDimensionsChangeMethodLock = false;
        }

        public virtual void LayoutRefresh()
        {

        }

        public abstract void SizeUpdate(bool useAni = true);
    }
}