using SCKRM.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Input.UI
{
    [AddComponentMenu("커널/Input/입력 리스트/입력 설정 버튼", 0)]
    public class ControlsSettingButton : MonoBehaviour
    {
        public ControlsButton controlsButton;

        public void InputSet() => StartCoroutine(InputSetStartCoroutine());

        IEnumerator InputSetStartCoroutine()
        {
            controlsButton.inputLockObject.SetActive(true);

            controlsButton.valueText.text = ">   <";
            controlsButton.valueText.color = Color.yellow;

            yield return new WaitUntil(() => UnityEngine.Input.anyKeyDown);

            bool inputUp = false;
            List<KeyCode> keyCodes = new List<KeyCode>();
            while (inputUp)
            {
                for (int i = 0; i < InputManager.unityKeyCodeList.Length; i++)
                {
                    KeyCode keyCode = InputManager.unityKeyCodeList[i];
                    if (UnityEngine.Input.GetKeyDown(keyCode))
                    {
                        if (keyCode == KeyCode.Escape)
                        {
                            keyCodes = new List<KeyCode>() { KeyCode.Escape };

                            if (InputManager.SaveData.controlSettingList.ContainsKey(controlsButton.key))
                                InputManager.SaveData.controlSettingList[controlsButton.key] = new List<KeyCode>() { KeyCode.Escape };
                            else
                                InputManager.SaveData.controlSettingList.Add(controlsButton.key, new List<KeyCode>() { KeyCode.Escape });
                        }
                        else
                        {
                            keyCodes.Add(keyCode);

                            if (InputManager.SaveData.controlSettingList.ContainsKey(controlsButton.key))
                                InputManager.SaveData.controlSettingList[controlsButton.key] = keyCodes;
                            else
                                InputManager.SaveData.controlSettingList.Add(controlsButton.key, keyCodes);
                        }
                    }

                    string text = "";
                    for (int j = 0; j < keyCodes.Count; j++)
                    {
                        if (j != keyCodes.Count - 1)
                            text += keyCode.KeyCodeToString() + " + ";
                        else
                            text = keyCode.KeyCodeToString();
                    }

                    if (UnityEngine.Input.GetKeyUp(keyCode))
                    {
                        controlsButton.valueText.text = text;
                        inputUp = true;
                    }
                    else
                        controlsButton.valueText.text = "> " + text + " <";
                }

                yield return null;
            }

            controlsButton.valueText.color = Color.white;

            controlsButton.inputLockObject.SetActive(false);
        }
    }
}