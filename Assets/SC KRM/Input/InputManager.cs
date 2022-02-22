using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.ProjectSetting;
using SCKRM.SaveLoad;
using SCKRM.Threads;
using SCKRM.UI.StatusBar;
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
        [ProjectSetting]
        public sealed class Data
        {
            [JsonProperty] public static Dictionary<string, List<KeyCode>> controlSettingList { get; set; } = new Dictionary<string, List<KeyCode>>();
            [JsonProperty] public static Dictionary<string, bool> inputLockList { get; set; } = new Dictionary<string, bool>();
        }

        [SaveLoad("control")]
        public sealed class SaveData
        {
            [JsonProperty] public static Dictionary<string, List<KeyCode>> controlSettingList { get; set; } = new Dictionary<string, List<KeyCode>>();
            [JsonProperty] public static bool mouseUpsideDown { get; set; } = false;
        }



        public static bool defaultInputLock { get; set; }

        public static KeyCode[] unityKeyCodeList { get; } = Enum.GetValues(typeof(KeyCode)) as KeyCode[];



        static Dictionary<string, List<KeyCode>> _controlSettingList;
        public static Dictionary<string, List<KeyCode>> controlSettingList
        {
            get
            {
                if (_controlSettingList == null)
                    _controlSettingList = InputListMerge();

                return _controlSettingList;
            }
        }
        static Dictionary<string, List<KeyCode>> InputListMerge()
        {
            Dictionary<string, List<KeyCode>> mergeList = new Dictionary<string, List<KeyCode>>();
            foreach (var item in Data.controlSettingList)
            {
                if (SaveData.controlSettingList.TryGetValue(item.Key, out List<KeyCode> value))
                    mergeList.Add(item.Key, value);
                else
                    mergeList.Add(item.Key, item.Value);
            }

            return mergeList;
        }



        public static Vector2 mousePosition { get; private set; }



        #region Input Check
        static bool TryInputCheck(string key, Func<KeyCode, bool> func)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetKey));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetKey));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetKey));

            if (controlSettingList.TryGetValue(key, out List<KeyCode> list))
            {
                if (list == null)
                    return false;
                else if (list.Count <= 0)
                    return false;

                for (int i = 0; i < list.Count; i++)
                {
                    if (i != list.Count - 1)
                    {
                        if (!UnityEngine.Input.GetKey(list[i]))
                            return false;
                    }
                    else if (!func.Invoke(list[i]))
                        return false;
                }

                return true;
            }

            return false;
        }
        static bool InputCheck(string key, Func<KeyCode, bool> func)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetKey));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetKey));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetKey));

            List<KeyCode> list = controlSettingList[key];
            if (list == null)
                return false;
            else if (list.Count <= 0)
                return false;

            for (int i = 0; i < list.Count; i++)
            {
                if (i != list.Count - 1)
                {
                    if (!UnityEngine.Input.GetKey(list[i]))
                        return false;
                }
                else if (!func.Invoke(list[i]))
                    return false;
            }

            return true;
        }


        public static bool GetKey(KeyCode keyCode, InputType inputType = InputType.Down, params string[] inputLockDeny)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetKey));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetKey));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetKey));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (!InputLockCheck(inputLockDeny))
            {
                if (inputType == InputType.Down && UnityEngine.Input.GetKeyDown(keyCode))
                    return true;
                else if (inputType == InputType.Alway && UnityEngine.Input.GetKey(keyCode))
                    return true;
                else if (inputType == InputType.Up && UnityEngine.Input.GetKeyUp(keyCode))
                    return true;
            }

            return false;
        }

        public static bool TryGetKey(string key, InputType inputType = InputType.Down, params string[] inputLockDeny)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetKey));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetKey));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetKey));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (!InputLockCheck(inputLockDeny))
            {
                if (inputType == InputType.Down && TryInputCheck(key, UnityEngine.Input.GetKeyDown))
                    return true;
                else if (inputType == InputType.Alway && TryInputCheck(key, UnityEngine.Input.GetKey))
                    return true;
                else if (inputType == InputType.Up && TryInputCheck(key, UnityEngine.Input.GetKeyUp))
                    return true;
            }

            return false;
        }
        public static bool GetKey(string key, InputType inputType = InputType.Down, params string[] inputLockDeny)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetKey));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetKey));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetKey));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (!InputLockCheck(inputLockDeny))
            {
                if (inputType == InputType.Down && InputCheck(key, UnityEngine.Input.GetKeyDown))
                    return true;
                else if (inputType == InputType.Alway && InputCheck(key, UnityEngine.Input.GetKey))
                    return true;
                else if (inputType == InputType.Up && InputCheck(key, UnityEngine.Input.GetKeyUp))
                    return true;
            }

            return false;
        }
        #endregion

        #region MouseInputCheck
        static Vector2 mouseDelta = Vector2.zero;
        public static Vector2 GetMouseDelta(params string[] inputLockDeny)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetMouseDelta));

#if UNITY_EDITOR
            if (!Application.isPlaying)
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

        public static bool GetMouseButton(int button, params string[] inputLockDeny)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetMouseButton));

#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(GetMouseButton));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(GetMouseButton));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (!InputLockCheck(inputLockDeny))
                return UnityEngine.Input.GetMouseButton(button);
            else
                return false;
        }

        static Vector2 mouseScrollDelta = Vector2.zero;
        public static Vector2 GetMouseScrollDelta(params string[] inputLockDeny)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetMouseScrollDelta));

#if UNITY_EDITOR
            if (!Application.isPlaying)
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
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetAnyKeyDown));

#if UNITY_EDITOR
            if (!Application.isPlaying)
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
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetAnyKey));

#if UNITY_EDITOR
            if (!Application.isPlaying)
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
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(InputLockCheck));

#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(InputLockCheck));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(InputLockCheck));

            if (inputLockDeny == null)
                inputLockDeny = new string[0];

            if (inputLockDeny.Contains("all"))
                return false;

            foreach (var item in Data.inputLockList)
            {
                if (item.Value && !inputLockDeny.Contains(item.Key))
                    return true;
            }

            return false;
        }
        #endregion

        #region InputLock
        public static void SetInputLock(string key)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(SetInputLock));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(SetInputLock));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(SetInputLock));

            if (Data.inputLockList.ContainsKey(key))
                Data.inputLockList[key] = !Data.inputLockList[key];
        }

        public static void SetInputLock(string key, bool value)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(SetInputLock));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(SetInputLock));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(SetInputLock));

            if (Data.inputLockList.ContainsKey(key))
                Data.inputLockList[key] = value;
        }
        #endregion



        [SerializeField] void OnDelta(InputAction.CallbackContext context) => mouseDelta = context.ReadValue<Vector2>();
        [SerializeField] void OnPosition(InputAction.CallbackContext context) => mousePosition = context.ReadValue<Vector2>();
        [SerializeField] void OnScroll(InputAction.CallbackContext context) => mouseScrollDelta = context.ReadValue<Vector2>();
    }

    public enum InputType
    {
        Down,
        Alway,
        Up,
    }
}