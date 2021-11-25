using TMPro;
using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Text/Text Mesh Pro")]
    [RequireComponent(typeof(TMP_Text))]
    public class CustomTextMeshProRenderer : CustomAllTextRenderer
    {
        TMP_Text textMeshPro;

        public override void Rerender()
        {
            if (textMeshPro == null)
                textMeshPro = GetComponent<TextMeshPro>();

            textMeshPro.text = (string)queue.Dequeue();
        }
    }
}