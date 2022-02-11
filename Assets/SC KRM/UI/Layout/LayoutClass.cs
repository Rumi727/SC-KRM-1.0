using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCKRM.UI.Layout
{
    public class Layout : MonoBehaviour
    {
        public RectTransform rectTransform => rectTransformInfo.rectTransform;

        [SerializeField, HideInInspector] RectTransformInfo _rectTransformInfo;
        public RectTransformInfo rectTransformInfo
        {
            get
            {
                if (_rectTransformInfo == null)
                    _rectTransformInfo = GetComponent<RectTransformInfo>();

                return _rectTransformInfo;
            }
        }



        [SerializeField, Min(0)] float _spacing;
        public float spacing { get => _spacing; set => _spacing = value; }
        [SerializeField] bool _lerp = true;
        public bool lerp { get => _lerp; set => _lerp = value; }


        [SerializeField] RectTransform[] _ignore = new RectTransform[0];
        public RectTransform[] ignore { get => _ignore; set => _ignore = value; }


        public List<RectTransform> childRectTransforms { get; } = new List<RectTransform>();
        [System.NonSerialized] int tempChildCount = -1;
        protected virtual void Update()
        {

            {
                {
                    bool update = true;
#if UNITY_EDITOR
                    if (tempChildCount != transform.childCount || !Application.isPlaying)
                    {
                        SetChild();
                        tempChildCount = transform.childCount;
                    }
                    else
                        update = false;
#else
                    if (tempChildCount != transform.childCount)
                    {
                        SetChild();
                        tempChildCount = transform.childCount;
                    }
                    else
                        update = false;
#endif
                    bool update2 = false;
                    for (int i = 0; i < childRectTransforms.Count; i++)
                    {
                        RectTransform rectTransform = childRectTransforms[i];
                        if (i != rectTransform.GetSiblingIndex())
                        {
                            SetChild();
                            update = true;
                            update2 = true;

                            break;
                        }
                    }

                    if (!update2)
                        update = false;

#if UNITY_EDITOR
                    if (!update && !lerp && Application.isPlaying)
#else
                    if (!update && !lerp)
#endif
                        return;
                }
            }
        }

        protected virtual void SetChild()
        {
            childRectTransforms.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                if (childTransform != ignore.Contains(childTransform))
                    childRectTransforms.Add(childTransform.GetComponent<RectTransform>());
            }
        }
    }

    public class LayoutSetting<ChildSettingComponent> : Layout
    {
        public List<ChildSettingComponent> childSettingComponents { get; } = new List<ChildSettingComponent>();

        protected override void SetChild()
        {
            childRectTransforms.Clear();
            childSettingComponents.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                if (childTransform != ignore.Contains(childTransform))
                {
                    childRectTransforms.Add(childTransform.GetComponent<RectTransform>());
                    childSettingComponents.Add(childTransform.GetComponent<ChildSettingComponent>());
                }
            }
        }
    }
}