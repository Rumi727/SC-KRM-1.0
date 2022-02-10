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

namespace SCKRM.UI.Setting
{
    [AddComponentMenu("커널/UI/인풋 필드 (설정)")]
    public class SettingInputField : SettingDrag
    {
        [SerializeField] string _textPlaceHolderNameSpace = ""; public string textPlaceHolderNameSpace { get => _textPlaceHolderNameSpace; set => _textPlaceHolderNameSpace = value; }
        [SerializeField] string _numberPlaceHolderNameSpace = ""; public string numberPlaceHolderNameSpace { get => _numberPlaceHolderNameSpace; set => _numberPlaceHolderNameSpace = value; }

        [SerializeField] string _textPlaceHolderPath = ""; public string textPlaceHolderPath { get => _textPlaceHolderPath; set => _textPlaceHolderPath = value; }
        [SerializeField] string _numberPlaceHolderPath = ""; public string numberPlaceHolderPath { get => _numberPlaceHolderPath; set => _numberPlaceHolderPath = value; }



        [SerializeField] TMP_InputField _inputField; public TMP_InputField inputField { get => _inputField; set => _inputField = value; }
        [SerializeField] CustomTextMeshProRenderer _placeholder; public CustomTextMeshProRenderer placeholder { get => _placeholder; set => _placeholder = value; }



        public override async UniTask Awake()
        {
            await base.Awake();

            if (variableType == VariableType.String)
            {
                inputField.contentType = TMP_InputField.ContentType.Standard;

                placeholder.nameSpace = textPlaceHolderNameSpace;
                placeholder.path = textPlaceHolderPath;

                placeholder.ResourceReload();
            }
            else if (variableType != VariableType.Float && variableType != VariableType.Double && variableType != VariableType.Decimal)
            {
                inputField.contentType = TMP_InputField.ContentType.IntegerNumber;

                placeholder.nameSpace = numberPlaceHolderNameSpace;
                placeholder.path = numberPlaceHolderPath;

                placeholder.ResourceReload();
            }
            else
            {
                inputField.contentType = TMP_InputField.ContentType.DecimalNumber;

                placeholder.nameSpace = numberPlaceHolderNameSpace;
                placeholder.path = numberPlaceHolderPath;

                placeholder.ResourceReload();
            }
        }

        public virtual void OnEndEdit()
        {
            if (variableType != VariableType.String && string.IsNullOrEmpty(inputField.text))
                inputField.text = "0";

            SaveStringValue(inputField.text);
        }

        public virtual void Update()
        {
            if (Kernel.isInitialLoadEnd && !inputField.isFocused)
                inputField.text = GetValue().ToString();
        }
    }
}