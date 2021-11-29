using SCKRM.Language;
using SCKRM.Object;
using SCKRM.Tool;
using UnityEngine;

namespace SCKRM.Input.UI
{
    [AddComponentMenu("커널/Input/조작 설정 리스트/조작 설정 리스트", 0)]
    public class ControlsList : MonoBehaviour
    {
        public GameObject inputLockObject;

        void OnEnable()
        {
            ControlsButton[] child = GetComponentsInChildren<ControlsButton>();
            for (int i = 0; i < child.Length; i++)
            {
                ControlsButton item = child[i];
                item.Remove();
            }

            foreach (var item in InputManager.Data.controlSettingList)
            {
                KeyCode keyCode = item.Value;

                if (keyCode == KeyCode.Escape)
                    continue;

                if (InputManager.SaveData.controlSettingList.ContainsKey(item.Key))
                    keyCode = InputManager.SaveData.controlSettingList[item.Key];

                ControlsButton controlsButton = ObjectPoolingSystem.ObjectCreate("controls_list.controls_button", transform).GetComponent<ControlsButton>();
                controlsButton.key = item.Key;
                controlsButton.inputLockObject = inputLockObject;

                string keyLanguage = LanguageManager.TextLoad(controlsButton.key);
                if (keyLanguage != "null")
                    controlsButton.keyText.text = keyLanguage;
                else
                    controlsButton.keyText.text = controlsButton.key;

                controlsButton.valueText.text = keyCode.KeyCodeToString();
            }
        }
    }
}