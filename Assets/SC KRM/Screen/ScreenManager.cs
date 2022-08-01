using SCKRM.Window;
using UnityEngine;

namespace SCKRM
{
    [AddComponentMenu("SC KRM/Screen/Screen Manager")]
    public sealed class ScreenManager : Manager<ScreenManager>
    {
        public static int width { get; private set; }
        public static int height { get; private set; }

        public static Resolution currentResolution { get; private set; }
        public static Resolution[] resolutions { get; private set; }

        void Awake()
        {
            if (SingletonCheck(this))
                ResolutionRefresh();
        }

        void Update()
        {
            width = Screen.width;
            height = Screen.height;
        }

        public static void ResolutionRefresh()
        {
            currentResolution = Screen.currentResolution;
            resolutions = Screen.resolutions;
        }
    }
}
