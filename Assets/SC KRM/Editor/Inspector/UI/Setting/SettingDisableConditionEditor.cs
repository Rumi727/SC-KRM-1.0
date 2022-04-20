using SCKRM.SaveLoad;
using SCKRM.UI.Setting;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SettingDisableCondition), true)]
    public sealed class SettingDisableConditionEditor : CustomInspectorEditor
    {
        SettingDisableCondition editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SettingDisableCondition)target;
        }

        public override void OnInspectorGUI()
        {
            if (SettingEditor.saveLoadClassList == null)
                SaveLoadManager.InitializeAll<GeneralSaveLoadAttribute>(out SettingEditor.saveLoadClassList);

            UseProperty("_disableGameObject", "비활성화 할 오브젝트");

            DrawLine();

            SaveLoadClass selectedSaveLoadClass = null;

            string[] fullNames = new string[SettingEditor.saveLoadClassList.Length];
            for (int i = 0; i < SettingEditor.saveLoadClassList.Length; i++)
            {
                SaveLoadClass saveLoadClass = SettingEditor.saveLoadClassList[i];
                string fullName = saveLoadClass.name;
                fullNames[i] = fullName;

                if (fullName == editor.saveLoadAttributeName)
                    selectedSaveLoadClass = saveLoadClass;
            }

            editor.saveLoadAttributeName = DrawStringArray("값을 변경 할 클래스", editor.saveLoadAttributeName, fullNames);

            if (selectedSaveLoadClass != null)
                editor.variableName = DrawStringArray("값을 변경 할 변수", editor.variableName, selectedSaveLoadClass.GetVariableNames());

            DrawLine();

            UseProperty("_reversal", "반전");

            if (editor.propertyInfo != null)
                EditorGUILayout.LabelField(editor.type + " " + editor.propertyInfo.Name + " = " + editor.propertyInfo.GetValue(editor.type));
            else if (editor.fieldInfo != null)
                EditorGUILayout.LabelField(editor.type + " " + editor.fieldInfo.Name + " = " + editor.fieldInfo.GetValue(editor.type));

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}