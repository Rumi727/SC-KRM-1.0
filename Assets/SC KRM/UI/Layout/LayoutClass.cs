using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.Layout
{
    public abstract class LayoutChild : UIAniLayout
    {
        [SerializeField, Min(0)] float _spacing;
        public float spacing { get => _spacing; set => _spacing = value; }


        [SerializeField] RectTransform[] _ignore = new RectTransform[0];
        public RectTransform[] ignore { get => _ignore; set => _ignore = value; }


        public List<RectTransform> childRectTransforms { get; } = new List<RectTransform>();

        /// <summary>
        /// Please put base.LayoutRefresh() when overriding
        /// </summary>
        public override void LayoutRefresh()
        {
            if ((transform.childCount - ignore.Length) != childRectTransforms.Count || !Kernel.isPlaying)
                SetChild();

            int childCount = transform.childCount;
            for (int i = 0; i < (childCount - ignore.Length); i++)
            {
                if (transform.GetChild(i) != childRectTransforms[i])
                {
                    SetChild();
                    break;
                }
            }
        }

        protected virtual void SetChild()
        {
            childRectTransforms.Clear();

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                if (childTransform != ignore.Contains(childTransform))
                    childRectTransforms.Add(childTransform.GetComponent<RectTransform>());
            }
        }
    }

    public abstract class LayoutChildSetting<ChildSettingComponent> : LayoutChild where ChildSettingComponent : Component
    {
        public List<ChildSettingComponent> childSettingComponents { get; } = new List<ChildSettingComponent>();

        protected override void SetChild()
        {
            childRectTransforms.Clear();
            childSettingComponents.Clear();

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
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