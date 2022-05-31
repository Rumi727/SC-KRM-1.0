using K4.Threading;
using Newtonsoft.Json;
using SCKRM.ProjectSetting;
using SCKRM.SaveLoad;
using SCKRM.Threads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM
{
    public class VideoManager : Manager<VideoManager>
    {
        [ProjectSettingSaveLoad]
        public sealed class Data
        {
            static float _standardFPS = 60; [JsonProperty] public static float standardFPS { get => _standardFPS.Clamp(0); set => _standardFPS = value; }
            static int _notFocusFpsLimit = 30; [JsonProperty] public static int notFocusFpsLimit { get => _notFocusFpsLimit.Clamp(0); set => _notFocusFpsLimit = value; }
        }

        [GeneralSaveLoad]
        public sealed class SaveData
        {
            static bool _vSync = true;
            [JsonProperty]
            public static bool vSync
            {
                get => _vSync;
                set
                {
                    _vSync = value;

                    if (ThreadManager.isMainThread)
                        FpsRefresh(Application.isFocused);
                    else
                        K4UnityThreadDispatcher.Execute(() => FpsRefresh(Application.isFocused));

                }
            }

            static int _fpsLimit = 480;
            [JsonProperty]
            public static int fpsLimit
            {
                get => _fpsLimit.Clamp(1);
                set
                {
                    _fpsLimit = value;

                    if (ThreadManager.isMainThread)
                        FpsRefresh(Application.isFocused);
                    else
                        K4UnityThreadDispatcher.Execute(() => FpsRefresh(Application.isFocused));
                }
            }
        }



        void OnApplicationFocus(bool focus) => FpsRefresh(focus);

        static void FpsRefresh(bool focus)
        {
            //FPS Limit
            //앱이 포커스 상태이거나 에디터 상태라면 사용자가 지정한 프레임으로 고정시킵니다
            if (focus || Application.isEditor)
            {
                //수직동기화
                if (SaveData.vSync)
                    QualitySettings.vSyncCount = 1;
                else
                {
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = SaveData.fpsLimit;
                }
            }
            else //앱이 포커스 상태가 아니라면 프로젝트에서 설정한 포커스가 아닌 프레임으로 고정시킵니다
            {
                Application.targetFrameRate = Data.notFocusFpsLimit;
                QualitySettings.vSyncCount = 0;
            }
        }
    }
}
