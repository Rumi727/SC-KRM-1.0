using SCKRM.Object;
using SCKRM.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Sound
{
    public abstract class SoundPlayerParent : ObjectPooling, IRefresh
    {
        public string key { get; set; }
        public string nameSpace { get; set; }



        public abstract float time { get; set; }
        public abstract float realTime { get; set; }

        public abstract float length { get; }
        public abstract float realLength { get; }

        public bool loop { get; set; }

        protected Action _timeChanged;
        protected Action _looped;
        public event Action timeChanged { add => _timeChanged += value; remove => _timeChanged -= value; }
        public event Action looped { add => _looped += value; remove => _looped -= value; }



        public abstract bool isLooped { get; protected set; }
        public abstract bool isPaused { get; set; }



        public float pitch { get; set; }
        public float tempo { get; set; }

        public abstract float speed { get; set; }
        


        public float volume { get; set; }

        public float minDistance { get; set; }
        public float maxDistance { get; set; }

        public float panStereo { get; set; }



        public bool spatial { get; set; }
        public Vector3 localPosition { get; set; }



        public abstract void Refresh();



        public override bool Remove()
        {
            if (!base.Remove())
                return false;

            time = 0;
            realTime = 0;

            isLooped = false;
            isPaused = false;

            key = "";
            nameSpace = "";

            volume = 1;
            loop = false;
            pitch = 1;
            tempo = 1;
            spatial = false;
            panStereo = 0;
            minDistance = 0;
            maxDistance = 16;
            localPosition = Vector3.zero;

            return true;
        }
    }
}
