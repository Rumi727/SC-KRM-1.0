using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RectTransformInfo), true)]
    public class RectTransformInfoEditor : CustomInspectorEditor
    {
        RectTransformInfo editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (RectTransformInfo)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField($"Rect - {editor.rect.x} {editor.rect.y} {editor.rect.width} {editor.rect.height}");
            EditorGUILayout.LabelField($"Local Rect - {editor.localRect.x} {editor.localRect.y} {editor.localRect.width} {editor.localRect.height}");
            EditorGUILayout.LabelField($"Size - {editor.localSize.x} {editor.localSize.y}");
        }
    }
}