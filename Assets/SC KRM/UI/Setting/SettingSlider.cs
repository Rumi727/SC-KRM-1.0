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
using UnityEngine.UI;

namespace SCKRM.UI.Setting.InputField
{
    [AddComponentMenu("커널/UI/슬라이더 (설정)")]
    public class SettingSlider : SettingInputField
    {
        [SerializeField] Slider _slider; public Slider slider { get => _slider; set => _slider = value; }

        public void OnValueChanged()
        {
            float value = slider.value;
            if (propertyInfo != null)
            {
                if (variableType == VariableType.Byte)
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
                if (variableType == VariableType.Byte)
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

        public override void Update()
        {
            base.Update();

            if (variableType != VariableType.String)
            {
                if (propertyInfo != null)
                    slider.value = (float)Convert.ChangeType(propertyInfo.GetValue(type), typeof(float));
                else if (fieldInfo != null)
                    slider.value = (float)Convert.ChangeType(fieldInfo.GetValue(type), typeof(float));
            }
        }
    }
}