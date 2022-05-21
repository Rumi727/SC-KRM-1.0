using SCKRM.SaveLoad;
using SCKRM.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM
{
    public class RhythmManager : Manager<RhythmManager>
    {
        [GeneralSaveLoad]
        public class SaveData
        {
            public double screenOffset { get; set; } = 0;
            public double soundOffset { get; set; } = 0;
        }

        public static Map map { get; private set; }
        public static SoundPlayerVariable soundPlayer { get; private set; }



        public static bool isPlaying { get; private set; } = false;



        public static bool dropPart { get; set; } = false;



        public static double bpm { get; private set; }
        public static float bpmFpsDeltaTime { get; private set; }
        public static float bpmUnscaledFpsDeltaTime { get; private set; }



        public static float time => (float)(soundPlayer?.time);
        public static double currentBeat { get; private set; }
        static double bpmOffsetBeat;
        static double bpmOffsetTime;



        public static event Action oneBeat = () => { };
        public static event Action oneBeatDropPart = () => { };



        void Awake() => SingletonCheck(this);



        static int tempCurrentBeat = 0;
        void Update()
        {
            if (isPlaying)
            {
                if (soundPlayer == null || map == null)
                    Stop();
                else
                {
                    SetCurrentBeat();

                    {
                        bpm = map.effect.bpm.GetValue(currentBeat, out double beat, out bool isValueChanged);

                        if (isValueChanged)
                        {
                            BPMChange(bpm, beat);
                            SetCurrentBeat();
                        }
                    }

                    {
                        bpmFpsDeltaTime = (float)(bpm * 0.01f * Kernel.fpsDeltaTime * soundPlayer.speed);
                        bpmUnscaledFpsDeltaTime = (float)(bpm * 0.01f * Kernel.fpsUnscaledDeltaTime * soundPlayer.speed);
                    }

                    dropPart = map.effect.dropPart.GetValue();

                    if (tempCurrentBeat != (int)currentBeat && currentBeat >= 0)
                    {
                        oneBeat?.Invoke();
                        if (dropPart)
                            oneBeatDropPart?.Invoke();

                        tempCurrentBeat = (int)currentBeat;
                    }

                    /*Debug.Log("time: " + time);
                    Debug.Log("currentBeat: " + currentBeat);
                    Debug.Log("bpm: " + bpm);
                    Debug.Log("dropPart: " + dropPart);*/
                }
            }
        }

        static void SetCurrentBeat() => currentBeat = (double)((time - map.info.offset - bpmOffsetTime) * (bpm / 60d)) + bpmOffsetBeat;

        static void BPMChange(double bpm, double offsetBeat)
        {
            bpmOffsetBeat = offsetBeat;

            bpmOffsetTime = 0;
            double tempBeat = 0;
            for (int i = 0; i < map.effect.bpm.Count; i++)
            {
                if (map.effect.bpm[0].beat >= offsetBeat)
                    break;

                double tempBPM;
                if (i - 1 < 0)
                    tempBPM = map.effect.bpm[0].value;
                else
                    tempBPM = map.effect.bpm[i - 1].value;

                bpmOffsetTime += (map.effect.bpm[i].beat - tempBeat) * (60d / tempBPM);
                tempBeat = map.effect.bpm[i].beat;

                if (map.effect.bpm[i].beat >= offsetBeat)
                    break;
            }

            RhythmManager.bpm = bpm;
        }

        public static void Play(SoundPlayerVariable soundPlayer, Map map)
        {
            currentBeat = 0;

            RhythmManager.soundPlayer = soundPlayer;
            RhythmManager.map = map;

            soundPlayer.timeChanged += SoundPlayerTimeChange;
            isPlaying = true;
        }

        public static void Stop()
        {
            currentBeat = 0;

            if (soundPlayer != null)
                soundPlayer.timeChanged -= SoundPlayerTimeChange;

            soundPlayer = null;
            map = null;

            isPlaying = false;
        }

        static void SoundPlayerTimeChange()
        {
            for (int i = 0; i < map.effect.bpm.Count; i++)
            {
                {
                    BeatValuePair<double> bpm = map.effect.bpm[i];
                    BPMChange(bpm.value, bpm.beat);
                    SetCurrentBeat();
                }

                if (bpmOffsetTime >= time)
                {
                    if (i - 1 >= 0)
                    {
                        BeatValuePair<double> bpm = map.effect.bpm[i - 1];
                        SetCurrentBeat();
                        BPMChange(bpm.value, bpm.beat);
                    }

                    break;
                }
            }
        }
    }

    public class Map
    {
        public MapInfo info { get; }
        public MapEffect effect { get; }

        public Map(MapInfo info, MapEffect effect)
        {
            if (info == null)
                info = new MapInfo();
            if (effect == null)
                effect = new MapEffect();

            this.info = info;
            this.effect = effect;
        }
    }

    public class MapInfo
    {
        public double offset = 0;
    }

    public class MapEffect
    {
        public BeatValuePairList<double> bpm = new BeatValuePairList<double>();
        public BeatValuePairList<bool> dropPart = new BeatValuePairList<bool>();
    }

    public sealed class BeatValuePairList<T> : List<BeatValuePair<T>> where T : struct
    {
        public T GetValue() => GetValue(RhythmManager.currentBeat, out _, out _);
        public T GetValue(double currentBeat) => GetValue(currentBeat, out _, out _);

        T tempValue = default;
        double tempBeat = 0;
        double? tempCurrentBeat = null;
        public T GetValue(double currentBeat, out double beat, out bool isValueChanged)
        {
            if (tempCurrentBeat != null && (double)tempCurrentBeat == currentBeat)
            {
                beat = tempBeat;
                isValueChanged = false;

                return tempValue;
            }

            tempCurrentBeat = currentBeat;

            T value;
            if (Count <= 0)
            {
                beat = 0;
                value = default;
            }
            else if (Count <= 1 || this[0].beat >= currentBeat)
            {
                beat = this[0].beat;
                value = this[0].value;
            }
            else
            {
                int findIndex = FindIndex(x => x.beat >= currentBeat);
                if (findIndex < 0)
                    findIndex = Count;

                BeatValuePair<T> beatValuePair = this[findIndex - 1];
                beat = beatValuePair.beat;
                value = beatValuePair.value;
            }

            isValueChanged = !tempValue.Equals(value);
            tempValue = value;
            tempBeat = beat;

            return value;
        }
    }

    public sealed class BeatValuePairAniListDouble : List<BeatValuePairAni<double>>
    {
        public double GetValue() => GetValue(RhythmManager.currentBeat, out _, out _);
        public double GetValue(double currentBeat) => GetValue(currentBeat, out _, out _);

        double tempValue = default;
        public double GetValue(double currentBeat, out double beat, out bool isValueChanged)
        {
            double value;
            if (Count <= 0)
            {
                beat = 0;
                value = default;
            }
            else if (Count <= 1 || this[0].beat >= currentBeat)
            {
                beat = this[0].beat;
                value = this[0].value;
            }
            else
            {
                int findIndex = FindIndex(x => x.beat >= currentBeat);
                if (findIndex < 0)
                    findIndex = Count;


                int index = findIndex - 1;
                BeatValuePairAni<double> beatValuePair = this[index];
                beat = beatValuePair.beat;

                if (index <= 0 || beatValuePair.length == 0)
                    value = beatValuePair.value;
                else
                {
                    BeatValuePairAni<double> previousBeatValuePair = this[index - 1];
                    double t = ((currentBeat - beatValuePair.beat) / beatValuePair.length).Clamp01();

                    value = EasingFunction.GetEasingFunction(beatValuePair.easingFuncion).Invoke(previousBeatValuePair.value, beatValuePair.value, t);
                }
            }

            isValueChanged = !tempValue.Equals(value);
            tempValue = value;

            return value;
        }
    }

    public struct BeatValuePair<TValue> where TValue : struct
    {
        public double beat;
        public TValue value;

        public BeatValuePair(double beat, TValue value)
        {
            this.beat = beat;
            this.value = value;
        }
    }

    public struct BeatValuePairAni<TValue> where TValue : struct
    {
        public double beat;
        public TValue value;

        public double length;
        public EasingFunction.Ease easingFuncion;

        public BeatValuePairAni(double beat, TValue value, double length, EasingFunction.Ease easingFuncion)
        {
            this.beat = beat;
            this.value = value;

            this.length = length;
            this.easingFuncion = easingFuncion;
        }
    }
}
