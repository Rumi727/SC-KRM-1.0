using SCKRM.UI.Setting.InputField;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SettingToggle), true)]
    public sealed class SettingToggleEditor : CustomInspectorEditor
    {
        SettingToggle editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SettingToggle)target;
        }

        public override void OnInspectorGUI()
        {
            UseProperty("_saveLoadAttributeName", "세이브 로드 어트리뷰트의 키");
            UseProperty("_variableName", "변수 이름");

            DrawLine();

            UseProperty("_toggle");

            if (editor.propertyInfo != null)
                EditorGUILayout.LabelField(editor.type + " " + editor.propertyInfo.Name + " = " + editor.propertyInfo.GetValue(editor.type));
            else if (editor.fieldInfo != null)
                EditorGUILayout.LabelField(editor.type + " " + editor.fieldInfo.Name + " = " + editor.fieldInfo.GetValue(editor.type));
        }
    }
}