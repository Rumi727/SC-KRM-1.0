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
    [AddComponentMenu("커널/UI/비활성화 조건 (설정)")]
    public class SettingDisableCondition : MonoBehaviour
    {
        [SerializeField] GameObject _disableGameObject; public GameObject disableGameObject { get => _disableGameObject; set => _disableGameObject = value; }

        [SerializeField] string _saveLoadAttributeName = ""; public string saveLoadAttributeName { get => _saveLoadAttributeName; set => _saveLoadAttributeName = value; }
        [SerializeField] string _variableName = ""; public string variableName { get => _variableName; set => _variableName = value; }
        [SerializeField] bool _reversal = false; public bool reversal { get => _reversal; set => _reversal = value; }



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
                            this.propertyInfo = propertyInfo.variableInfo;

                            break;
                        }
                    }

                    foreach (var fieldInfo in variableType.fieldInfos)
                    {
                        if (fieldInfo.variableInfo.Name == variableName)
                        {
                            type = fieldInfo.variableInfo.FieldType;
                            this.fieldInfo = fieldInfo.variableInfo;

                            break;
                        }
                    }
                }
            }

            if (type != typeof(bool))
                enabled = false;
        }

        public virtual void Update()
        {
            if (propertyInfo != null)
            {
                if ((bool)propertyInfo.GetValue(typeof(bool)))
                {
                    if (reversal)
                    {
                        if (disableGameObject.activeSelf)
                            disableGameObject.SetActive(false);
                    }
                    else if (!disableGameObject.activeSelf)
                            disableGameObject.SetActive(true);
                }
                else
                {
                    if (reversal)
                    {
                        if (!disableGameObject.activeSelf)
                            disableGameObject.SetActive(true);
                    }
                    else if (disableGameObject.activeSelf)
                        disableGameObject.SetActive(false);
                }
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