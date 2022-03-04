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
            if (variableType != VariableType.Bool)
                return;

            SaveValue(toggle.isOn);
            onValueChanged.Invoke();
        }

        public override void SetDefault()
        {
            base.SetDefault();
            onValueChanged.Invoke();
        }

        protected override void Update()
        {
            base.Update();

            if (!Kernel.isInitialLoadEnd || variableType != VariableType.Bool)
                return;

            bool value = (bool)GetValue();

            toggle.isOn = value;
            isDefault = (bool)defaultValue == value;
        }
    }
}