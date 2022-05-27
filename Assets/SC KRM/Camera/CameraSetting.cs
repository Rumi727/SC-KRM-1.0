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
#pragma warning disable CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.
        [NonSerialized] UnityEngine.Camera _camera; public UnityEngine.Camera camera => _camera = this.GetComponentFieldSave(_camera, ComponentTool.GetComponentMode.destroyIfNull);
#pragma warning restore CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.

        [SerializeField] bool _customSetting;
        public bool customSetting { get => _customSetting; set => _customSetting = value; }

        void Update()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
            {
                if (camera == null)
                    return;
                else if (customSetting)
                    return;

                RectTransform taskBar = StatusBarManager.instance.rectTransform;
                if (StatusBarManager.cropTheScreen)
                {
                    if (!StatusBarManager.SaveData.bottomMode)
                        camera.rect = new Rect(0, 0, 1, 1 - ((taskBar.rect.size.y - taskBar.anchoredPosition.y) * UIManager.currentGuiSize / Screen.height));
                    else
                    {
                        float y = (taskBar.rect.size.y + taskBar.anchoredPosition.y) * UIManager.currentGuiSize / Screen.height;
                        camera.rect = new Rect(0, y, 1, 1 - y);
                    }
                }
                else
                    camera.rect = new Rect(0, 0, 1, 1);
            }
        }
    }
}