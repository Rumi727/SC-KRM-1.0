using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HorizontalLayoutSetting), true)]
    public sealed class HorizontalLayoutSettingEditor : CustomInspectorEditor
    {
        HorizontalLayoutSetting editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (HorizontalLayoutSetting)target;
        }

        public override void OnInspectorGUI()
        {
            UseProperty("_custom");
            
            if (!editor.custom)
                UseProperty("_mode");
        }
    }
}