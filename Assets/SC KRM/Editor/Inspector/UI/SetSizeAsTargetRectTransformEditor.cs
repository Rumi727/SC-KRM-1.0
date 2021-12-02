using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SetSizeAsTargetRectTransform), true)]
    public sealed class SetSizeAsTargetRectTransformEditor : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            UseProperty("_targetRectTransform", "대상");

            EditorGUILayout.Space();

            UseProperty("_xSize", "X 크기 변경");
            UseProperty("_ySize", "Y 크기 변경");

            EditorGUILayout.Space();

            UseProperty("_offset");
            UseProperty("_min");
            UseProperty("_max");

            EditorGUILayout.Space();

            UseProperty("_lerp", "애니메이션 사용");
        }
    }
}