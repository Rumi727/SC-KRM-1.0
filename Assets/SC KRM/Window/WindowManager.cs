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
    public sealed class WindowManager : Manager<WindowManager>
    {
        void Awake() => SingletonCheck(this);

        void Update()
        {
            if (isMessageBoxShow)
            {
                messageBoxCanvasGroup.alpha = messageBoxCanvasGroup.alpha.Lerp(1, 0.2f * Kernel.fpsDeltaTime);
                if (messageBoxCanvasGroup.alpha > 0.99f)
                    messageBoxCanvasGroup.alpha = 1;

                messageBoxCanvasGroup.interactable = true;
                messageBoxCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                messageBoxCanvasGroup.alpha = messageBoxCanvasGroup.alpha.Lerp(0, 0.2f * Kernel.fpsDeltaTime);
                if (messageBoxCanvasGroup.alpha < 0.01f)
                    messageBoxCanvasGroup.alpha = 0;

                messageBoxCanvasGroup.interactable = false;
                messageBoxCanvasGroup.blocksRaycasts = false;
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
        [SerializeField] CustomAllSpriteRenderer messageBoxIcon;
        [SerializeField] MessageBoxButton messageBoxButtonPrefab;

        static MessageBoxButton[] createdMessageBoxButton = new MessageBoxButton[0];
        public static bool isMessageBoxShow { get; private set; } = false;

        public static async UniTask<int> MessageBox(NameSpacePathPair button, int defaultIndex, NameSpacePathPair info, NameSpacePathPair icon) => await messageBox(new NameSpacePathPair[] { button }, defaultIndex, info, icon, new ReplaceOldNewPair[0]);
        public static async UniTask<int> MessageBox(NameSpacePathPair button, int defaultIndex, NameSpacePathPair info, NameSpacePathPair icon, ReplaceOldNewPair replace) => await messageBox(new NameSpacePathPair[] { button }, defaultIndex, info, icon, new ReplaceOldNewPair[] { replace });
        public static async UniTask<int> MessageBox(NameSpacePathPair button, int defaultIndex, NameSpacePathPair info, NameSpacePathPair icon, ReplaceOldNewPair[] replace) => await messageBox(new NameSpacePathPair[] { button }, defaultIndex, info, icon, replace);
        public static async UniTask<int> MessageBox(NameSpacePathPair[] buttons, int defaultIndex, NameSpacePathPair info, NameSpacePathPair icon) => await messageBox(buttons, defaultIndex, info, icon, new ReplaceOldNewPair[0]);
        public static async UniTask<int> MessageBox(NameSpacePathPair[] buttons, int defaultIndex, NameSpacePathPair info, NameSpacePathPair icon, ReplaceOldNewPair replace) => await messageBox(buttons, defaultIndex, info, icon, new ReplaceOldNewPair[] { replace });
        public static async UniTask<int> MessageBox(NameSpacePathPair[] buttons, int defaultIndex, NameSpacePathPair info, NameSpacePathPair icon, ReplaceOldNewPair[] replace) => await messageBox(buttons, defaultIndex, info, icon, replace);

        static async UniTask<int> messageBox(NameSpacePathPair[] buttons, int defaultIndex, NameSpacePathPair info, NameSpacePathPair icon, ReplaceOldNewPair[] replace)
        {
            await UniTask.WaitUntil(() => instance != null);

            if (isMessageBoxShow)
                return defaultIndex;
            else if (defaultIndex < 0 || buttons.Length < defaultIndex)
                return defaultIndex;

            isMessageBoxShow = true;

            instance.messageBoxIcon.nameSpace = icon.nameSpace;
            instance.messageBoxIcon.path = icon.path;
            instance.messageBoxIcon.Refresh();

            instance.messageBoxInfo.replace = replace;

            instance.messageBoxInfo.nameSpace = info.nameSpace;
            instance.messageBoxInfo.path = info.path;
            instance.messageBoxInfo.Refresh();

            #region Button Object Create
            for (int i = 0; i < createdMessageBoxButton.Length; i++)
                ObjectPoolingSystem.ObjectRemove("window_manager.message_box_button", createdMessageBoxButton[i]);

            createdMessageBoxButton = new MessageBoxButton[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                MessageBoxButton button = (MessageBoxButton)ObjectPoolingSystem.ObjectCreate("window_manager.message_box_button", instance.messageBoxButtons);
                createdMessageBoxButton[i] = button;

                button.index = i;

                button.text.nameSpace = buttons[i].nameSpace;
                button.text.path = buttons[i].path;
                button.text.Refresh();

                //버튼이 눌렸을때, UI를 닫기 위해서 버튼에 있는 OnClick 이벤트에 clickedIndex를 button.index로 바꾸는 action 메소드를 추가합니다
                button.button.onClick.AddListener(() => action(button));
            }

            for (int i = 0; i < createdMessageBoxButton.Length; i++)
            {
                Navigation navigation = new Navigation();
                navigation.mode = Navigation.Mode.Explicit;

                if (i > 0)
                    navigation.selectOnUp = createdMessageBoxButton[i - 1].button;
                if (i < createdMessageBoxButton.Length - 1)
                    navigation.selectOnDown = createdMessageBoxButton[i + 1].button;

                createdMessageBoxButton[i].button.navigation = navigation;
            }
            #endregion

            StatusBarManager.tabSelectGameObject = createdMessageBoxButton[defaultIndex].gameObject;
            EventSystem.current.SetSelectedGameObject(createdMessageBoxButton[defaultIndex].gameObject);

            InputManager.forceInputLock = true;



            int clickedIndex = -1;

            //Back 버튼을 눌렀을때, UI를 닫기 위해 이벤트에 clickedIndex를 defaultIndex로 변경하는 BackEvent 메소드를 추가합니다
            UIManager.BackEventAdd(backEvent, true);

            GameObject oldSelectedGameObject = null;
            while (clickedIndex < 0)
            {
                /*
                 * BackEvent 함수가 호출되거나, 버튼이 눌려서 clickedIndex가 0 이상이 되거나
                 * select 함수가 호출되서 함수가 리턴 될 때까지 대기합니다
                 */

                if (EventSystem.current.currentSelectedGameObject == null)
                    EventSystem.current.SetSelectedGameObject(oldSelectedGameObject);
                else
                    oldSelectedGameObject = EventSystem.current.currentSelectedGameObject;

                if (await UniTask.DelayFrame(1, PlayerLoopTiming.Update, AsyncTaskManager.cancelToken).SuppressCancellationThrow())
                    return 0;
            }

            return select(clickedIndex);



            int select(int index)
            {
                isMessageBoxShow = false;
                StatusBarManager.tabSelectGameObject = null;
                EventSystem.current.SetSelectedGameObject(null);

                InputManager.forceInputLock = false;

                UIManager.BackEventRemove(backEvent, true);

                return index;
            }

            void backEvent() => clickedIndex = defaultIndex;
            void action(MessageBoxButton messageBoxButton) => clickedIndex = messageBoxButton.index;
        }
    }
}