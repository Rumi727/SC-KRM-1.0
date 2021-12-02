using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Text/Text Mesh")]
    [RequireComponent(typeof(TextMesh))]
    public class CustomTextMeshRenderer : CustomAllTextRenderer
    {
        [SerializeField, HideInInspector] TextMesh _text;
        public TextMesh text
        {
            get
            {
                if (_text == null)
                    _text = GetComponent<TextMesh>();

                return _text;
            }
        }

        public override void Rerender()
        {
            while (queue.TryDequeue(out object text))
                this.text.text = (string)text;
        }
    }
}