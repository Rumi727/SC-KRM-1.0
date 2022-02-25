using HSVPicker;
using SCKRM.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCKRM.UI.Setting
{
    [AddComponentMenu("커널/UI/컬러 피커 (세이브 파일 연동)")]
    public class SettingColorPicker : Setting
    {
        [SerializeField] ColorPicker _colorPicker; public ColorPicker colorPicker { get => _colorPicker; set => _colorPicker = value; }

        public virtual void OnValueChanged()
        {
            if (variableType == VariableType.JColor)
                SaveValue((JColor)colorPicker.CurrentColor);
            else if (variableType == VariableType.JColor32)
                SaveValue((JColor32)colorPicker.CurrentColor);
        }

        public virtual void Update()
        {
            if (variableType == VariableType.JColor)
                colorPicker.CurrentColor = (Color)(JColor)GetValue();
            else if (variableType == VariableType.JColor32)
                colorPicker.CurrentColor = (Color)(JColor32)GetValue();
        }
    }
}