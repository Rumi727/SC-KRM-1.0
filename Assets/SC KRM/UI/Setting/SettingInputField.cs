using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Renderer;
using SCKRM.SaveLoad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace SCKRM.UI.Setting.InputField
{
    [AddComponentMenu("커널/UI/인풋 필드 (설정)")]
    public class SettingInputField : MonoBehaviour
    {
        [SerializeField] string _saveLoadAttributeName = ""; public string saveLoadAttributeName { get => _saveLoadAttributeName; set => _saveLoadAttributeName = value; }
        [SerializeField] string _variableName = ""; public string variableName { get => _variableName; set => _variableName = value; }



        [SerializeField] float _mouseSensitivity = 1; public float mouseSensitivity { get => _mouseSensitivity; set => _mouseSensitivity = value; }



        [SerializeField] string _textPlaceHolderNameSpace = ""; public string textPlaceHolderNameSpace { get => _textPlaceHolderNameSpace; set => _textPlaceHolderNameSpace = value; }
        [SerializeField] string _numberPlaceHolderNameSpace = ""; public string numberPlaceHolderNameSpace { get => _numberPlaceHolderNameSpace; set => _numberPlaceHolderNameSpace = value; }

        [SerializeField] string _textPlaceHolderPath = ""; public string textPlaceHolderPath { get => _textPlaceHolderPath; set => _textPlaceHolderPath = value; }
        [SerializeField] string _numberPlaceHolderPath = ""; public string numberPlaceHolderPath { get => _numberPlaceHolderPath; set => _numberPlaceHolderPath = value; }



        [SerializeField] TMP_InputField _inputField; public TMP_InputField inputField { get => _inputField; set => _inputField = value; }
        [SerializeField] CustomTextMeshProRenderer _placeholder; public CustomTextMeshProRenderer placeholder { get => _placeholder; set => _placeholder = value; }



        public VariableType variableType { get; protected set; } = VariableType.String;

        public object defaultValue { get; protected set; } = null;
        public Type type { get; protected set; } = null;
        public PropertyInfo propertyInfo { get; protected set; } = null;
        public FieldInfo fieldInfo { get; protected set; } = null;



        public virtual async void Awake()
        {
            await UniTask.WaitUntil(() => Kernel.isInitialLoadEnd);
            VariableLoad();
        }

        public virtual void VariableLoad()
        {
            foreach (var variableType in SaveLoadManager.variableTypeList)
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

            if (type == typeof(string))
                variableType = VariableType.String;
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



            if (variableType == VariableType.String)
            {
                inputField.contentType = TMP_InputField.ContentType.Standard;

                placeholder.nameSpace = textPlaceHolderNameSpace;
                placeholder.path = textPlaceHolderPath;

                placeholder.ResourceReload();
            }
            else if (variableType != VariableType.Float && variableType != VariableType.Double && variableType != VariableType.Decimal)
            {
                inputField.contentType = TMP_InputField.ContentType.IntegerNumber;

                placeholder.nameSpace = numberPlaceHolderNameSpace;
                placeholder.path = numberPlaceHolderPath;

                placeholder.ResourceReload();
            }
            else
            {
                inputField.contentType = TMP_InputField.ContentType.DecimalNumber;

                placeholder.nameSpace = numberPlaceHolderNameSpace;
                placeholder.path = numberPlaceHolderPath;

                placeholder.ResourceReload();
            }
        }

        public virtual void OnEndEdit()
        {
            if (variableType != VariableType.String && string.IsNullOrEmpty(inputField.text))
                inputField.text = "0";

            string value = inputField.text;
            if (propertyInfo != null)
            {
                if (variableType == VariableType.String)
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
                if (variableType == VariableType.String)
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

        public virtual void OnDrag()
        {
            Vector2 mouseDeltaVector = InputManager.GetMouseDelta("all");
            float mouseDelta = mouseDeltaVector.magnitude * mouseSensitivity;

            if ((mouseDeltaVector.x + mouseDeltaVector.y) / 2 < 0)
                mouseDelta *= -1;

            if (propertyInfo != null)
            {
                if (variableType == VariableType.Byte)
                    propertyInfo.SetValue(type, (byte)(byte.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Sbyte)
                    propertyInfo.SetValue(type, (sbyte)(sbyte.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Short)
                    propertyInfo.SetValue(type, (short)(short.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Int)
                    propertyInfo.SetValue(type, (int)(int.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Long)
                    propertyInfo.SetValue(type, (long)(long.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Ushort)
                    propertyInfo.SetValue(type, (ushort)(ushort.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Uint)
                    propertyInfo.SetValue(type, (uint)(uint.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Ulong)
                    propertyInfo.SetValue(type, (ulong)(ulong.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Float)
                    propertyInfo.SetValue(type, float.Parse(inputField.text) + mouseDelta);
                if (variableType == VariableType.Double)
                    propertyInfo.SetValue(type, double.Parse(inputField.text) + mouseDelta);
                if (variableType == VariableType.Decimal)
                    propertyInfo.SetValue(type, decimal.Parse(inputField.text) + (decimal)mouseDelta);
            }
            else if (fieldInfo != null)
            {
                if (variableType == VariableType.Byte)
                    fieldInfo.SetValue(type, (byte)(byte.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Sbyte)
                    fieldInfo.SetValue(type, (sbyte)(sbyte.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Short)
                    fieldInfo.SetValue(type, (short)(short.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Int)
                    fieldInfo.SetValue(type, (int)(int.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Long)
                    fieldInfo.SetValue(type, (long)(long.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Ushort)
                    fieldInfo.SetValue(type, (ushort)(ushort.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Uint)
                    fieldInfo.SetValue(type, (uint)(uint.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Ulong)
                    fieldInfo.SetValue(type, (ulong)(ulong.Parse(inputField.text) + mouseDelta));
                if (variableType == VariableType.Float)
                    fieldInfo.SetValue(type, float.Parse(inputField.text) + mouseDelta);
                if (variableType == VariableType.Double)
                    fieldInfo.SetValue(type, double.Parse(inputField.text) + mouseDelta);
                if (variableType == VariableType.Decimal)
                    fieldInfo.SetValue(type, decimal.Parse(inputField.text) + (decimal)mouseDelta);
            }
        }

        public virtual void Update()
        {
            if (!inputField.isFocused)
            {
                if (propertyInfo != null)
                    inputField.text = propertyInfo.GetValue(type).ToString();
                else if (fieldInfo != null)
                    inputField.text = fieldInfo.GetValue(type).ToString();
            }
        }

        public enum VariableType
        {
            String,
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
            Decimal
        }
    }
}