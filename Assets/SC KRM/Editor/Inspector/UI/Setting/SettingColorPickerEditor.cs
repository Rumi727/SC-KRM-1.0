using SCKRM.UI.Setting;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SettingColorPicker), true)]
    public sealed class SettingColorPickerEditor : SettingEditor
    {
        SettingColorPicker editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SettingColorPicker)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

            UseProperty("_colorPicker");
        }
    }
}