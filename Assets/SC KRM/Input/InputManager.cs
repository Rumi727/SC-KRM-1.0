using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.ProjectSetting;
using SCKRM.SaveLoad;
using SCKRM.Threads;
using SCKRM.UI.TaskBar;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SCKRM.Input
{
    public sealed class InputManager : MonoBehaviour
    {
        [ProjectSetting("ControlSetting")]
        public sealed class Data
        {
            [JsonProperty] public static Dictionary<string, KeyCode> controlSettingList { get; set; } = new Dictionary<string, KeyCode>();
        }

        [SaveLoad("Input")]
        public sealed class SaveData
        {
            [JsonProperty] public static Dictionary<string, KeyCode> controlSettingList { get; set; } = new Dictionary<string, KeyCode>();
            [JsonProperty] public static bool mouseUpsideDown { get; set; } = false;
        }



        public static bool defaultInputLock { get; set; }

        public static KeyCode[] unityKeyCodeList { get; } = Enum.GetValues(typeof(KeyCode)) as KeyCode[];



        public void OnDelta(InputAction.CallbackContext context) => mouseDelta = context.ReadValue<Vector2>();
        public void OnPosition(InputAction.CallbackContext context) => mousePosition = context.ReadValue<Vector2>();
        public void OnScroll(InputAction.CallbackContext context) => mouseScrollDelta = context.ReadValue<Vector2>();


        #region Input Check
        public static bool GetKeyDown(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && ((keyDownToggle2.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny | keyDownToggle2[keyCode])) || UnityEngine.Input.GetKeyDown(keyCode));
        public static bool GetKeyDown(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException("");
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException("GetKeyDown");

            if (keyDownToggle.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny | keyDownToggle[keyCode]))
                return true;

            if (!InputLockCheck(inputLockDeny))
            {
                if (SaveData.controlSettingList.ContainsKey(keyCode))
                    return UnityEngine.Input.GetKeyDown(SaveData.controlSettingList[keyCode]);
                else if (Data.controlSettingList.ContainsKey(keyCode))
                    return UnityEngine.Input.GetKeyDown(Data.controlSettingList[keyCode]);
            }
            return false;
        }

        public static bool GetKey(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && (keyToggle2.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny | keyToggle2[keyCode]) || UnityEngine.Input.GetKey(keyCode));
        public static bool GetKey(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException("");
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException("GetKey");

            if (keyToggle.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny | keyToggle[keyCode]))
                return true;

            if (!InputLockCheck(inputLockDeny))
            {
                if (SaveData.controlSettingList.ContainsKey(keyCode))
                    return UnityEngine.Input.GetKey(SaveData.controlSettingList[keyCode]);
                else if (Data.controlSettingList.ContainsKey(keyCode))
                    return UnityEngine.Input.GetKey(Data.controlSettingList[keyCode]);
            }
            return false;
        }

        public static bool GetKeyUp(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && (keyUpToggle2.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny | keyUpToggle2[keyCode]) || UnityEngine.Input.GetKeyUp(keyCode));
        public static bool GetKeyUp(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException("");
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException("GetKeyUp");

            if (keyUpToggle.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny | keyUpToggle[keyCode]))
                return true;

            if (!InputLockCheck(inputLockDeny))
            {
                if (SaveData.controlSettingList.ContainsKey(keyCode))
                    return UnityEngine.Input.GetKeyUp(SaveData.controlSettingList[keyCode]);
                else if (Data.controlSettingList.ContainsKey(keyCode))
                    return UnityEngine.Input.GetKeyUp(Data.controlSettingList[keyCode]);
            }
            return false;
        }

        public static Vector2 mousePosition { get; private set; }

        static Vector2 mouseDelta = Vector2.zero;
        public static Vector2 GetMouseDelta(InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if (!InputLockCheck(inputLockDeny))
                return mouseDelta;
            else
                return Vector2.zero;
        }

        static Vector2 mouseScrollDelta = Vector2.zero;
        public static Vector2 GetMouseScrollDelta(InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if (!InputLockCheck(inputLockDeny))
                return mouseScrollDelta;
            else
                return Vector2.zero;
        }



        public static bool GetAnyKeyDown(InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && UnityEngine.Input.anyKeyDown;
        public static bool GetAnyKey(InputLockDeny inputLockDeny = InputLockDeny.None) => !InputLockCheck(inputLockDeny) && UnityEngine.Input.anyKey;

        public static bool InputLockCheck(InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if ((inputLockDeny & InputLockDeny.All) != 0)
                return false;

            bool inputLock = false;
            if (defaultInputLock && (inputLockDeny & InputLockDeny.Default) == 0)
                inputLock = true;
            if (TaskBarManager.isTaskBarShow && (inputLockDeny & InputLockDeny.TaskBar) == 0)
                inputLock = true;

            return inputLock;
        }
        #endregion


        #region Key Toggle
        static Dictionary<string, InputLockDeny> keyDownToggle { get; } = new Dictionary<string, InputLockDeny>();
        static Dictionary<string, InputLockDeny> keyToggle { get; } = new Dictionary<string, InputLockDeny>();
        static Dictionary<string, InputLockDeny> keyUpToggle { get; } = new Dictionary<string, InputLockDeny>();
        static Dictionary<KeyCode, InputLockDeny> keyDownToggle2 { get; } = new Dictionary<KeyCode, InputLockDeny>();
        static Dictionary<KeyCode, InputLockDeny> keyToggle2 { get; } = new Dictionary<KeyCode, InputLockDeny>();
        static Dictionary<KeyCode, InputLockDeny> keyUpToggle2 { get; } = new Dictionary<KeyCode, InputLockDeny>();

        public static async void KeyDownEnable(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if (keyDownToggle.ContainsKey(keyCode))
                keyDownToggle.Remove(keyCode);
            else
            {
                keyDownToggle.Add(keyCode, inputLockDeny);
                await UniTask.WaitForEndOfFrame();
                keyDownToggle.Remove(keyCode);
            }
        }

        public static async void KeyEnable(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if (keyToggle.ContainsKey(keyCode))
                keyToggle.Remove(keyCode);
            else
            {
                keyToggle.Add(keyCode, inputLockDeny);
                await UniTask.WaitForEndOfFrame();
                keyToggle.Remove(keyCode);
            }
        }

        public static void KeyToggle(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if (keyToggle.ContainsKey(keyCode))
                keyToggle.Remove(keyCode);
            else
                keyToggle.Add(keyCode, inputLockDeny);
        }

        public static async void KeyUpEnable(string keyCode, InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if (keyUpToggle.ContainsKey(keyCode))
                keyUpToggle.Remove(keyCode);
            else
            {
                keyUpToggle.Add(keyCode, inputLockDeny);
                await UniTask.WaitForEndOfFrame();
                keyUpToggle.Remove(keyCode);
            }
        }



        public static async void KeyDownEnable(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if (keyDownToggle2.ContainsKey(keyCode))
                keyDownToggle2.Remove(keyCode);
            else
            {
                keyDownToggle2.Add(keyCode, inputLockDeny);
                await UniTask.WaitForEndOfFrame();
                keyDownToggle2.Remove(keyCode);
            }
        }

        public static async void KeyEnable(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if (keyToggle2.ContainsKey(keyCode))
                keyToggle2.Remove(keyCode);
            else
            {
                keyToggle2.Add(keyCode, inputLockDeny);
                await UniTask.WaitForEndOfFrame();
                keyToggle2.Remove(keyCode);
            }
        }

        public static void KeyToggle(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if (keyToggle2.ContainsKey(keyCode))
                keyToggle2.Remove(keyCode);
            else
                keyToggle2.Add(keyCode, inputLockDeny);
        }

        public static async void KeyUpEnable(KeyCode keyCode, InputLockDeny inputLockDeny = InputLockDeny.None)
        {
            if (keyUpToggle2.ContainsKey(keyCode))
                keyUpToggle2.Remove(keyCode);
            else
            {
                keyUpToggle2.Add(keyCode, inputLockDeny);
                await UniTask.WaitForEndOfFrame();
                keyUpToggle2.Remove(keyCode);
            }
        }
        #endregion
    }

    [Flags]
    public enum InputLockDeny
    {
        None = 0,
        All = 1 << 1,
        Default = 1 << 2,
        TaskBar = 1 << 3,
    }
}