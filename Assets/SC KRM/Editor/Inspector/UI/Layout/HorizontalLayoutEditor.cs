using SCKRM.UI;
using SCKRM.UI.Layout;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HorizontalLayout), true)]
    public sealed class HorizontalLayoutEditor : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            UseProperty("_padding");

            EditorGUILayout.Space();

            UseProperty("_spacing");

            EditorGUILayout.Space();

            UseProperty("_lerp", "애니메이션 사용");

            EditorGUILayout.Space();

            UseProperty("_ignore", "무시");
        }
    }
}