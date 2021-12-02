using TMPro;
using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Text/Text Mesh Pro")]
    [RequireComponent(typeof(TMP_Text))]
    public class CustomTextMeshProRenderer : CustomAllTextRenderer
    {
        [SerializeField, HideInInspector] TMP_Text _textMeshPro;
        public TMP_Text textMeshPro
        {
            get
            {
                if (_textMeshPro == null)
                    _textMeshPro = GetComponent<TMP_Text>();

                return _textMeshPro;
            }
        }

        public override void Rerender()
        {
            while (queue.TryDequeue(out object text))
                textMeshPro.text = (string)text;
        }
    }
}