using SCKRM.UI.Setting;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Setting), true)]
    public class SettingEditor : CustomInspectorEditor
    {
        Setting editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (Setting)target;
        }

        public override void OnInspectorGUI()
        {
            UseProperty("_saveLoadAttributeName", "세이브 로드 어트리뷰트의 키");
            UseProperty("_variableName", "변수 이름");

            if (editor.propertyInfo != null)
                EditorGUILayout.LabelField(editor.type + " " + editor.propertyInfo.Name + " = " + editor.propertyInfo.GetValue(editor.type));
            else if (editor.fieldInfo != null)
                EditorGUILayout.LabelField(editor.type + " " + editor.fieldInfo.Name + " = " + editor.fieldInfo.GetValue(editor.type));
        }
    }
}