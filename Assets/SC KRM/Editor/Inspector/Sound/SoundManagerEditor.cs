using SCKRM.Sound;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SoundManager), true)]
    public sealed class SoundManagerEditor : CustomInspectorEditor
    {
        SoundManager editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SoundManager)target;
        }

        public override void OnInspectorGUI()
        {
            if (KernelWindowEditor.instance != null)
            {
                KernelWindowEditor.instance.Audio(300);
                DrawLine();
            }

            EditorGUILayout.HelpBox("오디오 믹서 그룹을 넣어주세요", MessageType.None);
            UseProperty("_audioMixerGroup");
        }
    }
}