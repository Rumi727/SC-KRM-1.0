using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CanvasSetting), true)]
    public class CanvasSettingEditor : CustomInspectorEditor
    {
        CanvasSetting editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (CanvasSetting)target;
        }

        public override void OnInspectorGUI()
        {
            UseProperty("_customRenderMode", "커스텀 렌더 모드");

            if (!editor.customRenderMode)
                UseProperty("_worldRenderMode", "월드 렌더 모드");
        }
    }
}