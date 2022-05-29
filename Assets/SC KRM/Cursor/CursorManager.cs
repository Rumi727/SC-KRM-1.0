using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.Input;
using SCKRM.SaveLoad;
using SCKRM.UI;
using SCKRM.UI.MessageBox;
using SCKRM.UI.Setting;
using SCKRM.Window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SCKRM
{
    public class CursorManager : ManagerUI<CursorManager>
    {
        [GeneralSaveLoad]
        public sealed class SaveData
        {
            [JsonProperty] public static bool IgnoreMouseAcceleration { get; set; } = false;
            [JsonProperty] public static float mouseSensitivity { get; set; } = 1;
        }

        static bool _visible = true; public static bool visible
        {
            get => _visible;
            set
            {
                if (value)
                    instance.canvasGroup.alpha = 1;
                else
                    instance.canvasGroup.alpha = 0;

                _visible = value;
            }
        }
        public static bool isFocused { get; private set; } = false;



        [SerializeField, NotNull] CanvasGroup _canvasGroup; public CanvasGroup canvasGroup => _canvasGroup = this.GetComponentFieldSave(_canvasGroup);



        protected override void OnEnable() => SingletonCheck(this);

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        static Vector2 IgnoreMouseAccelerationPos = Vector2.zero;
        protected override void Awake() => IgnoreMouseAccelerationPos = GetCursorPosition(0, 0);
#endif

        Vector2 dragStartMousePosition = Vector2.zero;
        bool dragStart = false;
        void LateUpdate()
        {
            isFocused = InputManager.mousePosition.x >= 0 && InputManager.mousePosition.x <= Screen.width && InputManager.mousePosition.y >= 0 && InputManager.mousePosition.y <= Screen.height;
            Cursor.visible = !isFocused;

            if (InitialLoadManager.isInitialLoadEnd)
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                if (SaveData.IgnoreMouseAcceleration && Application.isFocused && InputManager.mousePosition.x >= 0 && InputManager.mousePosition.x <= Screen.width && InputManager.mousePosition.y >= 0 && InputManager.mousePosition.y <= Screen.height)
                {
                    setCursorPosition((IgnoreMouseAccelerationPos.x).RoundToInt(), (IgnoreMouseAccelerationPos.y).RoundToInt(), 0, 0, true);

                    Vector2 delta = InputManager.GetMouseDelta(false, "all", "force");
                    delta.y = -delta.y;
                    IgnoreMouseAccelerationPos += delta;
                }
                else
                    IgnoreMouseAccelerationPos = GetCursorPosition(0, 0);
#endif

                #region Pos Move
                if (graphic.enabled != visible)
                {
                    graphic.enabled = visible;
                    transform.position = Vector3.zero;
                }

                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    dragStart = false;
                    dragStartMousePosition = InputManager.mousePosition;
                }
                else if (UnityEngine.Input.GetMouseButton(0))
                {
                    graphic.color = graphic.color.Lerp(UIManager.SaveData.systemColor * new Color(1, 1, 1, 0.5f), 0.2f * Kernel.fpsUnscaledDeltaTime);
                    transform.localScale = transform.localScale.Lerp(Vector3.one * 0.2f, 0.075f * Kernel.fpsUnscaledDeltaTime);

                    if (!dragStart && Vector2.Distance(transform.position, dragStartMousePosition) >= 10)
                        dragStart = true;
                    else if (dragStart)
                    {
                        Vector3 dir = (Vector2)transform.position - dragStartMousePosition;
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle + 67.5f)), 0.2f * Kernel.fpsUnscaledDeltaTime);
                    }
                    else
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), 0.2f * Kernel.fpsUnscaledDeltaTime);
                }
                else
                {
                    graphic.color = graphic.color.Lerp(new Color(0, 0, 0, 0.5f), 0.2f * Kernel.fpsUnscaledDeltaTime);
                    transform.localScale = transform.localScale.Lerp(Vector3.one * 0.25f, 0.3f * Kernel.fpsUnscaledDeltaTime);

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), 0.2f * Kernel.fpsUnscaledDeltaTime);
                }

                transform.position = InputManager.mousePosition;
                #endregion
            }
        }



        static bool highPrecisionMouseWarningLock = false;
        public static async void HighPrecisionMouseWarning(SettingToggle setting)
        {
            if (highPrecisionMouseWarningLock)
                return;

            if (InitialLoadManager.isInitialLoadEnd && SaveData.IgnoreMouseAcceleration)
            {
                highPrecisionMouseWarningLock = true;

                try
                {
                    SaveData.IgnoreMouseAcceleration = false;
                    setting.ScriptOnValueChanged();

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                    if (await MessageBoxManager.Show(new Renderer.NameSpacePathPair[] { "sc-krm:gui.yes", "sc-krm:gui.no" }, 1, "sc-krm:options.input.highPrecisionMouse.warning", "sc-krm:gui/exclamation_mark") == 0)
                    {
                        SaveData.IgnoreMouseAcceleration = true;
                        setting.ScriptOnValueChanged();
                    }
#else
                    await MessageBoxManager.Show("sc-krm:gui.yes", 0, "sc-krm:options.input.highPrecisionMouse.os_warning", "sc-krm:gui/exclamation_mark");
#endif
                }
                finally
                {
                    highPrecisionMouseWarningLock = false;
                }
            }
        }



        #region Cursor Pos
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);



        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);



        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);
