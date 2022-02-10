using SCKRM.UI.Setting;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SettingToggle), true)]
    public sealed class SettingToggleEditor : SettingEditor
    {
        SettingToggle editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SettingToggle)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

            UseProperty("_toggle");
        }
    }
}