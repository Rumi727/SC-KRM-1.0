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

    public class Vector2ArgumentType : ArgumentType<Vector2>
    {
        readonly float _minimum;
        readonly float _maximum;

        internal Vector2ArgumentType(float minimum, float maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public float Minimum() => _minimum;

        public float Maximum() => _maximum;

        public override Vector2 Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float x = reader.ReadFloat();
            if (x < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, x, _minimum);
            }

            if (x > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, x, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float y = reader.ReadFloat();
            if (y < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, y, _minimum);
            }

            if (y > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, y, _maximum);
            }

            return new Vector2(x, y);
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            else if (o is not Vector2ArgumentType)
                return false;

            Vector2ArgumentType vector2ArgumentType = (Vector2ArgumentType)o;
            if (_maximum == vector2ArgumentType._maximum)
                return _minimum == vector2ArgumentType._minimum;
            
            return false;
        }

        public override int GetHashCode() => (int)(31f * _minimum + _maximum);

        public override string ToString()
        {
            if (_minimum == float.MinValue && _maximum == float.MaxValue)
                return "Vector2()";

            if (_maximum == float.MaxValue)
                return $"Vector2({_minimum:#.0})";

            return $"Vector2({_minimum:#.0}, {_maximum:#.0})";
        }
    }

    public class Vector2IntArgumentType : ArgumentType<Vector2Int>
    {
        readonly int _minimum;
        readonly int _maximum;

        internal Vector2IntArgumentType(int minimum, int maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public int Minimum() => _minimum;

        public int Maximum() => _maximum;

        public override Vector2Int Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            int x = reader.ReadInt();
            if (x < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, x, _minimum);
            }

            if (x > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, x, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            int y = reader.ReadInt();
            if (y < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, y, _minimum);
            }

            if (y > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, y, _maximum);
            }

            return new Vector2Int(x, y);
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            else if (o is not Vector2IntArgumentType)
                return false;

            Vector2IntArgumentType vector2IntArgumentType = (Vector2IntArgumentType)o;
            if (_maximum == vector2IntArgumentType._maximum)
                return _minimum == vector2IntArgumentType._minimum;

            return false;
        }

        public override int GetHashCode() => (int)(31f * _minimum + _maximum);

        public override string ToString()
        {
            if (_minimum == int.MinValue && _maximum == int.MaxValue)
                return "Vector2Int()";

            if (_maximum == int.MaxValue)
                return $"Vector2Int({_minimum:#.0})";

            return $"Vector2Int({_minimum:#.0}, {_maximum:#.0})";
        }
    }

    public class Vector3ArgumentType : ArgumentType<Vector3>
    {
        readonly float _minimum;
        readonly float _maximum;

        internal Vector3ArgumentType(float minimum, float maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public float Minimum() => _minimum;

        public float Maximum() => _maximum;

        public override Vector3 Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float x = reader.ReadFloat();
            if (x < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, x, _minimum);
            }

            if (x > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, x, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float y = reader.ReadFloat();
            if (y < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, y, _minimum);
            }

            if (y > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, y, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float z = reader.ReadFloat();
            if (z < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, z, _minimum);
            }

            if (z > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, z, _maximum);
            }

            return new Vector3(x, y, z);
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            else if (o is not Vector3ArgumentType)
                return false;

            Vector3ArgumentType vector3ArgumentType = (Vector3ArgumentType)o;
            if (_maximum == vector3ArgumentType._maximum)
                return _minimum == vector3ArgumentType._minimum;

            return false;
        }

        public override int GetHashCode() => (int)(31f * _minimum + _maximum);

        public override string ToString()
        {
            if (_minimum == float.MinValue && _maximum == float.MaxValue)
                return "Vector3()";

            if (_maximum == float.MaxValue)
                return $"Vector3({_minimum:#.0})";

            return $"Vector3({_minimum:#.0}, {_maximum:#.0})";
        }
    }

    public class Vector3IntArgumentType : ArgumentType<Vector3Int>
    {
        readonly int _minimum;
        readonly int _maximum;

        internal Vector3IntArgumentType(int minimum, int maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public int Minimum() => _minimum;

        public int Maximum() => _maximum;

        public override Vector3Int Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            int x = reader.ReadInt();
            if (x < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooLow().CreateWithContext(reader, x, _minimum);
            }

            if (x > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooHigh().CreateWithContext(reader, x, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            int y = reader.ReadInt();
            if (y < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooLow().CreateWithContext(reader, y, _minimum);
            }

            if (y > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooHigh().CreateWithContext(reader, y, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            int z = reader.ReadInt();
            if (z < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooLow().CreateWithContext(reader, z, _minimum);
            }

            if (z > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooHigh().CreateWithContext(reader, z, _maximum);
            }

            return new Vector3Int(x, y, z);
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            else if (o is not Vector3IntArgumentType)
                return false;

            Vector3IntArgumentType vector3IntArgumentType = (Vector3IntArgumentType)o;
            if (_maximum == vector3IntArgumentType._maximum)
                return _minimum == vector3IntArgumentType._minimum;

            return false;
        }

        public override int GetHashCode() => (int)(31f * _minimum + _maximum);

        public override string ToString()
        {
            if (_minimum == int.MinValue && _maximum == int.MaxValue)
                return "Vector3Int()";

            if (_maximum == int.MaxValue)
                return $"Vector3Int({_minimum:#.0})";

            return $"Vector3Int({_minimum:#.0}, {_maximum:#.0})";
        }
    }

    public class Vector4ArgumentType : ArgumentType<Vector4>
    {
        readonly float _minimum;
        readonly float _maximum;

        internal Vector4ArgumentType(float minimum, float maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public float Minimum() => _minimum;

        public float Maximum() => _maximum;

        public override Vector4 Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float x = reader.ReadFloat();
            if (x < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, x, _minimum);
            }

            if (x > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, x, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float y = reader.ReadFloat();
            if (y < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, y, _minimum);
            }

            if (y > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, y, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float z = reader.ReadFloat();
            if (z < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, z, _minimum);
            }

            if (z > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, z, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float w = reader.ReadFloat();
            if (w < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, w, _minimum);
            }

            if (w > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, w, _maximum);
            }

            return new Vector4(x, y, z, w);
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            else if (o is not Vector4ArgumentType)
                return false;

            Vector4ArgumentType vector4ArgumentType = (Vector4ArgumentType)o;
            if (_maximum == vector4ArgumentType._maximum)
                return _minimum == vector4ArgumentType._minimum;

            return false;
        }

        public override int GetHashCode() => (int)(31f * _minimum + _maximum);

        public override string ToString()
        {
            if (_minimum == float.MinValue && _maximum == float.MaxValue)
                return "Vector4()";

            if (_maximum == float.MaxValue)
                return $"Vector4({_minimum:#.0})";

            return $"Vector4({_minimum:#.0}, {_maximum:#.0})";
        }
    }

    public class RectArgumentType : ArgumentType<Rect>
    {
        readonly float _minimum;
        readonly float _maximum;

        internal RectArgumentType(float minimum, float maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public float Minimum() => _minimum;

        public float Maximum() => _maximum;

        public override Rect Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float x = reader.ReadFloat();
            if (x < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, x, _minimum);
            }

            if (x > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, x, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float y = reader.ReadFloat();
            if (y < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, y, _minimum);
            }

            if (y > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, y, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float width = reader.ReadFloat();
            if (width < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, width, _minimum);
            }

            if (width > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, width, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float height = reader.ReadFloat();
            if (height < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, height, _minimum);
            }

            if (height > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, height, _maximum);
            }

            return new Rect(x, y, width, height);
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            else if (o is not RectArgumentType)
                return false;

            RectArgumentType rectArgumentType = (RectArgumentType)o;
            if (_maximum == rectArgumentType._maximum)
                return _minimum == rectArgumentType._minimum;

            return false;
        }

        public override int GetHashCode() => (int)(31f * _minimum + _maximum);

        public override string ToString()
        {
            if (_minimum == float.MinValue && _maximum == float.MaxValue)
                return "Rect()";

            if (_maximum == float.MaxValue)
                return $"Rect({_minimum:#.0})";

            return $"Rect({_minimum:#.0}, {_maximum:#.0})";
        }
    }

    public class RectIntArgumentType : ArgumentType<RectInt>
    {
        readonly int _minimum;
        readonly int _maximum;

        internal RectIntArgumentType(int minimum, int maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public int Minimum() => _minimum;

        public int Maximum() => _maximum;

        public override RectInt Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            int x = reader.ReadInt();
            if (x < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooLow().CreateWithContext(reader, x, _minimum);
            }

            if (x > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooHigh().CreateWithContext(reader, x, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            int y = reader.ReadInt();
            if (y < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooLow().CreateWithContext(reader, y, _minimum);
            }

            if (y > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooHigh().CreateWithContext(reader, y, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            int width = reader.ReadInt();
            if (width < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooLow().CreateWithContext(reader, width, _minimum);
            }

            if (width > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooHigh().CreateWithContext(reader, width, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            int height = reader.ReadInt();
            if (height < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooLow().CreateWithContext(reader, height, _minimum);
            }

            if (height > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.IntegerTooHigh().CreateWithContext(reader, height, _maximum);
            }

            return new RectInt(x, y, width, height);
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            else if (o is not RectIntArgumentType)
                return false;

            RectIntArgumentType rectIntArgumentType = (RectIntArgumentType)o;
            if (_maximum == rectIntArgumentType._maximum)
                return _minimum == rectIntArgumentType._minimum;

            return false;
        }

        public override int GetHashCode() => (int)(31f * _minimum + _maximum);

        public override string ToString()
        {
            if (_minimum == int.MinValue && _maximum == int.MaxValue)
                return "RectInt()";

            if (_maximum == int.MaxValue)
                return $"RectInt({_minimum:#.0})";

            return $"RectInt({_minimum:#.0}, {_maximum:#.0})";
        }
    }

    public class ColorArgumentType : ArgumentType<Color>
    {
        const float _minimum = 0;
        const float _maximum = 255;

        public override Color Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float r = reader.ReadFloat();
            if (r < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, r, _minimum);
            }

            if (r > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, r, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float g = reader.ReadFloat();
            if (g < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, g, _minimum);
            }

            if (g > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, g, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float b = reader.ReadFloat();
            if (b < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, b, _minimum);
            }

            if (b > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, b, _maximum);
            }

            return new Color(r, g, b);
        }

        public override bool Equals(object o) => o is ColorArgumentType;

        public override int GetHashCode() => (int)(31f * _minimum + _maximum);

        public override string ToString() => $"Color({_minimum}, {_maximum})";
    }

    public class ColorAlphaArgumentType : ArgumentType<Color>
    {
        const float _minimum = 0;
        const float _maximum = 255;

        public override Color Parse(IStringReader reader)
        {
            int firstCursor = reader.Cursor;
            int cursor = reader.Cursor;
            float r = reader.ReadFloat();
            if (r < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, r, _minimum);
            }

            if (r > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, r, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float g = reader.ReadFloat();
            if (g < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, g, _minimum);
            }

            if (g > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, g, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float b = reader.ReadFloat();
            if (b < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, b, _minimum);
            }

            if (b > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, b, _maximum);
            }

            if (!reader.CanRead(2))
            {
                reader.Cursor = firstCursor;
                throw CommandSyntaxException.BuiltInExceptions.DispatcherUnknownArgument().CreateWithContext(reader);
            }

            cursor = ++reader.Cursor;
            float a = reader.ReadFloat();
            if (a < _minimum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooLow().CreateWithContext(reader, a, _minimum);
            }

            if (a > _maximum)
            {
                reader.Cursor = cursor;
                throw CommandSyntaxException.BuiltInExceptions.FloatTooHigh().CreateWithContext(reader, a, _maximum);
            }

            return new Color(r, g, b, a);
        }

        public override bool Equals(object o) => o is ColorAlphaArgumentType;

        public override int GetHashCode() => (int)(31f * _minimum + _maximum);

        public override string ToString() => $"Color({_minimum}, {_maximum})";
    }

    public class TransformArgumentType : ArgumentType<Transform>
    {
        public override Transform Parse(IStringReader reader)
        {
            int hashCode = reader.ReadInt();
            return UnityEngine.Object.FindObjectsOfType<Transform>().First(x => x.GetHashCode() == hashCode);
        }

        public override bool Equals(object o) => o is TransformArgumentType;

        public override int GetHashCode() => 0;

        public override string ToString() => $"Transform()";
    }

    public class TransformsStringArgumentType : ArgumentType<Transform[]>
    {
        public override Transform[] Parse(IStringReader reader)
        {
            string name = reader.ReadString();
            return UnityEngine.Object.FindObjectsOfType<Transform>().Where(x => x.name == name).ToArray();
        }

        public override bool Equals(object o) => o is TransformsStringArgumentType;

        public override int GetHashCode() => 0;

        public override string ToString() => $"Transform[]()";
    }

    public class GameObjectArgumentType : ArgumentType<GameObject>
    {
        public override GameObject Parse(IStringReader reader)
        {
            int hashCode = reader.ReadInt();
            return UnityEngine.Object.FindObjectsOfType<GameObject>().First(x => x.GetHashCode() == hashCode);
        }

        public override bool Equals(object o) => o is GameObjectArgumentType;

        public override int GetHashCode() => 0;

        public override string ToString() => $"GameObject()";
    }

    public class GameObjectsStringArgumentType : ArgumentType<GameObject[]>
    {
        public override GameObject[] Parse(IStringReader reader)
        {
            string name = reader.ReadString();
            return UnityEngine.Object.FindObjectsOfType<GameObject>().Where(x => x.name == name).ToArray();
        }

        public override bool Equals(object o) => o is GameObjectsStringArgumentType;

        public override int GetHashCode() => 0;

        public override string ToString() => $"GameObject[]()";
    }

    public class SpriteArgumentType : ArgumentType<Sprite>
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

        public override bool Equals(object o) => o is SpriteArgumentType;

        public override int GetHashCode() => 0;

        public override string ToString() => $"Sprite()";
    }

    public class SpritesArgumentType : ArgumentType<Sprite[]>
    {
        public override Sprite[] Parse(IStringReader reader)
        {
            NameSpaceTypePathPair nameSpaceTypePathPair = reader.ReadNameSpaceTypePathPair();
            string nameSpace = nameSpaceTypePathPair.nameSpace;
            string type = nameSpaceTypePathPair.type;
            string name = nameSpaceTypePathPair.path;

            return ResourceManager.SearchSprites(type, name, nameSpace);
        }

        public override bool Equals(object o) => o is SpritesArgumentType;

        public override int GetHashCode() => 0;

        public override string ToString() => $"Sprite[]()";
    }

    public class NameSpacePathPairArgumentType : ArgumentType<NameSpacePathPair>
    {
        public override NameSpacePathPair Parse(IStringReader reader) => reader.ReadNameSpacePathPair();

        public override bool Equals(object o) => o is NameSpacePathPairArgumentType;

        public override int GetHashCode() => 0;

        public override string ToString() => $"NameSpacePathPair[]()";
    }

    public class NameSpaceTypePathPairArgumentType : ArgumentType<NameSpaceTypePathPair>
    {
        public override NameSpaceTypePathPair Parse(IStringReader reader) => reader.ReadNameSpaceTypePathPair();

        public override bool Equals(object o) => o is NameSpaceTypePathPairArgumentType;

        public override int GetHashCode() => 0;

        public override string ToString() => $"NameSpaceTypePathPair[]()";
    }

    public class NameSpaceIndexTypePathPairArgumentType : ArgumentType<NameSpaceIndexTypePathPair>
    {
        public override NameSpaceIndexTypePathPair Parse(IStringReader reader) => reader.ReadNameSpaceIndexTypePathPair();

        public override bool Equals(object o) => o is NameSpaceIndexTypePathPairArgumentType;

        public override int GetHashCode() => 0;

        public override string ToString() => $"NameSpaceIndexTypePathPair[]()";
    }
}