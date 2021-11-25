using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Text/Text")]
    [RequireComponent(typeof(Text))]
    public class CustomTextRenderer : CustomAllTextRenderer
    {
        Text text;

        public override void Rerender()
        {
            if (text == null)
                text = GetComponent<Text>();

            text.text = (string)queue.Dequeue();
        }
    }
}