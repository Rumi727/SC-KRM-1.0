using K4.Threading;
using SCKRM.Threads;
using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Text/Text Mesh")]
    [RequireComponent(typeof(TextMesh))]
    public sealed class CustomTextMeshRenderer : CustomAllTextRenderer
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