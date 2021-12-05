using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.ProjectSetting;
using SCKRM.SaveLoad;
using SCKRM.Threads;
using SCKRM.UI.TaskBar;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SCKRM.Input
{
    [AddComponentMenu("커널/Input/조작 매니저", 0)]
    public sealed class InputManager : MonoBehaviour
    {
        [ProjectSetting("ControlSetting")]
        public sealed class Data
        {
            [JsonProperty] public static Dictionary<string, List<KeyCode>> controlSettingList { get; set; } = new Dictionary<string, List<KeyCode>>();
            [JsonProperty] public static Dictionary<string, bool> inputLockList { get; set; } = new Dictionary<string, bool>();
        }

        [SaveLoad("Input")]
        public sealed class SaveData
        {
            [JsonProperty] public static Dictionary<string, List<KeyCode>> controlSettingList { get; set; } = new Dictionary<string, List<KeyCode>>();
            [JsonProperty] public static bool mouseUpsideDown { get; set; } = false;
        }



        public static bool defaultInputLock { get; set; }

        public static KeyCode[] unityKeyCodeList { get; } = Enum.GetValues(typeof(KeyCode)) as KeyCode[];



        public void OnDelta(InputAction.CallbackContext context) => mouseDelta = context.ReadValue<Vector2>();
        public void OnPosition(InputAction.CallbackContext context) => mousePosition = context.ReadValue<Vector2>();
        public void OnScroll(InputAction.CallbackContext context) => mouseScrollDelta = context.ReadValue<Vector2>();


        #region Input Check
        public static bool GetKeyDown(KeyCode keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetKeyDown));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetKeyDown));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyDownToggle2.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny.Concat(keyDownToggle2[keyCode]).ToArray()))
                return true;
            else if (!InputLockCheck(inputLockDeny))
                return UnityEngine.Input.GetKeyDown(keyCode);

            return false;
        }

        public static bool GetKeyDown(string keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetKeyDown));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetKeyDown));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyDownToggle.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny.Concat(keyDownToggle[keyCode]).ToArray()))
                return true;
            else if (!InputLockCheck(inputLockDeny))
            {
                List<KeyCode> list;

                if (!SaveData.controlSettingList.TryGetValue(keyCode, out list) && !Data.controlSettingList.TryGetValue(keyCode, out list))
                    return false;

                bool input = true;
                if (list.Count <= 0)
                    return false;

                for (int i = 0; i < list.Count; i++)
                {
                    if (i != list.Count - 1)
                    {
                        if (!UnityEngine.Input.GetKey(list[i]))
                            input = false;
                    }
                    else if (!UnityEngine.Input.GetKeyDown(list[i]))
                        input = false;
                }

                return input;
            }
            return false;
        }

        public static bool GetKey(KeyCode keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetKey));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetKey));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyToggle2.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny.Concat(keyToggle2[keyCode]).ToArray()))
                return true;
            else if (!InputLockCheck(inputLockDeny))
                return UnityEngine.Input.GetKey(keyCode);

            return false;
        }

        public static bool GetKey(string keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetKey));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetKey));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyToggle.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny.Concat(keyToggle[keyCode]).ToArray()))
                return true;
            else if (!InputLockCheck(inputLockDeny))
            {
                List<KeyCode> list;

                if (!SaveData.controlSettingList.TryGetValue(keyCode, out list) && !Data.controlSettingList.TryGetValue(keyCode, out list))
                    return false;

                bool input = true;
                if (list.Count <= 0)
                    return false;

                for (int i = 0; i < list.Count; i++)
                {
                    if (!UnityEngine.Input.GetKey(list[i]))
                        input = false;
                }

                return input;
            }
            return false;
        }

        public static bool GetKeyUp(KeyCode keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetKeyUp));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetKeyUp));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];
            
            if (keyUpToggle2.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny.Concat(keyUpToggle2[keyCode]).ToArray()))
                return true;
            else if (!InputLockCheck(inputLockDeny))
                return UnityEngine.Input.GetKeyUp(keyCode);

            return false;
        }

        public static bool GetKeyUp(string keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetKeyUp));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetKeyUp));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyUpToggle.ContainsKey(keyCode) && !InputLockCheck(inputLockDeny.Concat(keyUpToggle[keyCode]).ToArray()))
                return true;
            else if (!InputLockCheck(inputLockDeny))
            {
                List<KeyCode> list;

                if (!SaveData.controlSettingList.TryGetValue(keyCode, out list) && !Data.controlSettingList.TryGetValue(keyCode, out list))
                    return false;

                bool input = true;
                if (list.Count <= 0)
                    return false;

                for (int i = 0; i < list.Count; i++)
                {
                    if (i != list.Count - 1 && !UnityEngine.Input.GetKey(list[i]))
                        input = false;
                    else if (!UnityEngine.Input.GetKeyUp(list[i]))
                        input = false;
                }

                return input;
            }
            return false;
        }

        public static Vector2 mousePosition { get; private set; }

        static Vector2 mouseDelta = Vector2.zero;
        public static Vector2 GetMouseDelta(params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetMouseDelta));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetMouseDelta));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (!InputLockCheck(inputLockDeny))
                return mouseDelta;
            else
                return Vector2.zero;
        }

        static Vector2 mouseScrollDelta = Vector2.zero;
        public static Vector2 GetMouseScrollDelta(params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetMouseScrollDelta));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetMouseScrollDelta));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (!InputLockCheck(inputLockDeny))
                return mouseScrollDelta;
            else
                return Vector2.zero;
        }



        public static bool GetAnyKeyDown(params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetAnyKeyDown));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetAnyKeyDown));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            return !InputLockCheck(inputLockDeny) && UnityEngine.Input.anyKeyDown;
        }

        public static bool GetAnyKey(params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetAnyKey));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetAnyKey));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            return !InputLockCheck(inputLockDeny) && UnityEngine.Input.anyKey;
        }

        public static bool InputLockCheck(params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(InputLockCheck));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(InputLockCheck));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (inputLockDeny.Contains("all"))
            {
                foreach (var item in Data.inputLockList)
                {
                    if (item.Value && inputLockDeny.Contains(item.Key))
                        return true;
                }

                return false;
            }

            foreach (var item in Data.inputLockList)
            {
                if (item.Value && !inputLockDeny.Contains(item.Key))
                    return true;
            }

            return false;
        }
        #endregion


        #region Key Toggle
        static Dictionary<string, string[]> keyDownToggle { get; } = new Dictionary<string, string[]>();
        static Dictionary<string, string[]> keyToggle { get; } = new Dictionary<string, string[]>();
        static Dictionary<string, string[]> keyUpToggle { get; } = new Dictionary<string, string[]>();
        static Dictionary<KeyCode, string[]> keyDownToggle2 { get; } = new Dictionary<KeyCode, string[]>();
        static Dictionary<KeyCode, string[]> keyToggle2 { get; } = new Dictionary<KeyCode, string[]>();
        static Dictionary<KeyCode, string[]> keyUpToggle2 { get; } = new Dictionary<KeyCode, string[]>();

        public static async UniTaskVoid KeyDownEnable(string keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(KeyDownEnable));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(KeyDownEnable));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyDownToggle.ContainsKey(keyCode))
                keyDownToggle.Remove(keyCode);
            else
            {
                keyDownToggle.Add(keyCode, inputLockDeny);
                await UniTask.WaitForEndOfFrame();
                keyDownToggle.Remove(keyCode);
            }
        }

        public static async UniTaskVoid KeyEnable(string keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(KeyEnable));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(KeyEnable));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyToggle.ContainsKey(keyCode))
                keyToggle.Remove(keyCode);
            else
            {
                keyToggle.Add(keyCode, inputLockDeny);
                await UniTask.WaitForEndOfFrame();
                keyToggle.Remove(keyCode);
            }
        }

        public static void KeyToggle(string keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(KeyToggle));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(KeyToggle));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyToggle.ContainsKey(keyCode))
                keyToggle.Remove(keyCode);
            else
                keyToggle.Add(keyCode, inputLockDeny);
        }

        public static async UniTaskVoid KeyUpEnable(string keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(KeyUpEnable));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(KeyUpEnable));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyUpToggle.ContainsKey(keyCode))
                keyUpToggle.Remove(keyCode);
            else
            {
                keyUpToggle.Add(keyCode, inputLockDeny);
                await UniTask.WaitForEndOfFrame();
                keyUpToggle.Remove(keyCode);
            }
        }



        public static async UniTaskVoid KeyDownEnable(KeyCode keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(KeyDownEnable));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(KeyDownEnable));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyDownToggle2.ContainsKey(keyCode))
                keyDownToggle2.Remove(keyCode);
            else
            {
                keyDownToggle2.Add(keyCode, inputLockDeny);
                await UniTask.WaitForEndOfFrame();
                keyDownToggle2.Remove(keyCode);
            }
        }

        public static async UniTaskVoid KeyEnable(KeyCode keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(KeyEnable));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(KeyEnable));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyToggle2.ContainsKey(keyCode))
                keyToggle2.Remove(keyCode);
            else
            {
                keyToggle2.Add(keyCode, inputLockDeny);
                await UniTask.WaitForEndOfFrame();
                keyToggle2.Remove(keyCode);
            }
        }

        public static void KeyToggle(KeyCode keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(KeyToggle));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(KeyToggle));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (keyToggle2.ContainsKey(keyCode))
                keyToggle2.Remove(keyCode);
            else
                keyToggle2.Add(keyCode, inputLockDeny);
        }

        public static async UniTaskVoid KeyUpEnable(KeyCode keyCode, params string[] inputLockDeny)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(KeyUpEnable));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(KeyUpEnable));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

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

        public static void SetInputLock(string key)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(SetInputLock));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(SetInputLock));

            if (Data.inputLockList.ContainsKey(key))
                Data.inputLockList[key] = !Data.inputLockList[key];
        }

        public static void SetInputLock(string key, bool value)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(SetInputLock));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(SetInputLock));

            if (Data.inputLockList.ContainsKey(key))
                Data.inputLockList[key] = value;
        }
    }
}