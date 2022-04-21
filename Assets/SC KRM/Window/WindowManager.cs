using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Object;
using SCKRM.Renderer;
using SCKRM.Resource;
using SCKRM.Tool;
using SCKRM.UI;
using SCKRM.UI.StatusBar;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.Window
{
    public static class WindowManager
    {
        #region Window Pos, Size
#if UNITY_STANDALONE_WIN
        static float lerpX = 0;
        static float lerpY = 0;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        const int SWP_NOSIZE = 0x0001;
        const int SWP_NOMOVE = 0x0002;
        const int SWP_NOZORDER = 0x0004;
        const int SWP_SHOWWINDOW = 0x0040;

        static IntPtr handle;

        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(string LpClassName, string lpWindowName);
#endif



        public static IntPtr GetWindowHandle()
        {
#if UNITY_STANDALONE_WIN
            return GetActiveWindow();
#else
            throw new NotImplementedException();
#endif
        }

        public static Vector2Int GetWindowPos() => GetWindowPos(Vector2.zero, Vector2.zero);
        public static Vector2Int GetWindowPos(Vector2 windowDatumPoint, Vector2 screenDatumPoint)
        {
#if UNITY_STANDALONE_WIN
            IntPtr temp = GetWindowHandle();
            if (temp != IntPtr.Zero)
                handle = temp;

            GetWindowRect(handle, out RECT rect);

            return new Vector2Int(Mathf.RoundToInt(rect.Left + ((rect.Right - rect.Left) * windowDatumPoint.x) - (Screen.currentResolution.width * screenDatumPoint.x)), Mathf.RoundToInt(rect.Top + ((rect.Bottom - rect.Top) * windowDatumPoint.y) - (Screen.currentResolution.height * screenDatumPoint.y)));
#else
            throw new NotImplementedException();
#endif
        }

        public static Vector2Int GetWindowSize()
        {
#if UNITY_STANDALONE_WIN
            IntPtr temp = GetWindowHandle();
            if (temp != IntPtr.Zero)
                handle = temp;

            GetWindowRect(handle, out RECT rect);
            return new Vector2Int(rect.Right - rect.Left, rect.Bottom - rect.Top);
#else
            throw new NotImplementedException();
#endif
        }

        public static Vector2Int GetClientSize()
        {
#if UNITY_STANDALONE_WIN
            IntPtr temp = GetWindowHandle();
            if (temp != IntPtr.Zero)
                handle = temp;

            GetClientRect(handle, out RECT rect);
            return new Vector2Int(rect.Right, rect.Bottom);
#else
            throw new NotImplementedException();
#endif
        }


        /// <summary>
        /// The editor ignores this function.
        /// </summary>
        public static void SetWindowRect(Rect rect, Vector2 windowDatumPoint, Vector2 screenDatumPoint, bool clientDatum = true, bool Lerp = false, float time = 0.05f)
        {
#if UNITY_STANDALONE_WIN
            if (Screen.fullScreen)
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);

            IntPtr temp = GetWindowHandle();
            if (temp != IntPtr.Zero)
                handle = temp;

            Vector2 border = GetWindowSize() - GetClientSize();

            if (clientDatum)
                SetWindowPos(handle, IntPtr.Zero, 0, 0, Mathf.RoundToInt(rect.width + border.x), Mathf.RoundToInt(rect.height + border.y), SWP_NOZORDER | SWP_NOMOVE | SWP_SHOWWINDOW);
            else
                SetWindowPos(handle, IntPtr.Zero, 0, 0, Mathf.RoundToInt(rect.width), Mathf.RoundToInt(rect.height), SWP_NOZORDER | SWP_NOMOVE | SWP_SHOWWINDOW);

            Vector2 size = GetWindowSize();

            rect.x -= size.x * windowDatumPoint.x;
            rect.y -= size.y * windowDatumPoint.y;

            rect.x += Screen.currentResolution.width * screenDatumPoint.x;
            rect.y += Screen.currentResolution.height * screenDatumPoint.y;

            if (!Lerp)
            {
                lerpX = rect.x;
                lerpY = rect.y;
            }
            else
            {
                lerpX.LerpRef(rect.x, time);
                lerpY.LerpRef(rect.y, time);
            }

            if (!Lerp)
                SetWindowPos(handle, IntPtr.Zero, Mathf.RoundToInt(rect.x), Mathf.RoundToInt(rect.y), 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
            else
                SetWindowPos(handle, IntPtr.Zero, Mathf.RoundToInt(lerpX), Mathf.RoundToInt(lerpY), 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
#else
            throw new NotImplementedException();
#endif
        }
        #endregion

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

            Vector2Int clientSize = GetClientSize();
            Vector2 border = (Vector2)(GetWindowSize() - clientSize) * 0.5f;
            Vector2Int offset = GetWindowPos(Vector2.zero, Vector2.zero) + new Vector2Int(Mathf.RoundToInt(border.x), Mathf.RoundToInt(border.y));
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
            Vector2Int clientSize = GetClientSize();
            Vector2 border = (Vector2)(GetWindowSize() - clientSize) * 0.5f;
            Vector2Int offset = GetWindowPos(Vector2.zero, Vector2.zero) + new Vector2Int(Mathf.RoundToInt(border.x), Mathf.RoundToInt(border.y));
            SetCursorPos(Mathf.RoundToInt(x + ((clientSize.x - 1) * xDatumPoint)) - offset.x, Mathf.RoundToInt(y + ((clientSize.y - 1) * yDatumPoint)) - offset.y);
#else
            throw new NotImplementedException();
#endif
        }
        #endregion
    }
}