using SCKRM.Object;
using SCKRM.Renderer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SCKRM.UI.SideBar
{
    [RequireComponent(typeof(VerticalLayout), typeof(SetSizeAsChildRectTransform))]
    public class Notice : ObjectPooling
    {
        [SerializeField, HideInInspector] VerticalLayout _verticalLayout;
        public VerticalLayout verticalLayout
        {
            get
            {
                if (_verticalLayout == null)
                    _verticalLayout = GetComponent<VerticalLayout>();

                return _verticalLayout;
            }
        }
        [SerializeField, HideInInspector] SetSizeAsChildRectTransform _setSizeAsChildRectTransform;
        public SetSizeAsChildRectTransform setSizeAsChildRectTransform
        {
            get
            {
                if (_setSizeAsChildRectTransform == null)
                    _setSizeAsChildRectTransform = GetComponent<SetSizeAsChildRectTransform>();

                return _setSizeAsChildRectTransform;
            }
        }



        [SerializeField] CustomAllSpriteRenderer _icon;
        public CustomAllSpriteRenderer icon => _icon;

        [SerializeField] CustomAllTextRenderer _nameText;
        public CustomAllTextRenderer nameText => _nameText;

        [SerializeField] CustomAllTextRenderer _infoText;
        public CustomAllTextRenderer infoText => _infoText;

        public override void Remove()
        {
            base.Remove();

            if (icon.gameObject.activeSelf)
                icon.gameObject.SetActive(false);

            nameText.path = "";
            infoText.path = "";
            setSizeAsChildRectTransform.min = 40;
            verticalLayout.padding.left = 10;
        }
    }
}