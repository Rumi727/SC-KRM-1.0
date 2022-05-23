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
            editor.nameSpace = UsePropertyAndDrawNameSpace("_nameSpace", "네임스페이스", editor.nameSpace);
            editor.text = UsePropertyAndDrawStringArray("_text", "텍스트", editor.text, ResourceManager.GetLanguageKeys(LanguageManager.SaveData.currentLanguage, editor.nameSpace));

            EditorGUILayout.Space();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}