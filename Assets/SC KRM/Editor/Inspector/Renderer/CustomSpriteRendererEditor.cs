using UnityEngine;
using UnityEditor;
using SCKRM.Renderer;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CustomSpriteRenderer), true)]
    public class CustomSpriteRendererEditor : CustomAllSpriteRendererEditor
    {
        CustomSpriteRenderer _editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            _editor = (CustomSpriteRenderer)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            UseProperty("_drawMode");
            if (_editor.drawMode != SpriteDrawMode.Simple)
                UseProperty("_size");
        }
    }
}