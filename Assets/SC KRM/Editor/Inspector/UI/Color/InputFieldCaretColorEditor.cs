using SCKRM.UI;
using SCKRM.UI.Layout;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(InputFieldCaretColor))]
    public sealed class InputFieldCaretColorEditor : UIEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

            UseProperty("_inputField");
        }
    }
}