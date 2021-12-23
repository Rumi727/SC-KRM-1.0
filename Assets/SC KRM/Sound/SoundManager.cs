using Cysharp.Threading.Tasks;
using SCKRM.NBS;
using SCKRM.Object;
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
        [SerializeField] AudioMixerGroup _audioMixerGroup;
        public AudioMixerGroup audioMixerGroup => _audioMixerGroup;

        public static SoundManager instance { get; private set; }

        public static List<SoundObject> soundList { get; } = new List<SoundObject>();
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
        public static async UniTask SoundRefresh()
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(SoundRefresh));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(SoundRefresh));
#endif



            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = "notice.running_task.audio_refresh.name";
            threadMetaData.autoRemoveDisable = true;

            ThreadManager.runningThreads.Add(threadMetaData);

            SoundObject[] soundObjects = FindObjectsOfType<SoundObject>();
            NBSPlayer[] nbsPlayers = FindObjectsOfType<NBSPlayer>();

            int i;
            int length = soundObjects.Length + nbsPlayers.Length;
            if (length == 0)
            {
                threadMetaData.progress = 1;
                threadMetaData.autoRemoveDisable = false;
                return;
            }

            for (i = 0; i < soundObjects.Length; i++)
            {
                SoundObject soundObject = soundObjects[i];
                soundObject.Refesh();

                threadMetaData.progress = (float)i / (length - 1);

                await UniTask.DelayFrame(1);
            }

            for (int ii = 0; ii < nbsPlayers.Length; ii++)
            {
                threadMetaData.progress = (float)(i + ii) / (length - 1);

                NBSPlayer nbsPlayer = nbsPlayers[ii];
                nbsPlayer.Refesh();

                await UniTask.DelayFrame(1);
            }

            threadMetaData.autoRemoveDisable = false;
        }

        public static SoundObject PlaySound(string key, string nameSpace = "", float volume = 1, bool loop = false, float pitch = 1, float tempo = 1, float panStereo = 0, bool spatial = false, float minDistance = 0, float maxDistance = 16, Transform parent = null, float x = 0, float y = 0, float z = 0)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(PlaySound));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(PlaySound));
#endif

            if (soundList.Count >= maxSoundCount)
            {
                for (int i = 0; i < soundList.Count; i++)
                {
                    SoundObject soundObject2 = soundList[i];
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

            SoundObject soundObject = ObjectPoolingSystem.ObjectCreate("sound_manager.sound_object", parent).GetComponent<SoundObject>();
            soundObject.key = key;
            soundObject.nameSpace = nameSpace;

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

        public static void StopSound(string key, string nameSpace = "", bool all = true)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(StopSound));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(StopSound));
#endif

            if (key == null)
                key = "";
            if (nameSpace == null)
                nameSpace = "";

            if (nameSpace == "")
                nameSpace = ResourceManager.defaultNameSpace;

            for (int i = 0; i < soundList.Count; i++)
            {
                SoundObject soundObject = soundList[i];
                if (soundObject.key == key && soundObject.nameSpace == nameSpace)
                {
                    soundObject.Remove();
                    if (!all)
                        return;

                    i--;
                }
            }
        }

        public static void StopSoundAll()
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(StopSoundAll));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(StopSoundAll));
#endif

            for (int i = 0; i < soundList.Count; i++)
            {
                SoundObject soundObject = soundList[i];
                soundObject.Remove();
                i--;
            }
        }

        public static void StopSoundAll(bool bgm)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(StopSoundAll));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(StopSoundAll));
#endif

            for (int i = 0; i < soundList.Count; i++)
            {
                SoundObject soundObject = soundList[i];
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



        public static NBSPlayer PlayNBS(string key, string nameSpace = "", float volume = 1, bool loop = false, float pitch = 1, float tempo = 1, float panStereo = 0, bool spatial = false, float minDistance = 0, float maxDistance = 48, Transform parent = null, float x = 0, float y = 0, float z = 0)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(PlayNBS));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(PlayNBS));
#endif

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

            NBSPlayer nbsPlayer = ObjectPoolingSystem.ObjectCreate("sound_manager.nbs_player", parent).GetComponent<NBSPlayer>();
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

        public static void StopNBS(string key, string nameSpace = "", bool all = true)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(StopNBS));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(StopNBS));
#endif

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

        public static void StopNBSAll()
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(StopNBSAll));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(StopNBSAll));
#endif

            for (int i = 0; i < nbsList.Count; i++)
            {
                NBSPlayer nbsPlayer = nbsList[i];
                nbsPlayer.Remove();
                i--;
            }
        }
    }
}