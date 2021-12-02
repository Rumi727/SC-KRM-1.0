using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HorizontalLayout), true)]
    public class HorizontalLayoutEditor : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            UseProperty("_padding");

            EditorGUILayout.Space();

            UseProperty("_spacing");
        }
    }
}