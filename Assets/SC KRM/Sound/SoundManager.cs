using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.NBS;
using SCKRM.Object;
using SCKRM.ProjectSetting;
using SCKRM.Resource;
using SCKRM.Threads;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SCKRM.Sound
{
    [AddComponentMenu("커널/Audio/오디오 매니저", 0)]
    public sealed class SoundManager : MonoBehaviour
    {
        [ProjectSetting]
        public sealed class Data
        {
            [JsonProperty] public static bool useTempo { get; set; }
        }

        [SerializeField] AudioMixerGroup _audioMixerGroup;
        public AudioMixerGroup audioMixerGroup => _audioMixerGroup;

        public static SoundManager instance { get; private set; }

        public static List<SoundPlayer> soundList { get; } = new List<SoundPlayer>();
        public static List<NBSPlayer> nbsList { get; } = new List<NBSPlayer>();



        public const int maxSoundCount = 256;
        public const int maxNBSCount = 16;



        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        /// <summary>
        /// It should only run on the main thread
        /// </summary>
        public static void SoundRefresh()
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(SoundRefresh));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(SoundRefresh));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(SoundRefresh));



            SoundPlayer[] soundObjects = FindObjectsOfType<SoundPlayer>();
            NBSPlayer[] nbsPlayers = FindObjectsOfType<NBSPlayer>();

            for (int i = 0; i < soundObjects.Length; i++)
            {
                SoundPlayer soundObject = soundObjects[i];
                soundObject.Refesh();
            }

            for (int ii = 0; ii < nbsPlayers.Length; ii++)
            {
                NBSPlayer nbsPlayer = nbsPlayers[ii];
                nbsPlayer.Refesh();
            }
        }

        /// <summary>
        /// 소리를 재생합니다
        /// </summary>
        /// <param name="key">
        /// 오디오 키
        /// </param>
        /// <param name="nameSpace">
        /// 네임스페이스
        /// </param>
        /// <param name="volume">
        /// 볼륨
        /// </param>
        /// <param name="loop">
        /// 반복
        /// </param>
        /// <param name="pitch">
        /// 피치
        /// </param>
        /// <param name="tempo">
        /// 템포
        /// </param>
        /// <param name="panStereo">
        /// 스테레오
        /// </param>
        /// <returns></returns>
        public static SoundPlayer PlaySound(string key, string nameSpace = "", float volume = 1, bool loop = false, float pitch = 1, float tempo = 1, float panStereo = 0) => playSound(key, nameSpace, null, volume, loop, pitch, tempo, panStereo, false, 0, 16, null, 0, 0, 0);

        /// <summary>
        /// 소리를 재생합니다
        /// </summary>
        /// <param name="key">
        /// 오디오 키
        /// </param>
        /// <param name="nameSpace">
        /// 네임스페이스
        /// </param>
        /// <param name="volume">
        /// 볼륨
        /// </param>
        /// <param name="loop">
        /// 반복
        /// </param>
        /// <param name="pitch">
        /// 피치
        /// </param>
        /// <param name="tempo">
        /// 템포
        /// </param>
        /// <param name="panStereo">
        /// 스테레오
        /// </param>
        /// <param name="minDistance">
        /// 최소 거리
        /// </param>
        /// <param name="maxDistance">
        /// 최대 거리
        /// </param>
        /// <param name="parent">
        /// 부모
        /// </param>
        /// <param name="x">
        /// X 좌표
        /// </param>
        /// <param name="y">
        /// Y 좌표
        /// </param>
        /// <param name="z">
        /// Z 좌표
        /// </param>
        /// <returns></returns>
        public static SoundPlayer PlaySound(string key, string nameSpace, float volume, bool loop, float pitch, float tempo, float panStereo, float minDistance = 0, float maxDistance = 16, Transform parent = null, float x = 0, float y = 0, float z = 0) => playSound(key, nameSpace, null, volume, loop, pitch, tempo, panStereo, true, minDistance, maxDistance, parent, x, y, z);

        /// <summary>
        /// 소리를 재생합니다
        /// </summary>
        /// <param name="soundData">
        /// 사운드 데이터
        /// </param>
        /// <param name="volume">
        /// 볼륨
        /// </param>
        /// <param name="loop">
        /// 반복
        /// </param>
        /// <param name="pitch">
        /// 피치
        /// </param>
        /// <param name="tempo">
        /// 템포
        /// </param>
        /// <param name="panStereo">
        /// 스테레오
        /// </param>
        /// <param name="minDistance">
        /// 최소 거리
        /// </param>
        /// <param name="maxDistance">
        /// 최대 거리
        /// </param>
        /// <param name="parent">
        /// 부모
        /// </param>
        /// <param name="x">
        /// X 좌표
        /// </param>
        /// <param name="y">
        /// Y 좌표
        /// </param>
        /// <param name="z">
        /// Z 좌표
        /// </param>
        /// <returns></returns>
        public static SoundPlayer PlaySound(SoundData<SoundMetaData> soundData, float volume = 1, bool loop = false, float pitch = 1, float tempo = 1, float panStereo = 0, float minDistance = 0, float maxDistance = 16, Transform parent = null, float x = 0, float y = 0, float z = 0) => playSound("", "", soundData, volume, loop, pitch, tempo, panStereo, true, minDistance, maxDistance, parent, x, y, z);

        static SoundPlayer playSound(string key, string nameSpace, SoundData<SoundMetaData> soundData, float volume, bool loop, float pitch, float tempo, float panStereo, bool spatial, float minDistance, float maxDistance, Transform parent, float x, float y, float z)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(PlaySound));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(PlaySound));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(PlaySound));

            if (soundList.Count >= maxSoundCount)
            {
                for (int i = 0; i < soundList.Count; i++)
                {
                    SoundPlayer soundObject2 = soundList[i];
                    if (!soundObject2.soundData.isBGM)
                    {
                        soundList[i].Remove();
                        break;
                    }
                }
            }
            
            if (key == null)
                key = "";
            if (nameSpace == null)
                nameSpace = "";
            if (parent == null)
                parent = instance.transform;

            if (nameSpace == "")
                nameSpace = ResourceManager.defaultNameSpace;

            SoundPlayer soundObject = (SoundPlayer)ObjectPoolingSystem.ObjectCreate("sound_manager.sound_object", parent);
            soundObject.key = key;
            soundObject.nameSpace = nameSpace;
            if (soundData != null)
                soundObject.customSoundData = soundData;

            soundObject.volume = volume;
            soundObject.loop = loop;
            soundObject.pitch = pitch;
            soundObject.tempo = tempo;

            soundObject.panStereo = panStereo;
            soundObject.spatial = spatial;

            soundObject.minDistance = minDistance;
            soundObject.maxDistance = maxDistance;

            soundObject.localPosition = new Vector3(x, y, z);

            if (nameSpace == null || nameSpace == "")
                soundObject.name = ResourceManager.defaultNameSpace + ":" + key;
            else
                soundObject.name = nameSpace + ":" + key;

            soundObject.Refesh();
            return soundObject;
        }

        /// <summary>
        /// 소리를 중지합니다
        /// </summary>
        /// <param name="key">
        /// 중지할 오디오 키
        /// </param>
        /// <param name="nameSpace">
        /// 네임스페이스
        /// </param>
        /// <param name="all">
        /// 전부 삭제
        /// </param>
        /// <exception cref="NotMainThreadMethodException"></exception>
        /// <exception cref="NotPlayModeMethodException"></exception>
        /// <exception cref="NotInitialLoadEndMethodException"></exception>
        public static void StopSound(string key, string nameSpace = "", bool all = true)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(StopSound));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(StopSound));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(StopSound));

            if (key == null)
                key = "";
            if (nameSpace == null)
                nameSpace = "";

            if (nameSpace == "")
                nameSpace = ResourceManager.defaultNameSpace;

            for (int i = 0; i < soundList.Count; i++)
            {
                SoundPlayer soundObject = soundList[i];
                if (soundObject.key == key && soundObject.nameSpace == nameSpace)
                {
                    soundObject.Remove();
                    if (!all)
                        return;

                    i--;
                }
            }
        }

        /// <summary>
        /// 모든 오디오를 중지
        /// Stop all audio
        /// </summary>
        public static void StopSoundAll()
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(StopSoundAll));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(StopSoundAll));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(StopSoundAll));

            for (int i = 0; i < soundList.Count; i++)
            {
                SoundPlayer soundObject = soundList[i];
                soundObject.Remove();
                i--;
            }
        }

        /// <summary>
        /// 모든 효과음 또는 BGM 중지
        /// Stop all sounds or bgm
        /// </summary>
        public static void StopSoundAll(bool bgm)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(StopSoundAll));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(StopSoundAll));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(StopSoundAll));

            for (int i = 0; i < soundList.Count; i++)
            {
                SoundPlayer soundObject = soundList[i];
                if (bgm && soundObject.soundData.isBGM)
                {
                    soundObject.Remove();
                    i--;
                }
                else if (!bgm && !soundObject.soundData.isBGM)
                {
                    soundObject.Remove();
                    i--;
                }
            }
        }



        /// <summary>
        /// NBS 재생
        /// NBS Play
        /// </summary>
        public static NBSPlayer PlayNBS(string key, string nameSpace = "", float volume = 1, bool loop = false, float pitch = 1, float tempo = 1, float panStereo = 0, bool spatial = false, float minDistance = 0, float maxDistance = 48, Transform parent = null, float x = 0, float y = 0, float z = 0)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(PlayNBS));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(PlayNBS));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(PlayNBS));

            if (nbsList.Count >= maxNBSCount)
                nbsList[0].Remove();

            if (key == null)
                key = "";
            if (nameSpace == null)
                nameSpace = "";
            if (parent == null)
                parent = instance.transform;

            if (nameSpace == "")
                nameSpace = ResourceManager.defaultNameSpace;

            NBSPlayer nbsPlayer = (NBSPlayer)ObjectPoolingSystem.ObjectCreate("sound_manager.nbs_player", parent);
            nbsPlayer.key = key;
            nbsPlayer.nameSpace = nameSpace;

            nbsPlayer.volume = volume;
            nbsPlayer.loop = loop;
            nbsPlayer.pitch = pitch;
            nbsPlayer.tempo = tempo;

            nbsPlayer.panStereo = panStereo;
            nbsPlayer.spatial = spatial;

            nbsPlayer.minDistance = minDistance;
            nbsPlayer.maxDistance = maxDistance;

            nbsPlayer.localPosition = new Vector3(x, y, z);

            if (nameSpace == null || nameSpace == "")
                nbsPlayer.name = ResourceManager.defaultNameSpace + ":" + key;
            else
                nbsPlayer.name = nameSpace + ":" + key;

            nbsPlayer.Refesh();
            return nbsPlayer;
        }

        /// <summary>
        /// NBS 중지
        /// </summary>
        public static void StopNBS(string key, string nameSpace = "", bool all = true)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(StopNBS));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(StopNBS));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(StopNBS));

            if (key == null)
                key = "";
            if (nameSpace == null)
                nameSpace = "";

            if (nameSpace == "")
                nameSpace = ResourceManager.defaultNameSpace;

            for (int i = 0; i < nbsList.Count; i++)
            {
                NBSPlayer nbsPlayer = nbsList[i];
                if (nbsPlayer.key == key && nbsPlayer.nameSpace == nameSpace)
                {
                    nbsPlayer.Remove();
                    if (!all)
                        return;

                    i--;
                }
            }
        }

        /// <summary>
        /// 모든 NBS 중지
        /// </summary>
        /// <exception cref="NotMainThreadMethodException"></exception>
        /// <exception cref="NotPlayModeMethodException"></exception>
        /// <exception cref="NotInitialLoadEndMethodException"></exception>
        public static void StopNBSAll()
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(StopNBSAll));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(StopNBSAll));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(StopNBSAll));

            for (int i = 0; i < nbsList.Count; i++)
            {
                NBSPlayer nbsPlayer = nbsList[i];
                nbsPlayer.Remove();
                i--;
            }
        }
    }
}