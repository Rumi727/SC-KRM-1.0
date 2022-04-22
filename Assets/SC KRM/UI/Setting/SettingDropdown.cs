using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SCKRM.UI.Setting
{
    [AddComponentMenu("커널/UI/드롭다운 (세이브 파일 연동)")]
    public class SettingDropdown : Setting
    {
        [SerializeField] Dropdown _dropdown; public Dropdown dropdown { get => _dropdown; set => _dropdown = value; }
        [SerializeField] UnityEvent _onValueChanged = new UnityEvent(); public UnityEvent onValueChanged { get => _onValueChanged; set => _onValueChanged = value; }

        public virtual void OnValueChanged()
        {
            if (invokeLock)
                return;

            if (variableType == VariableType.String)
            {
                if (dropdown.value >= 0 && dropdown.value < dropdown.options.Length)
                    SaveValue(dropdown.options[dropdown.value]);
            }
            else
                SaveValue(dropdown.value);

            onValueChanged.Invoke();
        }

        public override void SetDefault()
        {
            base.SetDefault();
            onValueChanged.Invoke();
        }

        [NonSerialized] bool invokeLock = false;
        protected override void Update()
        {
            base.Update();

            if (!Kernel.isInitialLoadEnd || !isLoad)
                return;

            if (variableType == VariableType.String)
            {
                string value = (string)GetValue();

                invokeLock = true;
                dropdown.value = Array.IndexOf(dropdown.options, value);
                invokeLock = false;

                isDefault = (string)defaultValue == value;
            }
            else
            {
                int value = GetValueInt();

                invokeLock = true;
                dropdown.value = value;
                invokeLock = false;

                if (variableType == VariableType.Bool)
                    isDefault = defaultValue.ToString() == GetValue().ToString();
            }
        }
    }
}