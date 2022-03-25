using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SetSizeAsTargetRectTransform), true)]
    public sealed class SetSizeAsTargetRectTransformEditor : UIEditor
    {
        SetSizeAsTargetRectTransform editor;
        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SetSizeAsTargetRectTransform)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

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
            if (editor.lerp)
                UseProperty("_lerpValue", "애니메이션 속도");
        }
    }
}