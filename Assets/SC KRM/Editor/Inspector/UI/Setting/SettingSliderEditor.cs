using SCKRM.UI.Setting.InputField;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SettingSlider), true)]
    public sealed class SettingSliderEditor : CustomInspectorEditor
    {
        SettingSlider editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SettingSlider)target;
        }

        public override void OnInspectorGUI()
        {
            UseProperty("_saveLoadAttributeName", "세이브 로드 어트리뷰트의 키");
            UseProperty("_variableName", "변수 이름");

            DrawLine();

            UseProperty("_mouseSensitivity", "마우스 감도");

            DrawLine();

            editor.textPlaceHolderNameSpace = DrawNameSpace("플레이스홀더 네임스페이스", editor.textPlaceHolderNameSpace);
            UseProperty("_textPlaceHolderPath", "플레이스홀더 텍스트");

            DrawLine();

            editor.numberPlaceHolderNameSpace = DrawNameSpace("플레이스홀더 네임스페이스 (숫자)", editor.numberPlaceHolderNameSpace);
            UseProperty("_numberPlaceHolderPath", "플레이스홀더 텍스트 (숫자)");

            DrawLine();

            UseProperty("_slider");
            UseProperty("_inputField");
            UseProperty("_placeholder");

            if (editor.propertyInfo != null)
                EditorGUILayout.LabelField(editor.type + " " + editor.propertyInfo.Name + " = " + editor.propertyInfo.GetValue(editor.type));
            else if (editor.fieldInfo != null)
                EditorGUILayout.LabelField(editor.type + " " + editor.fieldInfo.Name + " = " + editor.fieldInfo.GetValue(editor.type));
        }
    }
}