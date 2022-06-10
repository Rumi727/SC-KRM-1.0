using SCKRM.UI.Setting;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SettingDropdown))]
    public class SettingDropdownEditor : SettingEditor
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