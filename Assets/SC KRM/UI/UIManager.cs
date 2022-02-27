using SCKRM.Input;
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