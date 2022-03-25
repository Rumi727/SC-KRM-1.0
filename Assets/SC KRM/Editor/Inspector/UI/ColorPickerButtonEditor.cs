using SCKRM.UI;
using SCKRM.UI.Layout;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ColorPickerButton), true)]
    public sealed class ColorPickerButtonEditor : UIAniEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

            UseProperty("colorPickerMask");
            UseProperty("colorPickerRectTransform");
            UseProperty("colorPicker");
        }
    }
}