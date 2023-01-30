using ExtendedNumerics;
using SCKRM.Json;
using SCKRM.Object;
using SCKRM.Renderer;
using SCKRM.UI;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using UnityEngine;

namespace SCKRM.SaveLoad.UI
{
    [Obsolete("Incomplete!")]
    public sealed class SaveLoadUI : UIObjectPooling
    {
        [SerializeField] bool _autoRefresh = false; public bool autoRefresh { get => _autoRefresh; set => _autoRefresh = value; }
        [SerializeField] string _saveLoadClassName = ""; public string saveLoadClassName { get => _saveLoadClassName; set => _saveLoadClassName = value; }

        [SerializeField] bool _isLast = false; public bool isLast { get => _isLast; set => _isLast = value; }

        [SerializeField] string _titlePrefab = "save_load.ui.title"; public string titlePrefab { get => _titlePrefab; set => _titlePrefab = value; }
        [SerializeField] string _linePrefab = "save_load.ui.line"; public string linePrefab { get => _linePrefab; set => _linePrefab = value; }
        [SerializeField] string _spacePrefab = "save_load.ui.space"; public string spacePrefab { get => _spacePrefab; set => _spacePrefab = value; }

        [SerializeField] SaveLoadUIPrefab _saveLoadUIPrefab = new SaveLoadUIPrefab(); public SaveLoadUIPrefab saveLoadUIPrefab { get => _saveLoadUIPrefab; }

        protected override void Awake()
        {
            if (autoRefresh)
                Refresh();
        }

