using SCKRM.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Sound
{
    public class SoundPlayerManager : ObjectPooling
    {
        public virtual float time { get; set; }
        public virtual float realTime { get; set; }

        public virtual float length { get; }
        public virtual float realLength { get; }

        public virtual float speed { get; set; }

        public virtual bool isLooped { get; protected set; } = false;

        public virtual bool isPaused { get; set; }



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
    }
}