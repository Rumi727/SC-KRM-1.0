using SCKRM.UI.Setting;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SettingDropdown))]
    public sealed class SettingDropdownEditor : SettingEditor
    {
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