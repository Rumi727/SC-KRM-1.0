using SCKRM.Resource;
using SCKRM.Sound;
using SCKRM.Tool;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SoundPlayer), true)]
    public sealed class SoundObjectEditor : CustomInspectorEditor
    {
        SoundPlayer editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (SoundPlayer)target;
        }

        public override void OnInspectorGUI() => GUI(editor);

        public static void GUI(SoundPlayer soundObject)
        {
            if (!Application.isPlaying || !Kernel.isInitialLoadEnd || soundObject == null || soundObject.audioSource == null || soundObject.soundData == null || soundObject.metaData == null)
                return;

            bool refesh;
            bool pauseToggle;
            bool stop;
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("네임스페이스", GUILayout.ExpandWidth(false));
                soundObject.nameSpace = DrawNameSpace(soundObject.nameSpace);
                GUILayout.Label("오디오 키", GUILayout.ExpandWidth(false));
                soundObject.key = DrawStringArray(soundObject.key, ResourceManager.GetSoundDataKeys(soundObject.nameSpace));
                GUILayout.Label("오디오 클립", GUILayout.ExpandWidth(false));
                soundObject.selectedAudioClip = (AudioClip)EditorGUILayout.ObjectField(soundObject.selectedAudioClip, typeof(AudioClip), true);

                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("볼륨", GUILayout.ExpandWidth(false));
                soundObject.volume = EditorGUILayout.Slider(soundObject.volume, 0, 1);
                GUILayout.Label("반복", GUILayout.ExpandWidth(false));
                soundObject.loop = EditorGUILayout.Toggle(soundObject.loop, GUILayout.Width(15));

                int minPitch = -3;
                if (soundObject.loadedAudioClip == null && soundObject.metaData.stream)
                    minPitch = 0;

                if (soundObject.loadedAudioClip == null && soundObject.soundData.isBGM && SoundManager.Data.useTempo)
                {
                    GUILayout.Label("피치", GUILayout.ExpandWidth(false));
                    soundObject.pitch = EditorGUILayout.Slider(soundObject.pitch, soundObject.tempo.Abs() * 0.5f, soundObject.tempo.Abs() * 2f);

                    GUILayout.Label("템포", GUILayout.ExpandWidth(false));
                    soundObject.tempo = EditorGUILayout.Slider(soundObject.tempo, minPitch, 3);
                }
                else
                {
                    GUILayout.Label("피치", GUILayout.ExpandWidth(false));
                    soundObject.pitch = EditorGUILayout.Slider(soundObject.pitch, minPitch, 3);
                }

                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("3D", GUILayout.ExpandWidth(false));
                soundObject.spatial = EditorGUILayout.Toggle(soundObject.spatial, GUILayout.Width(15));

                if (soundObject.spatial)
                {
                    GUILayout.Label("최소 거리", GUILayout.ExpandWidth(false));
                    soundObject.minDistance = EditorGUILayout.Slider(soundObject.minDistance, 0, 64);
                    GUILayout.Label("최대 거리", GUILayout.ExpandWidth(false));
                    soundObject.maxDistance = EditorGUILayout.Slider(soundObject.maxDistance, 0, 64);

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("위치", GUILayout.ExpandWidth(false));
                    soundObject.localPosition = EditorGUILayout.Vector3Field("", soundObject.localPosition);
                }
                else
                {
                    GUILayout.Label("스테레오", GUILayout.ExpandWidth(false));
                    soundObject.panStereo = EditorGUILayout.Slider(soundObject.panStereo, -1, 1);
                }

                refesh = GUILayout.Button("새로고침", GUILayout.ExpandWidth(false));
                string text;
                if (!soundObject.isPaused)
                    text = "일시정지";
                else
                    text = "재생";
                pauseToggle = GUILayout.Button(text, GUILayout.ExpandWidth(false));
                stop = GUILayout.Button("정지", GUILayout.ExpandWidth(false));

                EditorGUILayout.EndHorizontal();
            }

            DrawLine(1);

            {
                EditorGUILayout.BeginHorizontal();

                if (soundObject.loadedAudioClip == null && (soundObject.soundData == null || soundObject.soundData.sounds == null || soundObject.soundData.sounds.Length <= 0))
                {
                    GUILayout.Label("--:-- / --:--", GUILayout.ExpandWidth(false));
                    GUILayout.HorizontalSlider(0, 0, 1);
                }
                else
                {
                    string time = soundObject.time.ToTime();
                    string endTime = soundObject.length.ToTime();

                    if (soundObject.speed == 0)
                        GUILayout.Label($"--:-- / --:-- ({time} / {endTime})", GUILayout.ExpandWidth(false));
                    else if (soundObject.speed.Abs() != 1)
                    {
                        string pitchTime = soundObject.realTime.ToTime();
                        string pitchEndTime = soundObject.realLength.ToTime();

                        GUILayout.Label($"{pitchTime} / {pitchEndTime} ({time} / {endTime})", GUILayout.ExpandWidth(false));
                    }
                    else
                        GUILayout.Label($"{time} / {endTime}", GUILayout.ExpandWidth(false));

                    float audioTime = GUILayout.HorizontalSlider(soundObject.time, 0, soundObject.length);
                    if (soundObject.time != audioTime && !refesh)
                        soundObject.time = audioTime;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (refesh)
                soundObject.Refesh();
            else if (pauseToggle)
                soundObject.isPaused = !soundObject.isPaused;
            else if (stop)
                soundObject.Remove();
        }
    }
}