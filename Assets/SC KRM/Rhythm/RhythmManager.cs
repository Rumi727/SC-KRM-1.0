using SCKRM.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM
{
    public static class RhythmManager
    {
        public static bool isBeatPlay { get; set; } = false;
        public static bool dropPart { get; set; } = false;

        static float _bpmDeltaTime = 1;
        /// <summary>
        /// get = (bpm * 0.01f) * Kernel.fpsDeltaTime;
        /// set = bpm;
        /// </summary>
        public static float bpmFpsDeltaTime 
        { 
            get => unscaledFpsDeltaTimeEnable ? _bpmDeltaTime * Kernel.fpsUnscaledDeltaTime : _bpmDeltaTime * Kernel.fpsDeltaTime; 
            set => _bpmDeltaTime = value * 0.01f; 
        }

        public static bool unscaledFpsDeltaTimeEnable { get; set; } = false;

        public static event Action oneBeat = () => { };
        public static event Action oneBeatDropPart = () => { };

        public static void OneBeatInvoke()
        {
            if (!isBeatPlay)
                return;

            oneBeat();

            if (dropPart)
                oneBeatDropPart();
        }
    }
}
