using SCKRM.Sound;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SoundManager), true)]
    public class SoundManagerEditor : CustomInspectorEditor
    {
        SoundManager editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SoundManager)target;
        }

        public override void OnInspectorGUI()
        {
            if (editor.audioMixerGroup == null)
            {
                EditorGUILayout.HelpBox("오디오 믹서 그룹을 넣어주세요", MessageType.Warning);
                UseProperty("_audioMixerGroup");
            }

            KernelWindowEditor.instance.Audio(300);
        }
    }
}