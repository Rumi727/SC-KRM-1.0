using UnityEngine;
using UnityEditor;
using SCKRM.Resource;
using SCKRM.Language;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Tooltip.Tooltip), true)]
    public sealed class TooltipEditor : CustomInspectorEditor
    {
        Tooltip.Tooltip editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (Tooltip.Tooltip)target;
        }

        public override void OnInspectorGUI()
        {
            string[] languageKeys = ResourceManager.GetLanguageKeys(LanguageManager.SaveData.currentLanguage, editor.nameSpace);
            string[] languageKeysReplace = new string[languageKeys.Length];
            for (int i = 0; i < languageKeysReplace.Length; i++)
                languageKeysReplace[i] = languageKeys[i].Replace(".", "/");

            editor.nameSpace = UsePropertyAndDrawNameSpace("_nameSpace", "네임스페이스", editor.nameSpace);
            UsePropertyAndDrawStringArray("_text", "이름", editor.text.Replace(".", "/"), languageKeysReplace, out int index);
            if (index >= 0)
                editor.text = languageKeys[index];

            EditorGUILayout.Space();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}