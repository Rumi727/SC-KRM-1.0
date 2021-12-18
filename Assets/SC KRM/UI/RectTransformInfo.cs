using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    [AddComponentMenu("커널/UI/Rect Transform 정보")]
    public sealed class RectTransformInfo : MonoBehaviour
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

        [SerializeField, HideInInspector] GameObject tempParentGameobject;
        [SerializeField, HideInInspector] RectTransformInfo _parentRectTransformSetting;

        public RectTransformInfo parentRectTransformSetting
        {
            get
            {
                Transform parent = transform.parent;
                if (parent != null && (_parentRectTransformSetting == null || tempParentGameobject != parent.gameObject))
                {
                    _parentRectTransformSetting = parent.GetComponent<RectTransformInfo>();
                    tempParentGameobject = parent.gameObject;
                }

                return _parentRectTransformSetting;
            }
        }


        [System.NonSerialized] RectTransformInfo tempParentRectTransformSetting;
        [System.NonSerialized] Vector2 tempOffsetMin;
        [System.NonSerialized] Vector2 tempOffsetMax;
        [System.NonSerialized] Vector2 tempAnchorMin;
        [System.NonSerialized] Vector2 tempAnchorMax;

        /// <summary>
        /// 경고: 이 속성은 중복 값에 최적화되어 있지 않으므로 꽤 느릴 것입니다
        /// Warning: this property is not optimized for duplicate values, so it will be pretty slow
        /// </summary>
        public Rect rect
        {
            get
            {
                RectTransform rectTransform = this.rectTransform;
                if (rectTransform == null)
                    return Rect.zero;

                RectTransformInfo parentRectTransformSetting = this.parentRectTransformSetting;
                if (parentRectTransformSetting == null)
                {
                    Vector2 sizeDelta = rectTransform.sizeDelta;

                    Vector2 position = (Vector2)rectTransform.position - (sizeDelta * rectTransform.pivot);
                    Vector2 size = sizeDelta;

                    return new Rect(position, size);
                }

                Rect parentRect = parentRectTransformSetting.rect;
                Rect localRect = this.localRect;
                return new Rect(localRect.x + parentRect.x, localRect.y + parentRect.y, localRect.width, localRect.height);
            }
            set
            {
                RectTransform rectTransform = this.rectTransform;

                if (rectTransform == null)
                    return;

                Rect parentRect = parentRectTransformSetting.rect;
                localRect = new Rect(value.x - parentRect.x, value.y - parentRect.y, value.width, value.height);
            }
        }
        [System.NonSerialized] Rect _localRect; public Rect localRect
        {
            get
            {
                RectTransform rectTransform = this.rectTransform;
                if (rectTransform == null)
                    return Rect.zero;

                Vector2 offsetMin = rectTransform.offsetMin;
                Vector2 offsetMax = rectTransform.offsetMax;
                Vector2 anchorMin = rectTransform.anchorMin;
                Vector2 anchorMax = rectTransform.anchorMax;


                RectTransformInfo parentRectTransformSetting = this.parentRectTransformSetting;
                /*if (parentRectTransformSetting == tempParentRectTransformSetting && tempOffsetMin == offsetMin && tempOffsetMax == offsetMax && tempAnchorMin == anchorMin && tempAnchorMax == anchorMax)
                    return _localRect;*/

                tempParentRectTransformSetting = parentRectTransformSetting;
                tempOffsetMin = offsetMin;
                tempOffsetMax = offsetMax;
                tempAnchorMin = anchorMin;
                tempAnchorMax = anchorMax;

                

                if (parentRectTransformSetting == null)
                {
                    Vector2 position = Vector2.zero;
                    Vector2 size = rectTransform.sizeDelta;

                    return new Rect(position, size);
                }

                Rect parentLocalRect = parentRectTransformSetting.localRect;
                float x = offsetMin.x + (parentLocalRect.width * anchorMin.x);
                float y = offsetMin.y + (parentLocalRect.height * anchorMin.y);
                float width = offsetMax.x + (parentLocalRect.width * anchorMax.x) - x;
                float height = offsetMax.y + (parentLocalRect.height * anchorMax.y) - y;
                
                _localRect = new Rect(x, y, width, height);
                return _localRect;
            }
            set
            {
                RectTransform rectTransform = this.rectTransform;

                if (rectTransform == null)
                    return;

                Vector2 anchorMin = rectTransform.anchorMin;
                Vector2 anchorMax = rectTransform.anchorMax;

                Rect parentLocalRect = parentRectTransformSetting.localRect;
                float minX = value.x - (parentLocalRect.width * anchorMin.x);
                float minY = value.y - (parentLocalRect.height * anchorMin.y);
                float maxX = value.width - (parentLocalRect.width * anchorMax.x) + value.x;
                float maxY = value.height - (parentLocalRect.height * anchorMax.y) + value.y;
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
                if (rectTransform == null)
                    return Vector2.zero;

                Rect localRect = this.localRect;
                return new Vector2(localRect.width, localRect.height);
            }
            set
            {
                if (rectTransform == null)
                    return;

                Rect localRect = this.localRect;
                this.localRect = new Rect(localRect.x, localRect.y, value.x, value.y);
            }
        }



        [System.NonSerialized] Rect _optimizedRect; public Rect optimizedRect => _optimizedRect;
        [System.NonSerialized] Rect _optimizedLocalRect; public Rect optimizedLocalRect => _optimizedLocalRect;
        [System.NonSerialized] Vector2 _optimizedLocalSize; public Vector2 optimizedLocalSize => _optimizedLocalSize;

        public void OptimizedVariableRefresh()
        {
            _optimizedLocalRect = localRect;
            _optimizedLocalSize = localSize;
            _optimizedRect = optimizedRect;
        }



        public enum AutoSize
        {
            none,
            target,
            child
        }
    }
}