        List<IObjectPooling> objectPoolingBases = new List<IObjectPooling>();
        public void Refresh()
        {
            for (int i = 0; i < objectPoolingBases.Count; i++)
                objectPoolingBases[i].Remove();

            SaveLoadClass slc = null;
            for (int i = 0; i < SaveLoadManager.generalSLCList.Length; i++)
            {
                SaveLoadClass tempSlc = SaveLoadManager.generalSLCList[i];
                if (tempSlc.name == saveLoadClassName)
                    slc = tempSlc;
            }

            if (slc == null)
                return;

            Attribute[] attributes = Attribute.GetCustomAttributes(slc.type, typeof(SaveLoadUIAttribute));
            if (attributes == null || attributes.Length <= 0)
                return;

            SaveLoadUIAttribute saveLoadUIAttribute = (SaveLoadUIAttribute)attributes[0];
            NameSpacePathPair titleName = saveLoadUIAttribute.name;

            SaveLoadUITitle title = ObjectCreateMethod<SaveLoadUITitle>(titlePrefab);
            title.customTextMeshProRenderer.nameSpacePathPair = titleName;
            title.customTextMeshProRenderer.Refresh();

            for (int j = 0; j < slc.propertyInfos.Length; j++)
                ObjectCreate(slc.propertyInfos[j]);

            for (int j = 0; j < slc.fieldInfos.Length; j++)
                ObjectCreate(slc.fieldInfos[j]);

            if (!isLast)
            {
                ObjectCreateMethod<MonoBehaviour>(spacePrefab);
                ObjectCreateMethod<MonoBehaviour>(linePrefab);
                ObjectCreateMethod<MonoBehaviour>(spacePrefab);
            }

            void ObjectCreate<T>(SaveLoadClass.SaveLoadVariable<T> slv) where T : MemberInfo
            {
                T memberInfo = slv.variableInfo;
                if (Attribute.GetCustomAttribute(memberInfo, typeof(SaveLoadUIIgnoreAttribute)) != null)
                    return;

                Type type;
                string name = memberInfo.Name;
                if (memberInfo is PropertyInfo)
                    type = ((PropertyInfo)(MemberInfo)memberInfo).PropertyType;
                else if (memberInfo is FieldInfo)
                    type = ((FieldInfo)(MemberInfo)memberInfo).FieldType;
                else
                    return;

                #region Type Method Invoke
                if (type == typeof(char))
                    Text();
                else if (type == typeof(string))
                    Text();
                else if (type == typeof(bool))
                    Toggle();
                else if (type == typeof(byte))
                    Text();
                else if (type == typeof(sbyte))
                    Text();
                else if (type == typeof(short))
                    Text();
                else if (type == typeof(int))
                    Text();
                else if (type == typeof(int))
                    Text();
                else if (type == typeof(ushort))
                    Text();
                else if (type == typeof(uint))
                    Text();
                else if (type == typeof(ulong))
                    Text();
                else if (type == typeof(float))
                    Text();
                else if (type == typeof(double))
                    Text();
                else if (type == typeof(decimal))
                    Text();
                else if (type == typeof(JColor))
                    Color();
                else if (type == typeof(JColor32))
                    Color();
                else if (type == typeof(BigInteger))
                    Text();
                else if (type == typeof(BigDecimal))
                    Text();
                #endregion

                void Text()
                {
                    if (type != typeof(char) && type != typeof(string))
                    {
                        SaveLoadUISlider slider = ObjectCreateMethod<SaveLoadUISlider>(saveLoadUIPrefab.slider);

                        slider.saveLoadClassName = slc.name;
                        slider.variableName = name;

                        SaveLoadUISliderConfigAttribute sliderConfig = (SaveLoadUISliderConfigAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(SaveLoadUISliderConfigAttribute));
                        if (sliderConfig == null)
                        {
                            InputField();
                            return;
                        }

                        slider.roundingDigits = sliderConfig.roundingDigits;
                        slider.hotkeyToDisplays = sliderConfig.hotkeyToDisplay;

                        slider.mouseSensitivity = sliderConfig.mouseSensitivity;

                        slider.slider.minValue = sliderConfig.min;
                        slider.slider.maxValue = sliderConfig.max;

                        slider.Refresh();
                    }
                    else
                        InputField();

                    void InputField()
                    {
                        SaveLoadUIInputField inputField = ObjectCreateMethod<SaveLoadUIInputField>(saveLoadUIPrefab.inputField);

                        inputField.saveLoadClassName = slc.name;
                        inputField.variableName = name;

                        SaveLoadUIInputFieldConfigAttribute inputFieldConfig = (SaveLoadUIInputFieldConfigAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(SaveLoadUIInputFieldConfigAttribute));
                        if (inputFieldConfig == null)
                            return;

                        inputField.roundingDigits = inputFieldConfig.roundingDigits;
                        inputField.hotkeyToDisplays = inputFieldConfig.hotkeyToDisplay;

                        inputField.mouseSensitivity = inputFieldConfig.mouseSensitivity;

                        inputField.Refresh();
                    }
                }

                void Color()
                {
                    SaveLoadUIColorPicker colorPicker = ObjectCreateMethod<SaveLoadUIColorPicker>(saveLoadUIPrefab.colorPicker);

                    colorPicker.saveLoadClassName = slc.name;
                    colorPicker.variableName = name;

                    SaveLoadUIColorPickerConfigAttribute colorPickerConfig = (SaveLoadUIColorPickerConfigAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(SaveLoadUIColorPickerConfigAttribute));
                    if (colorPickerConfig == null)
                        return;

                    colorPicker.roundingDigits = colorPickerConfig.roundingDigits;
                    colorPicker.hotkeyToDisplays = colorPickerConfig.hotkeyToDisplay;

                    colorPicker.colorPicker.Setup.ShowAlpha = colorPickerConfig.alphaShow;

                    colorPicker.Refresh();
                }

                void Toggle()
                {
                    SaveLoadUIToggle toggle = ObjectCreateMethod<SaveLoadUIToggle>(saveLoadUIPrefab.toggle);

                    toggle.saveLoadClassName = slc.name;
                    toggle.variableName = name;

                    SaveLoadUIToggleConfigAttribute toggleConfig = (SaveLoadUIToggleConfigAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(SaveLoadUIToggleConfigAttribute));
                    if (toggleConfig == null)
                        return;

                    toggle.roundingDigits = toggleConfig.roundingDigits;
                    toggle.hotkeyToDisplays = toggleConfig.hotkeyToDisplay;

                    toggle.Refresh();
                }
            }

            T ObjectCreateMethod<T>(string key) where T : MonoBehaviour
            {
                (MonoBehaviour monoBehaviour, IObjectPooling objectPooling) = ObjectPoolingSystem.ObjectCreate(key, transform);
                objectPoolingBases.Add(objectPooling);

                return (T)monoBehaviour;
            }
        }

        [Serializable]
        public sealed class SaveLoadUIPrefab
        {
            public string colorPicker = "save_load.ui.color_picker";
            public string dropdown = "save_load.ui.dropdown";
            public string inputField = "save_load.ui.input_field";
            public string slider = "save_load.ui.slider";
            public string toggle = "save_load.ui.toggle";
        }
    }
}
