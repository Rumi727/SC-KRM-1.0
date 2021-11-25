using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Text/TextMeshPro - Text")]
    [RequireComponent(typeof(Text))]
    public class CustomTextMeshProRenderer : CustomAllTextRenderer
    {
        TextMeshPro textMeshPro;

        public override void Rerender()
        {
            if (textMeshPro == null)
                textMeshPro = GetComponent<TextMeshPro>();

            textMeshPro.text = (string)queue.Dequeue();
        }
    }
}