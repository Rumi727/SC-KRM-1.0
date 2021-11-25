using UnityEngine;
using System;
using SCKRM.Editor;

#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(SetNameAttribute), true)]
    public sealed class SetNameAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label, true);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUIContent GUIContent = new GUIContent();
            GUIContent.text = ((SetNameAttribute)attribute).label;

            EditorGUI.PropertyField(position, property, GUIContent, true);
        }
    }
}
#endif

namespace SCKRM.Editor
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SetNameAttribute : PropertyAttribute
    {
        public readonly string label;

        public SetNameAttribute(string label) => this.label = label;
    }
}