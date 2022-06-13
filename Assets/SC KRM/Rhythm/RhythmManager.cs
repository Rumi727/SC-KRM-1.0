using Newtonsoft.Json;
using SCKRM.Easing;
using SCKRM.Json;
using SCKRM.SaveLoad;
using SCKRM.Sound;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Rhythm
{
    [AddComponentMenu("SC KRM/Rhythm/Rhythm Manager")]
    public sealed class RhythmManager : Manager<RhythmManager>
    {
        [GeneralSaveLoad]
        public sealed class SaveData
        {
            [JsonProperty] public static double screenOffset { get; set; } = 0;
            [JsonProperty] public static double soundOffset { get; set; } = 0;
        }

        public static Map map { get; private set; }
        public static SoundPlayerParent soundPlayer { get; private set; }



        public static bool isPlaying { get; private set; } = false;



        public static bool dropPart { get; set; } = false;



        public static double bpm { get; private set; }
        public static float bpmFpsDeltaTime { get; private set; }
        public static float bpmUnscaledFpsDeltaTime { get; private set; }



        public static float time => (float)(soundPlayer?.time);
        public static double currentBeat { get; private set; }
        public static double currentBeatSound { get; private set; }
        public static double currentBeatScreen { get; private set; }
        public static double currentBeat1Beat { get; private set; }

        static double bpmOffsetBeat;
        static double bpmOffsetTime;



        [Obsolete("Use currentBeat1Beat instead")] public static event Action oneBeat;
        [Obsolete("Use currentBeat1Beat instead")] public static event Action oneBeatDropPart;



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

        static void SetCurrentBeat()
        {
            double soundTime = (double)time - map.info.offset - bpmOffsetTime;
            double bpmDivide60 = bpm / 60d;

            currentBeat = (soundTime * bpmDivide60) + bpmOffsetBeat;
            currentBeatSound = ((soundTime - SaveData.soundOffset) * bpmDivide60) + bpmOffsetBeat;
            currentBeatScreen = ((soundTime - SaveData.screenOffset) * bpmDivide60) + bpmOffsetBeat;

            if (currentBeat >= 0)
                currentBeat1Beat = currentBeat - (int)currentBeat;
            else
                currentBeat1Beat = 1 - ((int)currentBeat - currentBeat);
        }

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

        public static void Play(SoundPlayerParent soundPlayer, Map map)
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
        public MapInfo info { get; } = new MapInfo();
        public MapEffect effect { get; } = new MapEffect();

        public Map() { }

        public Map(MapInfo info, MapEffect effect)
        {
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

    #region Beat Value Pair List
    public class BeatValuePairList<T> : BeatValuePairList<T, BeatValuePair<T>> { }

    public class BeatValuePairList<TValue, TPair> : List<TPair> where TPair : IBeatValuePair<TValue>
    {
        public TValue GetValue() => GetValue(RhythmManager.currentBeat, out _, out _);
        public TValue GetValue(double currentBeat) => GetValue(currentBeat, out _, out _);

        TValue tempValue = default;
        double tempBeat = 0;
        double? tempCurrentBeat = null;
        public virtual TValue GetValue(double currentBeat, out double beat, out bool isValueChanged)
        {
            if (tempCurrentBeat != null && (double)tempCurrentBeat == currentBeat)
            {
                beat = tempBeat;
                isValueChanged = false;

                return tempValue;
            }

            tempCurrentBeat = currentBeat;

            TValue value;
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

                TPair beatValuePair = this[findIndex - 1];
                beat = beatValuePair.beat;
                value = beatValuePair.value;
            }

            isValueChanged = !tempValue.Equals(value);
            tempValue = value;
            tempBeat = beat;

            return value;
        }
    }
    #endregion

    #region Beat Value Pair Ani List
    public abstract class BeatValuePairAniList<T> : BeatValuePairAniList<T, BeatValuePairAni<T>> { }

    public abstract class BeatValuePairAniList<TValue, TPair> : BeatValuePairList<TValue, TPair> where TPair : IBeatValuePairAni<TValue>
    {
        public delegate TValue GetValueFunc(double currentBeat, double t, EasingFunction.Function easingFunction, IBeatValuePairAni<TValue> previousBeatValuePair, IBeatValuePairAni<TValue> beatValuePair);

        public override abstract TValue GetValue(double currentBeat, out double beat, out bool isValueChanged);

        TValue tempValue = default;
        protected TValue GetValueInternal(double currentBeat, out double beat, out bool isValueChanged, GetValueFunc func)
        {
            TValue value;
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
                TPair beatValuePair = this[index];
                beat = beatValuePair.beat;

                if (index <= 0 || beatValuePair.length == 0)
                    value = beatValuePair.value;
                else
                {
                    TPair previousBeatValuePair = this[index - 1];
                    double t = ((currentBeat - beatValuePair.beat) / beatValuePair.length).Clamp01();

                    value = func.Invoke(currentBeat, t, EasingFunction.GetEasingFunction(beatValuePair.easingFuncion), previousBeatValuePair, beatValuePair);
                }
            }

            isValueChanged = !tempValue.Equals(value);
            tempValue = value;

            return value;
        }
    }
    #endregion



    #region Built-in effect class
    public class BeatValuePairAniListFloat : BeatValuePairAniList<float>
    {
        public override float GetValue(double currentBeat, out double beat, out bool isValueChanged) => GetValueInternal(currentBeat, out beat, out isValueChanged, ValueCalculate);

        static float ValueCalculate(double currentBeat, double t, EasingFunction.Function easingFunction, IBeatValuePairAni<float> previousBeatValuePair, IBeatValuePairAni<float> beatValuePair)
            => (float)easingFunction.Invoke(previousBeatValuePair.value, beatValuePair.value, t);
    }

    public class BeatValuePairAniListDouble : BeatValuePairAniList<double>
    {
        public override double GetValue(double currentBeat, out double beat, out bool isValueChanged) => GetValueInternal(currentBeat, out beat, out isValueChanged, ValueCalculate);

        static double ValueCalculate(double currentBeat, double t, EasingFunction.Function easingFunction, IBeatValuePairAni<double> previousBeatValuePair, IBeatValuePairAni<double> beatValuePair)
            => easingFunction.Invoke(previousBeatValuePair.value, beatValuePair.value, t);
    }

    public class BeatValuePairAniListVector2 : BeatValuePairAniList<JVector2>
    {
        public override JVector2 GetValue(double currentBeat, out double beat, out bool isValueChanged) => GetValueInternal(currentBeat, out beat, out isValueChanged, ValueCalculate);

        static JVector2 ValueCalculate(double currentBeat, double t, EasingFunction.Function easingFunction, IBeatValuePairAni<JVector2> previousBeatValuePair, IBeatValuePairAni<JVector2> beatValuePair)
        {
            JVector2 pre = previousBeatValuePair.value;
            JVector2 value = beatValuePair.value;
            float x = (float)easingFunction.Invoke(pre.x, value.x, t);
            float y = (float)easingFunction.Invoke(pre.y, value.y, t);

            return new JVector2(x, y);
        }
    }

    public class BeatValuePairAniListVector3 : BeatValuePairAniList<JVector3>
    {
        public override JVector3 GetValue(double currentBeat, out double beat, out bool isValueChanged) => GetValueInternal(currentBeat, out beat, out isValueChanged, ValueCalculate);

        static JVector3 ValueCalculate(double currentBeat, double t, EasingFunction.Function easingFunction, IBeatValuePairAni<JVector3> previousBeatValuePair, IBeatValuePairAni<JVector3> beatValuePair)
        {
            JVector3 pre = previousBeatValuePair.value;
            JVector3 value = beatValuePair.value;
            float x = (float)easingFunction.Invoke(pre.x, value.x, t);
            float y = (float)easingFunction.Invoke(pre.y, value.y, t);
            float z = (float)easingFunction.Invoke(pre.z, value.z, t);

            return new JVector3(x, y, z);
        }
    }

    public class BeatValuePairAniListVector4 : BeatValuePairAniList<JVector4>
    {
        public override JVector4 GetValue(double currentBeat, out double beat, out bool isValueChanged) => GetValueInternal(currentBeat, out beat, out isValueChanged, ValueCalculate);

        static JVector4 ValueCalculate(double currentBeat, double t, EasingFunction.Function easingFunction, IBeatValuePairAni<JVector4> previousBeatValuePair, IBeatValuePairAni<JVector4> beatValuePair)
        {
            JVector4 pre = previousBeatValuePair.value;
            JVector4 value = beatValuePair.value;
            float x = (float)easingFunction.Invoke(pre.x, value.x, t);
            float y = (float)easingFunction.Invoke(pre.y, value.y, t);
            float z = (float)easingFunction.Invoke(pre.z, value.z, t);
            float w = (float)easingFunction.Invoke(pre.w, value.w, t);

            return new JVector4(x, y, z, w);
        }
    }

    public class BeatValuePairAniListColor : BeatValuePairAniList<JColor>
    {
        public override JColor GetValue(double currentBeat, out double beat, out bool isValueChanged) => GetValueInternal(currentBeat, out beat, out isValueChanged, ValueCalculate);

        static JColor ValueCalculate(double currentBeat, double t, EasingFunction.Function easingFunction, IBeatValuePairAni<JColor> previousBeatValuePair, IBeatValuePairAni<JColor> beatValuePair)
        {
            JColor pre = previousBeatValuePair.value;
            JColor value = beatValuePair.value;
            float r = (float)easingFunction.Invoke(pre.r, value.r, t);
            float g = (float)easingFunction.Invoke(pre.g, value.g, t);
            float b = (float)easingFunction.Invoke(pre.b, value.b, t);
            float a = (float)easingFunction.Invoke(pre.a, value.a, t);

            return new JColor(r, g, b, a);
        }
    }

    public class BeatValuePairAniListRect : BeatValuePairAniList<JRect>
    {
        public override JRect GetValue(double currentBeat, out double beat, out bool isValueChanged) => GetValueInternal(currentBeat, out beat, out isValueChanged, ValueCalculate);

        static JRect ValueCalculate(double currentBeat, double t, EasingFunction.Function easingFunction, IBeatValuePairAni<JRect> previousBeatValuePair, IBeatValuePairAni<JRect> beatValuePair)
        {
            JRect pre = previousBeatValuePair.value;
            JRect value = beatValuePair.value;
            float r = (float)easingFunction.Invoke(pre.x, value.x, t);
            float g = (float)easingFunction.Invoke(pre.y, value.y, t);
            float b = (float)easingFunction.Invoke(pre.width, value.width, t);
            float a = (float)easingFunction.Invoke(pre.height, value.height, t);

            return new JRect(r, g, b, a);
        }
    }
    #endregion



    #region Beat Value Pair
    public interface IBeatValuePair<TValue>
    {
        public double beat { get; set; }
        public TValue value { get; set; }
    }

    public interface IBeatValuePairAni<TValue> : IBeatValuePair<TValue>
    {
        public double length { get; set; }
        public EasingFunction.Ease easingFuncion { get; set; }
    }



    public struct BeatValuePair<TValue> : IBeatValuePair<TValue>
    {
        public double beat { get; set; }
        public TValue value { get; set; }

        public BeatValuePair(double beat, TValue value)
        {
            this.beat = beat;
            this.value = value;
        }
    }

    public struct BeatValuePairAni<TValue> : IBeatValuePairAni<TValue>
    {
        public double beat { get; set; }
        public TValue value { get; set; }

        public double length { get; set; }
        public EasingFunction.Ease easingFuncion { get; set; }

        public BeatValuePairAni(double beat, TValue value, double length, EasingFunction.Ease easingFuncion)
        {
            this.beat = beat;
            this.value = value;

            this.length = length;
            this.easingFuncion = easingFuncion;
        }
    }
    #endregion
}