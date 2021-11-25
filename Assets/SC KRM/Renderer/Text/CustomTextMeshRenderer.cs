using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Text/Text Mesh")]
    [RequireComponent(typeof(TextMesh))]
    public class CustomTextMeshRenderer : CustomAllTextRenderer
    {
        TextMesh text;

        public override void Rerender()
        {
            if (text == null)
                text = GetComponent<TextMesh>();

            text.text = (string)queue.Dequeue();
        }
    }
}