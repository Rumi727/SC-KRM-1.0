using SCKRM.Camera;
using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CameraSetting))]
    public class CameraSettingEditor : CustomInspectorEditor
    {

        [System.NonSerialized] CameraSetting editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (CameraSetting)target;
        }


        public override void OnInspectorGUI()
        {
            UseProperty("_customSetting", "커스텀 설정");

            if (!editor.customSetting)
            {
                UseProperty("_normalizedViewPortRect");
                editor.camera.rect = (editor.normalizedViewPortRect = editor.normalizedViewPortRect);
            }
        }
    }
}