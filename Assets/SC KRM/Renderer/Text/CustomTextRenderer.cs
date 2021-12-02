using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Text/Text")]
    [RequireComponent(typeof(Text))]
    public class CustomTextRenderer : CustomAllTextRenderer
    {
        [SerializeField, HideInInspector] Text _text;
        public Text text
        {
            get
            {
                if (_text == null)
                    _text = GetComponent<Text>();

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