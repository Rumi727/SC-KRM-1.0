using SCKRM.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    public class KernelCanvas : MonoBehaviour
    {
        public static KernelCanvas instance { get; private set; }



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

        public void KeyDownEnable(string keyCode) => InputManager.KeyDownEnable(keyCode, InputLockDeny.TaskBar);
        public void KeyEnable(string keyCode) => InputManager.KeyEnable(keyCode, InputLockDeny.TaskBar);
        public void KeyToggle(string keyCode) => InputManager.KeyToggle(keyCode, InputLockDeny.TaskBar);
        public void KeyUpEnable(string keyCode) => InputManager.KeyUpEnable(keyCode, InputLockDeny.TaskBar);
        public void KeyDownEnable2(KeyCode keyCode) => InputManager.KeyDownEnable(keyCode, InputLockDeny.TaskBar);
        public void KeyEnable2(KeyCode keyCode) => InputManager.KeyEnable(keyCode, InputLockDeny.TaskBar);
        public void KeyToggle2(KeyCode keyCode) => InputManager.KeyToggle(keyCode, InputLockDeny.TaskBar);
        public void KeyUpEnable2(KeyCode keyCode) => InputManager.KeyUpEnable(keyCode, InputLockDeny.TaskBar);
    }
}