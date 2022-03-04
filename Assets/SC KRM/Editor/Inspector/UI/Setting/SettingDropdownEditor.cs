using SCKRM.UI.Setting;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SettingDropdown), true)]
    public sealed class SettingDropdownEditor : SettingEditor
    {
        SettingDropdown editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SettingDropdown)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

            UseProperty("_dropdown");

            DrawLine();

            UseProperty("_onValueChanged");
        }
    }
}