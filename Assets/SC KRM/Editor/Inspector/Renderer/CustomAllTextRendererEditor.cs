using UnityEngine;
using UnityEditor;
using SCKRM.Renderer;
using SCKRM.Resource;
using SCKRM.Language;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CustomAllTextRenderer), true)]
    public sealed class CustomAllTextRendererEditor : CustomInspectorEditor
    {
        CustomAllTextRenderer editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (CustomAllTextRenderer)target;
        }

        public override void OnInspectorGUI()
        {
            string[] languageKeys = ResourceManager.GetLanguageKeys(LanguageManager.SaveData.currentLanguage, editor.nameSpace);
            string[] languageKeysReplace = new string[languageKeys.Length];
            for (int i = 0; i < languageKeysReplace.Length; i++)
                languageKeysReplace[i] = languageKeys[i].Replace(".", "/");

            editor.nameSpace = UsePropertyAndDrawNameSpace("_nameSpace", "네임스페이스", editor.nameSpace);
            UsePropertyAndDrawStringArray("_path", "이름", editor.path.Replace(".", "/"), languageKeysReplace, out int index);
            if (index >= 0)
                editor.path = languageKeys[index];

            EditorGUILayout.Space();

            if (GUI.changed || GUILayout.Button("새로고침"))
            {
                EditorUtility.SetDirty(target);

                if (editor.enabled)
                    editor.Refresh();
            }

            GUI.enabled = true;
        }
    }
}