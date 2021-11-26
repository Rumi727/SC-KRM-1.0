using SCKRM.Input;
using SCKRM.Json;
using SCKRM.NBS;
using SCKRM.Object;
using SCKRM.ProjectSetting;
using SCKRM.Resource;
using SCKRM.Sound;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    public class KernelEditor : EditorWindow
    {
        [MenuItem("커널/커널 설정")]
        public static void ShowWindow() => GetWindow<KernelEditor>(false, "커널");

        bool deleteSafety = true;
        int tabIndex = 0;
        int settingTabIndex = 0;

        Vector2 audioScrollPos = Vector2.zero;

        Vector2 controlSettingScrollPos = Vector2.zero;
        Vector2 objectPoolingSettingScrollPos = Vector2.zero;
        Vector2 audioSettingScrollPos = Vector2.zero;

        void OnGUI()
        {
            {
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                tabIndex = GUILayout.Toolbar(tabIndex, new string[] { "일반", "오디오", "NBS", "프로젝트 설정" }, GUILayout.ExpandWidth(false));

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                CustomInspectorEditor.DrawLine(2);
            }

            switch (tabIndex)
            {
                case 0:
                    Default();
                    break;
                case 1:
                    Audio();
                    break;
                case 2:
                    NBS();
                    break;
                    
                default:
                    Setting();
                    break;
            }
        }



        void Update()
        {
            if (Application.isPlaying && (tabIndex == 0 || tabIndex == 1 || tabIndex == 2))
                Repaint();
        }

        void Default()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("델타 타임 - " + Kernel.deltaTime);
                EditorGUILayout.LabelField("FPS 델타 타임 - " + Kernel.fpsDeltaTime);
                EditorGUILayout.LabelField("스케일 되지 않은 델타 타임 - " + Kernel.unscaledDeltaTime);
                EditorGUILayout.LabelField("스케일 되지 않은 FPS 델타 타임 - " + Kernel.fpsUnscaledDeltaTime);

                CustomInspectorEditor.DrawLine();

                EditorGUILayout.LabelField("FPS - " + Kernel.fps);

                CustomInspectorEditor.DrawLine();

                EditorGUILayout.LabelField("데이터 경로 - " + Kernel.dataPath);
                EditorGUILayout.LabelField("스트리밍 에셋 경로 - " + Kernel.streamingAssetsPath);
                EditorGUILayout.LabelField("영구 데이터 경로 - " + Kernel.persistentDataPath);

                CustomInspectorEditor.DrawLine();

                EditorGUILayout.LabelField("회사 이름 - " + Kernel.companyName);
                EditorGUILayout.LabelField("제품 이름 - " + Kernel.productName);
                EditorGUILayout.LabelField("버전 - " + Kernel.version);

                CustomInspectorEditor.DrawLine();

                EditorGUILayout.LabelField("실행 중인 플랫폼 - " + Kernel.platform);

                CustomInspectorEditor.DrawLine();

                Kernel.gameSpeed = EditorGUILayout.FloatField("게임 속도", Kernel.gameSpeed);

                if (Kernel.isAFK)
                {
                    CustomInspectorEditor.DrawLine();
                    EditorGUILayout.LabelField("AFK 상태");
                }
            }
            else
            {
                EditorGUILayout.LabelField("데이터 경로 - " + Kernel.dataPath);
                EditorGUILayout.LabelField("스트리밍 에셋 경로 - " + Kernel.streamingAssetsPath);
                EditorGUILayout.LabelField("영구 데이터 경로 - " + Kernel.persistentDataPath);

                CustomInspectorEditor.DrawLine();

                EditorGUILayout.LabelField("회사 이름 - " + Kernel.companyName);
                EditorGUILayout.LabelField("제품 이름 - " + Kernel.productName);
                EditorGUILayout.LabelField("버전 - " + Kernel.version);

                CustomInspectorEditor.DrawLine();

                EditorGUILayout.LabelField("실행 중인 플랫폼 - " + Kernel.platform);

                _ = "asdf";
            }
        }

        string audioNameSpace = "";
        string audioKey = "";

        float audioVolume = 1;
        bool audioLoop = false;

        float audioPitch = 1;
        float audioTempo = 1;

        float audioPanStereo = 0;
        bool audioSpatial = false;

        float audioMinDistance = 0;
        float audioMaxDistance = 16;

        Vector3 audioLocalPosition = Vector3.zero;
        void Audio()
        {
            EditorGUILayout.LabelField("제어판", EditorStyles.boldLabel);

            if (!Application.isPlaying)
                GUI.enabled = false;

            {
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("네임스페이스", GUILayout.ExpandWidth(false));
                    audioNameSpace = EditorGUILayout.TextField(audioNameSpace);
                    GUILayout.Label("오디오 키", GUILayout.ExpandWidth(false));
                    audioKey = EditorGUILayout.TextField(audioKey);

                    bool audioPlay = GUILayout.Button("오디오 재생", GUILayout.ExpandWidth(false));
                    if (GUILayout.Button("오디오 정지", GUILayout.ExpandWidth(false)))
                        SoundManager.StopSound(audioKey, audioNameSpace);

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("볼륨", GUILayout.ExpandWidth(false));
                    audioVolume = EditorGUILayout.Slider(audioVolume, 0, 1);
                    GUILayout.Label("반복", GUILayout.ExpandWidth(false));
                    audioLoop = EditorGUILayout.Toggle(audioLoop, GUILayout.Width(15));

                    GUILayout.Label("피치", GUILayout.ExpandWidth(false));
                    audioPitch = EditorGUILayout.Slider(audioPitch, -3, 3);
                    GUILayout.Label("템포", GUILayout.ExpandWidth(false));
                    audioTempo = EditorGUILayout.Slider(audioTempo, -3, 3);

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("3D", GUILayout.ExpandWidth(false));
                    audioSpatial = EditorGUILayout.Toggle(audioSpatial, GUILayout.Width(15));

                    if (audioSpatial)
                    {
                        GUILayout.Label("최소 거리", GUILayout.ExpandWidth(false));
                        audioMinDistance = EditorGUILayout.Slider(audioMinDistance, 0, 64);
                        GUILayout.Label("최대 거리", GUILayout.ExpandWidth(false));
                        audioMaxDistance = EditorGUILayout.Slider(audioMaxDistance, 0, 64);

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();

                        GUILayout.Label("위치", GUILayout.ExpandWidth(false));
                        audioLocalPosition = EditorGUILayout.Vector3Field("", audioLocalPosition);

                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUILayout.Label("스테레오", GUILayout.ExpandWidth(false));
                        audioPanStereo = EditorGUILayout.Slider(audioPanStereo, -1, 1);

                        EditorGUILayout.EndHorizontal();
                    }

                    if (audioPlay)
                        SoundManager.PlaySound(audioKey, audioNameSpace, audioVolume, audioLoop, audioPitch, audioTempo, audioPanStereo, audioSpatial, audioMinDistance, audioMaxDistance);
                }

                {
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("모든 음악 정지", GUILayout.ExpandWidth(false)))
                        SoundManager.StopSoundAll(true);
                    if (GUILayout.Button("모든 효과음 정지", GUILayout.ExpandWidth(false)))
                        SoundManager.StopSoundAll(false);
                    if (GUILayout.Button("모든 소리 정지", GUILayout.ExpandWidth(false)))
                        SoundManager.StopSoundAll();

                    GUILayout.Label($"{SoundManager.soundList.Count} / {SoundManager.maxSoundCount}", GUILayout.ExpandWidth(false));

                    EditorGUILayout.EndHorizontal();
                }
            }

            if (Application.isPlaying)
            {
                CustomInspectorEditor.DrawLine(2);

                EditorGUILayout.LabelField("재생 목록", EditorStyles.boldLabel);
                audioScrollPos = EditorGUILayout.BeginScrollView(audioScrollPos);

                for (int i = SoundManager.soundList.Count - 1; i >= 0; i--)
                {
                    SoundObject soundObject = SoundManager.soundList[i];
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("네임스페이스", GUILayout.ExpandWidth(false));
                    soundObject.nameSpace = EditorGUILayout.TextField(soundObject.nameSpace);
                    GUILayout.Label("오디오 키", GUILayout.ExpandWidth(false));
                    soundObject.key = EditorGUILayout.TextField(soundObject.key);

                    bool refesh = GUILayout.Button("새로고침", GUILayout.ExpandWidth(false));
                    bool stop = GUILayout.Button("정지", GUILayout.ExpandWidth(false));

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("볼륨", GUILayout.ExpandWidth(false));
                    soundObject.volume = EditorGUILayout.Slider(soundObject.volume, 0, 1);
                    GUILayout.Label("반복", GUILayout.ExpandWidth(false));
                    soundObject.loop = EditorGUILayout.Toggle(soundObject.loop, GUILayout.Width(15));

                    int minPitch = -3;
                    if (soundObject.soundMetaData.stream)
                        minPitch = 0;

                    if (soundObject.soundData.isBGM)
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
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("3D", GUILayout.ExpandWidth(false));
                    soundObject.spatial = EditorGUILayout.Toggle(soundObject.spatial, GUILayout.Width(15));

                    if (soundObject.spatial)
                    {
                        GUILayout.Label("최소 거리", GUILayout.ExpandWidth(false));
                        soundObject.minDistance = EditorGUILayout.Slider(soundObject.audioSource.minDistance, 0, 64);
                        GUILayout.Label("최대 거리", GUILayout.ExpandWidth(false));
                        soundObject.maxDistance = EditorGUILayout.Slider(soundObject.audioSource.maxDistance, 0, 64);

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();

                        GUILayout.Label("위치", GUILayout.ExpandWidth(false));
                        soundObject.localPosition = EditorGUILayout.Vector3Field("", soundObject.localPosition);

                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUILayout.Label("스테레오", GUILayout.ExpandWidth(false));
                        soundObject.panStereo = EditorGUILayout.Slider(soundObject.panStereo, -1, 1);

                        EditorGUILayout.EndHorizontal();
                    }

                    CustomInspectorEditor.DrawLine(1);

                    {
                        EditorGUILayout.BeginHorizontal();

                        if (soundObject.soundData == null || soundObject.soundData.sounds == null || soundObject.soundData.sounds.Length <= 0 || soundObject.audioSource.clip == null)
                        {
                            GUILayout.Label("--:-- / --:--", GUILayout.ExpandWidth(false));
                            GUILayout.HorizontalSlider(0, 0, 1);
                        }
                        else
                        {
                            string time = soundObject.audioSource.time.ToTime();
                            string endTime = soundObject.audioSource.clip.length.ToTime();

                            if (soundObject.soundData.isBGM)
                            {
                                if (soundObject.tempo == 0)
                                    GUILayout.Label($"--:-- / --:-- ({time} / {endTime})", GUILayout.ExpandWidth(false));
                                else if (soundObject.tempo.Abs() != 1)
                                {
                                    string pitchTime = (soundObject.audioSource.time * (1 / soundObject.tempo)).ToTime();
                                    string pitchEndTime = (soundObject.audioSource.clip.length * (1 / soundObject.tempo)).ToTime();

                                    GUILayout.Label($"{pitchTime} / {pitchEndTime} ({time} / {endTime})", GUILayout.ExpandWidth(false));
                                }
                                else
                                    GUILayout.Label($"{time} / {endTime}", GUILayout.ExpandWidth(false));
                            }
                            else
                            {
                                if (soundObject.pitch == 0)
                                    GUILayout.Label($"--:-- / --:-- ({time} / {endTime})", GUILayout.ExpandWidth(false));
                                else if (soundObject.pitch.Abs() != 1)
                                {
                                    string pitchTime = (soundObject.audioSource.time * (1 / soundObject.pitch)).ToTime();
                                    string pitchEndTime = (soundObject.audioSource.clip.length * (1 / soundObject.pitch)).ToTime();

                                    GUILayout.Label($"{pitchTime} / {pitchEndTime} ({time} / {endTime})", GUILayout.ExpandWidth(false));
                                }
                                else
                                    GUILayout.Label($"{time} / {endTime}", GUILayout.ExpandWidth(false));
                            }

                            float audioTime = GUILayout.HorizontalSlider(soundObject.audioSource.time, 0, soundObject.audioSource.clip.length);
                            if ((soundObject.audioSource.time - audioTime).Abs() >= 0.1f && !refesh)
                                soundObject.audioSource.time = audioTime;
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    if (refesh)
                        soundObject.Refesh();
                    if (stop)
                        soundObject.Remove();

                    CustomInspectorEditor.DrawLine(2);
                }

                EditorGUILayout.EndScrollView();
            }

            GUI.enabled = true;
        }

        string nbsNameSpace = "";
        string nbsKey = "";

        float nbsVolume = 1;
        bool nbsLoop = false;

        float nbsPitch = 1;
        float nbsTempo = 1;

        float nbsPanStereo = 0;
        bool nbsSpatial = false;

        float nbsMinDistance = 0;
        float nbsMaxDistance = 48;

        Vector3 nbsLocalPosition = Vector3.zero;
        void NBS()
        {
            EditorGUILayout.LabelField("제어판", EditorStyles.boldLabel);

            if (!Application.isPlaying)
                GUI.enabled = false;

            {
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("네임스페이스", GUILayout.ExpandWidth(false));
                    nbsNameSpace = EditorGUILayout.TextField(nbsNameSpace);
                    GUILayout.Label("NBS 키", GUILayout.ExpandWidth(false));
                    nbsKey = EditorGUILayout.TextField(nbsKey);

                    bool nbsPlay = GUILayout.Button("NBS 재생", GUILayout.ExpandWidth(false));
                    if (GUILayout.Button("NBS 정지", GUILayout.ExpandWidth(false)))
                        SoundManager.StopNBS(nbsKey, nbsNameSpace);
                    if (GUILayout.Button("모든 NBS 정지", GUILayout.ExpandWidth(false)))
                        SoundManager.StopNBSAll();

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("볼륨", GUILayout.ExpandWidth(false));
                    nbsVolume = EditorGUILayout.Slider(nbsVolume, 0, 1);
                    GUILayout.Label("반복", GUILayout.ExpandWidth(false));
                    nbsLoop = EditorGUILayout.Toggle(nbsLoop, GUILayout.Width(15));

                    GUILayout.Label("피치", GUILayout.ExpandWidth(false));
                    nbsPitch = EditorGUILayout.Slider(nbsPitch, -3, 3);
                    GUILayout.Label("템포", GUILayout.ExpandWidth(false));
                    nbsTempo = EditorGUILayout.Slider(nbsTempo, -3, 3);

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label($"{SoundManager.nbsList.Count} / {SoundManager.maxNBSCount}", GUILayout.ExpandWidth(false));

                    GUILayout.Label("3D", GUILayout.ExpandWidth(false));
                    nbsSpatial = EditorGUILayout.Toggle(nbsSpatial, GUILayout.Width(15));

                    if (nbsSpatial)
                    {
                        GUILayout.Label("최소 거리", GUILayout.ExpandWidth(false));
                        nbsMinDistance = EditorGUILayout.Slider(nbsMinDistance, 0, 64);
                        GUILayout.Label("최대 거리", GUILayout.ExpandWidth(false));
                        nbsMaxDistance = EditorGUILayout.Slider(nbsMaxDistance, 0, 64);

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();

                        GUILayout.Label("위치", GUILayout.ExpandWidth(false));
                        nbsLocalPosition = EditorGUILayout.Vector3Field("", nbsLocalPosition);

                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUILayout.Label("스테레오", GUILayout.ExpandWidth(false));
                        nbsPanStereo = EditorGUILayout.Slider(nbsPanStereo, -1, 1);

                        EditorGUILayout.EndHorizontal();
                    }

                    if (nbsPlay)
                        SoundManager.PlayNBS(nbsKey, nbsNameSpace, nbsVolume, nbsLoop, nbsPitch, nbsTempo, nbsPanStereo, nbsSpatial, nbsMinDistance, nbsMaxDistance);
                }
            }

            if (Application.isPlaying)
            {
                CustomInspectorEditor.DrawLine(2);

                EditorGUILayout.LabelField("재생 목록", EditorStyles.boldLabel);
                audioScrollPos = EditorGUILayout.BeginScrollView(audioScrollPos);

                for (int i = SoundManager.nbsList.Count - 1; i >= 0; i--)
                {
                    NBSPlayer nbsPlayer = SoundManager.nbsList[i];
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("네임스페이스", GUILayout.ExpandWidth(false));
                    nbsPlayer.nameSpace = EditorGUILayout.TextField(nbsPlayer.nameSpace);
                    GUILayout.Label("NBS 키", GUILayout.ExpandWidth(false));
                    nbsPlayer.key = EditorGUILayout.TextField(nbsPlayer.key);

                    bool refesh = GUILayout.Button("새로고침", GUILayout.ExpandWidth(false));
                    bool stop = GUILayout.Button("정지", GUILayout.ExpandWidth(false));

                    EditorGUILayout.EndHorizontal();
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

                    CustomInspectorEditor.DrawLine(1);

                    {
                        EditorGUILayout.BeginHorizontal();

                        if (nbsPlayer.nbsFile == null)
                        {
                            GUILayout.Label("--:-- / --:--", GUILayout.ExpandWidth(false));
                            GUILayout.HorizontalSlider(0, 0, 1);
                        }
                        else
                        {
                            float timer = nbsPlayer.tick * 0.05f;
                            float length = nbsPlayer.nbsFile.songLength * 0.05f;

                            string time = timer.ToTime();
                            string endTime = length.ToTime();

                            if (nbsPlayer.tempo == 0)
                                GUILayout.Label($"--:-- / --:-- ({time} / {endTime})", GUILayout.ExpandWidth(false));
                            else if (nbsPlayer.tempo.Abs() != 1)
                            {
                                string pitchTime = (nbsPlayer.tick * 0.05f * (1 / nbsPlayer.tempo)).ToTime();
                                string pitchEndTime = (nbsPlayer.nbsFile.songLength * 0.05f * (1 / nbsPlayer.tempo)).ToTime();

                                GUILayout.Label($"{pitchTime} / {pitchEndTime} ({time} / {endTime})", GUILayout.ExpandWidth(false));
                            }
                            else
                                GUILayout.Label($"{time} / {endTime}", GUILayout.ExpandWidth(false));

                            float audioTime = GUILayout.HorizontalSlider(timer, 0, length);
                            if ((timer - audioTime).Abs() >= 0.1f && !refesh)
                                nbsPlayer.tick = Mathf.RoundToInt(audioTime * 20);
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    if (refesh)
                        nbsPlayer.Refesh();
                    if (stop)
                        nbsPlayer.Remove();

                    CustomInspectorEditor.DrawLine(2);
                }

                EditorGUILayout.EndScrollView();
            }

            GUI.enabled = true;
        }

        void Setting()
        {
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                settingTabIndex = GUILayout.Toolbar(settingTabIndex, new string[] { "커널 설정", "조작 키 설정", "오브젝트 풀링 설정", "오디오 설정" }, GUILayout.Width(500));

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                CustomInspectorEditor.DrawLine(2);
            }

            switch (settingTabIndex)
            {
                case 0:
                    KernelSetting();
                    break;
                case 1:
                    ControlSetting();
                    break;
                case 2:
                    ObjectPoolingSetting();
                    break;
                case 3:
                    AudioSetting();
                    break;
            }
        }

        void KernelSetting()
        {
            if (!Application.isPlaying)
                ProjectSettingManager.Load(typeof(Kernel.Data));


            //GUI
            {
                EditorGUILayout.LabelField("커널 설정", EditorStyles.boldLabel);
                EditorGUILayout.Space();
            }

            {
                Kernel.Data.standardFPS = EditorGUILayout.FloatField("기준 프레임", Kernel.Data.standardFPS);

                EditorGUILayout.Space();

                Kernel.Data.notFocusFpsLimit = EditorGUILayout.IntField("포커스가 아닐때 FPS 제한", Kernel.Data.notFocusFpsLimit);
                Kernel.Data.afkFpsLimit = EditorGUILayout.IntField("잠수 상태일때 FPS 제한", Kernel.Data.afkFpsLimit);

                Kernel.Data.afkTimerLimit = EditorGUILayout.FloatField("잠수 상태로 바뀌는 시간", Kernel.Data.afkTimerLimit);

                Kernel.Data.standardFPS = Kernel.Data.standardFPS.Clamp(0.00000001f);
                Kernel.Data.notFocusFpsLimit = Kernel.Data.notFocusFpsLimit.Clamp(-1);
                Kernel.Data.afkFpsLimit = Kernel.Data.afkFpsLimit.Clamp(-1);
                Kernel.Data.afkTimerLimit = Kernel.Data.afkTimerLimit.Clamp(0);
            }

            //플레이 모드가 아니면 변경한 리스트의 데이터를 잃어버리지 않게 파일로 저장
            if (GUI.changed && !Application.isPlaying)
                ProjectSettingManager.Save(typeof(Kernel.Data));
        }

        void ControlSetting()
        {
            if (!Application.isPlaying)
                ProjectSettingManager.Load(typeof(InputManager.Data));

            if (InputManager.Data.controlSettingList == null)
                InputManager.Data.controlSettingList = new Dictionary<string, KeyCode>();



            //GUI
            {
                EditorGUILayout.LabelField("조작 설정", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("안전 삭제 모드 (삭제 할 리스트가 빈 값이 아니면 삭제 금지)", GUILayout.Width(330));
                deleteSafety = EditorGUILayout.Toggle(deleteSafety);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
            }

            //GUI
            {
                EditorGUILayout.BeginHorizontal();

                {
                    if (InputManager.Data.controlSettingList.ContainsKey(""))
                        GUI.enabled = false;

                    if (GUILayout.Button("추가", GUILayout.ExpandWidth(false)))
                        InputManager.Data.controlSettingList.Add("", KeyCode.None);

                    GUI.enabled = true;
                }

                {
                    if (InputManager.Data.controlSettingList.Count <= 0 || ((InputManager.Data.controlSettingList.Keys.ToList()[InputManager.Data.controlSettingList.Count - 1] != "" || InputManager.Data.controlSettingList.Values.ToList()[InputManager.Data.controlSettingList.Count - 1] != KeyCode.None) && deleteSafety))
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)) && InputManager.Data.controlSettingList.Count > 0)
                        InputManager.Data.controlSettingList.Remove(InputManager.Data.controlSettingList.ToList()[InputManager.Data.controlSettingList.Count - 1].Key);

                    GUI.enabled = true;
                }

                {
                    int count = EditorGUILayout.IntField("리스트 길이", InputManager.Data.controlSettingList.Count, GUILayout.Height(21));
                    //변수 설정
                    if (count < 0)
                        count = 0;

                    if (count > InputManager.Data.controlSettingList.Count)
                    {
                        for (int i = InputManager.Data.controlSettingList.Count; i < count; i++)
                        {
                            if (!InputManager.Data.controlSettingList.ContainsKey(""))
                                InputManager.Data.controlSettingList.Add("", KeyCode.None);
                            else
                                count--;
                        }
                    }
                    else if (count < InputManager.Data.controlSettingList.Count)
                    {
                        for (int i = InputManager.Data.controlSettingList.Count - 1; i >= count; i--)
                        {
                            if ((InputManager.Data.controlSettingList.Keys.ToList()[InputManager.Data.controlSettingList.Count - 1] == "" && InputManager.Data.controlSettingList.Values.ToList()[InputManager.Data.controlSettingList.Count - 1] == KeyCode.None) || !deleteSafety)
                                InputManager.Data.controlSettingList.Remove(InputManager.Data.controlSettingList.ToList()[InputManager.Data.controlSettingList.Count - 1].Key);
                            else
                                count++;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();



            {
                controlSettingScrollPos = EditorGUILayout.BeginScrollView(controlSettingScrollPos);

                List<KeyValuePair<string, KeyCode>> controlList = InputManager.Data.controlSettingList.ToList();

                //딕셔너리는 키를 수정할수 없기때문에, 리스트로 분활해줘야함
                List<string> keyList = new List<string>();
                List<KeyCode> valueList = new List<KeyCode>();

                for (int i = 0; i < InputManager.Data.controlSettingList.Count; i++)
                {
                    KeyValuePair<string, KeyCode> item = controlList[i];

                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("키 코드 키", GUILayout.ExpandWidth(false));
                    keyList.Add(EditorGUILayout.TextField(item.Key));

                    GUILayout.Label("키 코드", GUILayout.ExpandWidth(false));
                    valueList.Add((KeyCode)EditorGUILayout.EnumPopup(item.Value));

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();

                //키 중복 감지
                bool overlap = keyList.Count != keyList.Distinct().Count();
                if (!overlap)
                {
                    //리스트 2개를 딕셔너리로 변환
                    InputManager.Data.controlSettingList = keyList.Zip(valueList, (key, value) => new { key, value }).ToDictionary(a => a.key, a => a.value);
                }
            }

            //플레이 모드가 아니면 변경한 리스트의 데이터를 잃어버리지 않게 파일로 저장
            if (GUI.changed && !Application.isPlaying)
                ProjectSettingManager.Save(typeof(InputManager.Data));
        }

        void ObjectPoolingSetting()
        {
            if (!Application.isPlaying)
                ProjectSettingManager.Load(typeof(ObjectPoolingSystem.Data));

            if (ObjectPoolingSystem.Data.prefabList == null)
                ObjectPoolingSystem.Data.prefabList = new Dictionary<string, string>();

            //GUI
            {
                EditorGUILayout.LabelField("오브젝트 리스트", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("안전 삭제 모드 (삭제 할 리스트가 빈 값이 아니면 삭제 금지)", GUILayout.Width(330));
                deleteSafety = EditorGUILayout.Toggle(deleteSafety);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
            }

            //GUI
            {
                EditorGUILayout.BeginHorizontal();

                {
                    if (ObjectPoolingSystem.Data.prefabList.ContainsKey(""))
                        GUI.enabled = false;

                    if (GUILayout.Button("추가", GUILayout.ExpandWidth(false)))
                        ObjectPoolingSystem.Data.prefabList.Add("", "");

                    GUI.enabled = true;
                }

                {
                    if (ObjectPoolingSystem.Data.prefabList.Count <= 0 || ((ObjectPoolingSystem.Data.prefabList.Keys.ToList()[ObjectPoolingSystem.Data.prefabList.Count - 1] != "" || ObjectPoolingSystem.Data.prefabList.Values.ToList()[ObjectPoolingSystem.Data.prefabList.Count - 1] != "") && deleteSafety))
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)))
                        ObjectPoolingSystem.Data.prefabList.Remove(ObjectPoolingSystem.Data.prefabList.ToList()[ObjectPoolingSystem.Data.prefabList.Count - 1].Key);

                    GUI.enabled = true;
                }

                {
                    int count = EditorGUILayout.IntField("리스트 길이", ObjectPoolingSystem.Data.prefabList.Count, GUILayout.Height(21));

                    //변수 설정
                    if (count < 0)
                        count = 0;

                    if (count > ObjectPoolingSystem.Data.prefabList.Count)
                    {
                        for (int i = ObjectPoolingSystem.Data.prefabList.Count; i < count; i++)
                        {
                            if (!ObjectPoolingSystem.Data.prefabList.ContainsKey(""))
                                ObjectPoolingSystem.Data.prefabList.Add("", "");
                            else
                                count--;
                        }
                    }
                    else if (count < ObjectPoolingSystem.Data.prefabList.Count)
                    {
                        for (int i = ObjectPoolingSystem.Data.prefabList.Count - 1; i >= count; i--)
                        {
                            if ((ObjectPoolingSystem.Data.prefabList.Keys.ToList()[ObjectPoolingSystem.Data.prefabList.Count - 1] == "" && ObjectPoolingSystem.Data.prefabList.Values.ToList()[ObjectPoolingSystem.Data.prefabList.Count - 1] == "") || !deleteSafety)
                                ObjectPoolingSystem.Data.prefabList.Remove(ObjectPoolingSystem.Data.prefabList.ToList()[ObjectPoolingSystem.Data.prefabList.Count - 1].Key);
                            else
                                count++;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();



            {
                objectPoolingSettingScrollPos = EditorGUILayout.BeginScrollView(objectPoolingSettingScrollPos);

                //PrefabObject의 <string, string>를 <string, GameObject>로 바꿔서 인스펙터에 보여주고 인스펙터에서 변경한걸 <string, string>로 다시 바꿔서 PrefabObject에 저장
                /*
                 * 왜 이렇게 변환하냐면 JSON에 오브젝트를 저장할려면 우선적으로 string 값같은 경로가 있어야하고
                   인스펙터에서 쉽게 드래그로 오브젝트를 바꾸기 위해선
                   GameObject 형식이여야해서 이런 소용돌이가 나오게 된것
                */
                List<KeyValuePair<string, string>> prefabObject = ObjectPoolingSystem.Data.prefabList.ToList();

                //딕셔너리는 키를 수정할수 없기때문에, 리스트로 분활해줘야함
                List<string> keyList = new List<string>();
                List<string> valueList = new List<string>();

                for (int i = 0; i < ObjectPoolingSystem.Data.prefabList.Count; i++)
                {
                    KeyValuePair<string, string> item = prefabObject[i];

                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("프리팹 키", GUILayout.ExpandWidth(false));
                    string key = EditorGUILayout.TextField(item.Key);

                    GUILayout.Label("프리팹", GUILayout.ExpandWidth(false));
                    //문자열(경로)을 프리팹으로 변환
                    GameObject gameObject = (GameObject)EditorGUILayout.ObjectField("", UnityEngine.Resources.Load<GameObject>(item.Value), typeof(GameObject), true);

                    /*
                     * 변경한 프리팹이 리소스 폴더에 있지 않은경우
                       저장은 되지만 프리팹을 감지할수 없기때문에
                       조건문으로 경고를 표시해주고
                       경로가 중첩되는 현상을 대비하기 위해 경로를 빈 문자열로 변경해줌
                     */
                    string assetsPath = AssetDatabase.GetAssetPath(gameObject);
                    if (assetsPath.Contains("Resources/"))
                    {
                        keyList.Add(key);

                        assetsPath = assetsPath.Substring(assetsPath.IndexOf("Resources/") + 10);
                        assetsPath = assetsPath.Remove(assetsPath.LastIndexOf("."));

                        valueList.Add(assetsPath);

                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        keyList.Add(key);
                        valueList.Add("");

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.HelpBox("'Resources' 폴더에 있는 오브젝트를 넣어주세요", MessageType.Info);
                    }
                }

                EditorGUILayout.EndScrollView();

                //키 중복 감지
                bool overlap = keyList.Count != keyList.Distinct().Count();
                if (!overlap)
                {
                    //리스트 2개를 딕셔너리로 변환
                    ObjectPoolingSystem.Data.prefabList = keyList.Zip(valueList, (key, value) => new { key, value }).ToDictionary(a => a.key, a => a.value);
                }
            }

            //플레이 모드가 아니면 변경한 리스트의 데이터를 잃어버리지 않게 파일로 저장
            if (GUI.changed && !Application.isPlaying)
                ProjectSettingManager.Save(typeof(ObjectPoolingSystem.Data));
        }

        string audioSettingNameSpace = "";
        void AudioSetting()
        {
            //GUI
            {
                EditorGUILayout.LabelField("오디오 설정", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("안전 삭제 모드 (삭제 할 리스트가 빈 값이 아니면 삭제 금지)", GUILayout.Width(330));
                deleteSafety = EditorGUILayout.Toggle(deleteSafety);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
            }

            EditorGUILayout.BeginHorizontal();
            audioSettingNameSpace = EditorGUILayout.TextField("네임스페이스", audioSettingNameSpace);
            string path = KernelMethod.PathCombine(Kernel.streamingAssetsPath, ResourceManager.soundPath.Replace("%NameSpace%", audioSettingNameSpace));

            if (Application.isPlaying)
                GUI.enabled = false;

            if (!Directory.Exists(path))
            {
                if (GUILayout.Button("sounds 폴더 만들기", GUILayout.ExpandWidth(false)))
                {
                    Directory.CreateDirectory(path);
                    AssetDatabase.Refresh();
                }

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                string jsonPath = path + ".json";
                if (!File.Exists(jsonPath))
                {
                    if (GUILayout.Button("sounds.json 파일 만들기", GUILayout.ExpandWidth(false)))
                    {
                        File.WriteAllText(jsonPath, "{}");
                        AssetDatabase.Refresh();
                    }

                    {
                        if (Directory.GetFiles(path).Length > 0 && deleteSafety)
                            GUI.enabled = false;

                        if (GUILayout.Button("sounds 폴더 지우기", GUILayout.ExpandWidth(false)))
                        {
                            Directory.Delete(path, true);
                            File.Delete(path + ".meta");
                            AssetDatabase.Refresh();
                            EditorGUILayout.EndHorizontal();
                            return;
                        }

                        if (!Application.isPlaying)
                            GUI.enabled = true;
                    }

                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    Dictionary<string, SoundData> soundDatas = JsonManager.JsonRead<Dictionary<string, SoundData>>(jsonPath, true);

                    if (soundDatas == null)
                        soundDatas = new Dictionary<string, SoundData>();

                    {
                        if (soundDatas.Count > 0 && deleteSafety)
                            GUI.enabled = false;

                        if (GUILayout.Button("sounds.json 파일 지우기", GUILayout.ExpandWidth(false)))
                        {
                            File.Delete(jsonPath);
                            File.Delete(jsonPath + ".meta");
                            AssetDatabase.Refresh();
                            EditorGUILayout.EndHorizontal();
                            return;
                        }

                        if (!Application.isPlaying)
                            GUI.enabled = true;
                    }

                    {
                        if (Directory.GetFiles(path).Length > 0 && deleteSafety)
                            GUI.enabled = false;

                        if (GUILayout.Button("sounds 폴더 지우기", GUILayout.ExpandWidth(false)))
                        {
                            Directory.Delete(path, true);
                            File.Delete(path + ".meta");
                            AssetDatabase.Refresh();
                            EditorGUILayout.EndHorizontal();
                            return;
                        }

                        if (!Application.isPlaying)
                            GUI.enabled = true;
                    }

                    EditorGUILayout.EndHorizontal();

                    //GUI
                    {
                        EditorGUILayout.BeginHorizontal();

                        {
                            if (soundDatas.ContainsKey(""))
                                GUI.enabled = false;

                            if (GUILayout.Button("추가", GUILayout.ExpandWidth(false)))
                                soundDatas.Add("", new SoundData(SoundCategory.master, "", false, new SoundMetaData[0]));

                            if (!Application.isPlaying)
                                GUI.enabled = true;
                        }

                        {
                            if (soundDatas.Count > 0)
                            {
                                SoundData soundData = soundDatas.Values.ToList()[soundDatas.Count - 1];
                                if ((soundDatas.Keys.ToList()[soundDatas.Count - 1] != "" || soundData.subtitle != "" || soundData.sounds == null || soundData.sounds.Count() > 0) && deleteSafety)
                                    GUI.enabled = false;
                            }
                            else
                                GUI.enabled = false;

                            if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)) && soundDatas.Count > 0)
                                soundDatas.Remove(soundDatas.ToList()[soundDatas.Count - 1].Key);

                            if (!Application.isPlaying)
                                GUI.enabled = true;
                        }

                        {
                            int count = EditorGUILayout.IntField("리스트 길이", soundDatas.Count, GUILayout.Height(21));
                            //변수 설정
                            if (count < 0)
                                count = 0;

                            if (count > soundDatas.Count)
                            {
                                for (int i = soundDatas.Count; i < count; i++)
                                {
                                    if (!soundDatas.ContainsKey(""))
                                        soundDatas.Add("", new SoundData(SoundCategory.master, "", false, new SoundMetaData[0]));
                                    else
                                        count--;
                                }
                            }
                            else if (count < soundDatas.Count)
                            {
                                SoundData soundData = soundDatas.Values.ToList()[soundDatas.Count - 1];
                                for (int i = soundDatas.Count - 1; i >= count; i--)
                                {
                                    if ((soundDatas.Count > 0 && soundDatas.Keys.ToList()[soundDatas.Count - 1] == "" && soundData.subtitle == "" && soundData.sounds != null && soundData.sounds.Count() <= 0) || !deleteSafety)
                                        soundDatas.Remove(soundDatas.ToList()[soundDatas.Count - 1].Key);
                                    else
                                        count++;
                                }
                            }
                        }

                        EditorGUILayout.EndHorizontal();
                    }



                    {
                        audioSettingScrollPos = EditorGUILayout.BeginScrollView(audioSettingScrollPos);

                        //GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill, false, 0, Color.white * 0.25f, 0, 0);

                        List<KeyValuePair<string, SoundData>> tempSoundDatas = soundDatas.ToList();
                        //딕셔너리는 키를 수정할수 없기때문에, 리스트로 분활해줘야함
                        List<string> keyList = new List<string>();
                        List<SoundData> valueList = new List<SoundData>();

                        for (int i = 0; i < soundDatas.Count; i++)
                        {
                            KeyValuePair<string, SoundData> soundData = tempSoundDatas[i];

                            CustomInspectorEditor.DrawLine();

                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(30);

                            GUILayout.Label("오디오 키", GUILayout.ExpandWidth(false));
                            keyList.Add(EditorGUILayout.TextField(soundData.Key));

                            GUILayout.Label("자막", GUILayout.ExpandWidth(false));
                            string subtitle = EditorGUILayout.TextField(soundData.Value.subtitle);

                            GUILayout.Label("BGM", GUILayout.ExpandWidth(false));
                            bool isBGM = EditorGUILayout.Toggle(soundData.Value.isBGM);

                            GUILayout.Label("카테고리", GUILayout.ExpandWidth(false));
                            SoundCategory soundCategory = (SoundCategory)EditorGUILayout.EnumPopup(soundData.Value.category, GUILayout.Width(100));

                            EditorGUILayout.EndHorizontal();


                            //리스트 안의 리스트
                            {
                                List<SoundMetaData> soundMetaDatas = soundData.Value.sounds.ToList();
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    GUILayout.Space(30);

                                    {
                                        if (GUILayout.Button("추가", GUILayout.ExpandWidth(false)))
                                            soundMetaDatas.Add(new SoundMetaData("", false, 1, 1, null));
                                    }

                                    {
                                        if (soundMetaDatas.Count <= 0 || (soundMetaDatas[soundMetaDatas.Count - 1].path != "" && deleteSafety))
                                            GUI.enabled = false;

                                        if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)) && soundMetaDatas.Count > 0)
                                            soundMetaDatas.RemoveAt(soundMetaDatas.Count - 1);

                                        if (!Application.isPlaying)
                                            GUI.enabled = true;
                                    }

                                    EditorGUILayout.EndHorizontal();
                                }



                                {
                                    //scrollPosList[i] = EditorGUILayout.BeginScrollView(scrollPosList[i]);
                                    //GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill, false, 0, Color.white * 0.25f, 0, 0);

                                    for (int j = 0; j < soundMetaDatas.Count; j++)
                                    {
                                        SoundMetaData soundMetaData = soundMetaDatas[j];
                                        string path2 = soundMetaData.path;
                                        bool stream = soundMetaData.stream;
                                        float pitch = soundMetaData.pitch;
                                        float tempo = soundMetaData.tempo;

                                        //CustomInspectorEditor.DrawLine();

                                        EditorGUILayout.BeginHorizontal();
                                        GUILayout.Space(60);

                                        GUILayout.Label("경로", GUILayout.ExpandWidth(false));
                                        path2 = EditorGUILayout.TextField(path2);

                                        if (soundData.Value.isBGM)
                                        {
                                            GUILayout.Label("피치", GUILayout.ExpandWidth(false));
                                            pitch = EditorGUILayout.FloatField(pitch, GUILayout.Width(30)).Clamp(soundMetaData.tempo.Abs() * 0.5f, soundMetaData.tempo.Abs() * 2f);
                                            
                                            GUILayout.Label("템포", GUILayout.ExpandWidth(false));
                                            tempo = EditorGUILayout.FloatField(tempo, GUILayout.Width(30));

                                            if (soundMetaData.stream)
                                                tempo = tempo.Clamp(0);
                                        }
                                        else
                                        {
                                            GUILayout.Label("피치", GUILayout.ExpandWidth(false));
                                            pitch = EditorGUILayout.FloatField(pitch, GUILayout.Width(30));

                                            if (soundMetaData.stream)
                                                pitch = pitch.Clamp(0);
                                        }

                                        GUILayout.Label("스트림", GUILayout.ExpandWidth(false));
                                        stream = EditorGUILayout.Toggle(stream, GUILayout.Width(20));

                                        EditorGUILayout.EndHorizontal();

                                        soundMetaDatas[j] = new SoundMetaData(path2, stream, pitch, tempo, null);
                                    }
                                    valueList.Add(new SoundData(soundCategory, subtitle, isBGM, soundMetaDatas.ToArray()));

                                    //EditorGUILayout.EndScrollView();
                                }
                            }
                        }

                        EditorGUILayout.EndScrollView();

                        //키 중복 감지
                        bool overlap = keyList.Count != keyList.Distinct().Count();
                        if (!overlap)
                        {
                            //리스트 2개를 딕셔너리로 변환
                            soundDatas = keyList.Zip(valueList, (key, value) => new { key, value }).ToDictionary(a => a.key, a => a.value);
                        }
                    }

                    //플레이 모드가 아니면 변경한 리스트의 데이터를 잃어버리지 않게 파일로 저장
                    if (GUI.changed && !Application.isPlaying)
                        File.WriteAllText(jsonPath, JsonManager.ObjectToJson(soundDatas));
                }
            }
        }
    }
}