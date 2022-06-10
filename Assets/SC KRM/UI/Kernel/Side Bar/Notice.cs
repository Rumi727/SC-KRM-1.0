using SCKRM.Renderer;
using SCKRM.UI.Layout;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SCKRM.UI.SideBar
{
    [AddComponentMenu("SC KRM/UI/Kerenl/Side Bar/Notice")]
    [RequireComponent(typeof(VerticalLayout), typeof(ChildSizeFitter))]
    public sealed class Notice : UIObjectPooling, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, HideInInspector] VerticalLayout _verticalLayout; public VerticalLayout verticalLayout => _verticalLayout = this.GetComponentFieldSave(_verticalLayout);
        [SerializeField, HideInInspector] ChildSizeFitter _childSizeFitter; public ChildSizeFitter childSizeFitter => _childSizeFitter = this.GetComponentFieldSave(_childSizeFitter);



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
            if (pointer || removeButtonCanvasGroup.gameObject == EventSystem.current.currentSelectedGameObject)
                removeButtonCanvasGroup.alpha = removeButtonCanvasGroup.alpha.MoveTowards(1, 0.2f * Kernel.fpsDeltaTime);
            else
                removeButtonCanvasGroup.alpha = removeButtonCanvasGroup.alpha.MoveTowards(0, 0.2f * Kernel.fpsDeltaTime);
        }

        public override bool Remove()
        {
            if (base.Remove())
                return false;

            if (icon.gameObject.activeSelf)
                icon.gameObject.SetActive(false);

            nameText.path = "";
            infoText.path = "";
            childSizeFitter.min = 40;
            verticalLayout.padding.left = 10;
            removeButtonCanvasGroup.alpha = 0;

            return true;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => pointer = true;

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => pointer = false;
    }
}