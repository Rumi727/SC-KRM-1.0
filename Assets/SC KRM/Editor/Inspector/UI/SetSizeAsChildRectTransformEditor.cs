using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SetSizeAsChildRectTransform), true)]
    public class SetSizeAsChildRectTransformEditor : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            UseProperty("_mode");

            EditorGUILayout.Space();

            UseProperty("_spacing");
            UseProperty("_offset");

            EditorGUILayout.Space();

            UseProperty("_lerp", "애니메이션 사용");
        }
    }
}