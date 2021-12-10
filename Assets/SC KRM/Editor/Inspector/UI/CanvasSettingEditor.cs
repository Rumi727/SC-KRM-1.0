using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CanvasSetting), true)]
    public sealed class CanvasSettingEditor : CustomInspectorEditor
    {
        CanvasSetting editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (CanvasSetting)target;
        }

        public override void OnInspectorGUI()
        {
            UseProperty("_customSetting", "커스텀 설정");

            if (!editor.customSetting)
                UseProperty("_worldRenderMode", "월드 렌더 모드");
        }
    }
}