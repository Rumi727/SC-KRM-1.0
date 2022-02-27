using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SCKRM.Window
{
    public static class WindowManager
    {
#if UNITY_STANDALONE_WIN
#if !UNITY_EDITOR
        static float lerpX = 0;
        static float lerpY = 0;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        const int SWP_NOSIZE = 0x0001;
        const int SWP_NOMOVE = 0x0002;
        const int SWP_NOZORDER = 0x0004;
        const int SWP_SHOWWINDOW = 0x0040;
#endif
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

        public static Vector2 GetWindowPos(Vector2 windowDatumPoint, Vector2 screenDatumPoint)
        {
#if UNITY_STANDALONE_WIN
            IntPtr temp = GetWindowHandle();
            if (temp != IntPtr.Zero)
                handle = temp;

            GetWindowRect(handle, out RECT rect);

            return new Vector2(rect.Left + ((rect.Right - rect.Left) * windowDatumPoint.x) - (Screen.currentResolution.width * screenDatumPoint.x), rect.Top + ((rect.Bottom - rect.Top) * windowDatumPoint.y) - (Screen.currentResolution.height * screenDatumPoint.y));
#else
            throw new NotImplementedException();
#endif
        }

        public static Vector2 GetWindowSize()
        {
#if UNITY_STANDALONE_WIN
            IntPtr temp = GetWindowHandle();
            if (temp != IntPtr.Zero)
                handle = temp;

            GetWindowRect(handle, out RECT rect);
            return new Vector2(rect.Right - rect.Left, rect.Bottom - rect.Top);
#else
            throw new NotImplementedException();
#endif
        }

        public static Vector2 GetClientSize()
        {
#if UNITY_STANDALONE_WIN
            IntPtr temp = GetWindowHandle();
            if (temp != IntPtr.Zero)
                handle = temp;

            GetClientRect(handle, out RECT rect);
            return new Vector2(rect.Right, rect.Bottom);
#else
            throw new NotImplementedException();
#endif
        }


        /// <summary>
        /// The editor ignores this function.
        /// </summary>
        public static void SetWindowRect(Rect rect, Vector2 windowDatumPoint, Vector2 screenDatumPoint, bool clientDatum = true, bool Lerp = false, float time = 0.05f)
        {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
            if (UnityEngine.Screen.fullScreen)
                UnityEngine.Screen.SetResolution(UnityEngine.Screen.currentResolution.width, UnityEngine.Screen.currentResolution.height, false);

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

            rect.x += UnityEngine.Screen.currentResolution.width * screenDatumPoint.x;
            rect.y += UnityEngine.Screen.currentResolution.height * screenDatumPoint.y;

            if (!Lerp)
            {
                lerpX = rect.x;
                lerpY = rect.y;
            }
            else
            {
                lerpX = Mathf.Lerp(lerpX, rect.x, time * 60 * Kernel.deltaTime);
                lerpY = Mathf.Lerp(lerpY, rect.y, time * 60 * Kernel.deltaTime);
            }

            if (!Lerp)
                SetWindowPos(handle, IntPtr.Zero, Mathf.RoundToInt(rect.x), Mathf.RoundToInt(rect.y), 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
            else
                SetWindowPos(handle, IntPtr.Zero, Mathf.RoundToInt(lerpX), Mathf.RoundToInt(lerpY), 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
#elif !UNITY_EDITOR
            throw new NotImplementedException();
#endif
        }


        public static DialogResult MessageBox(string text, string caption, MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK, MessageBoxIcon messageBoxIcon = MessageBoxIcon.None)
        {
#if UNITY_STANDALONE_WIN
            System.Windows.Forms.MessageBoxButtons messageBoxButtons2;
            switch (messageBoxButtons)
            {
                case MessageBoxButtons.OKCancel:
                    messageBoxButtons2 = System.Windows.Forms.MessageBoxButtons.OKCancel;
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    messageBoxButtons2 = System.Windows.Forms.MessageBoxButtons.AbortRetryIgnore;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    messageBoxButtons2 = System.Windows.Forms.MessageBoxButtons.YesNoCancel;
                    break;
                case MessageBoxButtons.YesNo:
                    messageBoxButtons2 = System.Windows.Forms.MessageBoxButtons.YesNo;
                    break;
                case MessageBoxButtons.RetryCancel:
                    messageBoxButtons2 = System.Windows.Forms.MessageBoxButtons.RetryCancel;
                    break;
                default:
                    messageBoxButtons2 = System.Windows.Forms.MessageBoxButtons.OK;
                    break;
            }

            System.Windows.Forms.MessageBoxIcon messageBoxIcon2;
            switch (messageBoxIcon)
            {
                case MessageBoxIcon.Hand:
                    messageBoxIcon2 = System.Windows.Forms.MessageBoxIcon.Hand;
                    break;
                case MessageBoxIcon.Question:
                    messageBoxIcon2 = System.Windows.Forms.MessageBoxIcon.Question;
                    break;
                case MessageBoxIcon.Exclamation:
                    messageBoxIcon2 = System.Windows.Forms.MessageBoxIcon.Exclamation;
                    break;
                case MessageBoxIcon.Asterisk:
                    messageBoxIcon2 = System.Windows.Forms.MessageBoxIcon.Asterisk;
                    break;
                case MessageBoxIcon.Stop:
                    messageBoxIcon2 = System.Windows.Forms.MessageBoxIcon.Stop;
                    break;
                case MessageBoxIcon.Error:
                    messageBoxIcon2 = System.Windows.Forms.MessageBoxIcon.Error;
                    break;
                case MessageBoxIcon.Warning:
                    messageBoxIcon2 = System.Windows.Forms.MessageBoxIcon.Warning;
                    break;
                case MessageBoxIcon.Information:
                    messageBoxIcon2 = System.Windows.Forms.MessageBoxIcon.Information;
                    break;
                default:
                    messageBoxIcon2 = System.Windows.Forms.MessageBoxIcon.None;
                    break;
            }

            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(text, caption, messageBoxButtons2, messageBoxIcon2);

            return dialogResult switch
            {
                System.Windows.Forms.DialogResult.OK => DialogResult.OK,
                System.Windows.Forms.DialogResult.Cancel => DialogResult.Cancel,
                System.Windows.Forms.DialogResult.Abort => DialogResult.Abort,
                System.Windows.Forms.DialogResult.Retry => DialogResult.Retry,
                System.Windows.Forms.DialogResult.Ignore => DialogResult.Ignore,
                System.Windows.Forms.DialogResult.Yes => DialogResult.Yes,
                System.Windows.Forms.DialogResult.No => DialogResult.No,
                _ => DialogResult.None
            };
#else
            throw new NotImplementedException();
#endif
        }

        public enum DialogResult
        {
            None,
            OK,
            Cancel,
            Abort,
            Retry,
            Ignore,
            Yes,
            No
        }

        public enum MessageBoxButtons
        {
            OK,
            OKCancel,
            AbortRetryIgnore,
            YesNoCancel,
            YesNo,
            RetryCancel
        }

        public enum MessageBoxIcon
        {
            None,
            Hand,
            Question,
            Exclamation,
            Asterisk,
            Stop,
            Error,
            Warning,
            Information
        }
    }
}