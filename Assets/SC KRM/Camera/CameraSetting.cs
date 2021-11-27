using SCKRM.UI;
using SCKRM.UI.TaskBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Camera
{
    [AddComponentMenu("")]
    public sealed class CameraSetting : MonoBehaviour
    {
        public new UnityEngine.Camera camera { get; private set; }

        void Update()
        {
            if (camera == null)
                camera = GetComponent<UnityEngine.Camera>();

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
        }
    }
}