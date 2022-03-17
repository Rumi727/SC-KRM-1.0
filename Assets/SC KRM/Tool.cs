using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SCKRM.Tool
{
    public static class MathfTool
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

        public static Color Lerp(this Color current, Color target, float t, bool unclamped = false)
        {
            if (!unclamped)
                t.Clamp01Ref();
            return new Color(current.r + (target.r - current.r) * t, current.g + (target.g - current.g) * t, current.b + (target.b - current.b) * t, current.a + (target.a - current.a) * t);
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
            Vector2 a = target - current;
            float magnitude = a.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;

            return current + a / magnitude * maxDistanceDelta;
        }
        public static Vector3 MoveTowards(this Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            Vector3 a = target - current;
            float magnitude = a.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;

            return current + a / magnitude * maxDistanceDelta;
        }
        public static Vector4 MoveTowards(this Vector4 current, Vector4 target, float maxDistanceDelta)
        {
            Vector4 a = target - current;
            float magnitude = a.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;

            return current + a / magnitude * maxDistanceDelta;
        }
        public static Rect MoveTowards(this Rect current, Rect target, float maxDistanceDelta)
        {
            Rect a = new Rect(target.x - current.x, target.y - current.y, target.width - current.width, target.height - current.height);
            float magnitude = Mathf.Sqrt((a.x * a.x) + (a.y * a.y) + (a.width * a.width) + (a.height * a.height));
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;

            return new Rect(current.x + (a.x / magnitude * maxDistanceDelta), current.y + (a.y / magnitude * maxDistanceDelta), current.width + (a.width / magnitude * maxDistanceDelta), current.height + (a.height / magnitude * maxDistanceDelta));
        }
        public static Color MoveTowards(this Color current, Color target, float maxDistanceDelta)
        {
            Color a = target - current;
            float magnitude = Mathf.Sqrt((a.r * a.r) + (a.g * a.g) + (a.b * a.b) + (a.a * a.a));
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;

            return current + a / magnitude * maxDistanceDelta;
        }
    }

    public static class MathfRefTool
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
        }

        public static void SignRef(this ref short value)
        {
            if (value < 0)
                value = -1;
        }

        public static void SignRef(this ref int value)
        {
            if (value < 0)
                value = -1;
        }

        public static void SignRef(this ref long value)
        {
            if (value < 0)
                value = -1;
        }

        public static void SignRef(this ref float value)
        {
            if (value < 0)
                value = -1;
        }

        public static void SignRef(this ref double value)
        {
            if (value < 0)
                value = -1;
        }

        public static void SignRef(this ref decimal value)
        {
            if (value < 0)
                value = -1;
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
            Vector2 a = target - current;
            float magnitude = a.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
            {
                current = target;
                return;
            }

            current += a / magnitude * maxDistanceDelta;
        }
        public static void MoveTowardsRef(this ref Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            Vector3 a = target - current;
            float magnitude = a.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
            {
                current = target;
                return;
            }

            current += a / magnitude * maxDistanceDelta;
        }
        public static void MoveTowardsRef(this ref Vector4 current, Vector4 target, float maxDistanceDelta)
        {
            Vector4 a = target - current;
            float magnitude = a.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
            {
                current = target;
                return;
            }

            current += a / magnitude * maxDistanceDelta;
        }
        public static void MoveTowardsRef(this ref Rect current, Rect target, float maxDistanceDelta)
        {
            Rect a = new Rect(target.x - current.x, target.y - current.y, target.width - current.width, target.height - current.height);
            float magnitude = Mathf.Sqrt((a.x * a.x) + (a.y * a.y) + (a.width * a.width) + (a.height * a.height));
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
            {
                current = target;
                return;
            }

            current = new Rect(current.x + (a.x / magnitude * maxDistanceDelta), current.y + (a.y / magnitude * maxDistanceDelta), current.width + (a.width / magnitude * maxDistanceDelta), current.height + (a.height / magnitude * maxDistanceDelta));
        }
        public static void MoveTowardsRef(this ref Color current, Color target, float maxDistanceDelta)
        {
            Color a = target - current;
            float magnitude = Mathf.Sqrt((a.r * a.r) + (a.g * a.g) + (a.b * a.b) + (a.a * a.a));
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
            {
                current = target;
                return;
            }

            current += a / magnitude * maxDistanceDelta;
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
        public static T[] Add<T>(this T[] array, T item)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T[] tempArray = new T[array.Length + 1];

            for (int i = 0; i < array.Length; i++)
                tempArray[i] = array[i];
            tempArray[tempArray.Length - 1] = item;

            return tempArray;
        }

        public static T[] Insert<T>(this T[] array, int index, T item)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T[] tempArray = new T[array.Length + 1];
            bool insert = false;
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (i != index)
                {
                    if (!insert)
                        tempArray[i] = array[i];
                    else
                        tempArray[i] = array[i - 1];
                }
                else
                {
                    tempArray[i] = item;
                    insert = true;
                }
            }

            return tempArray;
        }

        public static T[] Remove<T>(this T[] array, T item)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T[] tempArray = new T[array.Length - 1];
            int remove = 0;
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (!item.Equals(tempArray[i]))
                    tempArray[i - remove] = array[i];
                else
                {
                    tempArray[i] = array[i + 1];
                    remove++;
                }
            }

            return tempArray;
        }

        public static T[] RemoveAt<T>(this T[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T[] tempArray = new T[array.Length - 1];

            bool remove = false;
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (i != index)
                {
                    if (!remove)
                        tempArray[i] = array[i];
                    else
                        tempArray[i - 1] = array[i];
                }
                else
                {
                    tempArray[i] = array[i + 1];
                    remove = true;
                }
            }

            return tempArray;
        }

        public static T[] Move<T>(this T[] array, int oldIndex, int newIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T temp = array[oldIndex];
            array = array.RemoveAt(oldIndex);
            return array.Insert(newIndex, temp);
        }

        public static T[] Change<T>(this T[] array, int oldIndex, int newIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T temp = array[newIndex];
            array[newIndex] = array[oldIndex];
            array[oldIndex] = temp;
            return array;
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

            value = value.Replace("%Platform%", Kernel.platform.ToString());

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
        public static string KeyCodeToString(this KeyCode keyCode)
        {
            string text;
            if (keyCode == KeyCode.Escape)
                text = "None";
            else if (keyCode == KeyCode.Return)
                text = "␣";
            else if (keyCode == KeyCode.Alpha0)
                text = "0";
            else if (keyCode == KeyCode.Alpha1)
                text = "1";
            else if (keyCode == KeyCode.Alpha2)
                text = "2";
            else if (keyCode == KeyCode.Alpha3)
                text = "3";
            else if (keyCode == KeyCode.Alpha4)
                text = "4";
            else if (keyCode == KeyCode.Alpha5)
                text = "5";
            else if (keyCode == KeyCode.Alpha6)
                text = "6";
            else if (keyCode == KeyCode.Alpha7)
                text = "7";
            else if (keyCode == KeyCode.Alpha8)
                text = "8";
            else if (keyCode == KeyCode.Alpha9)
                text = "9";
            else if (keyCode == KeyCode.AltGr)
                text = "AG";
            else if (keyCode == KeyCode.Ampersand)
                text = "&";
            else if (keyCode == KeyCode.Asterisk)
                text = "*";
            else if (keyCode == KeyCode.At)
                text = "@";
            else if (keyCode == KeyCode.BackQuote)
                text = "`";
            else if (keyCode == KeyCode.Backslash)
                text = "\\";
            else if (keyCode == KeyCode.Caret)
                text = "^";
            else if (keyCode == KeyCode.Colon)
                text = ":";
            else if (keyCode == KeyCode.Comma)
                text = ",";
            else if (keyCode == KeyCode.Dollar)
                text = "$";
            else if (keyCode == KeyCode.DoubleQuote)
                text = "\"";
            else if (keyCode == KeyCode.Equals)
                text = "=";
            else if (keyCode == KeyCode.Exclaim)
                text = "!";
            else if (keyCode == KeyCode.Greater)
                text = ">";
            else if (keyCode == KeyCode.Hash)
                text = "#";
            else if (keyCode == KeyCode.Keypad0)
                text = "0";
            else if (keyCode == KeyCode.Keypad1)
                text = "1";
            else if (keyCode == KeyCode.Keypad2)
                text = "2";
            else if (keyCode == KeyCode.Keypad3)
                text = "3";
            else if (keyCode == KeyCode.Keypad4)
                text = "4";
            else if (keyCode == KeyCode.Keypad5)
                text = "5";
            else if (keyCode == KeyCode.Keypad6)
                text = "6";
            else if (keyCode == KeyCode.Keypad7)
                text = "7";
            else if (keyCode == KeyCode.Keypad8)
                text = "8";
            else if (keyCode == KeyCode.Keypad9)
                text = "9";
            else if (keyCode == KeyCode.KeypadDivide)
                text = "/";
            else if (keyCode == KeyCode.KeypadEnter)
                text = "↵";
            else if (keyCode == KeyCode.KeypadEquals)
                text = "=";
            else if (keyCode == KeyCode.KeypadMinus)
                text = "-";
            else if (keyCode == KeyCode.KeypadMultiply)
                text = "*";
            else if (keyCode == KeyCode.KeypadPeriod)
                text = ".";
            else if (keyCode == KeyCode.KeypadPlus)
                text = "+";
            else if (keyCode == KeyCode.LeftApple)
                text = "Left Command";
            else if (keyCode == KeyCode.LeftBracket)
                text = "[";
            else if (keyCode == KeyCode.LeftCurlyBracket)
                text = "{";
            else if (keyCode == KeyCode.LeftParen)
                text = "(";
            else if (keyCode == KeyCode.Less)
                text = "<";
            else if (keyCode == KeyCode.Minus)
                text = "-";
            else if (keyCode == KeyCode.Mouse0)
                text = "LM";
            else if (keyCode == KeyCode.Mouse1)
                text = "RM";
            else if (keyCode == KeyCode.Mouse2)
                text = "MM";
            else if (keyCode == KeyCode.Mouse3)
                text = "3M";
            else if (keyCode == KeyCode.Mouse4)
                text = "4M";
            else if (keyCode == KeyCode.Mouse5)
                text = "5M";
            else if (keyCode == KeyCode.Mouse6)
                text = "6M";
            else if (keyCode == KeyCode.Percent)
                text = "%";
            else if (keyCode == KeyCode.Period)
                text = ".";
            else if (keyCode == KeyCode.Pipe)
                text = "|";
            else if (keyCode == KeyCode.Plus)
                text = "+";
            else if (keyCode == KeyCode.Question)
                text = "?";
            else if (keyCode == KeyCode.Quote)
                text = "'";
            else if (keyCode == KeyCode.RightApple)
                text = "Right Command";
            else if (keyCode == KeyCode.RightBracket)
                text = "]";
            else if (keyCode == KeyCode.RightCurlyBracket)
                text = "}";
            else if (keyCode == KeyCode.RightParen)
                text = ")";
            else if (keyCode == KeyCode.Semicolon)
                text = ";";
            else if (keyCode == KeyCode.Slash)
                text = "/";
            else if (keyCode == KeyCode.SysReq)
                text = "Print Screen";
            else if (keyCode == KeyCode.Tilde)
                text = "~";
            else if (keyCode == KeyCode.Underscore)
                text = "_";
            else if (keyCode == KeyCode.UpArrow)
                text = "↑";
            else if (keyCode == KeyCode.DownArrow)
                text = "↓";
            else if (keyCode == KeyCode.LeftArrow)
                text = "←";
            else if (keyCode == KeyCode.RightArrow)
                text = "→";
            else if (keyCode == KeyCode.LeftControl)
                text = "LC";
            else if (keyCode == KeyCode.RightControl)
                text = "RC";
            else if (keyCode == KeyCode.LeftAlt)
                text = "LA";
            else if (keyCode == KeyCode.RightAlt)
                text = "RA";
            else if (keyCode == KeyCode.LeftShift)
                text = "L⇧";
            else if (keyCode == KeyCode.RightShift)
                text = "R⇧";
            else if (keyCode == KeyCode.Backspace)
                text = "B←";
            else if (keyCode == KeyCode.Delete)
                text = "D←";
            else if (keyCode == KeyCode.PageUp)
                text = "P↑";
            else if (keyCode == KeyCode.PageDown)
                text = "P↓";
            else
                text = keyCode.ToString();

            return text.AddSpacesToSentence();
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
}