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

        public override void Rerender() => text.text = (string)queue.Dequeue();
    }
}