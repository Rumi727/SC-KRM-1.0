using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VerticalLayout), true)]
    public sealed class VerticalLayoutEditor : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            UseProperty("_padding");

            EditorGUILayout.Space();

            UseProperty("_spacing");

            EditorGUILayout.Space();

            UseProperty("_lerp", "애니메이션 사용");
        }
    }
}