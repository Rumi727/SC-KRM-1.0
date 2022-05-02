using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Object;
using SCKRM.Renderer;
using SCKRM.Resource;
using SCKRM.UI;
using SCKRM.UI.StatusBar;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.Window
{
    public static class WindowManager
    {
        public static Process currentProcess { get; } = Process.GetCurrentProcess();
        public static IntPtr currentHandle { get; } = currentProcess.MainWindowHandle;

#if UNITY_STANDALONE_WIN
        static float lerpX = 0;
        static float lerpY = 0;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int sizeX, int sizeY, int uFlags);

        const int SWP_NOSIZE = 0x0001;
        const int SWP_NOMOVE = 0x0002;
        const int SWP_NOZORDER = 0x0004;
        const int SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll")]
        [Obsolete("You can now get the main window handle from the process class. Use only when you can't bring it")]
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
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
#endif



        [Obsolete("You can now get the main window handle from the process class. Use only when you can't bring it")]
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
            GetWindowRect(currentHandle, out RECT rect);
            return new Vector2Int(Mathf.RoundToInt(rect.Left + ((rect.Right - rect.Left) * windowDatumPoint.x) - (Screen.currentResolution.width * screenDatumPoint.x)), Mathf.RoundToInt(rect.Top + ((rect.Bottom - rect.Top) * windowDatumPoint.y) - (Screen.currentResolution.height * screenDatumPoint.y)));
#else
            throw new NotImplementedException();
#endif
        }

        public static Vector2Int GetWindowSize()
        {
#if UNITY_STANDALONE_WIN
            GetWindowRect(currentHandle, out RECT rect);
            return new Vector2Int(rect.Right - rect.Left, rect.Bottom - rect.Top);
#else
            throw new NotImplementedException();
#endif
        }

        public static Vector2Int GetClientSize()
        {
#if UNITY_STANDALONE_WIN
            GetClientRect(currentHandle, out RECT rect);
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
            
            Vector2 border = GetWindowSize() - GetClientSize();

            if (clientDatum)
                SetWindowPos(currentHandle, IntPtr.Zero, 0, 0, Mathf.RoundToInt(rect.width + border.x), Mathf.RoundToInt(rect.height + border.y), SWP_NOZORDER | SWP_NOMOVE | SWP_SHOWWINDOW);
            else
                SetWindowPos(currentHandle, IntPtr.Zero, 0, 0, Mathf.RoundToInt(rect.width), Mathf.RoundToInt(rect.height), SWP_NOZORDER | SWP_NOMOVE | SWP_SHOWWINDOW);

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
                SetWindowPos(currentHandle, IntPtr.Zero, Mathf.RoundToInt(rect.x), Mathf.RoundToInt(rect.y), 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
            else
                SetWindowPos(currentHandle, IntPtr.Zero, Mathf.RoundToInt(lerpX), Mathf.RoundToInt(lerpY), 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
#else
            throw new NotImplementedException();
#endif
        }
    }
}