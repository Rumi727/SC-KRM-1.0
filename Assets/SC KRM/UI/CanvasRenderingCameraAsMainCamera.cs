using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM
{
    [ExecuteAlways]
    public class CanvasRenderingCameraAsMainCamera : MonoBehaviour
    {
        Canvas _canvas; public Canvas canvas => _canvas = this.GetComponentFieldSave(_canvas);

        void Update()
        {
            if (canvas.worldCamera != UnityEngine.Camera.main)
                canvas.worldCamera = UnityEngine.Camera.main;
        }
    }
}
