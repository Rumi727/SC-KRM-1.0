using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Json;
using SCKRM.Log;
using SCKRM.SaveLoad;
using SCKRM.Tool;
using SCKRM.UI.SideBar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SCKRM.UI.Setting
{
    public class Setting : UI
    {
        [SerializeField] string _saveLoadAttributeName = ""; public string saveLoadAttributeName { get => _saveLoadAttributeName; set => _saveLoadAttributeName = value; }
        [SerializeField] string _variableName = ""; public string variableName { get => _variableName; set => _variableName = value; }

        [SerializeField] CanvasGroup _resetButton; public CanvasGroup resetButton { get => _resetButton; set => _resetButton = value; }
        [SerializeField] RectTransform _nameText; public RectTransform nameText { get => _nameText; set => _nameText = value; }



        public VariableType variableType { get; private set; } = VariableType.String;

        public object defaultValue { get; private set; } = null;
        public Type type { get; private set; } = null;
        public PropertyInfo propertyInfo { get; private set; } = null;
        public FieldInfo fieldInfo { get; private set; } = null;

        public bool isDefault { get; protected set; } = true;

        public enum VariableType
        {
            Char,
            String,
            Bool,
            Byte,
            Sbyte,
            Short,
            Int,
            Long,
            Ushort,
            Uint,
            Ulong,
            Float,
            Double,
            Decimal,
            JColor,
            JColor32
        }

        /// <returns>is Cancel?</returns>
        new protected virtual async UniTask<bool> Awake()
        {
            if (await UniTask.WaitUntil(() => Kernel.isInitialLoadEnd, cancellationToken: this.GetCancellationTokenOnDestroy()).SuppressCancellationThrow())
                return true;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                return true;
#endif

            foreach (var variableType in SaveLoadManager.generalSLVList)
            {
                if (variableType.name == saveLoadAttributeName)
                {
                    foreach (var propertyInfo in variableType.propertyInfos)
                    {
                        if (propertyInfo.variableInfo.Name == variableName)
                        {
                            type = propertyInfo.variableInfo.PropertyType;

                            defaultValue = propertyInfo.defaultValue;
                            this.propertyInfo = propertyInfo.variableInfo;

                            break;
                        }
                    }

                    foreach (var fieldInfo in variableType.fieldInfos)
                    {
                        if (fieldInfo.variableInfo.Name == variableName)
                        {
                            type = fieldInfo.variableInfo.FieldType;

                            defaultValue = fieldInfo.defaultValue;
                            this.fieldInfo = fieldInfo.variableInfo;

                            break;
                        }
                    }
                }
            }

            if (type == typeof(char))
                variableType = VariableType.Char;
            else if (type == typeof(string))
                variableType = VariableType.String;
            else if (type == typeof(bool))
                variableType = VariableType.Bool;
            else if (type == typeof(byte))
                variableType = VariableType.Byte;
            else if (type == typeof(sbyte))
                variableType = VariableType.Sbyte;
            else if (type == typeof(short))
                variableType = VariableType.Short;
            else if (type == typeof(int))
                variableType = VariableType.Int;
            else if (type == typeof(int))
                variableType = VariableType.Long;
            else if (type == typeof(ushort))
                variableType = VariableType.Ushort;
            else if (type == typeof(uint))
                variableType = VariableType.Uint;
            else if (type == typeof(ulong))
                variableType = VariableType.Ulong;
            else if (type == typeof(float))
                variableType = VariableType.Float;
            else if (type == typeof(double))
                variableType = VariableType.Double;
            else if (type == typeof(decimal))
                variableType = VariableType.Decimal;
            else if (type == typeof(JColor))
                variableType = VariableType.JColor;
            else if (type == typeof(JColor32))
                variableType = VariableType.JColor32;

            return false;
        }

        protected virtual void Update()
        {
            if (isDefault)
            {
                resetButton.interactable = false;
                nameText.offsetMin = nameText.offsetMin.Lerp(new Vector2(0, nameText.offsetMin.y), 0.2f * Kernel.fpsUnscaledDeltaTime);
                resetButton.alpha = resetButton.alpha.Lerp(0, 0.2f * Kernel.fpsUnscaledDeltaTime);

                if (resetButton.alpha < 0.01f)
                    resetButton.alpha = 0;
            }
            else
            {
                resetButton.interactable = true;
                nameText.offsetMin = nameText.offsetMin.Lerp(new Vector2(22, nameText.offsetMin.y), 0.2f * Kernel.fpsUnscaledDeltaTime);
                resetButton.alpha = resetButton.alpha.Lerp(1, 0.2f * Kernel.fpsUnscaledDeltaTime);

                if (resetButton.alpha > 0.99f)
                    resetButton.alpha = 1;
            }
        }

        public virtual void SetDefault()
        {
            if (propertyInfo != null)
                propertyInfo.SetValue(type, defaultValue);
            else if (fieldInfo != null)
                fieldInfo.SetValue(type, defaultValue);
        }

        public virtual object GetValue()
        {
            if (propertyInfo != null)
                return propertyInfo.GetValue(type);
            else if (fieldInfo != null)
                return fieldInfo.GetValue(type);

            return null;
        }

        public virtual int GetValueInt()
        {
            if (propertyInfo != null)
            {
                if (variableType == VariableType.Char)
                    return int.Parse(((char)propertyInfo.GetValue(type)).ToString());
                else if (variableType == VariableType.String)
                    return int.Parse((string)propertyInfo.GetValue(type));
                else if (variableType == VariableType.Bool)
                    return (bool)propertyInfo.GetValue(type) ? 1 : 0;
                else if (variableType == VariableType.Byte)
                    return (byte)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Sbyte)
                    return (sbyte)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Short)
                    return (short)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Int)
                    return (int)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Long)
                    return (int)(long)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Ushort)
                    return (ushort)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Uint)
                    return (int)(uint)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Ulong)
                    return (int)(ulong)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Float)
                    return (int)(float)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Double)
                    return (int)(double)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Decimal)
                    return (int)(decimal)propertyInfo.GetValue(type);
            }
            else if (fieldInfo != null)
            {
                if (variableType == VariableType.Char)
                    return int.Parse(((char)fieldInfo.GetValue(type)).ToString());
                if (variableType == VariableType.String)
                    return int.Parse((string)fieldInfo.GetValue(type));
                else if (variableType == VariableType.Bool)
                    return (bool)fieldInfo.GetValue(type) ? 1 : 0;
                else if (variableType == VariableType.Byte)
                    return (byte)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Sbyte)
                    return (sbyte)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Short)
                    return (short)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Int)
                    return (int)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Long)
                    return (int)(long)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Ushort)
                    return (ushort)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Uint)
                    return (int)(uint)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Ulong)
                    return (int)(ulong)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Float)
                    return (int)(float)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Double)
                    return (int)(double)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Decimal)
                    return (int)(decimal)fieldInfo.GetValue(type);
            }

            return 0;
        }

        public virtual float GetValueFloat()
        {
            if (propertyInfo != null)
            {
                if (variableType == VariableType.Char)
                    return float.Parse(((char)propertyInfo.GetValue(type)).ToString());
                else if (variableType == VariableType.String)
                    return float.Parse((string)propertyInfo.GetValue(type));
                else if (variableType == VariableType.Bool)
                    return (bool)propertyInfo.GetValue(type) ? 1 : 0;
                else if (variableType == VariableType.Byte)
                    return (byte)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Sbyte)
                    return (sbyte)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Short)
                    return (short)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Int)
                    return (int)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Long)
                    return (long)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Ushort)
                    return (ushort)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Uint)
                    return (uint)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Ulong)
                    return (ulong)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Float)
                    return (float)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Double)
                    return (float)(double)propertyInfo.GetValue(type);
                else if (variableType == VariableType.Decimal)
                    return (float)(decimal)propertyInfo.GetValue(type);
            }
            else if (fieldInfo != null)
            {
                if (variableType == VariableType.Char)
                    return float.Parse(((char)fieldInfo.GetValue(type)).ToString());
                if (variableType == VariableType.String)
                    return float.Parse((string)fieldInfo.GetValue(type));
                else if (variableType == VariableType.Bool)
                    return (bool)fieldInfo.GetValue(type) ? 1 : 0;
                else if (variableType == VariableType.Byte)
                    return (byte)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Sbyte)
                    return (sbyte)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Short)
                    return (short)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Int)
                    return (int)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Long)
                    return (long)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Ushort)
                    return (ushort)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Uint)
                    return (uint)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Ulong)
                    return (ulong)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Float)
                    return (float)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Double)
                    return (float)(double)fieldInfo.GetValue(type);
                else if (variableType == VariableType.Decimal)
                    return (float)(decimal)fieldInfo.GetValue(type);
            }

            return 0;
        }

        public virtual void SaveValue(object value)
        {
            if (propertyInfo != null)
                propertyInfo.SetValue(type, value);
            else if (fieldInfo != null)
                fieldInfo.SetValue(type, value);
        }

        public virtual void SaveValueFloat(float value)
        {
            if (propertyInfo != null)
            {
                if (variableType == VariableType.Char)
                    throw new ArgumentException();
                else if (variableType == VariableType.String)
                    propertyInfo.SetValue(type, value.ToString());
                else if (variableType == VariableType.Byte)
                    propertyInfo.SetValue(type, (byte)value);
                else if (variableType == VariableType.Sbyte)
                    propertyInfo.SetValue(type, (sbyte)value);
                else if (variableType == VariableType.Short)
                    propertyInfo.SetValue(type, (short)value);
                else if (variableType == VariableType.Int)
                    propertyInfo.SetValue(type, (int)value);
                else if (variableType == VariableType.Long)
                    propertyInfo.SetValue(type, (long)value);
                else if (variableType == VariableType.Ushort)
                    propertyInfo.SetValue(type, (ushort)value);
                else if (variableType == VariableType.Uint)
                    propertyInfo.SetValue(type, (uint)value);
                else if (variableType == VariableType.Ulong)
                    propertyInfo.SetValue(type, (ulong)value);
                else if (variableType == VariableType.Float)
                    propertyInfo.SetValue(type, value);
                else if (variableType == VariableType.Double)
                    propertyInfo.SetValue(type, (double)value);
                else if (variableType == VariableType.Decimal)
                    propertyInfo.SetValue(type, (decimal)value);
            }
            else if (fieldInfo != null)
            {
                if (variableType == VariableType.Char)
                    throw new ArgumentException();
                else if (variableType == VariableType.String)
                    fieldInfo.SetValue(type, value.ToString());
                else if (variableType == VariableType.Byte)
                    fieldInfo.SetValue(type, (byte)value);
                else if (variableType == VariableType.Sbyte)
                    fieldInfo.SetValue(type, (sbyte)value);
                else if (variableType == VariableType.Short)
                    fieldInfo.SetValue(type, (short)value);
                else if (variableType == VariableType.Int)
                    fieldInfo.SetValue(type, (int)value);
                else if (variableType == VariableType.Long)
                    fieldInfo.SetValue(type, (long)value);
                else if (variableType == VariableType.Ushort)
                    fieldInfo.SetValue(type, (ushort)value);
                else if (variableType == VariableType.Uint)
                    fieldInfo.SetValue(type, (uint)value);
                else if (variableType == VariableType.Ulong)
                    fieldInfo.SetValue(type, (ulong)value);
                else if (variableType == VariableType.Float)
                    fieldInfo.SetValue(type, value);
                else if (variableType == VariableType.Double)
                    fieldInfo.SetValue(type, (double)value);
                else if (variableType == VariableType.Decimal)
                    fieldInfo.SetValue(type, (decimal)value);
            }
        }

        public virtual void SaveStringValue(string value)
        {
            try
            {
                if (propertyInfo != null)
                {
                    if (variableType == VariableType.Char)
                    {
                        if (value.Length > 0)
                            propertyInfo.SetValue(type, value[0]);
                        else
                            propertyInfo.SetValue(type, char.MinValue);
                    }
                    else if (variableType == VariableType.String)
                        propertyInfo.SetValue(type, value);
                    else if (variableType == VariableType.Byte)
                        propertyInfo.SetValue(type, byte.Parse(value));
                    else if (variableType == VariableType.Sbyte)
                        propertyInfo.SetValue(type, sbyte.Parse(value));
                    else if (variableType == VariableType.Short)
                        propertyInfo.SetValue(type, short.Parse(value));
                    else if (variableType == VariableType.Int)
                        propertyInfo.SetValue(type, int.Parse(value));
                    else if (variableType == VariableType.Long)
                        propertyInfo.SetValue(type, long.Parse(value));
                    else if (variableType == VariableType.Ushort)
                        propertyInfo.SetValue(type, ushort.Parse(value));
                    else if (variableType == VariableType.Uint)
                        propertyInfo.SetValue(type, uint.Parse(value));
                    else if (variableType == VariableType.Ulong)
                        propertyInfo.SetValue(type, ulong.Parse(value));
                    else if (variableType == VariableType.Float)
                        propertyInfo.SetValue(type, float.Parse(value));
                    else if (variableType == VariableType.Double)
                        propertyInfo.SetValue(type, double.Parse(value));
                    else if (variableType == VariableType.Decimal)
                        propertyInfo.SetValue(type, decimal.Parse(value));
                }
                else if (fieldInfo != null)
                {
                    if (variableType == VariableType.Char)
                    {
                        if (value.Length > 0)
                            fieldInfo.SetValue(type, value[0]);
                        else
                            fieldInfo.SetValue(type, char.MinValue);
                    }
                    else if (variableType == VariableType.String)
                        fieldInfo.SetValue(type, value);
                    else if (variableType == VariableType.Byte)
                        fieldInfo.SetValue(type, byte.Parse(value));
                    else if (variableType == VariableType.Sbyte)
                        fieldInfo.SetValue(type, sbyte.Parse(value));
                    else if (variableType == VariableType.Short)
                        fieldInfo.SetValue(type, short.Parse(value));
                    else if (variableType == VariableType.Int)
                        fieldInfo.SetValue(type, int.Parse(value));
                    else if (variableType == VariableType.Long)
                        fieldInfo.SetValue(type, long.Parse(value));
                    else if (variableType == VariableType.Ushort)
                        fieldInfo.SetValue(type, ushort.Parse(value));
                    else if (variableType == VariableType.Uint)
                        fieldInfo.SetValue(type, uint.Parse(value));
                    else if (variableType == VariableType.Ulong)
                        fieldInfo.SetValue(type, ulong.Parse(value));
                    else if (variableType == VariableType.Float)
                        fieldInfo.SetValue(type, float.Parse(value));
                    else if (variableType == VariableType.Double)
                        fieldInfo.SetValue(type, double.Parse(value));
                    else if (variableType == VariableType.Decimal)
                        fieldInfo.SetValue(type, decimal.Parse(value));
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    public class SettingDrag : Setting
    {
        [SerializeField] float _mouseSensitivity = 1; public float mouseSensitivity { get => _mouseSensitivity; set => _mouseSensitivity = value; }

        public virtual void OnDrag()
        {
            Vector2 mouseDeltaVector = InputManager.GetMouseDelta("all");
            float mouseDelta = mouseDeltaVector.magnitude * mouseSensitivity;

            if ((mouseDeltaVector.x + mouseDeltaVector.y) / 2 < 0)
                mouseDelta *= -1;

            if (propertyInfo != null)
            {
                object value = propertyInfo.GetValue(type);

                if (variableType == VariableType.Byte)
                    propertyInfo.SetValue(type, (byte)((byte)value + mouseDelta));
                if (variableType == VariableType.Sbyte)
                    propertyInfo.SetValue(type, (sbyte)((sbyte)value + mouseDelta));
                if (variableType == VariableType.Short)
                    propertyInfo.SetValue(type, (short)((short)value + mouseDelta));
                if (variableType == VariableType.Int)
                    propertyInfo.SetValue(type, (int)((int)value + mouseDelta));
                if (variableType == VariableType.Long)
                    propertyInfo.SetValue(type, (long)((long)value + mouseDelta));
                if (variableType == VariableType.Ushort)
                    propertyInfo.SetValue(type, (ushort)((ushort)value + mouseDelta));
                if (variableType == VariableType.Uint)
                    propertyInfo.SetValue(type, (uint)((uint)value + mouseDelta));
                if (variableType == VariableType.Ulong)
                    propertyInfo.SetValue(type, (ulong)((ulong)value + mouseDelta));
                if (variableType == VariableType.Float)
                    propertyInfo.SetValue(type, (float)value + mouseDelta);
                if (variableType == VariableType.Double)
                    propertyInfo.SetValue(type, (double)value + mouseDelta);
                if (variableType == VariableType.Decimal)
                    propertyInfo.SetValue(type, (decimal)value + (decimal)mouseDelta);
            }
            else if (fieldInfo != null)
            {
                object value2 = fieldInfo.GetValue(type);

                if (variableType == VariableType.Byte)
                    fieldInfo.SetValue(type, (byte)((byte)value2 + mouseDelta));
                if (variableType == VariableType.Sbyte)
                    fieldInfo.SetValue(type, (sbyte)((sbyte)value2 + mouseDelta));
                if (variableType == VariableType.Short)
                    fieldInfo.SetValue(type, (short)((short)value2 + mouseDelta));
                if (variableType == VariableType.Int)
                    fieldInfo.SetValue(type, (int)((int)value2 + mouseDelta));
                if (variableType == VariableType.Long)
                    fieldInfo.SetValue(type, (long)((long)value2 + mouseDelta));
                if (variableType == VariableType.Ushort)
                    fieldInfo.SetValue(type, (ushort)((ushort)value2 + mouseDelta));
                if (variableType == VariableType.Uint)
                    fieldInfo.SetValue(type, (uint)((uint)value2 + mouseDelta));
                if (variableType == VariableType.Ulong)
                    fieldInfo.SetValue(type, (ulong)((ulong)value2 + mouseDelta));
                if (variableType == VariableType.Float)
                    fieldInfo.SetValue(type, (float)value2 + mouseDelta);
                if (variableType == VariableType.Double)
                    fieldInfo.SetValue(type, (double)value2 + mouseDelta);
                if (variableType == VariableType.Decimal)
                    fieldInfo.SetValue(type, (decimal)value2 + (decimal)mouseDelta);
            }
        }
    }
}