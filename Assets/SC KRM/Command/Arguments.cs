using Brigadier.NET;
using Brigadier.NET.ArgumentTypes;
using Brigadier.NET.Context;
using Brigadier.NET.Exceptions;
using SCKRM.Renderer;
using SCKRM.Resource;
using System;
using System.Linq;
using UnityEngine;

namespace SCKRM.Command
{
    public static class Arguments
    {
        public static IntegerArgumentType Integer(int min = int.MinValue, int max = int.MaxValue) => Brigadier.NET.Arguments.Integer(min, max);
        public static int GetInteger<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<int>(name);

        public static BoolArgumentType Bool() => Brigadier.NET.Arguments.Bool();
        public static bool GetBool<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<bool>(name);

        public static DoubleArgumentType Double(double min = double.MinValue, double max = double.MaxValue) => Brigadier.NET.Arguments.Double(min, max);
        public static double GetDouble<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<double>(name);

        public static FloatArgumentType Float(float min = float.MinValue, float max = float.MaxValue) => Brigadier.NET.Arguments.Float(min, max);
        public static float GetFloat<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<float>(name);

        public static LongArgumentType Long(long min = long.MinValue, long max = long.MaxValue) => Brigadier.NET.Arguments.Long(min, max);
        public static long GetLong<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<long>(name);

        public static StringArgumentType Word() => Brigadier.NET.Arguments.Word();
        public static StringArgumentType String() => Brigadier.NET.Arguments.String();
        public static StringArgumentType GreedyString() => Brigadier.NET.Arguments.GreedyString();
        public static string GetString<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<string>(name);

        public static Vector2ArgumentType Vector2(float min = float.MinValue, float max = float.MaxValue) => new Vector2ArgumentType(min, max);
        public static Vector2 GetVector2<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Vector2>(name);

        public static Vector2IntArgumentType Vector2Int(int min = int.MinValue, int max = int.MaxValue) => new Vector2IntArgumentType(min, max);
        public static Vector2Int GetVector2Int<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Vector2Int>(name);

        public static Vector3ArgumentType Vector3(float min = float.MinValue, float max = float.MaxValue) => new Vector3ArgumentType(min, max);
        public static Vector3 GetVector3<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Vector3>(name);

        public static Vector3IntArgumentType Vector3Int(int min = int.MinValue, int max = int.MaxValue) => new Vector3IntArgumentType(min, max);
        public static Vector3Int GetVector3Int<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Vector3Int>(name);

        public static Vector4ArgumentType Vector4(float min = float.MinValue, float max = float.MaxValue) => new Vector4ArgumentType(min, max);
        public static Vector4 GetVector4<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Vector4>(name);

        public static RectArgumentType Rect(float min = float.MinValue, float max = float.MaxValue) => new RectArgumentType(min, max);
        public static Rect GetRect<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Rect>(name);

        public static RectIntArgumentType RectInt(int min = int.MinValue, int max = int.MaxValue) => new RectIntArgumentType(min, max);
        public static RectInt GetRectInt<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<RectInt>(name);

        public static ColorArgumentType Color() => new ColorArgumentType();
        public static Color GetColor<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Color>(name);

        public static ColorAlphaArgumentType ColorAlpha() => new ColorAlphaArgumentType();
        public static Color GetColorAlpha<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Color>(name);

        public static TransformArgumentType Transform() => new TransformArgumentType();
        public static Transform GetTransform<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Transform>(name);

        public static TransformsStringArgumentType Transforms() => new TransformsStringArgumentType();
        public static Transform[] GetTransforms<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Transform[]>(name);

        public static GameObjectArgumentType GameObject() => new GameObjectArgumentType();
        public static GameObject GetGameObject<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<GameObject>(name);

        public static GameObjectsStringArgumentType GameObjects() => new GameObjectsStringArgumentType();
        public static GameObject[] GetGameObjects<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<GameObject[]>(name);

        public static SpriteArgumentType Sprite() => new SpriteArgumentType();
        public static Sprite GetSprite<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Sprite>(name);

        public static SpritesArgumentType Sprites() => new SpritesArgumentType();
        public static Sprite[] GetSprites<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<Sprite[]>(name);

        public static NameSpacePathPairArgumentType NameSpacePathPair() => new NameSpacePathPairArgumentType();
        public static NameSpacePathPair GetNameSpacePathPair<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<NameSpacePathPair>(name);

