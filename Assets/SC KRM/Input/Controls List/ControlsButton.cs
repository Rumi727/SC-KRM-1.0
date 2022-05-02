using Newtonsoft.Json.Linq;
using SCKRM.Object;
using SCKRM.ProjectSetting;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Input.UI
{
    [AddComponentMenu("커널/Input/입력 버튼", 0)]
    public class ControlsButton : ObjectPooling
    {
        public string key = "";

        public Text keyText;
        public Text valueText;

        public GameObject inputLockObject;

        public void KeyReset()
        {
            JObject jObject = ProjectSettingManager.Read(typeof(InputManager.Data));
            KeyCode keyCode = jObject["controlSettingList"][key].ToObject<KeyCode>();
            InputManager.SaveData.controlSettingList.Remove(key);
            valueText.text = keyCode.KeyCodeToString();
        }

        public override void Remove()
        {
            base.Remove();

            key = "";
            keyText.text = "";
            valueText.text = "";
            valueText.color = Color.white;
        }
    }
}