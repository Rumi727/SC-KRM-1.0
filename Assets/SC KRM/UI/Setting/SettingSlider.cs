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
using UnityEngine.Events;
using UnityEngine.UI;

namespace SCKRM.UI.Setting
{
    [AddComponentMenu("커널/UI/슬라이더 (세이브 파일 연동)")]
    public class SettingSlider : SettingInputField
    {
        [SerializeField] Slider _slider; public Slider slider { get => _slider; set => _slider = value; }
        [SerializeField] UnityEvent _onValueChanged = new UnityEvent(); public UnityEvent onValueChanged { get => _onValueChanged; set => _onValueChanged = value; }

        public void OnValueChanged()
        {
            if (invokeLock)
                return;

            SaveValueFloat(slider.value);
            onValueChanged.Invoke();
        }

        public override void SetDefault()
        {
            base.SetDefault();
            onValueChanged.Invoke();
        }

        bool invokeLock = false;
        protected override void Update()
        {
            base.Update();

            if (Kernel.isInitialLoadEnd && variableType != VariableType.String)
            {
                invokeLock = true;
                slider.value = GetValueFloat();
                invokeLock = false;

                isDefault = GetValue().ToString() == defaultValue.ToString();
            }
        }
    }
}