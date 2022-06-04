using K4.Threading;
using Newtonsoft.Json;
using SCKRM.Input;
using SCKRM.SaveLoad;
using SCKRM.Sound;
using SCKRM.Text;
using SCKRM.Threads;
using SCKRM.UI;
using SCKRM.UI.StatusBar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

namespace SCKRM.DebugUI
{
    public sealed class DebugManager : UIManager<DebugManager>
    {
        [GeneralSaveLoad]
        public class SaveData
        {
            static float _speed = 2;
            [JsonProperty] public static float graphSpeed { get => _speed.Clamp(1); set => _speed = value.Clamp(1); }



            [JsonProperty] public static bool textShow { get; set; } = true;
            [JsonProperty] public static bool graphShow { get; set; } = true;
        }

        public delegate void DebugText(FastString fastString);
        public static event DebugText leftDebugText;
        public static event DebugText rightDebugText;

        public static bool isShow { get; set; } = false;

        [SerializeField] TMP_Text _leftText; public TMP_Text leftText => _leftText;
        [SerializeField] TMP_Text _rightText; public TMP_Text rightText => _rightText;



        [SerializeField] GameObject _textLayout; public GameObject textLayout => _textLayout;
        [SerializeField] GameObject _graphLayout; public GameObject graphLayout => _graphLayout;



        protected override void Awake()
        {
            leftDebugText += LeftDebug;
            rightDebugText += RightDebug;

            void LeftDebug(FastString fastString)
            {
                LabelValue("Delta Time", Kernel.deltaTime, fastString);
                LabelValue("FPS Delta Time", Kernel.fpsDeltaTime, fastString, true);

                LabelValue("Unscaled Delta Time", Kernel.unscaledDeltaTime, fastString);
                LabelValue("Unscaled FPS Delta Time", Kernel.fpsUnscaledDeltaTime, fastString, true);

                LabelValue("FPS", Kernel.fps, fastString, true);

                LabelValue("Total Allocated Memory (MB)", (Profiler.GetTotalAllocatedMemoryLong() / 1048576f).Round(4), fastString, true);

                LabelValue("Game Speed", Kernel.gameSpeed, fastString, true);

                LabelValue("Async Tasks Count", AsyncTaskManager.asyncTasks.Count, fastString, true);

                LabelValue("Main Thread ID", ThreadManager.mainThreadId, fastString);
                LabelValue("Running Threads Count", ThreadManager.runningThreads.Count, fastString, true);

                LabelValue("Sound List Count", SoundManager.soundList.Count, fastString);
                LabelValue("NBS List Count", SoundManager.nbsList.Count, fastString);
            }

            void RightDebug(FastString fastString)
            {
                LabelValue("Data Path", Kernel.dataPath, fastString);
                LabelValue("Streaming Assets Path", Kernel.streamingAssetsPath, fastString);
                LabelValue("Persistent Data Path", Kernel.persistentDataPath, fastString);
                LabelValue("Temporary Cache Path", Kernel.temporaryCachePath, fastString);
                LabelValue("Save Data Path", Kernel.saveDataPath, fastString);
                LabelValue("Resource Pack Path", Kernel.resourcePackPath, fastString);
                LabelValue("Project Setting Path", Kernel.projectSettingPath, fastString, true);

                LabelValue("Company Name", Kernel.companyName, fastString);
                LabelValue("Product Name", Kernel.productName, fastString, true);

                LabelValue("Version", Kernel.version, fastString);
                LabelValue("Unity Version", Application.unityVersion, fastString, true);

                LabelValue("Platform", Kernel.platform.ToString(), fastString, true);


                LabelValue("OS", SystemInfo.operatingSystem, fastString, true);

                LabelValue("Device Model", SystemInfo.deviceModel, fastString);
                LabelValue("Device Name", SystemInfo.deviceName, fastString, true);

                LabelValue("Battery Status", SystemInfo.batteryStatus.ToString(), fastString, true);

                LabelValue("Processor Type", SystemInfo.processorType, fastString);
                LabelValue("Processor Frequency", SystemInfo.processorFrequency, fastString);
                LabelValue("Processor Count", SystemInfo.processorCount, fastString, true);

                LabelValue("Graphics Device Name", SystemInfo.graphicsDeviceName, fastString);
                LabelValue("Graphics Memory Size", SystemInfo.graphicsMemorySize, fastString, true);

                LabelValue("System Memory Size", SystemInfo.systemMemorySize, fastString);
            }
        }

        float timer = 0;
        void Update()
        {
            rectTransform.offsetMin = StatusBarManager.cropedRect.min;
            rectTransform.offsetMax = StatusBarManager.cropedRect.max;

            if (InitialLoadManager.isInitialLoadEnd && InputManager.GetKey("debug_manager.toggle", InputType.Down, "all", "force"))
            {
                Refresh();
                isShow = !isShow;
            }

            if (textLayout.activeSelf != (isShow && SaveData.textShow))
                textLayout.SetActive(isShow && SaveData.textShow);
            if (graphLayout.activeSelf != (isShow && SaveData.graphShow))
                graphLayout.SetActive(isShow && SaveData.graphShow);

            if (isShow)
            {
                timer += Kernel.unscaledDeltaTime;

                if (timer >= 0.1f)
                {
                    LeftRefresh();
                    timer = 0;
                }
            }
        }

        public static void LabelValue(string label, string value, FastString fastString, bool line = false)
        {
            fastString.Append(label);
            fastString.Append(" - ");
            fastString.Append(value);

            if (line)
                fastString.Append("\n\n");
            else
                fastString.Append("\n");
        }

        public static void LabelValue(string label, int value, FastString fastString, bool line = false)
        {
            fastString.Append(label);
            fastString.Append(" - ");
            fastString.Append(value);

            if (line)
                fastString.Append("\n\n");
            else
                fastString.Append("\n");
        }

        public static void LabelValue(string label, float value, FastString fastString, bool line = false)
        {
            fastString.Append(label);
            fastString.Append(" - ");
            fastString.Append(value);

            if (line)
                fastString.Append("\n\n");
            else
                fastString.Append("\n");
        }



        //static int leftFastStringLock = 0;
        //static int rightFastStringLock = 0;

        static FastString _leftFastString = new FastString(2048);
        static FastString _rightFastString = new FastString(2048);

        public static FastString leftFastString
        {
            get
            {
                /*while (Interlocked.CompareExchange(ref leftFastStringLock, 1, 0) != 0)
                    Thread.Sleep(1);*/

                FastString replace = _leftFastString;

                //Interlocked.Decrement(ref leftFastStringLock);
                return replace;
            }
        }
        public static FastString rightFastString
        {
            get
            {
                /*while (Interlocked.CompareExchange(ref rightFastStringLock, 1, 0) != 0)
                    Thread.Sleep(1);
                */
                FastString replace = _rightFastString;

                /*Interlocked.Decrement(ref rightFastStringLock);*/
                return replace;
            }
        }



        public void LeftRefresh()
        {
            leftFastString.Clear();
            leftDebugText?.Invoke(leftFastString);

            leftText.text = leftFastString.ToString();
        }

        public void Refresh()
        {
            leftFastString.Clear();
            rightFastString.Clear();

            leftDebugText?.Invoke(leftFastString);
            rightDebugText?.Invoke(rightFastString);

            leftText.text = leftFastString.ToString();
            rightText.text = rightFastString.ToString();
        }
    }
}
