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

namespace SCKRM.UI.Setting.InputField
{
    [AddComponentMenu("커널/UI/토글")]
    public class SettingToggle : MonoBehaviour
    {
        [SerializeField] string _saveLoadAttributeName = ""; public string saveLoadAttributeName { get => _saveLoadAttributeName; set => _saveLoadAttributeName = value; }
        [SerializeField] string _variableName = ""; public string variableName { get => _variableName; set => _variableName = value; }


        [SerializeField] Toggle _toggle; public Toggle toggle { get => _toggle; set => _toggle = value; }



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
        }

        public virtual void OnValueChanged()
        {
            if (type != typeof(bool))
                return;

            bool value = toggle.isOn;
            if (propertyInfo != null)
                propertyInfo.SetValue(type, value);
            else if (fieldInfo != null)
                fieldInfo.SetValue(type, value);
        }

        public virtual void Update()
        {
            if (type != typeof(bool))
                return;

            if (propertyInfo != null)
                toggle.isOn = (bool)propertyInfo.GetValue(type);
            else if (fieldInfo != null)
                toggle.isOn = (bool)fieldInfo.GetValue(type);
        }
    }
}