using SCKRM.Sound;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Kernel), true)]
    public sealed class KernelEditor : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            KernelWindowEditor.Default();
            DrawLine();

            EditorGUILayout.HelpBox("로딩이 끝날때 페이드 아웃되는 배경 이미지를 넣어주세요", MessageType.None);
            UseProperty("_splashScreenBackground");
        }
    }
}