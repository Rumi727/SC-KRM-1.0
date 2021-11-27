using SCKRM.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    public class TaskBarCanvas : MonoBehaviour
    {
        public static TaskBarCanvas instance { get; private set; }



        [SerializeField] RectTransform _rectTransform;
        public RectTransform rectTransform { get => _rectTransform; }

        [SerializeField] Canvas _canvas;
        public Canvas canvas { get => _canvas; }

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        public void KeyDownEnable(string keyCode) => InputManager.KeyDownEnable(keyCode, "taskbar");
        public void KeyEnable(string keyCode) => InputManager.KeyEnable(keyCode, "taskbar");
        public void KeyToggle(string keyCode) => InputManager.KeyToggle(keyCode, "taskbar");
        public void KeyUpEnable(string keyCode) => InputManager.KeyUpEnable(keyCode, "taskbar");
        public void KeyDownEnable2(KeyCode keyCode) => InputManager.KeyDownEnable(keyCode, "taskbar");
        public void KeyEnable2(KeyCode keyCode) => InputManager.KeyEnable(keyCode, "taskbar");
        public void KeyToggle2(KeyCode keyCode) => InputManager.KeyToggle(keyCode, "taskbar");
        public void KeyUpEnable2(KeyCode keyCode) => InputManager.KeyUpEnable(keyCode, "taskbar");
    }
}