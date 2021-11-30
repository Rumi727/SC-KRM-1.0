using SCKRM.UI;
using SCKRM.UI.TaskBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SCKRM.Camera
{
    [ExecuteAlways]
    [AddComponentMenu("커널/Camera/카메라 설정"), RequireComponent(typeof(UnityEngine.Camera), typeof(PostProcessLayer))]
    public sealed class CameraSetting : MonoBehaviour
    {
        public bool CustomAntiAliasing = false;

        [System.NonSerialized] UnityEngine.Camera _camera;
        public new UnityEngine.Camera camera
        {
            get
            {
                if (_camera == null)
                    _camera = GetComponent<UnityEngine.Camera>();

                return _camera;
            }
        }

        [System.NonSerialized] PostProcessLayer _postProcessLayer;
        public PostProcessLayer postProcessLayer
        {
            get
            {
                if (_postProcessLayer == null)
                    _postProcessLayer = GetComponent<PostProcessLayer>();

                return _postProcessLayer;
            }
        }

        void Update()
        {
            if (!CustomAntiAliasing)
            {
                postProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                postProcessLayer.fastApproximateAntialiasing.fastMode = true;
                postProcessLayer.fastApproximateAntialiasing.keepAlpha = true;
            }

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                RectTransform taskBar = TaskBarManager.instance.rectTransform;
                RectTransform canvas = KernelCanvas.instance.rectTransform;

                if (TaskBarManager.cropTheScreen)
                {
                    if (!TaskBarManager.SaveData.topMode)
                        camera.rect = new Rect(0, 0, 1, 1 - ((taskBar.sizeDelta.y - taskBar.anchoredPosition.y) / canvas.sizeDelta.y));
                    else
                    {
                        float y = (taskBar.sizeDelta.y + taskBar.anchoredPosition.y) / canvas.sizeDelta.y;
                        camera.rect = new Rect(0, y, 1, 1 - y);
                    }
                }
                else
                    camera.rect = new Rect(0, 0, 1, 1);
            }
#else
            RectTransform taskBar = TaskBarManager.instance.rectTransform;
            RectTransform canvas = TaskBarCanvas.instance.rectTransform;

            if (TaskBarManager.cropTheScreen)
            {
                if (!TaskBarManager.SaveData.topMode)
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