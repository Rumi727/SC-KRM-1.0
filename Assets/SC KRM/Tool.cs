using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SCKRM
{
    public static class MathTool
    {
        public static sbyte Abs(this sbyte value)
        {
            if (value < 0)
                return (sbyte)-value;
            else
                return value;
        }

        public static short Abs(this short value)
        {
            if (value < 0)
                return (sbyte)-value;
            else
                return value;
        }

        public static int Abs(this int value)
        {
            if (value < 0)
                return -value;
            else
                return value;
        }

        public static long Abs(this long value)
        {
            if (value < 0)
                return -value;
            else
                return value;
        }

        public static float Abs(this float value)
        {
            if (value < 0)
                return -value;
            else
                return value;
        }

        public static double Abs(this double value)
        {
            if (value < 0)
                return -value;
            else
                return value;
        }

        public static decimal Abs(this decimal value)
        {
            if (value < 0)
                return -value;
            else
                return value;
        }

        public static int Sign(this sbyte value)
        {
            if (value < 0)
                return -1;
            else
                return 1;
        }

        public static short Sign(this short value)
        {
            if (value < 0)
                return -1;
            else
                return 1;
        }

        public static int Sign(this int value)
        {
            if (value < 0)
                return -1;
            else
                return 1;
        }

        public static long Sign(this long value)
        {
            if (value < 0)
                return -1;
            else
                return 1;
        }

        public static int Sign(this float value)
        {
            if (value < 0)
                return -1;
            else
                return 1;
        }

        public static int Sign(this double value)
        {
            if (value < 0)
                return -1;
            else
                return 1;
        }

        public static int Sign(this decimal value)
        {
            if (value < 0)
                return -1;
            else
                return 1;
        }

        public static byte Clamp(this byte value, byte min, byte max = byte.MaxValue)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static sbyte Clamp(this sbyte value, sbyte min, sbyte max = sbyte.MaxValue)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static short Clamp(this short value, short min, short max = short.MaxValue)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static ushort Clamp(this ushort value, ushort min, ushort max = ushort.MaxValue)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static int Clamp(this int value, int min, int max = int.MaxValue)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static uint Clamp(this uint value, uint min, uint max = uint.MaxValue)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static long Clamp(this long value, long min, long max = long.MaxValue)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static ulong Clamp(this ulong value, ulong min, ulong max = ulong.MaxValue)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static float Clamp(this float value, float min, float max = float.PositiveInfinity)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static double Clamp(this double value, double min, double max = double.PositiveInfinity)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static decimal Clamp(this decimal value, decimal min, decimal max = decimal.MaxValue)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static byte Clamp01(this byte value)
        {
            if (value > 1)
                return 1;
            else
                return value;
        }

        public static sbyte Clamp01(this sbyte value)
        {
            if (value < 0)
                return 0;
            else if (value > 1)
                return 1;
            else
                return value;
        }

        public static short Clamp01(this short value)
        {
            if (value < 0)
                return 0;
            else if (value > 1)
                return 1;
            else
                return value;
        }

        public static ushort Clamp01(this ushort value)
        {
            if (value > 1)
                return 1;
            else
                return value;
        }

        public static int Clamp01(this int value)
        {
            if (value < 0)
                return 0;
            else if (value > 1)
                return 1;
            else
                return value;
        }

        public static uint Clamp01(this uint value)
        {
            if (value > 1)
                return 1;
            else
                return value;
        }

        public static long Clamp01(this long value)
        {
            if (value < 0)
                return 0;
            else if (value > 1)
                return 1;
            else
                return value;
        }

        public static ulong Clamp01(this ulong value)
        {
            if (value > 1)
                return 1;
            else
                return value;
        }

        public static float Clamp01(this float value)
        {
            if (value < 0)
                return 0;
            else if (value > 1)
                return 1;
            else
                return value;
        }

        public static double Clamp01(this double value)
        {
            if (value < 0)
                return 0;
            else if (value > 1)
                return 1;
            else
                return value;
        }

        public static decimal Clamp01(this decimal value)
        {
            if (value < 0)
                return 0;
            else if (value > 1)
                return 1;
            else
                return value;
        }

        public static byte Lerp(this byte current, byte target, byte t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (byte)(((1 - t) * current) + (target * t));
        }

        public static byte Lerp(this byte current, byte target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (byte)(((1 - t) * current) + (target * t));
        }

        public static byte Lerp(this byte current, byte target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (byte)(((1 - t) * current) + (target * t));
        }

        public static byte Lerp(this byte current, byte target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (byte)(((1 - t) * current) + (target * t));
        }

        public static sbyte Lerp(this sbyte current, sbyte target, sbyte t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (sbyte)(((1 - t) * current) + (target * t));
        }

        public static sbyte Lerp(this sbyte current, sbyte target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (sbyte)(((1 - t) * current) + (target * t));
        }

        public static sbyte Lerp(this sbyte current, sbyte target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (sbyte)(((1 - t) * current) + (target * t));
        }

        public static sbyte Lerp(this sbyte current, sbyte target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (sbyte)(((1 - t) * current) + (target * t));
        }

        public static short Lerp(this short current, short target, short t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (short)(((1 - t) * current) + (target * t));
        }

        public static short Lerp(this short current, short target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (short)(((1 - t) * current) + (target * t));
        }

        public static short Lerp(this short current, short target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (short)(((1 - t) * current) + (target * t));
        }

        public static short Lerp(this short current, short target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (short)(((1 - t) * current) + (target * t));
        }

        public static ushort Lerp(this ushort current, ushort target, ushort t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (ushort)(((1 - t) * current) + (target * t));
        }

        public static ushort Lerp(this ushort current, ushort target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (ushort)(((1 - t) * current) + (target * t));
        }

        public static ushort Lerp(this ushort current, ushort target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (ushort)(((1 - t) * current) + (target * t));
        }

        public static ushort Lerp(this ushort current, ushort target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (ushort)(((1 - t) * current) + (target * t));
        }

        public static int Lerp(this int current, int target, int t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (int)(((1 - t) * current) + (target * t));
        }

        public static int Lerp(this int current, int target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (int)(((1 - t) * current) + (target * t));
        }

        public static int Lerp(this int current, int target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (int)(((1 - t) * current) + (target * t));
        }

        public static int Lerp(this int current, int target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (int)(((1 - t) * current) + (target * t));
        }

        public static uint Lerp(this uint current, uint target, uint t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (uint)(((1 - t) * current) + (target * t));
        }

        public static uint Lerp(this uint current, uint target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (uint)(((1 - t) * current) + (target * t));
        }

        public static uint Lerp(this uint current, uint target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (uint)(((1 - t) * current) + (target * t));
        }

        public static uint Lerp(this uint current, uint target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (uint)(((1 - t) * current) + (target * t));
        }

        public static long Lerp(this long current, long target, long t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (long)(((1 - t) * current) + (target * t));
        }

        public static long Lerp(this long current, long target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (long)(((1 - t) * current) + (target * t));
        }

        public static long Lerp(this long current, long target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (long)(((1 - t) * current) + (target * t));
        }

        public static long Lerp(this long current, long target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (long)(((1 - t) * current) + (target * t));
        }

        public static ulong Lerp(this ulong current, ulong target, ulong t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (ulong)(((1 - t) * current) + (target * t));
        }

        public static ulong Lerp(this ulong current, ulong target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (ulong)(((1 - t) * current) + (target * t));
        }

        public static ulong Lerp(this ulong current, ulong target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (ulong)(((1 - t) * current) + (target * t));
        }

        public static ulong Lerp(this ulong current, ulong target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return (ulong)(((1 - t) * current) + (target * t));
        }

        public static float Lerp(this float current, float target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return ((1 - t) * current) + (target * t);
        }

        public static double Lerp(this double current, double target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return ((1 - t) * current) + (target * t);
        }

        public static decimal Lerp(this decimal current, decimal target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return ((1 - t) * current) + (target * t);
        }

        public static Vector2 Lerp(this Vector2 current, Vector2 target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return new Vector2(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t);
        }

        public static Vector3 Lerp(this Vector3 current, Vector3 target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return new Vector3(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t, current.z + (target.z - current.z) * t);
        }

        public static Vector4 Lerp(this Vector4 current, Vector4 target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return new Vector4(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t, current.z + (target.z - current.z) * t, current.w + (target.w - current.w) * t);
        }

        public static Rect Lerp(this Rect current, Rect target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return new Rect(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t, current.width + (target.width - current.width) * t, current.height + (target.height - current.height) * t);
        }

        public static Color Lerp(this Color current, Color target, float t, bool alpha = true, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();

            if (alpha)
                return new Color(current.r + (target.r - current.r) * t, current.g + (target.g - current.g) * t, current.b + (target.b - current.b) * t, current.a + (target.a - current.a) * t);
            else
                return new Color(current.r + (target.r - current.r) * t, current.g + (target.g - current.g) * t, current.b + (target.b - current.b) * t, current.a);
        }

        public static byte MoveTowards(this byte current, byte target, byte maxDelta)
        {
            if ((target - current) <= maxDelta)
                return target;

            return (byte)(current + maxDelta);
        }

        public static sbyte MoveTowards(this sbyte current, sbyte target, sbyte maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
                return target;

            return (sbyte)(current + (target - current).Sign() * maxDelta);
        }

        public static short MoveTowards(this short current, short target, short maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
                return target;

            return (short)(current + (target - current).Sign() * maxDelta);
        }

        public static ushort MoveTowards(this ushort current, ushort target, ushort maxDelta)
        {
            if ((target - current) <= maxDelta)
                return target;

            return (ushort)(current + maxDelta);
        }

        public static int MoveTowards(this int current, int target, int maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
                return target;

            return current + (target - current).Sign() * maxDelta;
        }

        public static uint MoveTowards(this uint current, uint target, uint maxDelta)
        {
            if ((target - current) <= maxDelta)
                return target;

            return current + maxDelta;
        }

        public static long MoveTowards(this long current, long target, long maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
                return target;

            return current + (target - current).Sign() * maxDelta;
        }

        public static ulong MoveTowards(this ulong current, ulong target, ulong maxDelta)
        {
            if ((target - current) <= maxDelta)
                return target;

            return current + maxDelta;
        }

        public static float MoveTowards(this float current, float target, float maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
                return target;

            return current + (target - current).Sign() * maxDelta;
        }

        public static double MoveTowards(this double current, double target, double maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
                return target;

            return current + (target - current).Sign() * maxDelta;
        }

        public static decimal MoveTowards(this decimal current, decimal target, decimal maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
                return target;

            return current + (target - current).Sign() * maxDelta;
        }

        public static Vector2 MoveTowards(this Vector2 current, Vector2 target, float maxDistanceDelta)
        {
            float num = target.x - current.x;
            float num2 = target.y - current.y;
            float num3 = num * num + num2 * num2;
            if (num3 == 0f || (maxDistanceDelta >= 0f && num3 <= maxDistanceDelta * maxDistanceDelta))
                return target;

            float num4 = (float)Math.Sqrt(num3);
            return new Vector2(current.x + num / num4 * maxDistanceDelta, current.y + num2 / num4 * maxDistanceDelta);
        }
        public static Vector3 MoveTowards(this Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            float num = target.x - current.x;
            float num2 = target.y - current.y;
            float num3 = target.z - current.z;
            float num4 = num * num + num2 * num2 + num3 * num3;
            if (num4 == 0f || (maxDistanceDelta >= 0f && num4 <= maxDistanceDelta * maxDistanceDelta))
                return target;

            float num5 = (float)Math.Sqrt(num4);
            return new Vector3(current.x + num / num5 * maxDistanceDelta, current.y + num2 / num5 * maxDistanceDelta, current.z + num3 / num5 * maxDistanceDelta);
        }
        public static Vector4 MoveTowards(this Vector4 current, Vector4 target, float maxDistanceDelta)
        {
            float num = target.x - current.x;
            float num2 = target.y - current.y;
            float num3 = target.z - current.z;
            float num4 = target.w - current.w;
            float num5 = num * num + num2 * num2 + num3 * num3 + num4 * num4;
            if (num5 == 0f || (maxDistanceDelta >= 0f && num5 <= maxDistanceDelta * maxDistanceDelta))
                return target;

            float num6 = (float)Math.Sqrt(num5);
            return new Vector4(current.x + num / num6 * maxDistanceDelta, current.y + num2 / num6 * maxDistanceDelta, current.z + num3 / num6 * maxDistanceDelta, current.w + num4 / num6 * maxDistanceDelta);
        }
        public static Rect MoveTowards(this Rect current, Rect target, float maxDistanceDelta)
        {
            float num = target.x - current.x;
            float num2 = target.y - current.y;
            float num3 = target.width - current.width;
            float num4 = target.height - current.height;
            float num5 = num * num + num2 * num2 + num3 * num3 + num4 * num4;
            if (num5 == 0f || (maxDistanceDelta >= 0f && num5 <= maxDistanceDelta * maxDistanceDelta))
                return target;

            float num6 = (float)Math.Sqrt(num5);
            return new Rect(current.x + num / num6 * maxDistanceDelta, current.y + num2 / num6 * maxDistanceDelta, current.width + num3 / num6 * maxDistanceDelta, current.height + num4 / num6 * maxDistanceDelta);
        }
        public static Color MoveTowards(this Color current, Color target, float maxDistanceDelta)
        {
            float num = target.r - current.r;
            float num2 = target.g - current.g;
            float num3 = target.b - current.b;
            float num4 = target.a - current.a;
            float num5 = num * num + num2 * num2 + num3 * num3 + num4 * num4;
            if (num5 == 0f || (maxDistanceDelta >= 0f && num5 <= maxDistanceDelta * maxDistanceDelta))
                return target;

            float num6 = (float)Math.Sqrt(num5);
            return new Color(current.r + num / num6 * maxDistanceDelta, current.g + num2 / num6 * maxDistanceDelta, current.b + num3 / num6 * maxDistanceDelta, current.a + num4 / num6 * maxDistanceDelta);
        }



        public static float Ceil(this float value) => (float)Math.Ceiling(value);
        public static double Ceil(this double value) => Math.Ceiling(value);
        public static decimal Ceil(this decimal value) => Math.Ceiling(value);

        public static int CeilToInt(this float value) => (int)Math.Ceiling(value);
        public static int CeilToInt(this double value) => (int)Math.Ceiling(value);
        public static int CeilToInt(this decimal value) => (int)Math.Ceiling(value);

        public static float Floor(this float value) => (float)Math.Floor(value);
        public static double Floor(this double value) => Math.Floor(value);
        public static decimal Floor(this decimal value) => Math.Floor(value);

        public static int FloorToInt(this float value) => (int)Math.Floor(value);
        public static int FloorToInt(this double value) => (int)Math.Floor(value);
        public static int FloorToInt(this decimal value) => (int)Math.Floor(value);

        public static float Round(this float value) => (float)Math.Round(value);
        public static double Round(this double value) => Math.Round(value);
        public static decimal Round(this decimal value) => Math.Round(value);

        public static int RoundToInt(this float value) => (int)Math.Round(value);
        public static int RoundToInt(this double value) => (int)Math.Round(value);
        public static int RoundToInt(this decimal value) => (int)Math.Round(value);

        public static float Round(this float value, int digits) => (float)Math.Round(value, digits);
        public static double Round(this double value, int digits) => Math.Round(value, digits);
        public static decimal Round(this decimal value, int digits) => Math.Round(value, digits);
    }

    public static class MathRefTool
    {
        public static void AbsRef(this ref sbyte value)
        {
            if (value < 0)
                value = (sbyte)-value;
        }

        public static void AbsRef(this ref short value)
        {
            if (value < 0)
                value = (sbyte)-value;
        }

        public static void AbsRef(this ref int value)
        {
            if (value < 0)
                value = -value;
        }

        public static void AbsRef(this ref long value)
        {
            if (value < 0)
                value = -value;
        }

        public static void AbsRef(this ref float value)
        {
            if (value < 0)
                value = -value;
        }

        public static void AbsRef(this ref double value)
        {
            if (value < 0)
                value = -value;
        }

        public static void AbsRef(this ref decimal value)
        {
            if (value < 0)
                value = -value;
        }

        public static void SignRef(this ref sbyte value)
        {
            if (value < 0)
                value = -1;
            else
                value = 1;
        }

        public static void SignRef(this ref short value)
        {
            if (value < 0)
                value = -1;
            else
                value = 1;
        }

        public static void SignRef(this ref int value)
        {
            if (value < 0)
                value = -1;
            else
                value = 1;
        }

        public static void SignRef(this ref long value)
        {
            if (value < 0)
                value = -1;
            else
                value = 1;
        }

        public static void SignRef(this ref float value)
        {
            if (value < 0)
                value = -1;
            else
                value = 1;
        }

        public static void SignRef(this ref double value)
        {
            if (value < 0)
                value = -1;
            else
                value = 1;
        }

        public static void SignRef(this ref decimal value)
        {
            if (value < 0)
                value = -1;
            else
                value = 1;
        }

        public static void ClampRef(this ref byte value, byte min, byte max = byte.MaxValue)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void ClampRef(this ref sbyte value, sbyte min, sbyte max = sbyte.MaxValue)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void ClampRef(this ref short value, short min, short max = short.MaxValue)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void ClampRef(this ref ushort value, ushort min, ushort max = ushort.MaxValue)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void ClampRef(this ref int value, int min, int max = int.MaxValue)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void ClampRef(this ref uint value, uint min, uint max = uint.MaxValue)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void ClampRef(this ref long value, long min, long max = long.MaxValue)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void ClampRef(this ref ulong value, ulong min, ulong max = ulong.MaxValue)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void ClampRef(this ref float value, float min, float max = float.PositiveInfinity)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void ClampRef(this ref double value, double min, double max = double.PositiveInfinity)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void ClampRef(this ref decimal value, decimal min, decimal max = decimal.MaxValue)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public static void Clamp01Ref(this ref byte value)
        {
            if (value > 1)
                value = 1;
        }

        public static void Clamp01Ref(this ref sbyte value)
        {
            if (value < 0)
                value = 0;
            else if (value > 1)
                value = 1;
        }

        public static void Clamp01Ref(this ref short value)
        {
            if (value < 0)
                value = 0;
            else if (value > 1)
                value = 1;
        }

        public static void Clamp01Ref(this ref ushort value)
        {
            if (value > 1)
                value = 1;
        }

        public static void Clamp01Ref(this ref int value)
        {
            if (value < 0)
                value = 0;
            else if (value > 1)
                value = 1;
        }

        public static void Clamp01Ref(this ref uint value)
        {
            if (value > 1)
                value = 1;
        }

        public static void Clamp01Ref(this ref long value)
        {
            if (value < 0)
                value = 0;
            else if (value > 1)
                value = 1;
        }

        public static void Clamp01Ref(this ref ulong value)
        {
            if (value > 1)
                value = 1;
        }

        public static void Clamp01Ref(this ref float value)
        {
            if (value < 0)
                value = 0;
            else if (value > 1)
                value = 1;
        }

        public static void Clamp01Ref(this ref double value)
        {
            if (value < 0)
                value = 0;
            else if (value > 1)
                value = 1;
        }

        public static void Clamp01Ref(this ref decimal value)
        {
            if (value < 0)
                value = 0;
            else if (value > 1)
                value = 1;
        }

        public static void LerpRef(this ref byte current, byte target, byte t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (byte)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref byte current, byte target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (byte)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref byte current, byte target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (byte)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref byte current, byte target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (byte)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref sbyte current, sbyte target, sbyte t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (sbyte)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref sbyte current, sbyte target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (sbyte)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref sbyte current, sbyte target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (sbyte)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref sbyte current, sbyte target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (sbyte)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref short current, short target, short t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (short)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref short current, short target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (short)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref short current, short target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (short)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref short current, short target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (short)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref ushort current, ushort target, ushort t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (ushort)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref ushort current, ushort target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (ushort)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref ushort current, ushort target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (ushort)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref ushort current, ushort target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (ushort)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref int current, int target, int t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (int)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref int current, int target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (int)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref int current, int target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (int)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref int current, int target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (int)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref uint current, uint target, uint t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (uint)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref uint current, uint target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (uint)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref uint current, uint target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (uint)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref uint current, uint target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (uint)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref long current, long target, long t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (long)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref long current, long target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (long)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref long current, long target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (long)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref long current, long target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (long)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref ulong current, ulong target, ulong t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (ulong)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref ulong current, ulong target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (ulong)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref ulong current, ulong target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (ulong)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref ulong current, ulong target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = (ulong)(((1 - t) * current) + (target * t));
        }

        public static void LerpRef(this ref float current, float target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = ((1 - t) * current) + (target * t);
        }

        public static void LerpRef(this ref double current, double target, double t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = ((1 - t) * current) + (target * t);
        }

        public static void LerpRef(this ref decimal current, decimal target, decimal t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = ((1 - t) * current) + (target * t);
        }

        public static void LerpRef(this ref Vector2 current, Vector2 target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = new Vector2(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t);
        }

        public static void LerpRef(this ref Vector3 current, Vector3 target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = new Vector3(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t, current.z + (target.z - current.z) * t);
        }

        public static void LerpRef(this ref Vector4 current, Vector4 target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = new Vector4(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t, current.z + (target.z - current.z) * t, current.w + (target.w - current.w) * t);
        }

        public static void LerpRef(this ref Rect current, Rect target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = new Rect(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t, current.width + (target.width - current.width) * t, current.height + (target.height - current.height) * t);
        }

        public static void LerpRef(this ref Color current, Color target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            current = new Color(current.r + (target.r - current.r) * t, current.g + (target.g - current.g) * t, current.b + (target.b - current.b) * t, current.a + (target.a - current.a) * t);
        }

        public static void MoveTowardsRef(this ref byte current, byte target, byte maxDelta)
        {
            if ((target - current) <= maxDelta)
            {
                current = target;
                return;
            }

            current = (byte)(current + maxDelta);
        }

        public static void MoveTowardsRef(this ref sbyte current, sbyte target, sbyte maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
            {
                current = target;
                return;
            }

            current += (sbyte)((target - current).Sign() * maxDelta);
        }

        public static void MoveTowardsRef(this ref short current, short target, short maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
            {
                current = target;
                return;
            }

            current += (short)((target - current).Sign() * maxDelta);
        }

        public static void MoveTowardsRef(this ref ushort current, ushort target, ushort maxDelta)
        {
            if ((target - current) <= maxDelta)
            {
                current = target;
                return;
            }

            current = (ushort)(current + maxDelta);
        }

        public static void MoveTowardsRef(this ref int current, int target, int maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
            {
                current = target;
                return;
            }

            current += (target - current).Sign() * maxDelta;
        }

        public static void MoveTowardsRef(this ref uint current, uint target, uint maxDelta)
        {
            if ((target - current) <= maxDelta)
            {
                current = target;
                return;
            }

            current = current + maxDelta;
        }

        public static void MoveTowardsRef(this ref long current, long target, long maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
            {
                current = target;
                return;
            }

            current += (target - current).Sign() * maxDelta;
        }

        public static void MoveTowardsRef(this ref ulong current, ulong target, ulong maxDelta)
        {
            if ((target - current) <= maxDelta)
            {
                current = target;
                return;
            }

            current += maxDelta;
        }

        public static void MoveTowardsRef(this ref float current, float target, float maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
            {
                current = target;
                return;
            }

            current += (target - current).Sign() * maxDelta;
        }

        public static void MoveTowardsRef(this ref double current, double target, double maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
            {
                current = target;
                return;
            }

            current += (target - current).Sign() * maxDelta;
        }

        public static void MoveTowardsRef(this ref decimal current, decimal target, decimal maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
            {
                current = target;
                return;
            }

            current += (target - current).Sign() * maxDelta;
        }

        public static void MoveTowardsRef(this ref Vector2 current, Vector2 target, float maxDistanceDelta)
        {
            float num = target.x - current.x;
            float num2 = target.y - current.y;
            float num3 = num * num + num2 * num2;
            if (num3 == 0f || (maxDistanceDelta >= 0f && num3 <= maxDistanceDelta * maxDistanceDelta))
                current = target;

            float num4 = (float)Math.Sqrt(num3);
            current = new Vector2(current.x + num / num4 * maxDistanceDelta, current.y + num2 / num4 * maxDistanceDelta);
        }
        public static void MoveTowardsRef(this ref Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            float num = target.x - current.x;
            float num2 = target.y - current.y;
            float num3 = target.z - current.z;
            float num4 = num * num + num2 * num2 + num3 * num3;
            if (num4 == 0f || (maxDistanceDelta >= 0f && num4 <= maxDistanceDelta * maxDistanceDelta))
                current = target;

            float num5 = (float)Math.Sqrt(num4);
            current = new Vector3(current.x + num / num5 * maxDistanceDelta, current.y + num2 / num5 * maxDistanceDelta, current.z + num3 / num5 * maxDistanceDelta);
        }
        public static void MoveTowardsRef(this ref Vector4 current, Vector4 target, float maxDistanceDelta)
        {
            float num = target.x - current.x;
            float num2 = target.y - current.y;
            float num3 = target.z - current.z;
            float num4 = target.w - current.w;
            float num5 = num * num + num2 * num2 + num3 * num3 + num4 * num4;
            if (num5 == 0f || (maxDistanceDelta >= 0f && num5 <= maxDistanceDelta * maxDistanceDelta))
                current = target;

            float num6 = (float)Math.Sqrt(num5);
            current = new Vector4(current.x + num / num6 * maxDistanceDelta, current.y + num2 / num6 * maxDistanceDelta, current.z + num3 / num6 * maxDistanceDelta, current.w + num4 / num6 * maxDistanceDelta);
        }
        public static void MoveTowardsRef(this ref Rect current, Rect target, float maxDistanceDelta)
        {
            float num = target.x - current.x;
            float num2 = target.y - current.y;
            float num3 = target.width - current.width;
            float num4 = target.height - current.height;
            float num5 = num * num + num2 * num2 + num3 * num3 + num4 * num4;
            if (num5 == 0f || (maxDistanceDelta >= 0f && num5 <= maxDistanceDelta * maxDistanceDelta))
                current = target;

            float num6 = (float)Math.Sqrt(num5);
            current = new Rect(current.x + num / num6 * maxDistanceDelta, current.y + num2 / num6 * maxDistanceDelta, current.width + num3 / num6 * maxDistanceDelta, current.height + num4 / num6 * maxDistanceDelta);
        }
        public static void MoveTowardsRef(this ref Color current, Color target, float maxDistanceDelta)
        {
            float num = target.r - current.r;
            float num2 = target.g - current.g;
            float num3 = target.b - current.b;
            float num4 = target.a - current.a;
            float num5 = num * num + num2 * num2 + num3 * num3 + num4 * num4;
            if (num5 == 0f || (maxDistanceDelta >= 0f && num5 <= maxDistanceDelta * maxDistanceDelta))
                current = target;

            float num6 = (float)Math.Sqrt(num5);
            current = new Color(current.r + num / num6 * maxDistanceDelta, current.g + num2 / num6 * maxDistanceDelta, current.b + num3 / num6 * maxDistanceDelta, current.a + num4 / num6 * maxDistanceDelta);
        }
    }

    public static class ListTool
    {
        public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            T temp = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, temp);
        }

        public static void Change<T>(this List<T> list, int oldIndex, int newIndex)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            T temp = list[newIndex];
            list[newIndex] = list[oldIndex];
            list[oldIndex] = temp;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static byte CloseValue(this List<byte> list, byte target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => (x - target) < (y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static sbyte CloseValue(this List<sbyte> list, sbyte target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static short CloseValue(this List<short> list, short target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static ushort CloseValue(this List<ushort> list, ushort target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => (x - target) < (y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static int CloseValue(this List<int> list, int target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static uint CloseValue(this List<uint> list, uint target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => (x - target) < (y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static long CloseValue(this List<long> list, long target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static ulong CloseValue(this List<ulong> list, ulong target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => (x - target) < (y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static float CloseValue(this List<float> list, float target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static double CloseValue(this List<double> list, double target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static decimal CloseValue(this List<decimal> list, decimal target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<byte> list, byte target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<sbyte> list, sbyte target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<short> list, short target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<ushort> list, ushort target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<int> list, int target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<uint> list, uint target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<long> list, long target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<ulong> list, ulong target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<float> list, float target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<double> list, double target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<decimal> list, decimal target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<byte> list, byte target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<sbyte> list, sbyte target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<short> list, short target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<ushort> list, ushort target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<int> list, int target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<uint> list, uint target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<long> list, long target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<ulong> list, ulong target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<float> list, float target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<double> list, double target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<decimal> list, decimal target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.CloseValue(target));

            return 0;
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                    throw new InvalidOperationException("Empty sequence");

                var comparer = Comparer<TKey>.Default;
                TSource min = sourceIterator.Current;
                TKey minKey = selector(min);

                while (sourceIterator.MoveNext())
                {
                    TSource current = sourceIterator.Current;
                    TKey currentKey = selector(current);

                    if (comparer.Compare(currentKey, minKey) >= 0)
                        continue;

                    min = current;
                    minKey = currentKey;
                }

                return min;
            }
        }
    }

    public static class ArrayTool
    {
        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// byte
        /// </returns>
        public static byte CloseValue(this byte[] array, byte target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => (x - target) < (y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// sbyte
        /// </returns>
        public static sbyte CloseValue(this sbyte[] array, sbyte target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// short
        /// </returns>
        public static short CloseValue(this short[] array, short target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// ushort
        /// </returns>
        public static ushort CloseValue(this ushort[] array, ushort target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => (x - target) < (y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// int
        /// </returns>
        public static int CloseValue(this int[] array, int target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// uint
        /// </returns>
        public static uint CloseValue(this uint[] array, uint target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => (x - target) < (y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// long
        /// </returns>
        public static long CloseValue(this long[] array, long target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => (x - target) < (y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// ulong
        /// </returns>
        public static ulong CloseValue(this ulong[] array, ulong target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => (x - target) < (y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// float
        /// </returns>
        public static float CloseValue(this float[] array, float target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// double
        /// </returns>
        public static double CloseValue(this double[] array, double target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// double
        /// </returns>
        public static decimal CloseValue(this decimal[] array, decimal target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => (x - target).Abs() < (y - target).Abs() ? x : y);

            return 0;
        }
    }

    public static class StringTool
    {
        public static string ConstEnvironmentVariable(this string value)
        {
            if (value == null)
                return null;

            value = value.Replace("%DataPath%", Kernel.dataPath);
            value = value.Replace("%StreamingAssetsPath%", Kernel.streamingAssetsPath);
            value = value.Replace("%PersistentDataPath%", Kernel.persistentDataPath);

            value = value.Replace("%CompanyName%", Kernel.companyName);
            value = value.Replace("%ProductName%", Kernel.productName);
            value = value.Replace("%Version%", Kernel.version);

            return value;
        }

        /// <summary>
        /// (text = "AddSpacesToSentence") = "Add Spaces To Sentence"
        /// </summary>
        /// <param name="text">텍스트</param>
        /// <param name="preserveAcronyms">약어(준말) 보존 (true = (UnscaledFPSDeltaTime = Unscaled FPS Delta Time), false = (UnscaledFPSDeltaTime = Unscaled FPSDelta Time))</param>
        /// <returns></returns>
        public static string AddSpacesToSentence(this string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) || (preserveAcronyms && char.IsUpper(text[i - 1]) && i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }

        /// <summary>
        /// (keyCode = KeyCode.RightArrow) = "→"
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public static string KeyCodeToString(this KeyCode keyCode, bool simply = false)
        {
            string text;
            switch (keyCode)
            {
                case KeyCode.Escape:
                    text = "ESC";
                    break;
                case KeyCode.Return when !simply:
                    text = "Enter";
                    break;
                case KeyCode.Return when simply:
                    text = "↵";
                    break;
                case KeyCode.Alpha0:
                    text = "0";
                    break;
                case KeyCode.Alpha1:
                    text = "1";
                    break;
                case KeyCode.Alpha2:
                    text = "2";
                    break;
                case KeyCode.Alpha3:
                    text = "3";
                    break;
                case KeyCode.Alpha4:
                    text = "4";
                    break;
                case KeyCode.Alpha5:
                    text = "5";
                    break;
                case KeyCode.Alpha6:
                    text = "6";
                    break;
                case KeyCode.Alpha7:
                    text = "7";
                    break;
                case KeyCode.Alpha8:
                    text = "8";
                    break;
                case KeyCode.Alpha9:
                    text = "9";
                    break;
                case KeyCode.AltGr when simply:
                    text = "AG";
                    break;
                case KeyCode.Ampersand:
                    text = "&";
                    break;
                case KeyCode.Asterisk:
                    text = "*";
                    break;
                case KeyCode.At:
                    text = "@";
                    break;
                case KeyCode.BackQuote:
                    text = "`";
                    break;
                case KeyCode.Backslash:
                    text = "\\";
                    break;
                case KeyCode.Caret:
                    text = "^";
                    break;
                case KeyCode.Colon:
                    text = ":";
                    break;
                case KeyCode.Comma:
                    text = ",";
                    break;
                case KeyCode.Dollar:
                    text = "$";
                    break;
                case KeyCode.DoubleQuote:
                    text = "\"";
                    break;
                case KeyCode.Equals:
                    text = "=";
                    break;
                case KeyCode.Exclaim:
                    text = "!";
                    break;
                case KeyCode.Greater:
                    text = ">";
                    break;
                case KeyCode.Hash:
                    text = "#";
                    break;
                case KeyCode.Keypad0 when !simply:
                    text = "Keypad 0";
                    break;
                case KeyCode.Keypad1 when !simply:
                    text = "Keypad 1";
                    break;
                case KeyCode.Keypad2 when !simply:
                    text = "Keypad 2";
                    break;
                case KeyCode.Keypad3 when !simply:
                    text = "Keypad 3";
                    break;
                case KeyCode.Keypad4 when !simply:
                    text = "Keypad 4";
                    break;
                case KeyCode.Keypad5 when !simply:
                    text = "Keypad 5";
                    break;
                case KeyCode.Keypad6 when !simply:
                    text = "Keypad 6";
                    break;
                case KeyCode.Keypad7 when !simply:
                    text = "Keypad 7";
                    break;
                case KeyCode.Keypad8 when !simply:
                    text = "Keypad 8";
                    break;
                case KeyCode.Keypad9 when !simply:
                    text = "Keypad 9";
                    break;
                case KeyCode.KeypadDivide when !simply:
                    text = "Keypad /";
                    break;
                case KeyCode.KeypadEnter when !simply:
                    text = "Keypad ↵";
                    break;
                case KeyCode.KeypadEquals when !simply:
                    text = "Keypad =";
                    break;
                case KeyCode.KeypadMinus when !simply:
                    text = "Keypad -";
                    break;
                case KeyCode.KeypadMultiply when !simply:
                    text = "Keypad *";
                    break;
                case KeyCode.KeypadPeriod when !simply:
                    text = "Keypad .";
                    break;
                case KeyCode.KeypadPlus when !simply:
                    text = "Keypad +";
                    break;
                case KeyCode.Keypad0 when simply:
                    text = "K0";
                    break;
                case KeyCode.Keypad1 when simply:
                    text = "K1";
                    break;
                case KeyCode.Keypad2 when simply:
                    text = "K2";
                    break;
                case KeyCode.Keypad3 when simply:
                    text = "K3";
                    break;
                case KeyCode.Keypad4 when simply:
                    text = "K4";
                    break;
                case KeyCode.Keypad5 when simply:
                    text = "K5";
                    break;
                case KeyCode.Keypad6 when simply:
                    text = "K6";
                    break;
                case KeyCode.Keypad7 when simply:
                    text = "K7";
                    break;
                case KeyCode.Keypad8 when simply:
                    text = "K8";
                    break;
                case KeyCode.Keypad9 when simply:
                    text = "K9";
                    break;
                case KeyCode.KeypadDivide when simply:
                    text = "K/";
                    break;
                case KeyCode.KeypadEnter when simply:
                    text = "K↵";
                    break;
                case KeyCode.KeypadEquals when simply:
                    text = "K=";
                    break;
                case KeyCode.KeypadMinus when simply:
                    text = "K-";
                    break;
                case KeyCode.KeypadMultiply when simply:
                    text = "K*";
                    break;
                case KeyCode.KeypadPeriod when simply:
                    text = "K.";
                    break;
                case KeyCode.KeypadPlus when simply:
                    text = "K+";
                    break;
                case KeyCode.LeftApple:
                    text = "Left Command";
                    break;
                case KeyCode.LeftBracket:
                    text = "[";
                    break;
                case KeyCode.LeftCurlyBracket:
                    text = "{";
                    break;
                case KeyCode.LeftParen:
                    text = "(";
                    break;
                case KeyCode.LeftWindows when simply:
                    text = "LW";
                    break;
                case KeyCode.Less:
                    text = "<";
                    break;
                case KeyCode.Minus:
                    text = "-";
                    break;
                case KeyCode.Mouse0 when !simply:
                    text = "Left Mouse";
                    break;
                case KeyCode.Mouse1 when !simply:
                    text = "Right Mouse";
                    break;
                case KeyCode.Mouse2 when !simply:
                    text = "Middle Mouse";
                    break;
                case KeyCode.Mouse0 when simply:
                    text = "LM";
                    break;
                case KeyCode.Mouse1 when simply:
                    text = "RM";
                    break;
                case KeyCode.Mouse2 when simply:
                    text = "MM";
                    break;
                case KeyCode.Mouse3 when simply:
                    text = "M3";
                    break;
                case KeyCode.Mouse4 when simply:
                    text = "M4";
                    break;
                case KeyCode.Mouse5 when simply:
                    text = "M5";
                    break;
                case KeyCode.Mouse6 when simply:
                    text = "M6";
                    break;
                case KeyCode.Percent:
                    text = "%";
                    break;
                case KeyCode.Period:
                    text = ".";
                    break;
                case KeyCode.Pipe:
                    text = "|";
                    break;
                case KeyCode.Plus:
                    text = "+";
                    break;
                case KeyCode.Print when !simply:
                    text = "Print Screen";
                    break;
                case KeyCode.Print when simply:
                    text = "PS";
                    break;
                case KeyCode.Question:
                    text = "?";
                    break;
                case KeyCode.Quote:
                    text = "'";
                    break;
                case KeyCode.RightApple:
                    text = "Right Command";
                    break;
                case KeyCode.RightBracket:
                    text = "]";
                    break;
                case KeyCode.RightCurlyBracket:
                    text = "}";
                    break;
                case KeyCode.RightParen:
                    text = ")";
                    break;
                case KeyCode.RightWindows when simply:
                    text = "LW";
                    break;
                case KeyCode.Semicolon:
                    text = ";";
                    break;
                case KeyCode.Slash:
                    text = "/";
                    break;
                case KeyCode.Space when simply:
                    text = "␣";
                    break;
                case KeyCode.SysReq when !simply:
                    text = "Print Screen";
                    break;
                case KeyCode.SysReq when simply:
                    text = "PS";
                    break;
                case KeyCode.Tilde:
                    text = "~";
                    break;
                case KeyCode.Underscore:
                    text = "_";
                    break;
                case KeyCode.UpArrow:
                    text = "↑";
                    break;
                case KeyCode.DownArrow:
                    text = "↓";
                    break;
                case KeyCode.LeftArrow:
                    text = "←";
                    break;
                case KeyCode.RightArrow:
                    text = "→";
                    break;
                case KeyCode.LeftControl when !simply:
                    text = "Left Ctrl";
                    break;
                case KeyCode.RightControl when !simply:
                    text = "Right Ctrl";
                    break;
                case KeyCode.LeftControl when simply:
                    text = "LC";
                    break;
                case KeyCode.RightControl when simply:
                    text = "RC";
                    break;
                case KeyCode.LeftAlt when simply:
                    text = "LA";
                    break;
                case KeyCode.RightAlt when simply:
                    text = "RA";
                    break;
                case KeyCode.LeftShift when simply:
                    text = "L⇧";
                    break;
                case KeyCode.RightShift when simply:
                    text = "R⇧";
                    break;
                case KeyCode.Backspace when simply:
                    text = "B←";
                    break;
                case KeyCode.Delete when simply:
                    text = "D←";
                    break;
                case KeyCode.PageUp when simply:
                    text = "P↑";
                    break;
                case KeyCode.PageDown when simply:
                    text = "P↓";
                    break;
                default:
                    text = keyCode.ToString().AddSpacesToSentence();
                    break;
            }

            return text;
        }

        /// <summary>
        /// (value = 5, max = 10, length = 10) = "■■■■■□□□□□"
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ToBar(this int value, int max, int length, string fill = "■", string half = "▣", string empty = "□") => ToBar((double)value, max, length, fill, half, empty);

        /// <summary>
        /// (value = 5.5, max = 10, length = 10) = "■■■■■▣□□□□"
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ToBar(this float value, float max, int length, string fill = "■", string half = "▣", string empty = "□") => ToBar((double)value, max, length, fill, half, empty);

        /// <summary>
        /// (value = 5.5, max = 10, length = 10) = "■■■■■▣□□□□"
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ToBar(this double value, double max, int length, string fill = "■", string half = "▣", string empty = "□")
        {
            if (fill == null)
                fill = "";
            if (half == null)
                half = "";
            if (empty == null)
                empty = "";

            string text = "";

            for (double i = 0.5; i < length + 0.5; i++)
            {
                if (value / max >= i / length)
                    text += fill;
                else
                {
                    if (value / max >= (i - 0.5) / length)
                        text += half;
                    else
                        text += empty;
                }
            }
            return text;
        }
    }

    public static class PathTool
    {
        /// <summary>
        /// (paths = ("asdf", "asdf")) = "asdf/asdf" (Path.Combine is "asdf\asdf")
        /// </summary>
        /// <param name="paths">경로</param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
            if (paths == null || paths.Length < 0)
                throw new NullReferenceException();

            string path = "";
            if (paths.Length == 1)
                path = paths[0];
            else
            {
                for (int i = 0; i < paths.Length - 1; i++)
                {
                    string tempPath = paths[i];
                    string tempPath2 = paths[i + 1];
                    if (tempPath == null || tempPath == "")
                        continue;
                    else if (tempPath2 == null || tempPath2 == "")
                        continue;


                    if (tempPath[tempPath.Length - 1] == '/' && tempPath2[0] == '/')
                    {
                        if (i == 0)
                            path += tempPath;
                        path += tempPath2.Substring(1, tempPath2.Length - 2);
                    }
                    else if (tempPath[tempPath.Length - 1] == '/' && tempPath2[0] != '/')
                    {
                        if (i == 0)
                            path += tempPath;
                        path += tempPath2;
                    }
                    else if (tempPath[tempPath.Length - 1] != '/' && tempPath2[0] == '/')
                    {
                        if (i == 0)
                            path += tempPath;
                        path += tempPath2;
                    }
                    else if (tempPath[tempPath.Length - 1] != '/' && tempPath2[0] != '/')
                    {
                        if (i == 0)
                            path += tempPath;
                        path += "/" + tempPath2;
                    }
                }
            }

            if (!(path == null || path == "") && path[path.Length - 1] == '/')
                path = path.Substring(1, path.Length - 2);

            return path;
        }
    }

    public static class TimeTool
    {
        /// <summary>
        /// (second = 70) = "1:10"
        /// </summary>
        /// <param name="second">초</param>
        /// <param name="minuteAlwayShow">분 단위 항상 표시</param>
        /// <param name="hourAlwayShow">시간, 분 단위 항상 표시</param>
        /// <param name="dayAlwayShow">하루, 시간, 분 단위 항상 표시</param>
        /// <returns></returns>
        public static string ToTime(this int second, bool minuteAlwayShow = false, bool hourAlwayShow = false, bool dayAlwayShow = false)
        {
            try
            {
                int secondAbs = second.Abs();
                if (second <= TimeSpan.MinValue.TotalSeconds)
                    return TimeSpan.MinValue.ToString(@"d\:hh\:mm\:ss");
                else if (second > TimeSpan.MaxValue.TotalSeconds)
                    return TimeSpan.MaxValue.ToString(@"d\:h\:mm\:ss");
                else if (secondAbs >= 86400 || dayAlwayShow)
                    return TimeSpan.FromSeconds(second).ToString(@"d\:hh\:mm\:ss");
                else if (secondAbs >= 3600 || hourAlwayShow)
                    return TimeSpan.FromSeconds(second).ToString(@"h\:mm\:ss");
                else if (secondAbs >= 60 || minuteAlwayShow)
                    return TimeSpan.FromSeconds(second).ToString(@"m\:ss");
                else
                    return TimeSpan.FromSeconds(second).ToString(@"s");
            }
            catch (Exception) { return "--:--"; }
        }

        /// <summary>
        /// (second = 70.1f) = "1:10.1"
        /// </summary>
        /// <param name="second">초</param>
        /// <param name="decimalShow">소수 표시</param>
        /// <param name="minuteAlwayShow">분 단위 항상 표시</param>
        /// <param name="hourAlwayShow">시간, 분 단위 항상 표시</param>
        /// <param name="dayAlwayShow">하루, 시간, 분 단위 항상 표시</param>
        /// <returns></returns>
        public static string ToTime(this float second, bool decimalShow = true, bool minuteAlwayShow = false, bool hourAlwayShow = false, bool dayAlwayShow = false) => ToTime((double)second, decimalShow, minuteAlwayShow, hourAlwayShow, dayAlwayShow);

        /// <summary>
        /// (second = 70.1) = "1:10.1"
        /// </summary>
        /// <param name="second">초</param>
        /// <param name="decimalShow">소수 표시</param>
        /// <param name="minuteAlwayShow">분 단위 항상 표시</param>
        /// <param name="hourAlwayShow">시간, 분 단위 항상 표시</param>
        /// <param name="dayAlwayShow">하루, 시간, 분 단위 항상 표시</param>
        /// <returns></returns>
        public static string ToTime(this double second, bool decimalShow = true, bool minuteAlwayShow = false, bool hourAlwayShow = false, bool dayAlwayShow = false)
        {
            try
            {
                double secondAbs = second.Abs();
                if (decimalShow)
                {
                    if (second <= TimeSpan.MinValue.TotalSeconds || secondAbs == float.NegativeInfinity)
                        return TimeSpan.MinValue.ToString(@"d\:hh\:mm\:ss\.ff");
                    else if (second > TimeSpan.MaxValue.TotalSeconds || secondAbs == float.PositiveInfinity)
                        return TimeSpan.FromSeconds(TimeSpan.MaxValue.TotalSeconds).ToString(@"d\:hh\:mm\:ss\.ff");
                    else if (secondAbs >= 86400 || dayAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"d\:hh\:mm\:ss\.ff");
                    else if (secondAbs >= 3600 || hourAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"h\:mm\:ss\.ff");
                    else if (secondAbs >= 60 || minuteAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"m\:ss\.ff");
                    else
                        return TimeSpan.FromSeconds(second).ToString(@"s\.ff");
                }
                else
                {
                    if (second <= TimeSpan.MinValue.TotalSeconds || secondAbs == float.NegativeInfinity)
                        return TimeSpan.MinValue.ToString(@"d\:hh\:mm\:ss");
                    else if (second > TimeSpan.MaxValue.TotalSeconds || secondAbs == float.PositiveInfinity)
                        return TimeSpan.FromSeconds(TimeSpan.MaxValue.TotalSeconds).ToString(@"d\:h\:mm\:ss");
                    else if (secondAbs >= 86400 || dayAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"d\:hh\:mm\:ss");
                    else if (secondAbs >= 3600 || hourAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"h\:mm\:ss");
                    else if (secondAbs >= 60 || minuteAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"m\:ss");
                    else
                        return TimeSpan.FromSeconds(second).ToString(@"s");
                }
            }
            catch (Exception) { return "--:--"; }
        }

        public static DateTime ToLunarDate(this DateTime dateTime)
        {
            KoreanLunisolarCalendar klc = new KoreanLunisolarCalendar();

            int year = klc.GetYear(dateTime);
            int month = klc.GetMonth(dateTime);
            int day = klc.GetDayOfMonth(dateTime);

            //1년이 12이상이면 윤달이 있음..
            if (klc.GetMonthsInYear(year) > 12)
            {
                //년도의 윤달이 몇월인지?
                int leapMonth = klc.GetLeapMonth(year);

                //달이 윤월보다 같거나 크면 -1을 함 즉 윤8은->9 이기때문
                if (month >= leapMonth)
                    month--;
            }

            return new DateTime(year, month, day);
        }

        public static DateTime ToSolarDate(this DateTime dateTime, bool isLeapMonth = false)
        {
            KoreanLunisolarCalendar klc = new KoreanLunisolarCalendar();

            int year = dateTime.Year;
            int month = dateTime.Month;
            int day = dateTime.Day;

            if (klc.GetMonthsInYear(year) > 12)
            {
                int leapMonth = klc.GetLeapMonth(year);

                if (month > leapMonth - 1)
                    month++;
                else if (month == leapMonth - 1 && isLeapMonth)
                    month++;
            }

            return klc.ToDateTime(year, month, day, 0, 0, 0, 0);
        }
    }

    public static class ComponentTool
    {
        public static T GetComponentFieldSave<T>(this Component component, T fieldToSave, GetComponentMode mode = GetComponentMode.addIfNull) where T : Component
        {
            if (fieldToSave == null || fieldToSave.gameObject != component.gameObject)
            {
                fieldToSave = component.GetComponent<T>();
                if (fieldToSave == null)
                {
                    if (mode == GetComponentMode.addIfNull)
                        return component.gameObject.AddComponent<T>();
                    else if (mode == GetComponentMode.destroyIfNull)
                    {
                        UnityEngine.Object.DestroyImmediate(component);
                        return null;
                    }
                }
            }

            return fieldToSave;
        }

        public enum GetComponentMode
        {
            none,
            addIfNull,
            destroyIfNull
        }
    }
}