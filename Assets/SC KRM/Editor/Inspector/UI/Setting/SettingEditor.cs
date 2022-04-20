using SCKRM.SaveLoad;
using SCKRM.UI.Setting;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Setting), true)]
    public class SettingEditor : UIEditor
    {
        public static SaveLoadClass[] saveLoadClassList;

        Setting editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (Setting)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (saveLoadClassList == null)
                SaveLoadManager.InitializeAll<GeneralSaveLoadAttribute>(out saveLoadClassList);

            DrawLine();

            SaveLoadClass selectedSaveLoadClass = null;

            string[] fullNames = new string[saveLoadClassList.Length];
            for (int i = 0; i < saveLoadClassList.Length; i++)
            {
                SaveLoadClass saveLoadClass = saveLoadClassList[i];
                string fullName = saveLoadClass.name;
                fullNames[i] = fullName;

                if (fullName == editor.saveLoadAttributeName)
                    selectedSaveLoadClass = saveLoadClass;
            }

            editor.saveLoadAttributeName = DrawStringArray("값을 변경 할 클래스", editor.saveLoadAttributeName, fullNames);

            if (selectedSaveLoadClass != null)
                editor.variableName = DrawStringArray("값을 변경 할 변수", editor.variableName, selectedSaveLoadClass.GetVariableNames());

            DrawLine();

            UseProperty("_resetButton", "리셋 버튼");
            UseProperty("_nameText", "이름 텍스트");

            if (editor.propertyInfo != null)
                EditorGUILayout.LabelField(editor.type + " " + editor.propertyInfo.Name + " = " + editor.propertyInfo.GetValue(editor.type));
            else if (editor.fieldInfo != null)
                EditorGUILayout.LabelField(editor.type + " " + editor.fieldInfo.Name + " = " + editor.fieldInfo.GetValue(editor.type));

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}