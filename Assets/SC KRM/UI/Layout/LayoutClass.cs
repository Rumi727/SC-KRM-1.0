using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.Layout
{
    public abstract class LayoutChild : UIAni
    {
        [SerializeField, Min(0)] float _spacing;
        public float spacing { get => _spacing; set => _spacing = value; }


        [SerializeField] RectTransform[] _ignore = new RectTransform[0];
        public RectTransform[] ignore { get => _ignore; set => _ignore = value; }


        public List<RectTransform> childRectTransforms { get; } = new List<RectTransform>();

        protected override void Update()
        {
            base.Update();

            {
                {
                    bool update = true;
#if UNITY_EDITOR
                    if (transform.childCount != childRectTransforms.Count || !Application.isPlaying)
                        SetChild();
                    else
                        update = false;
#else
                    if (transform.childCount != childRectTransforms.Count)
                        SetChild();
                    else
                        update = false;
#endif
                    bool update2 = false;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i) != childRectTransforms[i])
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

    public abstract class LayoutChildSetting<ChildSettingComponent> : LayoutChild
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