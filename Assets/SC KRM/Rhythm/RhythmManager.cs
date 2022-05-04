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
        public static float bpm { get; set; } = 100;

        public static float bpmFpsDeltaTime
        {
            get
            {
                if (unscaledFpsDeltaTimeEnable)
                    return bpm * 0.01f * Kernel.fpsUnscaledDeltaTime;
                else
                    return bpm * 0.01f * Kernel.fpsDeltaTime;
            }
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
