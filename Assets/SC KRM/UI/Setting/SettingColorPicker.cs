using HSVPicker;
using SCKRM.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SCKRM.UI.Setting
{
    [AddComponentMenu("커널/UI/컬러 피커 (세이브 파일 연동)")]
    public class SettingColorPicker : Setting
    {
        [SerializeField] ColorPicker _colorPicker; public ColorPicker colorPicker { get => _colorPicker; set => _colorPicker = value; }
        [SerializeField] UnityEvent _onValueChanged = new UnityEvent(); public UnityEvent onValueChanged { get => _onValueChanged; set => _onValueChanged = value; }

        public virtual void OnValueChanged()
        {
            if (variableType == VariableType.JColor)
                SaveValue((JColor)colorPicker.CurrentColor);
            else if (variableType == VariableType.JColor32)
                SaveValue((JColor32)colorPicker.CurrentColor);

            onValueChanged.Invoke();
        }

        public override void SetDefault()
        {
            base.SetDefault();
            onValueChanged.Invoke();
        }

        protected override void Update()
        {
            base.Update();

            if (variableType == VariableType.JColor)
            {
                Color value = (Color)(JColor)GetValue();

                colorPicker.CurrentColor = value;
                isDefault = (Color)(JColor)defaultValue == value;
            }
            else if (variableType == VariableType.JColor32)
            {
                Color value = (Color)(JColor32)GetValue();

                colorPicker.CurrentColor = value;
                isDefault = (Color)(JColor32)defaultValue == value;
            }
        }
    }
}