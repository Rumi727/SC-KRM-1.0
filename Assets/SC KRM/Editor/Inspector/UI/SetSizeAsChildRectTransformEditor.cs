using SCKRM.UI;
using SCKRM.UI.Layout;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SetSizeAsChildRectTransform), true)]
    public sealed class SetSizeAsChildRectTransformEditor : CustomInspectorEditor
    {
        SetSizeAsChildRectTransform editor;
        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SetSizeAsChildRectTransform)target;
        }

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
            if (editor.lerp)
                UseProperty("_lerpValue", "애니메이션 속도");

            EditorGUILayout.Space();

            UseProperty("_ignore", "무시");
        }
    }
}