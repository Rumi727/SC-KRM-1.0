using SCKRM.UI;
using SCKRM.UI.StatusBar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SCKRM.Camera
{
    [ExecuteAlways]
    [AddComponentMenu("커널/Camera/카메라 설정")]
    public sealed class CameraSetting : MonoBehaviour
    {
        [NonSerialized] UnityEngine.Camera _camera; new public UnityEngine.Camera camera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = GetComponent<UnityEngine.Camera>();
                    if (_camera == null)
                        DestroyImmediate(this);
                }

                return _camera;
            }
        }

        [SerializeField] bool _customSetting;
        public bool customSetting { get => _customSetting; set => _customSetting = value; }

        void Update()
        {
            if (camera == null)
                return;

            if (customSetting)
                return;

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                RectTransform taskBar = StatusBarManager.instance.rectTransform;
                if (StatusBarManager.cropTheScreen)
                {
                    if (!StatusBarManager.SaveData.bottomMode)
                        camera.rect = new Rect(0, 0, 1, 1 - ((taskBar.sizeDelta.y - taskBar.anchoredPosition.y) / Screen.height));
                    else
                    {
                        float y = (taskBar.sizeDelta.y + taskBar.anchoredPosition.y) / Screen.height;
                        camera.rect = new Rect(0, y, 1, 1 - y);
                    }
                }
                else
                    camera.rect = new Rect(0, 0, 1, 1);
            }
#else
            RectTransform taskBar = StatusBarManager.instance.rectTransform;
            RectTransform canvas = KernelCanvas.instance.rectTransform;

            if (StatusBarManager.cropTheScreen)
            {
                if (!StatusBarManager.SaveData.bottomMode)
                    camera.rect = new Rect(0, 0, 1, 1 - ((taskBar.sizeDelta.y - taskBar.anchoredPosition.y) / canvas.sizeDelta.y));
                else
                {
                    float y = (taskBar.sizeDelta.y + taskBar.anchoredPosition.y) / canvas.sizeDelta.y;
                    camera.rect = new Rect(0, y, 1, 1 - y);
                }
            }
            else
                camera.rect = new Rect(0, 0, 1, 1);
#endif
        }
    }
}