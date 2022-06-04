using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM
{
    public class ScreenManager : Manager<ScreenManager>
    {
        public static int width { get; private set; }
        public static int height { get; private set; }

        public static Resolution currentResolution { get; private set; }

        void Awake() => SingletonCheck(this);

        void Update()
        {
            width = Screen.width;
            height = Screen.height;

            currentResolution = Screen.currentResolution;
        }
    }
}
