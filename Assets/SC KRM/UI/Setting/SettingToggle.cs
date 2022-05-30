using SCKRM.Renderer;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SCKRM.UI.Setting
{
    [AddComponentMenu("커널/UI/토글 (세이브 파일 연동)")]
    public class SettingToggle : Setting
    {
        [SerializeField] Toggle _toggle; public Toggle toggle { get => _toggle; set => _toggle = value; }
        [SerializeField] UnityEvent _onValueChanged = new UnityEvent(); public UnityEvent onValueChanged { get => _onValueChanged; set => _onValueChanged = value; }

        public virtual void OnValueChanged()
        {
            if (invokeLock)
                return;
            else if (variableType != VariableType.Bool)
                return;

            SaveValue(toggle.isOn);
            ScriptOnValueChanged();
        }

        public override void SetDefault()
        {
            base.SetDefault();
            ScriptOnValueChanged();
        }

        public override void ScriptOnValueChanged(bool settingInfoInvoke = true)
        {
            Update();

            if (settingInfoInvoke)
            {
                if (toggle.isOn)
                    SettingInfoInvoke(new NameSpacePathPair("gui.on", "sc-krm"));
                else
                    SettingInfoInvoke(new NameSpacePathPair("gui.off", "sc-krm"));
            }

            onValueChanged.Invoke();
        }

        [NonSerialized] bool invokeLock = false;
        protected override void Update()
        {
            base.Update();

            if (!InitialLoadManager.isInitialLoadEnd || !isLoad || variableType != VariableType.Bool)
                return;

            bool value = (bool)GetValue();

            invokeLock = true;
            toggle.isOn = value;
            invokeLock = false;

            isDefault = (bool)defaultValue == value;
        }
    }
}