using SCKRM.Input;
using SCKRM.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.UI
{
    public sealed class AnimationCurveKey : Selectable, IUI, IObjectPooling, IDragHandler
    {
        #region IUI
        [SerializeField] Canvas _canvas; public Canvas canvas => _canvas = this.GetComponentInParentFieldSave(_canvas);

        [SerializeField] RectTransform _parentRectTransform; public RectTransform parentRectTransform
        {
            get
            {
                if (_parentRectTransform == null || _parentRectTransform.gameObject != transform.parent.gameObject)
                    _parentRectTransform = transform.parent as RectTransform;

                return _parentRectTransform;
            }
        }
        [SerializeField] RectTransform _rectTransform; public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null || _rectTransform.gameObject != gameObject)
                {
                    _rectTransform = transform as RectTransform;
                    if (_rectTransform == null)
                        _rectTransform = gameObject.AddComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }
        [SerializeField] RectTransformTool _rectTransformTool; public RectTransformTool rectTransformTool => _rectTransformTool = this.GetComponentFieldSave(_rectTransformTool);

        [SerializeField] Graphic _graphic; public Graphic graphic => _graphic = this.GetComponentFieldSave(_graphic, ComponentTool.GetComponentMode.none);
        #endregion

        #region IObjectPooling
        [WikiDescription("오브젝트 키")] public string objectKey { get; set; }

        [WikiDescription("삭제 여부")] public bool isRemoved => !isActived;

        [WikiDescription("활성화 여부")] public bool isActived { get; private set; }
        bool IObjectPooling.isActived { get => isActived; set => isActived = value; }



        IRefreshable[] _refreshableObjects;
        [WikiDescription("새로고침 가능한 오브젝트들을 가져옵니다")] public IRefreshable[] refreshableObjects => _refreshableObjects = this.GetComponentsInChildrenFieldSave(_refreshableObjects, true);



        /// <summary>
        /// Please put base.OnCreate() when overriding
        /// </summary>
        public void OnCreate() => IObjectPooling.OnCreateDefault(transform, this);

        /// <summary>
        /// Please put base.Remove() when overriding
        /// </summary>
        [WikiDescription("오브젝트 삭제")]
        public bool Remove() => IObjectPooling.RemoveDefault(this, this);
        #endregion

        public AnimationCurveWindow animationCurveWindow { get; [Obsolete("It is managed by the AnimationCurveWindow class. Please do not touch it.")] internal set; }
        public int index { get; [Obsolete("It is managed by the AnimationCurveWindow class. Please do not touch it.")] internal set; }



        AnimationCurve animationCurve => animationCurveWindow.curve;



        [SerializeField] Graphic borderRhombus;
        [SerializeField] GameObject tangent;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            
            borderRhombus.color = Color.white;
            tangent.SetActive(true);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            borderRhombus.color = Color.black;
            tangent.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UnityEngine.Camera camera;
            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                camera = canvas.worldCamera;
            else
                camera = null;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(animationCurveWindow.linePivot, InputManager.mousePosition, camera, out Vector2 position);
            position /= animationCurveWindow.lineSimpleZoom.CurrentZoom;

            float min = float.MinValue;
            float max = float.MaxValue;
            if (index > 0)
                min = animationCurve[index - 1].time + MathTool.epsilonFloatWithAccuracy;
            if (index < animationCurve.keys.Length - 1)
                max = animationCurve[index + 1].time - MathTool.epsilonFloatWithAccuracy;
            
            Keyframe keyframe = animationCurve[index];
            
            keyframe.time = position.x.Clamp(min, max);
            keyframe.value = position.y;

            animationCurve.MoveKey(index, keyframe);
            animationCurveWindow.KeyRefresh();
        }
    }
}
