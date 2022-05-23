using SCKRM.Renderer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SCKRM.Tooltip
{
    public sealed class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] string _nameSpace; public string nameSpace { get => _nameSpace; set => _nameSpace = value; }
        [SerializeField] string _text; public string text { get => _text; set => _text = value; }

        public void OnPointerEnter(PointerEventData eventData) => TooltipManager.Show(text, nameSpace);
        public void OnPointerExit(PointerEventData eventData) => TooltipManager.Hide();
    }
}
