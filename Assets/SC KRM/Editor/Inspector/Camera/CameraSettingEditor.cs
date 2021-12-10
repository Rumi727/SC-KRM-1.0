using SCKRM.Camera;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CameraSetting), true)]
    public sealed class CameraSettingEditor : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            UseProperty("_customSetting", "커스텀 설정");
        }
    }
}