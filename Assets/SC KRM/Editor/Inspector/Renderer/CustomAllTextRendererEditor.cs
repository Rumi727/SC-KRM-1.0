using UnityEngine;
using UnityEditor;
using SCKRM.Renderer;

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
            editor.nameSpace = DrawNameSpace("네임스페이스", editor.nameSpace);
            UseProperty("_path", "이름");

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