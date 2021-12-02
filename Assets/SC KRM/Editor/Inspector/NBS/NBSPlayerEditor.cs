using SCKRM.NBS;
using SCKRM.Tool;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(NBSPlayer), true)]
    public class NBSPlayerEditor : CustomInspectorEditor
    {
        NBSPlayer editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (NBSPlayer)target;
        }

        public override void OnInspectorGUI() => GUI(editor);

        public static void GUI(NBSPlayer nbsPlayer)
        {
            bool refesh;
            bool pauseToggle;
            bool stop;
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("네임스페이스", GUILayout.ExpandWidth(false));
                nbsPlayer.nameSpace = EditorGUILayout.TextField(nbsPlayer.nameSpace);
                GUILayout.Label("NBS 키", GUILayout.ExpandWidth(false));
                nbsPlayer.key = EditorGUILayout.TextField(nbsPlayer.key);

                refesh = GUILayout.Button("새로고침", GUILayout.ExpandWidth(false));
                string text;
                if (!nbsPlayer.isPaused)
                    text = "일시정지";
                else
                    text = "재생";
                pauseToggle = GUILayout.Button(text, GUILayout.ExpandWidth(false));
                stop = GUILayout.Button("정지", GUILayout.ExpandWidth(false));

                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("볼륨", GUILayout.ExpandWidth(false));
                nbsPlayer.volume = EditorGUILayout.Slider(nbsPlayer.volume, 0, 1);
                GUILayout.Label("반복", GUILayout.ExpandWidth(false));
                nbsPlayer.loop = EditorGUILayout.Toggle(nbsPlayer.loop, GUILayout.Width(15));

                GUILayout.Label("피치", GUILayout.ExpandWidth(false));
                nbsPlayer.pitch = EditorGUILayout.Slider(nbsPlayer.pitch, -3, 3);
                GUILayout.Label("템포", GUILayout.ExpandWidth(false));
                nbsPlayer.tempo = EditorGUILayout.Slider(nbsPlayer.tempo, -3, 3);

                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("3D", GUILayout.ExpandWidth(false));
                nbsPlayer.spatial = EditorGUILayout.Toggle(nbsPlayer.spatial, GUILayout.Width(15));

                if (nbsPlayer.spatial)
                {
                    GUILayout.Label("최소 거리", GUILayout.ExpandWidth(false));
                    nbsPlayer.minDistance = EditorGUILayout.Slider(nbsPlayer.minDistance, 0, 64);
                    GUILayout.Label("최대 거리", GUILayout.ExpandWidth(false));
                    nbsPlayer.maxDistance = EditorGUILayout.Slider(nbsPlayer.maxDistance, 0, 64);

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("위치", GUILayout.ExpandWidth(false));
                    nbsPlayer.localPosition = EditorGUILayout.Vector3Field("", nbsPlayer.localPosition);

                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.Label("스테레오", GUILayout.ExpandWidth(false));
                    nbsPlayer.panStereo = EditorGUILayout.Slider(nbsPlayer.panStereo, -1, 1);

                    EditorGUILayout.EndHorizontal();
                }
            }

            DrawLine(1);

            {
                EditorGUILayout.BeginHorizontal();

                if (nbsPlayer.nbsFile == null)
                {
                    GUILayout.Label("--:-- / --:--", GUILayout.ExpandWidth(false));
                    GUILayout.HorizontalSlider(0, 0, 1);
                }
                else
                {
                    float timer = nbsPlayer.tick * 0.05f + (0.05f - nbsPlayer.timer);
                    float length = nbsPlayer.length * 0.05f;

                    string time = timer.ToTime();
                    string endTime = length.ToTime();

                    if (nbsPlayer.tempo == 0)
                        GUILayout.Label($"--:-- / --:-- ({time} / {endTime})", GUILayout.ExpandWidth(false));
                    else if (nbsPlayer.tempo.Abs() != 1)
                    {
                        string pitchTime = (((nbsPlayer.tick * 0.05f) + (0.05f - nbsPlayer.timer)) * (1 / nbsPlayer.tempo)).ToTime();
                        string pitchEndTime = (nbsPlayer.length * 0.05f * (1 / nbsPlayer.tempo)).ToTime();

                        GUILayout.Label($"{pitchTime} / {pitchEndTime} ({time} / {endTime}) ({nbsPlayer.tick} / {nbsPlayer.length})", GUILayout.ExpandWidth(false));
                    }
                    else
                        GUILayout.Label($"{time} / {endTime} ({nbsPlayer.tick} / {nbsPlayer.length})", GUILayout.ExpandWidth(false));

                    float audioTime = GUILayout.HorizontalSlider(timer, 0, length);
                    if ((timer - audioTime).Abs() >= 0.1f && !refesh)
                        nbsPlayer.tick = Mathf.RoundToInt(audioTime * 20);
                }

                GUILayout.Label($"{nbsPlayer.index} / {nbsPlayer.nbsFile.nbsNotes.Count - 1}", GUILayout.ExpandWidth(false));

                EditorGUILayout.EndHorizontal();
            }

            if (refesh)
                nbsPlayer.Refesh();
            else if (pauseToggle)
                nbsPlayer.isPaused = !nbsPlayer.isPaused;
            else if (stop)
                nbsPlayer.Remove();
        }
    }
}