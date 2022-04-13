using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Object;
using SCKRM.Renderer;
using SCKRM.Tool;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SCKRM.Window
{
    public sealed class WindowManager : Manager<WindowManager>
    {
        void Awake() => SingletonCheck(this);

        void Update()
        {
            if (isMessageBoxShow)
            {
                messageBoxCanvasGroup.alpha = messageBoxCanvasGroup.alpha.MoveTowards(1, 0.01f * Kernel.fpsDeltaTime);
                messageBoxCanvasGroup.interactable = true;
            }
            else
            {
                messageBoxCanvasGroup.alpha = messageBoxCanvasGroup.alpha.MoveTowards(0, 0.01f * Kernel.fpsDeltaTime);
                messageBoxCanvasGroup.interactable = false;
            }
        }



        #region Window Pos, Size
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
        #endregion



        [SerializeField] CanvasGroup messageBoxCanvasGroup;
        [SerializeField] Transform messageBoxButtons;
        [SerializeField] CustomAllTextRenderer messageBoxInfo;

        static MessageBoxButton[] createdMessageBoxButton = new MessageBoxButton[0];
        public static bool isMessageBoxShow { get; private set; } = false;

        public static async UniTask<int> MessageBox(NameSpacePathPair buttons, int defaultIndex, string text, string nameSpace = "") => await messageBox(new NameSpacePathPair[] { buttons }, defaultIndex, text, nameSpace, new ReplaceOldNewPair[0]);
        public static async UniTask<int> MessageBox(NameSpacePathPair buttons, int defaultIndex, string text, string nameSpace, ReplaceOldNewPair replace) => await messageBox(new NameSpacePathPair[] { buttons }, defaultIndex, text, nameSpace, new ReplaceOldNewPair[] { replace });
        public static async UniTask<int> MessageBox(NameSpacePathPair buttons, int defaultIndex, string text, string nameSpace, ReplaceOldNewPair[] replace) => await messageBox(new NameSpacePathPair[] { buttons }, defaultIndex, text, nameSpace, replace);
        public static async UniTask<int> MessageBox(NameSpacePathPair[] buttons, int defaultIndex, string text, string nameSpace = "") => await messageBox(buttons, defaultIndex, text, nameSpace, new ReplaceOldNewPair[0]);
        public static async UniTask<int> MessageBox(NameSpacePathPair[] buttons, int defaultIndex, string text, string nameSpace, ReplaceOldNewPair replace) => await messageBox(buttons, defaultIndex, text, nameSpace, new ReplaceOldNewPair[] { replace });
        public static async UniTask<int> MessageBox(NameSpacePathPair[] buttons, int defaultIndex, string text, string nameSpace, ReplaceOldNewPair[] replace) => await messageBox(buttons, defaultIndex, text, nameSpace, replace);

        static async UniTask<int> messageBox(NameSpacePathPair[] buttons, int defaultIndex, string text, string nameSpace, ReplaceOldNewPair[] replace)
        {
            if (isMessageBoxShow)
                return defaultIndex;

            for (int i = 0; i < createdMessageBoxButton.Length; i++)
                ObjectPoolingSystem.ObjectRemove("window_manager.message_box_button", createdMessageBoxButton[i]);

            createdMessageBoxButton = new MessageBoxButton[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                MessageBoxButton button = (MessageBoxButton)ObjectPoolingSystem.ObjectCreate("window_manager.message_box_button", instance.messageBoxButtons);
                createdMessageBoxButton[i] = button;

                instance.messageBoxInfo.replace = replace;

                instance.messageBoxInfo.nameSpace = nameSpace;
                instance.messageBoxInfo.path = text;

                button.index = i;

                button.text.nameSpace = buttons[i].nameSpace;
                button.text.path = buttons[i].path;
                button.text.Refresh();

                button.button.onClick.AddListener(() => action(button));
            }

            int clickedIndex = -1;
            if (await UniTask.WaitUntil(() => InputManager.GetKey("gui.back", InputType.Down, "all") || clickedIndex >= 0, PlayerLoopTiming.Update, AsyncTaskManager.cancelToken).SuppressCancellationThrow())
                return defaultIndex;
            else if (clickedIndex < 0)
                return defaultIndex;
            else
                return clickedIndex;

            void action(MessageBoxButton messageBoxButton) => clickedIndex = messageBoxButton.index;
        }
    }
}