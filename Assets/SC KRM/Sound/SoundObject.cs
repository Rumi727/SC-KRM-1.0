using UnityEngine;
using SCKRM.Resource;
using SCKRM.Object;
using SCKRM.Tool;

namespace SCKRM.Sound
{
    [AddComponentMenu("")]
    [RequireComponent(typeof(AudioSource))]
    public sealed class SoundObject : ObjectPooling
    {
        [SerializeField, HideInInspector] AudioSource _audioSource;
        public AudioSource audioSource
        {
            get
            {
                if (_audioSource == null)
                    _audioSource = GetComponent<AudioSource>();

                return _audioSource;
            }
        }



        public SoundData soundData { get; private set; }
        public SoundMetaData soundMetaData { get; private set; }

        public float time { get => audioSource.time; set => audioSource.time = value; }
        public float length { get => (float)(audioSource.clip?.length); }

        public bool isLooped { get; private set; } = false;

        bool _isPaused = false;
        public bool isPaused
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

        #region variable
        [SerializeField] string _key = "";
        [SerializeField] string _nameSpace = "";
        public string key { get => _key; set => _key = value; }
        public string nameSpace { get => _nameSpace; set => _nameSpace = value; }

        [SerializeField] float _volume = 1;
        [SerializeField] bool _loop = false;
        [SerializeField] float _tempo = 1;
        [SerializeField] float _pitch = 1;
        [SerializeField] bool _spatial = false;
        [SerializeField] float _panStereo = 0;
        [SerializeField] float _minDistance = 0;
        [SerializeField] float _maxDistance = 16;
        [SerializeField] Vector3 _localPosition = Vector3.zero;
        public float volume { get => _volume; set => _volume = value; }
        public bool loop { get => _loop; set => _loop = value; }
        public float pitch { get => _pitch; set => _pitch = value; }
        public float tempo { get => _tempo; set => _tempo = value; }
        public bool spatial { get => _spatial; set => _spatial = value; }
        public float panStereo { get => _panStereo; set => _panStereo = value; }
        public float minDistance { get => _minDistance; set => _minDistance = value; }
        public float maxDistance { get => _maxDistance; set => _maxDistance = value; }
        public Vector3 localPosition { get => _localPosition; set => _localPosition = value; }
        #endregion

        public void Refesh()
        {
            if (!Kernel.isInitialLoadEnd)
            {
                Remove();
                return;
            }

            float tempTime = audioSource.time;
            audioSource.time = 0;

            soundData = ResourceManager.SearchSoundData(key, nameSpace);

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

            if (!SoundManager.soundList.Contains(this))
                SoundManager.soundList.Add(this);

            soundMetaData = soundData.sounds[Random.Range(0, soundData.sounds.Length)];

            if (soundData.isBGM)
                audioSource.outputAudioMixerGroup = SoundManager.instance.audioMixerGroup;

            SetVariable();

            audioSource.clip = soundMetaData.audioClip;
            audioSource.Play();
            
            if (audioSource.pitch < 0 && !soundMetaData.stream && tempTime == 0)
                audioSource.time = length - 0.001f;
            else if (tempTime >= length)
            {
                if (loop)
                {
                    if (audioSource.pitch > 0)
                        audioSource.time = 0;
                    else
                        audioSource.time = length - 0.001f;
                }
                else
                    Remove();
            }
            else
                audioSource.time = tempTime;
        }

        void SetVariable()
        {
            if (soundData.isBGM)
            {
                if (soundMetaData.stream)
                    tempo = tempo.Clamp(0);

                float allPitch = pitch * soundMetaData.pitch;
                float allTempo = tempo * soundMetaData.tempo;

                pitch *= soundMetaData.pitch;
                pitch = pitch.Clamp(allTempo.Abs() * 0.5f, allTempo.Abs() * 2f);
                pitch /= soundMetaData.pitch;

                allTempo *= Kernel.gameSpeed;
                audioSource.pitch = allTempo;
                audioSource.outputAudioMixerGroup.audioMixer.SetFloat("pitch", 1f / allTempo.Abs() * allPitch.Clamp(allTempo.Abs() * 0.5f, allTempo.Abs() * 2f));
            }
            else
            {
                if (soundMetaData.stream)
                    pitch = pitch.Clamp(0);

                audioSource.pitch = pitch * soundMetaData.pitch * Kernel.gameSpeed;
            }

            if (audioSource.pitch == 0)
                audioSource.volume = 0;
            else
                audioSource.volume = volume;

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

        float tempTime = 0;
        void Update()
        {
            SetVariable();

            if (audioSource.loop)
            {
                isLooped = false;
                if (audioSource.pitch < 0)
                {
                    if (audioSource.time > tempTime)
                        isLooped = true;
                }
                else
                {
                    if (audioSource.time < tempTime)
                        isLooped = true;
                }
                tempTime = audioSource.time;
            }

            if (!isPaused && !audioSource.isPlaying)
                Remove();
        }

        public override void Remove()
        {
            base.Remove();

            key = "";
            nameSpace = "";
            volume = 1;
            tempo = 1;
            pitch = 1;
            panStereo = 0;

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