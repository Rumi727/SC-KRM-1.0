using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VerticalLayout), true)]
    public class VerticalLayoutEditor : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            UseProperty("_padding");

            EditorGUILayout.Space();

            UseProperty("_spacing");
        }
    }
}