using SCKRM.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    [AddComponentMenu("")]
    public sealed class KernelCanvas : MonoBehaviour
    {
        public static KernelCanvas instance { get; private set; }



        [SerializeField, HideInInspector] RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();

                return _rectTransform;
            }
        }

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

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        [SerializeField] void KeyDownEnable(string keyCode) => InputManager.KeyDownEnable(keyCode, "statusbar").Forget();
        [SerializeField] void KeyEnable(string keyCode) => InputManager.KeyEnable(keyCode, "statusbar").Forget();
        [SerializeField] void KeyToggle(string keyCode) => InputManager.KeyToggle(keyCode, "statusbar");
        [SerializeField] void KeyUpEnable(string keyCode) => InputManager.KeyUpEnable(keyCode, "statusbar").Forget();
        [SerializeField] void KeyDownEnable2(KeyCode keyCode) => InputManager.KeyDownEnable(keyCode, "statusbar").Forget();
        [SerializeField] void KeyEnable2(KeyCode keyCode) => InputManager.KeyEnable(keyCode, "statusbar").Forget();
        [SerializeField] void KeyToggle2(KeyCode keyCode) => InputManager.KeyToggle(keyCode, "statusbar");
        [SerializeField] void KeyUpEnable2(KeyCode keyCode) => InputManager.KeyUpEnable(keyCode, "statusbar").Forget();
    }
}