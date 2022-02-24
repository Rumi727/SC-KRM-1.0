using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.Setting
{
    [AddComponentMenu("커널/UI/토글 (세이브 파일 연동)")]
    public class SettingToggle : Setting
    {
        [SerializeField] Toggle _toggle; public Toggle toggle { get => _toggle; set => _toggle = value; }

        public virtual void OnValueChanged()
        {
            if (variableType != VariableType.Bool)
                return;

            SaveValue(toggle.isOn);
        }

        public virtual void Update()
        {
            if (!Kernel.isInitialLoadEnd || variableType != VariableType.Bool)
                return;

            toggle.isOn = (bool)GetValue();
        }
    }
}