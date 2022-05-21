using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SCKRM.Tooltip
{
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] string _nameSpace; public string nameSpace { get => _nameSpace; set => _nameSpace = value; }
        [SerializeField] string _tooltip; public string tooltip { get => _tooltip; set => _tooltip = value; }



        public void OnPointerEnter(PointerEventData eventData) => TooltipManager.Show(tooltip, nameSpace);
        public void OnPointerExit(PointerEventData eventData) => TooltipManager.Hide();
    }
}
