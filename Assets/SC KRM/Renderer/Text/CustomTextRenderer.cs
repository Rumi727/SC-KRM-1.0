using K4.Threading;
using SCKRM.Threads;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [AddComponentMenu("SC KRM/Renderer/Text/Text")]
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public sealed class CustomTextRenderer : CustomAllTextRenderer
    {
        [SerializeField, HideInInspector] UnityEngine.UI.Text _text; public UnityEngine.UI.Text text => _text = this.GetComponentFieldSave(_text);

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