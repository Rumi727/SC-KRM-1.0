using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Text/TextMeshPro - Text (UI)")]
    [RequireComponent(typeof(Text))]
    public class CustomTextMeshProUGUIRenderer : CustomAllTextRenderer
    {
        TextMeshProUGUI textMeshProUGUI;

        public override void Rerender()
        {
            if (textMeshProUGUI == null)
                textMeshProUGUI = GetComponent<TextMeshProUGUI>();

            textMeshProUGUI.text = (string)queue.Dequeue();
        }
    }
}