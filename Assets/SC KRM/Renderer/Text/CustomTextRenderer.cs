using K4.Threading;
using SCKRM.Threads;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Text/Text")]
    [RequireComponent(typeof(Text))]
    public sealed class CustomTextRenderer : CustomAllTextRenderer
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

        public override void Refresh()
        {
            string text = GetText();
            if (ThreadManager.isMainThread)
                this.text.text = text;
            else
                K4UnityThreadDispatcher.Execute(() => this.text.text = text);
        }
    }
}