        public static NameSpaceTypePathPairArgumentType NameSpaceTypePathPair() => new NameSpaceTypePathPairArgumentType();
        public static NameSpaceTypePathPair GetNameSpaceTypePathPair<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<NameSpaceTypePathPair>(name);

        public static NameSpaceIndexTypePathPairArgumentType NameSpaceIndexTypePathPair() => new NameSpaceIndexTypePathPairArgumentType();
        public static NameSpaceIndexTypePathPair GetNameSpaceIndexTypePathPair<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<NameSpaceIndexTypePathPair>(name);

        public static PosSwizzleArgumentType PosSwizzle() => new PosSwizzleArgumentType();
        public static PosSwizzleEnum GetPosSwizzle<TSource>(CommandContext<TSource> context, string name) => context.GetArgument<PosSwizzleEnum>(name);

        [Flags]
        public enum PosSwizzleEnum
        {
            none = 0,
            x = 1 << 1,
            y = 1 << 2,
            z = 1 << 3,

            all = x | y | z
        }
    }

    public abstract class BaseArgumentType<T, TResult> : ArgumentType<TResult> where T : BaseArgumentType<T, TResult>
    {
        internal BaseArgumentType()
        {
        }

        public override bool Equals(object o) => o is T;

        public override int GetHashCode() => 0;

        public override string ToString() => $"{typeof(TResult).Name}()";
    }

    public abstract class BaseDuplicateIntArgumentType<T, TResult> : BaseArgumentType<BaseDuplicateIntArgumentType<T, TResult>, TResult> where T : BaseDuplicateIntArgumentType<T, TResult>
    {
        public virtual int minimum { get; }
        public virtual int maximum { get; }

        internal BaseDuplicateIntArgumentType(int minimum, int maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public void MinMaxThrow(IStringReader reader, int cursor, int value)
        {
            if (value < minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooLow().CreateWithContext(reader, value, minimum);
            }

            if (value > maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooHigh().CreateWithContext(reader, value, maximum);
            }
        }

        public void CanReadThrow(IStringReader reader, int firstCursor)
        {
            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            else if (o is not T)
                return false;

            T baseDuplicateIntArgument = (T)o;
            if (minimum == baseDuplicateIntArgument.minimum && maximum == baseDuplicateIntArgument.maximum)
                return true;

            return false;
        }

        public override int GetHashCode() => (int)(31f * minimum + maximum);

        public override string ToString()
        {
            if (minimum == int.MinValue && maximum == int.MaxValue)
                return typeof(TResult).Name + "()";

            if (maximum == int.MaxValue)
                return $"{typeof(TResult).Name}({minimum})";

            return $"{typeof(TResult).Name}({minimum}, {maximum})";
        }
    }

    public abstract class BaseDuplicateFloatArgumentType<T, TResult> : BaseArgumentType<BaseDuplicateFloatArgumentType<T, TResult>, TResult> where T : BaseDuplicateFloatArgumentType<T, TResult>
    {
        public virtual float minimum { get; }
        public virtual float maximum { get; }

        internal BaseDuplicateFloatArgumentType(float minimum, float maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public void MinMaxThrow(IStringReader reader, int cursor, float value)
        {
            if (value < minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, value, minimum);
            }

            if (value > maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, value, maximum);
            }
        }

        public void CanReadThrow(IStringReader reader, int firstCursor)
        {
            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            else if (o is not T)
                return false;

            T baseDuplicateFloatArgument = (T)o;
            if (minimum == baseDuplicateFloatArgument.minimum && maximum == baseDuplicateFloatArgument.maximum)
                return true;

            return false;
        }

        public override int GetHashCode() => (int)(31f * minimum + maximum);

        public override string ToString()
        {
            if (minimum == float.MinValue && maximum == float.MaxValue)
                return typeof(TResult) + "()";

            if (maximum == float.MaxValue)
                return $"{typeof(TResult)}({minimum:#.0})";

            return $"{typeof(TResult)}({minimum:#.0}, {maximum:#.0})";
        }
    }

    public class Vector2ArgumentType : BaseDuplicateFloatArgumentType<Vector2ArgumentType, Vector2>
    {
        internal Vector2ArgumentType(float minimum, float maximum) : base(minimum, maximum)
        {
        }

