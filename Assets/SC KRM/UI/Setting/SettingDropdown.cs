using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCKRM.UI.Setting
{
    public class SettingDropdown : Setting
    {
        [SerializeField] Dropdown _dropdown; public Dropdown dropdown { get => _dropdown; set => _dropdown = value; }

        public virtual void OnValueChanged()
        {
            if (variableType == VariableType.String)
            {
                if (dropdown.value >= 0 && dropdown.value < dropdown.options.Length)
                    SaveValue(dropdown.options[dropdown.value]);
            }
            else
                SaveValue(dropdown.value);
        }

        public virtual void Update()
        {
            if (variableType == VariableType.String)
                dropdown.value = Array.IndexOf(dropdown.options, (string)GetValue());
            else
                dropdown.value = GetValueInt();
        }
    }
}