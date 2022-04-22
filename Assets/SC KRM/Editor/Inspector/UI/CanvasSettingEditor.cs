using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CanvasSetting))]
    public sealed class CanvasSettingEditor : UIEditor
    {
        [System.NonSerialized] CanvasSetting editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (CanvasSetting)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

            UseProperty("_customSetting", "커스텀 설정");

            if (!editor.customSetting)
                UseProperty("_worldRenderMode", "월드 렌더 모드");

            if (!editor.customSetting || editor.canvas.renderMode != UnityEngine.RenderMode.WorldSpace)
            {
                DrawLine();

                UseProperty("_customGuiSize", "GUI 크기 커스텀");
            }
        }
    }
}