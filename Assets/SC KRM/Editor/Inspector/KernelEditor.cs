using SCKRM.Sound;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Kernel), true)]
    public class KernelEditor : CustomInspectorEditor
    {
        Kernel editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (Kernel)target;
        }

        public override void OnInspectorGUI()
        {
            if (editor.splashScreenBackground == null)
            {
                EditorGUILayout.HelpBox("로딩이 끝날때 페이드 아웃되는 배경 이미지를 넣어주세요", MessageType.Warning);
                UseProperty("_splashScreenBackground");
            }

            KernelWindowEditor.instance.Default();
        }
    }
}