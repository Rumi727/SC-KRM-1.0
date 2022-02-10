using Newtonsoft.Json;
using SCKRM.Resource;
using UnityEngine;

namespace SCKRM.Json
{
    public static class JsonManager
    {
        public static T JsonRead<T>(string path, bool pathExtensionUse = false)
        {
            string json;
            json = ResourceManager.GetText(path, pathExtensionUse);
            
            if (json != "")
                return JsonConvert.DeserializeObject<T>(json);
            else
                return default;
        }

        public static T JsonRead<T>(string path, string nameSpace) => JsonConvert.DeserializeObject<T>(ResourceManager.SearchText(path, nameSpace));



        public static T JsonToObject<T>(string json) => JsonConvert.DeserializeObject<T>(json);
        public static string ObjectToJson(object value) => JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings() { });
        public static string ObjectToJson(params object[] value) => JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings() { });
    }

    public struct JVector2
    {
        public float x;
        public float y;

        public static JVector2 zero { get; } = new JVector2();

        public JVector2(Vector2 value)
        {
            x = value.x;
            y = value.y;
        }

        public JVector2(float value) => x = y = value;
        public JVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator JVector3(JVector2 value) => new JVector3(value.x, value.y);
        public static implicit operator JVector4(JVector2 value) => new JVector4(value.x, value.y);

        public static implicit operator JVector2(JRect value) => new JVector2(value.x, value.y);

        public static implicit operator JVector2(Vector2 value) => new JVector3(value);
        public static implicit operator JVector2(Vector3 value) => new JVector3(value);
        public static implicit operator JVector2(Vector4 value) => new JVector3(value);
        public static implicit operator Vector2(JVector2 value) => new Vector3(value.x, value.y);
        public static implicit operator Vector3(JVector2 value) => new Vector3(value.x, value.y);
        public static implicit operator Vector4(JVector2 value) => new Vector4(value.x, value.y);

        public override string ToString() => $"(x: {x}, y: {y})";
    }

    public struct JVector3
    {
        public float x;
        public float y;
        public float z;

        public static JVector3 zero { get; } = new JVector3();

        public JVector3(Vector3 value)
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }

        public JVector3(float value) => x = y = z = value;

        public JVector3(float x, float y)
        {
            this.x = x;
            this.y = y;
            z = 0;
        }

        public JVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator JVector2(JVector3 value) => new JVector2(value.x, value.y);
        public static implicit operator JVector4(JVector3 value) => new JVector4(value.x, value.y);

        public static implicit operator JVector3(JRect value) => new JVector3(value.x, value.y, value.width);

        public static implicit operator JVector3(Vector2 value) => new JVector3(value);
        public static implicit operator JVector3(Vector3 value) => new JVector3(value);
        public static implicit operator JVector3(Vector4 value) => new JVector3(value);
        public static implicit operator Vector2(JVector3 value) => new Vector3(value.x, value.y);
        public static implicit operator Vector3(JVector3 value) => new Vector3(value.x, value.y, value.z);
        public static implicit operator Vector4(JVector3 value) => new Vector4(value.x, value.y, value.z);

        public override string ToString() => $"({x}, {y}, {z})";
    }
    public struct JVector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public static JVector4 zero { get; } = new JVector4();

        public JVector4(Vector4 value)
        {
            x = value.x;
            y = value.y;
            z = value.z;
            w = value.w;
        }

        public JVector4(float value) => x = y = z = w = value;

        public JVector4(float x, float y)
        {
            this.x = x;
            this.y = y;
            z = 0;
            w = 0;
        }

        public JVector4(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            w = 0;
        }

        public JVector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static implicit operator JVector2(JVector4 value) => new JVector2(value.x, value.y);
        public static implicit operator JVector3(JVector4 value) => new JVector3(value.x, value.y, value.z);

        public static implicit operator JVector4(JRect value) => new JVector4(value.x, value.y, value.width, value.height);
        public static implicit operator JVector4(Rect value) => new JVector4(value.x, value.y, value.width, value.height);

        public static implicit operator JVector4(Vector2 value) => new JVector3(value);
        public static implicit operator JVector4(Vector3 value) => new JVector3(value);
        public static implicit operator JVector4(Vector4 value) => new JVector3(value);
        public static implicit operator Vector2(JVector4 value) => new Vector3(value.x, value.y);
        public static implicit operator Vector3(JVector4 value) => new Vector3(value.x, value.y, value.z);
        public static implicit operator Vector4(JVector4 value) => new Vector4(value.x, value.y, value.z, value.w);

        public override string ToString() => $"({x}, {y}, {z}, {w})";
    }

    public struct JRect
    {
        public float x;
        public float y;
        public float width;
        public float height;

        public static JRect zero { get; } = new JRect();

        public JRect(Rect value)
        {
            x = value.x;
            y = value.y;
            width = value.width;
            height = value.height;
        }

        public JRect(float value) => x = y = width = height = value;

        public JRect(float x, float y)
        {
            this.x = x;
            this.y = y;
            width = 0;
            height = 0;
        }

        public JRect(float x, float y, float width)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            height = 0;
        }

        public JRect(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public static implicit operator JRect(JVector2 value) => new JRect(value.x, value.y);
        public static implicit operator JRect(JVector3 value) => new JRect(value.x, value.y, value.z);
        public static implicit operator JRect(JVector4 value) => new JRect(value.x, value.y, value.z, value.w);
        public static implicit operator JRect(JColor value) => new JRect(value.r, value.g, value.b, value.a);

        public static implicit operator JRect(Vector2 value) => new JRect(value.x, value.y);
        public static implicit operator JRect(Vector3 value) => new JRect(value.x, value.y, value.z);
        public static implicit operator JRect(Vector4 value) => new JRect(value.x, value.y, value.z, value.w);
        public static implicit operator JRect(Color value) => new JRect(value.r, value.g, value.b, value.a);

        public static implicit operator Vector2(JRect value) => new Vector2(value.x, value.y);
        public static implicit operator Vector3(JRect value) => new Vector3(value.x, value.y, value.width);
        public static implicit operator Vector4(JRect value) => new Vector4(value.x, value.y, value.width, value.height);
        public static implicit operator Color(JRect value) => new Color(value.x, value.y, value.width, value.height);

        public static implicit operator JRect(Rect value) => new JRect(value);
        public static implicit operator Rect(JRect value) => new Rect() { x = value.x, y = value.y, width = value.width, height = value.height };

        public override string ToString() => $"(x:{x}, y:{y}, width:{width}, height:{height})";
    }

    public struct JColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public static JColor zero { get; } = new JColor();
        public static JColor one { get; } = new JColor(1);

        public JColor(Color value)
        {
            r = value.r;
            g = value.g;
            b = value.b;
            a = value.a;
        }

        public JColor(float value)
        {
            r = g = b = value;
            a = 1;
        }

        public JColor(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            a = 1;
        }

        public JColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static implicit operator JColor(Vector3 value) => new JColor(value.x, value.y, value.z);
        public static implicit operator JColor(Vector4 value) => new JColor(value.x, value.y, value.z, value.w);
        public static implicit operator JColor(Rect value) => new JColor(value.x, value.y, value.width, value.height);

        public static implicit operator JColor(JVector3 value) => new JColor(value.x, value.y, value.z);
        public static implicit operator JColor(JVector4 value) => new JColor(value.x, value.y, value.z, value.w);
        public static implicit operator JColor(JRect value) => new JColor(value.x, value.y, value.width, value.height);

        public static implicit operator Vector3(JColor value) => new Vector3(value.r, value.g, value.b);
        public static implicit operator Vector4(JColor value) => new Vector4(value.r, value.g, value.b, value.a);
        public static implicit operator Rect(JColor value) => new Rect(value.r, value.g, value.b, value.a);

        public static implicit operator JColor(Color value) => new JColor(value);
        public static implicit operator Color(JColor value) => new Color() { r = value.r, g = value.g, b = value.b, a = value.a };

        public override string ToString() => $"(r:{r}, g:{g}, b:{b}, a:{a})";
    }
}