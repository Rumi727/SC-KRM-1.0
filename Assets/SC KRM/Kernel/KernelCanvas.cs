using SCKRM.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    [AddComponentMenu("")]
    public sealed class KernelCanvas : UI
    {
        public static KernelCanvas instance { get; private set; }

        [SerializeField, HideInInspector] Canvas _canvas;
        public Canvas canvas
        {
            get
            {
                if (_canvas == null)
                    _canvas = GetComponent<Canvas>();

                return _canvas;
            }
        }



        public static List<Action> backEventList { get; } = new List<Action>();
        public static List<Action> kernelBackEventList { get; } = new List<Action>();

        public static event Action homeEvent;



        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                if (InputManager.GetKey("gui.back", InputType.Down, "all"))
                {
                    if (kernelBackEventList.Count > 0)
                        kernelBackEventList[kernelBackEventList.Count - 1]?.Invoke();
                    else if (backEventList.Count > 0)
                        backEventList[backEventList.Count - 1]?.Invoke();
                }
                else if (InputManager.GetKey("gui.home", InputType.Down, "all"))
                    homeEvent?.Invoke();
            }
        }
    }
}