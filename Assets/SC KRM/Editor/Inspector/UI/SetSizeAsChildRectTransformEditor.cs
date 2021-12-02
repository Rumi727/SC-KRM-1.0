using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SetSizeAsChildRectTransform), true)]
    public sealed class SetSizeAsChildRectTransformEditor : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            UseProperty("_mode");

            EditorGUILayout.Space();

            UseProperty("_spacing");
            UseProperty("_offset");
            UseProperty("_min");
            UseProperty("_max");

            EditorGUILayout.Space();

            UseProperty("_lerp", "애니메이션 사용");

            EditorGUILayout.Space();

            UseProperty("_ignore", "무시");
        }
    }
}