        public override Vector2 Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float x = reader.ReadFloat();
            MinMaxThrow(reader, cursor, x);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float y = reader.ReadFloat();
            MinMaxThrow(reader, cursor, y);

            return new Vector2(x, y);
        }
    }

    public class Vector2IntArgumentType : BaseDuplicateIntArgumentType<Vector2IntArgumentType, Vector2Int>
    {
        internal Vector2IntArgumentType(int minimum, int maximum) : base(minimum, maximum)
        {
        }

        public override Vector2Int Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            int x = reader.ReadInt();
            MinMaxThrow(reader, cursor, x);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            int y = reader.ReadInt();
            MinMaxThrow(reader, cursor, y);

            return new Vector2Int(x, y);
        }
    }

    public class Vector3ArgumentType : BaseDuplicateFloatArgumentType<Vector3ArgumentType, Vector3>
    {
        internal Vector3ArgumentType(float minimum, float maximum) : base(minimum, maximum)
        {
        }

        public override Vector3 Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float x = reader.ReadFloat();
            MinMaxThrow(reader, cursor, x);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float y = reader.ReadFloat();
            MinMaxThrow(reader, cursor, y);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float z = reader.ReadFloat();
            MinMaxThrow(reader, cursor, z);

            return new Vector3(x, y, z);
        }
    }

    public class Vector3IntArgumentType : BaseDuplicateIntArgumentType<Vector3IntArgumentType, Vector3Int>
    {
        internal Vector3IntArgumentType(int minimum, int maximum) : base(minimum, maximum)
        {
        }

        public override Vector3Int Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            int x = reader.ReadInt();
            MinMaxThrow(reader, cursor, x);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            int y = reader.ReadInt();
            MinMaxThrow(reader, cursor, y);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            int z = reader.ReadInt();
            MinMaxThrow(reader, cursor, z);

            return new Vector3Int(x, y, z);
        }
    }

    public class Vector4ArgumentType : BaseDuplicateFloatArgumentType<Vector4ArgumentType, Vector4>
    {
        internal Vector4ArgumentType(float minimum, float maximum) : base(minimum, maximum)
        {
        }

        public override Vector4 Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float x = reader.ReadFloat();
            MinMaxThrow(reader, cursor, x);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float y = reader.ReadFloat();
            MinMaxThrow(reader, cursor, y);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float z = reader.ReadFloat();
            MinMaxThrow(reader, cursor, z);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float w = reader.ReadFloat();
            MinMaxThrow(reader, cursor, w);

            return new Vector4(x, y, z, w);
        }
    }

    public class RectArgumentType : BaseDuplicateFloatArgumentType<RectArgumentType, Rect>
    {
        internal RectArgumentType(float minimum, float maximum) : base(minimum, maximum)
        {
        }

        public override Rect Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float x = reader.ReadFloat();
            MinMaxThrow(reader, cursor, x);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float y = reader.ReadFloat();
            MinMaxThrow(reader, cursor, y);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float width = reader.ReadFloat();
            MinMaxThrow(reader, cursor, width);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float height = reader.ReadFloat();
            MinMaxThrow(reader, cursor, height);

            return new Rect(x, y, width, height);
        }
    }

    public class RectIntArgumentType : BaseDuplicateIntArgumentType<RectIntArgumentType, RectInt>
    {
        internal RectIntArgumentType(int minimum, int maximum) : base(minimum, maximum)
        {
        }

        public override RectInt Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            int x = reader.ReadInt();
            MinMaxThrow(reader, cursor, x);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            int y = reader.ReadInt();
            MinMaxThrow(reader, cursor, y);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            int width = reader.ReadInt();
            MinMaxThrow(reader, cursor, width);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            int height = reader.ReadInt();
            MinMaxThrow(reader, cursor, height);

            return new RectInt(x, y, width, height);
        }
    }

    public class ColorArgumentType : BaseDuplicateFloatArgumentType<ColorArgumentType, Color>
    {
        public override float minimum => 0;
        public override float maximum => 255;

        internal ColorArgumentType() : base(0, 255)
        {
        }

        public override Color Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float r = reader.ReadFloat();
            MinMaxThrow(reader, cursor, r);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float g = reader.ReadFloat();
            MinMaxThrow(reader, cursor, g);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float b = reader.ReadFloat();
            MinMaxThrow(reader, cursor, b);

            return new Color(r, g, b);
        }
    }

    public class ColorAlphaArgumentType : BaseDuplicateFloatArgumentType<ColorAlphaArgumentType, Color>
    {
        public override float minimum => 0;
        public override float maximum => 255;

        internal ColorAlphaArgumentType() : base(0, 255)
        {

        }

        public override Color Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float r = reader.ReadFloat();
            MinMaxThrow(reader, cursor, r);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float g = reader.ReadFloat();
            MinMaxThrow(reader, cursor, g);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float b = reader.ReadFloat();
            MinMaxThrow(reader, cursor, b);
            CanReadThrow(reader, firstCursor);

            cursor = ++reader.Cursor;
            float a = reader.ReadFloat();
            MinMaxThrow(reader, cursor, a);

            return new Color(r, g, b, a);
        }
    }

    public class TransformArgumentType : BaseArgumentType<TransformArgumentType, Transform>
    {
        public override Transform Parse(IStringReader reader)
        {
            int hashCode = reader.ReadInt();
            return UnityEngine.Object.FindObjectsOfType<Transform>().First(x => x.GetHashCode() == hashCode);
        }
    }

    public class TransformsStringArgumentType : BaseArgumentType<TransformsStringArgumentType, Transform[]>
    {
        public override Transform[] Parse(IStringReader reader)
        {
            string name = reader.ReadString();
            return UnityEngine.Object.FindObjectsOfType<Transform>().Where(x => x.name == name).ToArray();
        }
    }

    public class GameObjectArgumentType : BaseArgumentType<GameObjectArgumentType, GameObject>
    {
        public override GameObject Parse(IStringReader reader)
        {
            int hashCode = reader.ReadInt();
            return UnityEngine.Object.FindObjectsOfType<GameObject>().First(x => x.GetHashCode() == hashCode);
        }
    }

    public class GameObjectsStringArgumentType : BaseArgumentType<GameObjectsStringArgumentType, GameObject[]>
    {
        public override GameObject[] Parse(IStringReader reader)
        {
            string name = reader.ReadString();
            return UnityEngine.Object.FindObjectsOfType<GameObject>().Where(x => x.name == name).ToArray();
        }
    }

    public class SpriteArgumentType : BaseArgumentType<SpriteArgumentType, Sprite>
    {
        public override Sprite Parse(IStringReader reader)
        {
            NameSpaceIndexTypePathPair nameSpaceIndexTypePathPair = reader.ReadNameSpaceIndexTypePathPair();
            string nameSpace = nameSpaceIndexTypePathPair.nameSpace;
            int index = nameSpaceIndexTypePathPair.index;
            string type = nameSpaceIndexTypePathPair.type;
            string name = nameSpaceIndexTypePathPair.path;

            return ResourceManager.SearchSprites(type, name, nameSpace)[index];
        }
    }

    public class SpritesArgumentType : BaseArgumentType<SpritesArgumentType, Sprite[]>
    {
        public override Sprite[] Parse(IStringReader reader)
        {
            NameSpaceTypePathPair nameSpaceTypePathPair = reader.ReadNameSpaceTypePathPair();
            string nameSpace = nameSpaceTypePathPair.nameSpace;
            string type = nameSpaceTypePathPair.type;
            string name = nameSpaceTypePathPair.path;

            return ResourceManager.SearchSprites(type, name, nameSpace);
        }
    }

    public class NameSpacePathPairArgumentType : BaseArgumentType<NameSpacePathPairArgumentType, NameSpacePathPair>
    {
        public override NameSpacePathPair Parse(IStringReader reader) => reader.ReadNameSpacePathPair();
    }

    public class NameSpaceTypePathPairArgumentType : BaseArgumentType<NameSpaceTypePathPairArgumentType, NameSpaceTypePathPair>
    {
        public override NameSpaceTypePathPair Parse(IStringReader reader) => reader.ReadNameSpaceTypePathPair();
    }

    public class NameSpaceIndexTypePathPairArgumentType : BaseArgumentType<NameSpaceIndexTypePathPairArgumentType, NameSpaceIndexTypePathPair>
    {
        public override NameSpaceIndexTypePathPair Parse(IStringReader reader) => reader.ReadNameSpaceIndexTypePathPair();
    }

    public class PosSwizzleArgumentType : BaseArgumentType<PosSwizzleArgumentType, Arguments.PosSwizzleEnum>
    {
        public override Arguments.PosSwizzleEnum Parse(IStringReader reader) => reader.ReadPosSwizzle();
    }
}