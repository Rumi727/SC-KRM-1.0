using SCKRM.Input;
using SCKRM.UI.StatusBar;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SCKRM.UI
{
    [AddComponentMenu("")]
    public sealed class UIManager : Manager<UIManager>
    {
        [SerializeField] Canvas _kernelCanvas; public Canvas kernelCanvas => _kernelCanvas;
        [SerializeField] RectTransform _kernelCanvasUI; public RectTransform kernelCanvasUI => _kernelCanvasUI;
        [SerializeField] TMP_Text _exceptionText; public TMP_Text exceptionText => _exceptionText;



        static List<Action> backEventList { get; } = new List<Action>();
        static List<Action> highPriorityBackEventList { get; } = new List<Action>();

        public static event Action homeEvent = delegate { };



        void Awake() => SingletonCheck(this);

        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                if (InputManager.GetKey("gui.back", InputType.Down, "all", "force"))
                    BackEventInvoke();
                else if (InputManager.GetKey("gui.home", InputType.Down, "all", "force"))
                    homeEvent.Invoke();
            }

            RectTransform taskBarManager = StatusBarManager.instance.rectTransform;
            if (StatusBarManager.SaveData.bottomMode)
            {
                kernelCanvasUI.offsetMin = new Vector2(kernelCanvasUI.offsetMin.x, taskBarManager.rect.size.y + taskBarManager.anchoredPosition.y);
                kernelCanvasUI.offsetMax = new Vector2(kernelCanvasUI.offsetMax.x, 0);
            }
            else
            {
                kernelCanvasUI.offsetMin = new Vector2(kernelCanvasUI.offsetMin.x, 0);
                kernelCanvasUI.offsetMax = new Vector2(kernelCanvasUI.offsetMax.x, -(taskBarManager.rect.size.y - taskBarManager.anchoredPosition.y));
            }
        }

        public static void BackEventInvoke()
        {
            if (highPriorityBackEventList.Count > 0)
                highPriorityBackEventList[0].Invoke();
            else if (backEventList.Count > 0)
                backEventList[0].Invoke();
        }

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