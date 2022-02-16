using SCKRM.Object;
using SCKRM.Renderer;
using SCKRM.Tool;
using SCKRM.UI.Layout;
using SCKRM.UI.StatusBar;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SCKRM.UI.SideBar
{
    [RequireComponent(typeof(VerticalLayout), typeof(SetSizeAsChildRectTransform))]
    public class Notice : ObjectPoolingUI, IPointerEnterHandler, IPointerExitHandler
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



        [SerializeField] CanvasGroup _removeButtonCanvasGroup;
        public CanvasGroup removeButtonCanvasGroup => _removeButtonCanvasGroup;



        [SerializeField] CustomAllSpriteRenderer _icon;
        public CustomAllSpriteRenderer icon => _icon;

        [SerializeField] CustomAllTextRenderer _nameText;
        public CustomAllTextRenderer nameText => _nameText;

        [SerializeField] CustomAllTextRenderer _infoText;
        public CustomAllTextRenderer infoText => _infoText;

        bool pointer = false;
        void Update()
        {
            if (pointer || removeButtonCanvasGroup.gameObject == StatusBarManager.instance.eventSystem.currentSelectedGameObject)
                removeButtonCanvasGroup.alpha = removeButtonCanvasGroup.alpha.MoveTowards(1, 0.2f * Kernel.fpsDeltaTime);
            else
                removeButtonCanvasGroup.alpha = removeButtonCanvasGroup.alpha.MoveTowards(0, 0.2f * Kernel.fpsDeltaTime);
        }

        public override void Remove()
        {
            base.Remove();

            if (icon.gameObject.activeSelf)
                icon.gameObject.SetActive(false);

            nameText.path = "";
            infoText.path = "";
            setSizeAsChildRectTransform.min = 40;
            verticalLayout.padding.left = 10;
            removeButtonCanvasGroup.alpha = 0;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => pointer = true;

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => pointer = false;
    }
}