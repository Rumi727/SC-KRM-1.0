using SCKRM.UI.Setting;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SettingSlider), true)]
    public sealed class SettingSliderEditor : SettingInputFieldEditor
    {
        SettingSlider editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SettingSlider)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UseProperty("_placeholder");
        }
    }
}