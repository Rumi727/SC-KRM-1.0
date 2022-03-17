using SCKRM.Input;
using SCKRM.UI.StatusBar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    [AddComponentMenu("")]
    public sealed class UIManager : Manager<UIManager>
    {
        [SerializeField] Canvas _kernelCanvas;
        public Canvas kernelCanvas => _kernelCanvas;
        [SerializeField] RectTransform _kernelCanvasUI;
        public RectTransform kernelCanvasUI => _kernelCanvasUI;



        static List<Action> backEventList { get; } = new List<Action>();
        static List<Action> highPriorityBackEventList { get; } = new List<Action>();

        public static event Action homeEvent = delegate { };



        void Awake() => SingletonCheck(this);

        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                if (InputManager.GetKey("gui.back", InputType.Down, "all"))
                    BackEventInvoke();
                else if (InputManager.GetKey("gui.home", InputType.Down, "all"))
                    homeEvent.Invoke();
            }

            StatusBarManager taskBarManager = StatusBarManager.instance;
            if (StatusBarManager.SaveData.bottomMode)
            {
                kernelCanvasUI.offsetMin = new Vector2(kernelCanvasUI.offsetMin.x, taskBarManager.rectTransform.sizeDelta.y + taskBarManager.rectTransform.anchoredPosition.y);
                kernelCanvasUI.offsetMax = new Vector2(kernelCanvasUI.offsetMax.x, 0);
            }
            else
            {
                kernelCanvasUI.offsetMin = new Vector2(kernelCanvasUI.offsetMin.x, 0);
                kernelCanvasUI.offsetMax = new Vector2(kernelCanvasUI.offsetMax.x, -(taskBarManager.rectTransform.sizeDelta.y - taskBarManager.rectTransform.anchoredPosition.y));
            }
        }

        [SerializeField] void backEventInvoke() => BackEventInvoke();
        public static void BackEventInvoke()
        {
            if (highPriorityBackEventList.Count > 0)
                highPriorityBackEventList[0].Invoke();
            else if (backEventList.Count > 0)
                backEventList[0].Invoke();
        }

        [SerializeField] void homeEventInvoke() => HomeEventInvoke();
        public static void HomeEventInvoke() => homeEvent.Invoke();

        public static void BackEventAdd(Action action, bool highPriority = false)
        {
            if (highPriority)
                highPriorityBackEventList.Insert(0, action);
            else
                backEventList.Insert(0, action);
        }

        public static void BackEventRemove(Action action, bool highPriority = false)
        {
            if (highPriority)
                highPriorityBackEventList.Remove(action);
            else
                backEventList.Remove(action);
        }
    }
}