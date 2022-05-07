using UnityEngine;
using SCKRM.Resource;
using SCKRM.Object;
using Cysharp.Threading.Tasks;
using SCKRM.UI.StatusBar;

namespace SCKRM.Sound
{
    [AddComponentMenu("")]
    [RequireComponent(typeof(AudioSource))]
    public sealed class SoundPlayer : SoundPlayerParent<SoundMetaData>
    {
        [System.NonSerialized] AudioSource _audioSource; public AudioSource audioSource
        {
            get
            {
                if (_audioSource == null)
                    _audioSource = GetComponent<AudioSource>();

                return _audioSource;
            }
        }
        [System.NonSerialized] AudioLowPassFilter _audioLowPassFilter; public AudioLowPassFilter audioLowPassFilter
        {
            get
            {
                if (_audioLowPassFilter == null)
                    _audioLowPassFilter = GetComponent<AudioLowPassFilter>();

                return _audioLowPassFilter;
            }
        }



        public override float time
        {
            get => audioSource.time;
            set
            {
                audioSource.time = value;
                tempTime = audioSource.time;

                TimeChangedInvoke();
            }
        }
        public override float realTime { get => time / speed; set => time = value * speed; }

        public override float length => (float)(audioSource.clip != null ? audioSource.clip.length : 0);
        public override float realLength => length / speed;

        public override float speed
        {
            get => (soundData != null && soundData.isBGM && SoundManager.Data.useTempo) ? tempo : pitch;
            set
            {
                if (soundData != null && soundData.isBGM && SoundManager.Data.useTempo)
                    tempo = value;
                else
                    pitch = value;
            }
        }



        public override bool isLooped { get; protected set; } = false;

        bool _isPaused = false;
        public override bool isPaused
        {
            get => _isPaused;
            set
            {
                if (value)
                    audioSource.Pause();
                else
                    audioSource.UnPause();

                _isPaused = value;
            }
        }



        public delegate void OnAudioFilterReadAction(float[] data, int channels);
        public event OnAudioFilterReadAction onAudioFilterReadEvent;



        float tempTime = 0;
        void Update()
        {
            SetVariable();

            if (audioSource.loop)
            {
                isLooped = false;
                if (audioSource.pitch < 0)
                {
                    if (audioSource.time < metaData.loopStartTime)
                        audioSource.time = length - 0.001f;

                    if (audioSource.time > tempTime)
                    {
                        isLooped = true;
                        LoopedInvoke();
                    }
                }
                else
                {
                    if (audioSource.time < tempTime)
                    {
                        if (metaData.loopStartTime > 0)
                            audioSource.time = metaData.loopStartTime;

                        isLooped = true;
                        LoopedInvoke();
                    }
                }

                tempTime = audioSource.time;
            }

            if (!isPaused && !audioSource.isPlaying && !ResourceManager.isAudioReset)
                Remove();
        }



        void OnAudioFilterRead(float[] data, int channels) => onAudioFilterReadEvent?.Invoke(data, channels);



        public override void Refresh()
        {
            {
                if (!InitialLoadManager.isInitialLoadEnd)
                {
                    Remove();
                    return;
                }
                else if (ResourceManager.isAudioReset)
                {
                    Remove();
                    return;
                }



                if (customSoundData == null)
                    soundData = ResourceManager.SearchSoundData(key, nameSpace);
                else
                    soundData = customSoundData;

                if (soundData == null)
                {
                    Remove();
                    return;
                }
                else if (soundData.sounds == null || soundData.sounds.Length <= 0)
                {
                    Remove();
                    return;
                }
                else if (pitch == 0)
                {
                    Remove();
                    return;
                }
            }

            {
                metaData = soundData.sounds[Random.Range(0, soundData.sounds.Length)];
                audioSource.clip = metaData.audioClip;

                if (soundData.isBGM && SoundManager.Data.useTempo)
                    audioSource.outputAudioMixerGroup = SoundManager.instance.audioMixerGroup;
                else
                    audioSource.outputAudioMixerGroup = null;
            }

            if (!SoundManager.soundList.Contains(this))
                SoundManager.soundList.Add(this);

            {
                SetVariable();

                if (isPaused)
                    isPaused = false;

                if (audioSource.pitch < 0 && !metaData.stream && tempTime == 0)
                    audioSource.time = length - 0.001f;
                else
                    audioSource.time = Mathf.Min(time, length - 0.001f);

                tempTime = audioSource.time;
                audioSource.Play();
            }
        }



        void SetVariable()
        {
            SetTempoAndPitch();
            SetVolume();

            /*if (StatusBarManager.selectedStatusBar)
                audioLowPassFilter.cutoffFrequency = 687.5f;
            else
                audioLowPassFilter.cutoffFrequency = 11000f;*/

            if (spatial)
                audioSource.spatialBlend = 1;
            else
                audioSource.spatialBlend = 0;

            audioSource.loop = loop;
            audioSource.panStereo = panStereo;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;

            transform.localPosition = localPosition;
        }

        void SetTempoAndPitch()
        {
            if (soundData.isBGM && SoundManager.Data.useTempo)
            {
                if (metaData.stream)
                    tempo = tempo.Clamp(0);

                float allPitch = pitch * metaData.pitch;
                float allTempo = tempo * metaData.tempo;

                pitch = allPitch.Clamp(allTempo.Abs() * 0.5f, allTempo.Abs() * 2f) / metaData.pitch;

                allTempo *= Kernel.gameSpeed;
                audioSource.pitch = allTempo;
                audioSource.outputAudioMixerGroup.audioMixer.SetFloat("pitch", 1f / allTempo.Abs() * allPitch.Clamp(allTempo.Abs() * 0.5f, allTempo.Abs() * 2f));
            }
            else
            {
                if (metaData.stream)
                    pitch = pitch.Clamp(0);

                audioSource.pitch = pitch * metaData.pitch * Kernel.gameSpeed;
            }
        }

        void SetVolume()
        {
            if (audioSource.pitch == 0)
                audioSource.volume = 0;
            else
            {
                if (soundData.isBGM)
                    audioSource.volume = volume * (SoundManager.SaveData.bgmVolume * 0.01f);
                else
                    audioSource.volume = volume * (SoundManager.SaveData.soundVolume * 0.01f);
            }
        }



        public override void Remove()
        {
            base.Remove();

            tempTime = 0;

            audioSource.clip = null;
            audioSource.pitch = 1;
            audioSource.loop = false;
            audioSource.volume = 1;
            audioSource.panStereo = 0;
            audioSource.spatialBlend = 0;
            audioSource.minDistance = 0;
            audioSource.maxDistance = 10;

            audioSource.outputAudioMixerGroup = null;

            audioSource.time = 0;
            audioSource.Stop();

            SoundManager.soundList.Remove(this);
        }
    }
}