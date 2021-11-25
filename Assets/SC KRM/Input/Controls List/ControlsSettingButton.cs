using System.Collections;
using UnityEngine;

namespace SCKRM.Input.UI
{
    [AddComponentMenu("커널/Input/입력 리스트/입력 설정 버튼", 0)]
    public class ControlsSettingButton : MonoBehaviour
    {
        public ControlsButton controlsButton;

        public void InputSet() => StartCoroutine(inputSet());

        IEnumerator inputSet()
        {
            InputManager.defaultInputLock = true;
            controlsButton.inputLockObject.SetActive(true);

            controlsButton.valueText.text = ">   <";
            controlsButton.valueText.color = Color.yellow;

            yield return new WaitUntil(() => UnityEngine.Input.anyKeyDown);

            controlsButton.valueText.color = Color.white;

            for (int i = 0; i < InputManager.unityKeyCodeList.Length; i++)
            {
                KeyCode keyCode = InputManager.unityKeyCodeList[i];
                if (UnityEngine.Input.GetKeyDown(keyCode))
                {
                    if (keyCode == KeyCode.Escape)
                    {
                        if (InputManager.SaveData.controlSettingList.ContainsKey(controlsButton.key))
                            InputManager.SaveData.controlSettingList[controlsButton.key] = KeyCode.None;
                        else
                            InputManager.SaveData.controlSettingList.Add(controlsButton.key, KeyCode.None);
                    }
                    else
                    {
                        if (InputManager.SaveData.controlSettingList.ContainsKey(controlsButton.key))
                            InputManager.SaveData.controlSettingList[controlsButton.key] = keyCode;
                        else
                            InputManager.SaveData.controlSettingList.Add(controlsButton.key, keyCode);
                    }

                    controlsButton.valueText.text = keyCode.KeyCodeToString();
                }
            }

            InputManager.defaultInputLock = false;
            controlsButton.inputLockObject.SetActive(false);
        }
    }
}