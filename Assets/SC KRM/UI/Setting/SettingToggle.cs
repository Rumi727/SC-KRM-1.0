using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Renderer;
using SCKRM.SaveLoad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.Setting
{
    [AddComponentMenu("커널/UI/토글")]
    public class SettingToggle : Setting
    {
        [SerializeField] Toggle _toggle; public Toggle toggle { get => _toggle; set => _toggle = value; }

        public virtual void OnValueChanged()
        {
            if (type != typeof(bool))
                return;

            SaveValue(toggle.isOn);
        }

        public virtual void Update()
        {
            if (!Kernel.isInitialLoadEnd || type != typeof(bool))
                return;

            toggle.isOn = (bool)GetValue();
        }
    }
}