using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.Input;
using SCKRM.SaveLoad;
using SCKRM.Tool;
using SCKRM.UI;
using SCKRM.UI.MessageBox;
using SCKRM.Window;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SCKRM
{
    public class CursorManager : ManagerUI<CursorManager>
    {
        [GeneralSaveLoad]
        public class SaveData
        {
            [JsonProperty] public static bool highPrecisionMouse { get; set; } = false;
            [JsonProperty] public static float mouseSensitivity { get; set; } = 1;
        }



        public static Vector2 highPrecisionMousePos { get; set; } = Vector2.zero;

        static bool _visible = true;
        public static bool visible
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



        [SerializeField, NotNull] CanvasGroup _canvasGroup; public CanvasGroup canvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                {
                    _canvasGroup = GetComponent<CanvasGroup>();
                    if (_canvasGroup == null)
                        return null;
                }

                return _canvasGroup;
            }
        }



        protected override void OnEnable() => SingletonCheck(this);

        protected override void Awake()
        {
            Vector2Int pos = GetCursorPosition(0, 1);
            highPrecisionMousePos = new Vector2Int(pos.x, Mathf.RoundToInt(Screen.currentResolution.height - pos.y));
        }

        Vector2 dragStartMousePosition = Vector2.zero;
        bool dragStart = false;
        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                if (SaveData.highPrecisionMouse && Application.isFocused && InputManager.mousePosition.x >= 0 && InputManager.mousePosition.x <= Screen.width && InputManager.mousePosition.y >= 0 && InputManager.mousePosition.y <= Screen.height)
                {
                    SetCursorPosition(Mathf.RoundToInt(highPrecisionMousePos.x), Mathf.RoundToInt(Screen.currentResolution.height - highPrecisionMousePos.y), 0, 1);
                    highPrecisionMousePos += InputManager.GetMouseDelta(false, "all", "force");
                }
                else
                {
                    Vector2Int pos = GetCursorPosition(0, 1);
                    highPrecisionMousePos = new Vector2Int(pos.x, Mathf.RoundToInt(Screen.currentResolution.height - pos.y));
                }

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
                    graphic.color = graphic.color.Lerp(Kernel.SaveData.systemColor * new Color(1, 1, 1, 0.5f), 0.2f * Kernel.fpsUnscaledDeltaTime);
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



        public static async void HighPrecisionMouseWarning()
        {
            if (Kernel.isInitialLoadEnd && SaveData.highPrecisionMouse)
            {
                SaveData.highPrecisionMouse = false;
                if (await MessageBoxManager.Show(new Renderer.NameSpacePathPair[] { "sc-krm:gui.yes", "sc-krm:gui.no" }, 1, "sc-krm:options.input.highPrecisionMouse.warning", "sc-krm:gui/exclamation_mark") == 0)
                    SaveData.highPrecisionMouse = true;
            }
        }



        #region Cursor Pos
#if UNITY_STANDALONE_WIN
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
#endif



        public static Vector2Int GetCursorPosition() => GetCursorPosition(0, 0);
        public static Vector2Int GetCursorPosition(Vector2 datumPoint) => GetCursorPosition(datumPoint.x, datumPoint.y);
        public static Vector2Int GetCursorPosition(float xDatumPoint, float yDatumPoint)
        {
#if UNITY_STANDALONE_WIN
            POINT lpPoint;
            bool success = GetCursorPos(out lpPoint);
            if (!success)
                return Vector2Int.zero;

            return new Vector2Int(Mathf.RoundToInt(lpPoint.X - (Screen.currentResolution.width * xDatumPoint)), Mathf.RoundToInt(lpPoint.Y - (Screen.currentResolution.height * yDatumPoint)));
#else
            throw new NotImplementedException();
#endif
        }


        public static Vector2Int GetClientCursorPosition() => GetClientCursorPosition(0, 0);
        public static Vector2Int GetClientCursorPosition(Vector2 datumPoint) => GetClientCursorPosition(datumPoint.x, datumPoint.y);
        public static Vector2Int GetClientCursorPosition(float xDatumPoint, float yDatumPoint)
        {
#if UNITY_STANDALONE_WIN
            POINT lpPoint;
            bool success = GetCursorPos(out lpPoint);
            if (!success)
                return Vector2Int.zero;

            Vector2Int clientSize = WindowManager.GetClientSize();
            Vector2 border = (Vector2)(WindowManager.GetWindowSize() - clientSize) * 0.5f;
            Vector2Int offset = WindowManager.GetWindowPos(Vector2.zero, Vector2.zero) + new Vector2Int(Mathf.RoundToInt(border.x), Mathf.RoundToInt(border.y));
            return new Vector2Int(Mathf.RoundToInt(lpPoint.X - (clientSize.x * xDatumPoint)) - offset.x, Mathf.RoundToInt(lpPoint.Y - offset.y - (clientSize.y * yDatumPoint)) - offset.y);
#else
            throw new NotImplementedException();
#endif
        }



        public static void SetCursorPosition(Vector2Int pos) => SetCursorPosition(pos.x, pos.y);
        public static void SetCursorPosition(Vector2Int pos, Vector2 datumPoint) => SetCursorPosition(pos.x, pos.y, datumPoint.x, datumPoint.y);
        public static void SetCursorPosition(int x, int y, float xDatumPoint = 0, float yDatumPoint = 0) => SetCursorPos(Mathf.RoundToInt(x + ((Screen.currentResolution.width - 1) * xDatumPoint)), Mathf.RoundToInt(y + ((Screen.currentResolution.height - 1) * yDatumPoint)));

        public static void SetClientCursorPosition(Vector2Int pos) => SetCursorPosition(pos.x, pos.y);
        public static void SetClientCursorPosition(Vector2Int pos, Vector2 datumPoint) => SetCursorPosition(pos.x, pos.y, datumPoint.x, datumPoint.y);
        public static void SetClientCursorPosition(int x, int y, float xDatumPoint = 0, float yDatumPoint = 0)
        {
#if UNITY_STANDALONE_WIN
            Vector2Int clientSize = WindowManager.GetClientSize();
            Vector2 border = (Vector2)(WindowManager.GetWindowSize() - clientSize) * 0.5f;
            Vector2Int offset = WindowManager.GetWindowPos(Vector2.zero, Vector2.zero) + new Vector2Int(Mathf.RoundToInt(border.x), Mathf.RoundToInt(border.y));
            SetCursorPos(Mathf.RoundToInt(x + ((clientSize.x - 1) * xDatumPoint)) - offset.x, Mathf.RoundToInt(y + ((clientSize.y - 1) * yDatumPoint)) - offset.y);
#else
            throw new NotImplementedException();
#endif
        }
        #endregion
    }
}
