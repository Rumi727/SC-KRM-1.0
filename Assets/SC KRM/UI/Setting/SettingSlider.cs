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

namespace SCKRM.UI.Setting
{
    [AddComponentMenu("커널/UI/슬라이더 (세이브 파일 연동)")]
    public class SettingSlider : SettingInputField
    {
        [SerializeField] Slider _slider; public Slider slider { get => _slider; set => _slider = value; }

        public void OnValueChanged() => SaveValueFloat(slider.value);

        public override void Update()
        {
            base.Update();

            if (Kernel.isInitialLoadEnd && variableType != VariableType.String)
                slider.value = GetValueFloat();
        }
    }
}