#endif



        public static Vector2Int GetCursorPosition() => GetCursorPosition(0, 0);
        public static Vector2Int GetCursorPosition(Vector2 datumPoint) => GetCursorPosition(datumPoint.x, datumPoint.y);
        public static Vector2Int GetCursorPosition(float xDatumPoint, float yDatumPoint)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            bool success = GetCursorPos(out POINT lpPoint);
            if (!success)
                return Vector2Int.zero;

            return new Vector2Int((lpPoint.X - (Screen.currentResolution.width * xDatumPoint)).RoundToInt(), (lpPoint.Y - (Screen.currentResolution.height * yDatumPoint)).RoundToInt());
#else
            throw new NotSupportedException();
#endif
        }


        public static Vector2Int GetClientCursorPosition() => GetClientCursorPosition(0, 0);
        public static Vector2Int GetClientCursorPosition(Vector2 datumPoint) => GetClientCursorPosition(datumPoint.x, datumPoint.y);
        public static Vector2Int GetClientCursorPosition(float xDatumPoint, float yDatumPoint)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            bool success = GetCursorPos(out POINT lpPoint);
            if (!success)
                return Vector2Int.zero;

            Vector2Int clientSize = WindowManager.GetClientSize();
            Vector2 border = (Vector2)(WindowManager.GetWindowSize() - clientSize) * 0.5f;
            Vector2Int offset = WindowManager.GetWindowPos(Vector2.zero, Vector2.zero) + new Vector2Int((border.x).RoundToInt(), (border.y).RoundToInt());
            return new Vector2Int(lpPoint.X - (clientSize.x * xDatumPoint).RoundToInt() - offset.x, (lpPoint.Y - offset.y - (clientSize.y * yDatumPoint)).RoundToInt() - offset.y);
#else
            throw new NotSupportedException();
#endif
        }



        public static void SetCursorPosition(Vector2Int pos) => SetCursorPosition(pos.x, pos.y);
        public static void SetCursorPosition(Vector2Int pos, Vector2 datumPoint) => SetCursorPosition(pos.x, pos.y, datumPoint.x, datumPoint.y);
        public static void SetCursorPosition(int x, int y, float xDatumPoint = 0, float yDatumPoint = 0) => setCursorPosition(x, y, xDatumPoint, yDatumPoint, false);

        static void setCursorPosition(int x, int y, float xDatumPoint, float yDatumPoint, bool force)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            if (!SaveData.IgnoreMouseAcceleration || force)
                SetCursorPos((x + ((Screen.currentResolution.width - 1) * xDatumPoint)).RoundToInt(), (y + ((Screen.currentResolution.height - 1) * yDatumPoint)).RoundToInt());
            else
                IgnoreMouseAccelerationPos = new Vector2(x, y);
#else
            throw new NotSupportedException();
#endif
        }

        public static void SetClientCursorPosition(Vector2Int pos) => SetCursorPosition(pos.x, pos.y);
        public static void SetClientCursorPosition(Vector2Int pos, Vector2 datumPoint) => SetCursorPosition(pos.x, pos.y, datumPoint.x, datumPoint.y);
        public static void SetClientCursorPosition(int x, int y, float xDatumPoint = 0, float yDatumPoint = 0) => setClientCursorPosition(x, y, xDatumPoint, yDatumPoint, false);
        public static void setClientCursorPosition(int x, int y, float xDatumPoint, float yDatumPoint, bool force)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            Vector2Int clientSize = WindowManager.GetClientSize();
            Vector2 border = (Vector2)(WindowManager.GetWindowSize() - clientSize) * 0.5f;
            Vector2Int offset = WindowManager.GetWindowPos(Vector2.zero, Vector2.zero) + new Vector2Int((border.x).RoundToInt(), (border.y).RoundToInt());
            int x2 = (x + ((clientSize.x - 1) * xDatumPoint)).RoundToInt() - offset.x;
            int y2 = (y + ((clientSize.y - 1) * yDatumPoint)).RoundToInt() - offset.y;

            if (!SaveData.IgnoreMouseAcceleration || force)
                SetCursorPos(x, y);
            else
                IgnoreMouseAccelerationPos = new Vector2(x, y);
#else
            throw new NotSupportedException();
#endif
        }
        #endregion



        public Vector2Int ClientPosToScreenPos(Vector2Int pos) => ClientPosToScreenPos(pos.x, pos.y);

        public Vector2Int ClientPosToScreenPos(int x, int y)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            POINT point = new POINT() { X = x, Y = y };
            ClientToScreen(WindowManager.currentHandle, ref point);

            return new Vector2Int(point.X, point.Y); 
#else
            throw new NotSupportedException();
#endif
        }
    }
}
