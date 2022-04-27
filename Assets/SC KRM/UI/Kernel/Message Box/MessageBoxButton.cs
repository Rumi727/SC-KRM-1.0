using SCKRM.Object;
using SCKRM.Renderer;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.MessageBox
{
    public class MessageBoxButton : ObjectPooling
    {
        [SerializeField] Button _button; public Button button => _button;
        [SerializeField] CustomAllTextRenderer _text; public CustomAllTextRenderer text => _text;

        public int index { get; set; }
    }
}
