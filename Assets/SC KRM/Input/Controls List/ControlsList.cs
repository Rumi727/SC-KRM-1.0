using SCKRM.Language;
using SCKRM.Object;
using SCKRM.Resource;
using SCKRM.Tool;
using System.Collections.Generic;
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
                List<KeyCode> keyCodes = item.Value;

                if (keyCodes.Contains(KeyCode.Escape))
                    continue;

                if (InputManager.SaveData.controlSettingList.ContainsKey(item.Key))
                    keyCodes = InputManager.SaveData.controlSettingList[item.Key];

                ControlsButton controlsButton = (ControlsButton)ObjectPoolingSystem.ObjectCreate("controls_list.controls_button", transform);
                controlsButton.key = item.Key;
                controlsButton.inputLockObject = inputLockObject;

                string keyLanguage = ResourceManager.SearchLanguage(controlsButton.key);
                if (keyLanguage != "null")
                    controlsButton.keyText.text = keyLanguage;
                else
                    controlsButton.keyText.text = controlsButton.key;


                string text = "";
                for (int i = 0; i < keyCodes.Count; i++)
                {
                    KeyCode keyCode = keyCodes[i];

                    if (i != keyCodes.Count - 1)
                        text += keyCode.KeyCodeToString() + " + ";
                    else
                        text = keyCode.KeyCodeToString();
                }
                controlsButton.valueText.text = text;
            }
        }
    }
}