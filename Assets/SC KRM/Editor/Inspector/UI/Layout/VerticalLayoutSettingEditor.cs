using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VerticalLayoutSetting), true)]
    public sealed class VerticalLayoutSettingEditor : CustomInspectorEditor
    {
        VerticalLayoutSetting editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (VerticalLayoutSetting)target;
        }

        public override void OnInspectorGUI() => UseProperty("_mode");
    }
}