using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    [AddComponentMenu("커널/UI/Rect Transform 정보")]
    public class RectTransformInfo : MonoBehaviour
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

        [SerializeReference, HideInInspector] GameObject tempParentGameobject;
        [SerializeField, HideInInspector] RectTransformInfo _parentRectTransformSetting;
        public RectTransformInfo parentRectTransformSetting
        {
            get
            {
                if (transform.parent != null && (_parentRectTransformSetting == null || tempParentGameobject != transform.parent.gameObject))
                {
                    _parentRectTransformSetting = transform.parent.GetComponent<RectTransformInfo>();
                    tempParentGameobject = transform.parent.gameObject;
                }

                return _parentRectTransformSetting;
            }
        }

        public Rect rect
        {
            get
            {
                if (parentRectTransformSetting == null)
                {
                    Vector2 position = (Vector2)rectTransform.position - (rectTransform.sizeDelta * rectTransform.pivot);
                    Vector2 size = rectTransform.sizeDelta;

                    return new Rect(position, size);
                }

                Rect parentRect = parentRectTransformSetting.rect;
                Rect localRect = this.localRect;
                return new(localRect.x + parentRect.x, localRect.y + parentRect.y, localRect.width, localRect.height);
            }
            set
            {
                Rect parentRect = parentRectTransformSetting.rect;
                localRect = new Rect(value.x - parentRect.x, value.y - parentRect.y, value.width, value.height);
            }
        }
        public Rect localRect
        {
            get
            {
                if (parentRectTransformSetting == null)
                {
                    Vector2 position = Vector2.zero;
                    Vector2 size = rectTransform.sizeDelta;

                    return new Rect(position, size);
                }

                Rect parentLocalRect = parentRectTransformSetting.localRect;
                float x = rectTransform.offsetMin.x + (parentLocalRect.width * rectTransform.anchorMin.x);
                float y = rectTransform.offsetMin.y + (parentLocalRect.height * rectTransform.anchorMin.y);
                float width = rectTransform.offsetMax.x + (parentLocalRect.width * rectTransform.anchorMax.x) - x;
                float height = rectTransform.offsetMax.y + (parentLocalRect.height * rectTransform.anchorMax.y) - y;

                return new Rect(x, y, width, height);
            }
            set
            {
                Rect parentLocalRect = parentRectTransformSetting.localRect;
                float minX = value.x - (parentLocalRect.width * rectTransform.anchorMin.x);
                float minY = value.y - (parentLocalRect.height * rectTransform.anchorMin.y);
                float maxX = value.width - (parentLocalRect.width * rectTransform.anchorMax.x) + value.x;
                float maxY = value.height - (parentLocalRect.height * rectTransform.anchorMax.y) + value.y;
                rectTransform.offsetMin = new Vector2(minX, minY);
                rectTransform.offsetMax = new Vector2(maxX, maxY);
            }
        }

        /// <summary>
        /// set is not recommended to be used, it doesn't move like RectTransform.sizeDelta (anchor doesn't work)
        /// set은 사용하지 않는것을 추천합니다, RectTransform.sizeDelta 처럼 움직이지 않습니다 (앵커가 작동하지 않습니다)
        /// </summary>
        public Vector2 localSize
        {
            get
            {
                Rect localRect = this.localRect;
                return new Vector2(localRect.width, localRect.height);
            }
            set
            {
                Rect localRect = this.localRect;
                this.localRect = new Rect(localRect.x, localRect.y, value.x, value.y);
            }
        }

        public enum AutoSize
        {
            none,
            target,
            child
        }
    }
}