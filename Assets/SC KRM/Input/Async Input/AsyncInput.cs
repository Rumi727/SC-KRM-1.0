using SCKRM.Threads;
using SharpHook;
using System;
using System.Diagnostics;

namespace SCKRM.Input.Async
{
    public static class AsyncInput
    {
        public static TaskPoolGlobalHook hook
        {
#if UNITY_STANDALONE
            get => _hook;
#else
            get => throw new NotSupportedException();
#endif
        }
        static readonly TaskPoolGlobalHook _hook = new TaskPoolGlobalHook();

        [Awaken, Conditional("UNITY_STANDALONE")]
        static void Awaken()
        {
            Kernel.shutdownEvent += ShutdownEvent;

            hook.HookEnabled += OnHookEnabled;
            hook.HookDisabled += OnHookDisabled;

            hook.KeyPressed += OnKeyPressed;
            hook.KeyReleased += OnKeyReleased;

            hook.MousePressed += OnMousePressed;
            hook.MouseReleased += OnMouseReleased;

            ThreadManager.Create(hook.Run, "", "", true, true);
        }

        private static bool ShutdownEvent()
        {
            hook.Dispose();
            return true;
        }

        static void OnHookEnabled(object sender, HookEventArgs e) => Debug.Log("Async Input Hook Enabled");
        static void OnHookDisabled(object sender, HookEventArgs e) => Debug.Log("Async Input Hook Disabled");

        static void OnKeyTyped(object sender, KeyboardHookEventArgs e) => Debug.Log(nameof(OnKeyTyped) + ": " + e.Data.KeyCode.ToString());
        static void OnKeyPressed(object sender, KeyboardHookEventArgs e) => Debug.Log(nameof(OnKeyPressed) + ": " + e.Data.KeyCode.ToString());
        static void OnKeyReleased(object sender, KeyboardHookEventArgs e) => Debug.Log(nameof(OnKeyReleased) + ": " + e.Data.KeyCode.ToString());

        static void OnMouseClicked(object sender, MouseHookEventArgs e) => Debug.Log(nameof(OnMouseClicked) + ": " + e.Data.Button.ToString());
        static void OnMousePressed(object sender, MouseHookEventArgs e) => Debug.Log(nameof(OnMousePressed) + ": " + e.Data.Button.ToString());
        static void OnMouseReleased(object sender, MouseHookEventArgs e) => Debug.Log(nameof(OnMouseReleased) + ": " + e.Data.Button.ToString());

        static void OnMouseMoved(object sender, MouseHookEventArgs e) => Debug.Log(nameof(OnMouseMoved) + ": " + e.Data.X.ToString() + ", " + e.Data.Y.ToString());
        static void OnMouseDragged(object sender, MouseHookEventArgs e) => Debug.Log(nameof(OnMouseDragged) + ": " + e.Data.Button.ToString());
        static void OnMouseWheel(object sender, MouseWheelHookEventArgs e) => Debug.Log(nameof(OnMouseWheel) + ": " + e.Data.Type.ToString());
    }
}
