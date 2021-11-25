using UnityEngine;
using UnityEditor;
using SCKRM.Renderer;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CustomAllTextRenderer), true)]
    public class CustomAllTextRendererEditor : CustomInspectorEditor
    {
        CustomAllTextRenderer _editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            _editor = (CustomAllTextRenderer)target;
        }

        public override void OnInspectorGUI()
        {
            UseProperty("_nameSpace", "네임스페이스");
            UseProperty("_path", "이름");

            EditorGUILayout.Space();

            if (GUI.changed || GUILayout.Button("새로고침"))
            {
                EditorUtility.SetDirty(target);
                _editor.ResourceReload();
                _editor.Rerender();
            }

            GUI.enabled = true;
        }
    }
}