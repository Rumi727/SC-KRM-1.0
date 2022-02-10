using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.Tool;
using SCKRM.Input;
using SCKRM.Language;
using SCKRM.ProjectSetting;
using SCKRM.Renderer;
using SCKRM.Resource;
using SCKRM.SaveLoad;
using SCKRM.Sound;
using SCKRM.Splash;
using SCKRM.Threads;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SCKRM.Window;
using System.Threading.Tasks;
using SCKRM.UI.SideBar;
using SCKRM.UI.TaskBar;

namespace SCKRM
{
    [AddComponentMenu("커널/커널")]
    public sealed class Kernel : MonoBehaviour
    {
        [ProjectSetting]
        public sealed class Data
        {
            [JsonProperty] public static float standardFPS { get; set; } = 60;



            [JsonProperty] public static int notFocusFpsLimit { get; set; } = 30;
            [JsonProperty] public static int afkFpsLimit { get; set; } = 30;

            [JsonProperty] public static float afkTimerLimit { get; set; } = 60;



            [JsonProperty] public static string splashScreenPath { get; set; } = "Assets/SC KRM/Splash Screen";
            [JsonProperty] public static string splashScreenName { get; set; } = "Splash Screen";
        }

        [SaveLoad("default")]
        public sealed class SaveData
        {
            [JsonProperty] public static int mainVolume { get; set; } = 100;
            [JsonProperty] public static int bgmVolume { get; set; } = 100;
            [JsonProperty] public static int soundVolume { get; set; } = 100;

            [JsonProperty] public static bool vSync { get; set; } = true;
            [JsonProperty] public static int fpsLimit { get; set; } = 300;
            [JsonProperty] public static float guiSize { get; set; } = 1;
            [JsonProperty] public static float fixedGuiSize { get; set; } = 1;
            [JsonProperty] public static bool fixedGuiSizeEnable { get; set; } = true;
        }

        public static Kernel instance;

        public static float fps { get; private set; } = 60;

        public static float deltaTime { get; private set; } = fps60second;
        public static float fpsDeltaTime { get; private set; } = 1;
        public static float unscaledDeltaTime { get; private set; } = fps60second;
        public static float fpsUnscaledDeltaTime { get; private set; } = 1;

        public const float fps60second = 1f / 60f;
        
        static string _dataPath = "";
        /// <summary>
        /// Application.dataPath
        /// </summary>
        public static string dataPath
        {
            get
            {
                if (_dataPath != "")
                    return _dataPath;
                else
                    return _dataPath = Application.dataPath;
            }
        }

        /// <summary>
        /// Application.streamingAssetsPath
        /// </summary>
        public static string streamingAssetsPath { get; } = Application.streamingAssetsPath;

        static string _persistentDataPath = "";
        /// <summary>
        /// Application.persistentDataPath
        /// </summary>
        public static string persistentDataPath
        {
            get
            {
                if (_persistentDataPath != "")
                    return _persistentDataPath;
                else
                    return _persistentDataPath = Application.persistentDataPath;
            }
        }

        static string _saveDataPath = "";
        /// <summary>
        /// Kernel.persistentDataPath + "/Save Data"
        /// </summary>
        public static string saveDataPath
        {
            get
            {
                if (_saveDataPath != "")
                    return _saveDataPath;
                else
                    return _saveDataPath = persistentDataPath + "/Save Data";
            }
        }

        /// <summary>
        /// Kernel.streamingAssetsPath + "/projectSettings"
        /// </summary>
        public static string projectSettingPath { get; } = streamingAssetsPath + "/projectSettings";



        static string _companyName = "";
        public static string companyName
        {
            get
            {
                if (_companyName != "")
                    return _companyName;
                else
                    return _companyName = Application.companyName;
            }
        }

        static string _productName = "";
        public static string productName
        {
            get
            {
                if (_productName != "")
                    return _productName;
                else
                    return _productName = Application.productName;
            }
        }

        static string _version = "";
        public static string version
        {
            get
            {
                if (_version != "")
                    return _version;
                else
                    return _version = Application.version;
            }
        }



        static string _unityVersion = "";
        public static string unityVersion
        {
            get
            {
                if (_version != "")
                    return _unityVersion;
                else
                    return _unityVersion = Application.unityVersion;
            }
        }



        public static RuntimePlatform platform { get; } = Application.platform;



        public static bool isAFK { get; private set; } = false;
        public static float afkTimer { get; private set; } = 0;






        public static float gameSpeed { get; set; } = 1;
        public static float guiSize { get; private set; } = 1;

        public static bool isInitialLoadStart { get; private set; } = false;
        public static bool isInitialLoadEnd { get; private set; } = false;



        [SerializeField] Image _splashScreenBackground;
        public Image splashScreenBackground => _splashScreenBackground;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            InitialLoad().Forget();
        }

#if !UNITY_EDITOR
        IEnumerator Start()
        {
            while (true)
            {
                if (isInitialLoadEnd && InputManager.GetKeyDown("kernel.full_screen", "all"))
                {
                    if (Screen.fullScreen)
                        Screen.SetResolution((int)(Screen.currentResolution.width / 1.5f), (int)(Screen.currentResolution.height / 1.5f), false);
                    else
                    {
                        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
                        yield return new WaitForEndOfFrame();
                        yield return new WaitForEndOfFrame();
                        yield return new WaitForEndOfFrame();
                        yield return new WaitForEndOfFrame();
                        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                    }
                }
                yield return null;
            }
        }
#endif

        static int tempYear;
        static int tempMonth;
        static int tempDay;
        void Update()
        {
            fps = 1f / deltaTime;
            deltaTime = Time.deltaTime;
            fpsDeltaTime = deltaTime * Data.standardFPS;
            unscaledDeltaTime = Time.unscaledDeltaTime;
            fpsUnscaledDeltaTime = unscaledDeltaTime * Data.standardFPS;

            //AFK
            if (isInitialLoadEnd && InputManager.GetAnyKeyDown("all"))
                afkTimer = 0;

            if (afkTimer >= Data.afkTimerLimit)
                isAFK = true;
            else
            {
                isAFK = false;
                afkTimer += unscaledDeltaTime;
            }

            gameSpeed = gameSpeed.Clamp(0, 100);
            Time.timeScale = gameSpeed;

            //기념일
            DateTime dateTime = DateTime.Now;
            if (isInitialLoadEnd && (tempYear != dateTime.Year || tempMonth != dateTime.Month || tempDay != dateTime.Day))
            {
                if (dateTime.Month == 8 && dateTime.Day == 7)
                    NoticeManager.Notice("kurumi_chan.birthday.title", "kurumi_chan.birthday.description");
                else if (dateTime.Month == 2 && dateTime.Day == 9)
                    NoticeManager.Notice("onell0.birthday.title", "onell0.birthday.description", "%value%", (dateTime.Year - 2010).ToString());

                tempYear = dateTime.Year;
                tempMonth = dateTime.Month;
                tempDay = dateTime.Day;
            }
        }

        //변수 업데이트
        void LateUpdate()
        {
            //현제 해상도의 가로랑 1920을 나눠서 모든 해상도에도 가로 픽셀 크기는 똑같이 유지되게 함
            float defaultGuiSize = (float)Screen.width / 1920;

            SaveData.mainVolume = SaveData.mainVolume.Clamp(0, 200);
            SaveData.bgmVolume = SaveData.bgmVolume.Clamp(0, 200);
            SaveData.soundVolume = SaveData.soundVolume.Clamp(0, 200);

            SaveData.fpsLimit = SaveData.fpsLimit.Clamp(30);
            SaveData.fixedGuiSize = SaveData.fixedGuiSize.Clamp(defaultGuiSize * 0.5f, defaultGuiSize * 4f);
            SaveData.guiSize = SaveData.guiSize.Clamp(0.5f, 4);
            Data.notFocusFpsLimit = Data.notFocusFpsLimit.Clamp(1);
            Data.afkFpsLimit = Data.afkFpsLimit.Clamp(1);
            Data.afkTimerLimit = Data.afkTimerLimit.Clamp(0);

            //GUI 크기 설정
            if (!SaveData.fixedGuiSizeEnable)
                guiSize = SaveData.guiSize * defaultGuiSize;
            else
                guiSize = SaveData.fixedGuiSize;

            //FPS Limit
            if (!isAFK && (Application.isFocused || Application.isEditor))
                Application.targetFrameRate = SaveData.fpsLimit;
            else if (!isAFK && !Application.isFocused)
                Application.targetFrameRate = Data.notFocusFpsLimit;
            else
                Application.targetFrameRate = Data.afkFpsLimit;

            if (!SaveData.vSync)
                QualitySettings.vSyncCount = 0;
            else
                QualitySettings.vSyncCount = 1;

            AudioListener.volume = SaveData.mainVolume * 0.005f;
        }

        public static event Action InitialLoadStart;
        public static event Action InitialLoadEnd;
        public static event Action InitialLoadEndSceneMove;
        async UniTaskVoid InitialLoad()
        {
            if (!isInitialLoadStart)
            {
                try
                {
                    if (!ThreadManager.isMainThread)
                        throw new NotMainThreadMethodException(nameof(InitialLoad));
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        throw new NotPlayModeMethodException(nameof(InitialLoad));
#endif

                    isInitialLoadStart = true;
                    InitialLoadStart?.Invoke();

                    splashScreenBackground.gameObject.SetActive(true);
                    splashScreenBackground.color = new Color(splashScreenBackground.color.r, splashScreenBackground.color.g, splashScreenBackground.color.b, 1);

                    //ThreadManager.Create(() => ThreadManager.ThreadAutoRemove(true), "notice.running_task.thread_auto_remove.name", "notice.running_task.thread_auto_remove.info", true);
                    ThreadManager.ThreadAutoRemove();

#if UNITY_EDITOR
                    Scene scene = SceneManager.GetActiveScene();
                    int startedSceneIndex = scene.buildIndex;
                    if (startedSceneIndex != 0)
                        SceneManager.LoadScene(0);
#else
                    Scene scene = SceneManager.GetActiveScene();
                    int index = scene.buildIndex;
#endif

                    {
                        _ = dataPath;
                        _ = persistentDataPath;
                        _ = saveDataPath;

                        _ = companyName;
                        _ = productName;

                        _ = version;
                        _ = unityVersion;
                    }

                    Debug.Log("Kernel: Waiting for settings to load...");
                    {
                        ThreadMetaData projectSettingLoad = ThreadManager.Create(ProjectSettingManager.Load, "Project Setting Load");
                        await UniTask.WaitUntil(() => projectSettingLoad.thread == null, PlayerLoopTiming.Initialization);
                        projectSettingLoad = ThreadManager.Create(SaveLoadManager.VariableInfoLoad, "Save Data Load");
                        await UniTask.WaitUntil(() => projectSettingLoad.thread == null, PlayerLoopTiming.Initialization);
                        projectSettingLoad = ThreadManager.Create(SaveLoadManager.Load, "Save Data Load");
                        await UniTask.WaitUntil(() => projectSettingLoad.thread == null, PlayerLoopTiming.Initialization);
                    }

                    {
                        Debug.Log("Kernel: Waiting for resource to load...");
                        await ResourceManager.ResourceRefresh();
                    }
                    
                    {
                        InitialLoadEnd?.Invoke();
                        isInitialLoadEnd = true;

                        RendererManager.AllRerender();

                        Debug.Log("Kernel: Initial loading finished!");
                    }

                    {
                        if (startedSceneIndex == 0)
                        {
                            Debug.Log("Kernel: Waiting for scene animation...");
                            await UniTask.WaitUntil(() => !SplashScreen.isAniPlayed, PlayerLoopTiming.Initialization);
                        }
                    }

                    TaskBarManager.allowTaskBarShow = true;

                    SceneManager.sceneLoaded += LoadedSceneEvent;

                    GC.Collect();

#if UNITY_EDITOR
                    if (startedSceneIndex != 0)
                        SceneManager.LoadScene(startedSceneIndex);
                    else
                        SceneManager.LoadScene(startedSceneIndex + 1);
#else
                    SceneManager.LoadScene(index + 1);
#endif
                    InitialLoadEndSceneMove?.Invoke();

                    while (splashScreenBackground.color.a > 0)
                    {
                        splashScreenBackground.color = new Color(splashScreenBackground.color.r, splashScreenBackground.color.g, splashScreenBackground.color.b, splashScreenBackground.color.a - 0.05f * fpsDeltaTime);
                        await UniTask.DelayFrame(1, PlayerLoopTiming.Initialization);
                    }

                    splashScreenBackground.gameObject.SetActive(false);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);

                    if (!isInitialLoadEnd)
                    {
                        Debug.LogError("Kernel: Initial loading failed");
#if UNITY_EDITOR
                        if (Application.isPlaying)
                        {
                            GameObject[] gameObjects = FindObjectsOfType<GameObject>(true);
                            int length = gameObjects.Length;
                            for (int i = 0; i < length; i++)
                                DestroyImmediate(gameObjects[i]);

                            UnityEditor.EditorApplication.isPlaying = false;
                        }
                        else
                            Debug.LogWarning("Kernel: Do not exit play mode during initial loading");
#else
                        Application.Quit(1);
#endif
                    }
                }
            }
        }

        static void LoadedSceneEvent(Scene scene, LoadSceneMode mode) => RendererManager.AllRerender();


        public static event Action AllRefreshStart;
        public static event Action AllRefreshEnd;
        public static async UniTaskVoid AllRefresh(bool onlyText = false)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(AllRefresh));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(AllRefresh));
#endif
            AllRefreshStart?.Invoke();

            if (onlyText)
                RendererManager.AllTextRerender();
            else
            {
/*#if !UNITY_EDITOR
                if (SoundManager.soundList.Count > 0)
                {
#if UNITY_STANDALONE_WIN
                    string text = LanguageManager.TextLoad("kernel.allrefresh.warning");
                    string caption = LanguageManager.TextLoad("gui.warning");
                    WindowManager.DialogResult dialogResult = WindowManager.MessageBox(text, caption, WindowManager.MessageBoxButtons.OKCancel, WindowManager.MessageBoxIcon.Warning);
                    if (dialogResult != WindowManager.DialogResult.OK)
                        return;
#else
                Debug.LogError(LanguageManager.TextLoad("kernel.allrefresh.error"));
                return;
#endif
                }
#endif*/
                if (!ResourceManager.isResourceRefesh)
                {
                    await ResourceManager.ResourceRefresh();
                    RendererManager.AllRerender();

                    SoundManager.SoundRefresh();
                    ResourceManager.AudioGarbageRemoval();
                }
            }
            
            GC.Collect();
            AllRefreshEnd?.Invoke();
        }

        void OnApplicationQuit()
        {
            ThreadManager.AllThreadRemove();

            if (isInitialLoadEnd)
                SaveLoadManager.Save();
        }
    }


    public class NotPlayModeMethodException : Exception
    {
        /// <summary>
        /// It is not possible to use this function when not in play mode.
        /// 플레이 모드가 아닐때 이 함수를 사용하는건 불가능합니다.
        /// </summary>
        public NotPlayModeMethodException() : base("It is not possible to use this function when not in play mode.\n플레이 모드가 아닐때 이 함수를 사용하는건 불가능합니다") { }

        /// <summary>
        /// It is not possible to use {method} functions when not in play mode.
        /// 플레이 모드가 아닐때 이 함수를 사용하는건 불가능합니다.
        /// </summary>
        public NotPlayModeMethodException(string method) : base($"It is not possible to use {method} functions when not in play mode.\n플레이 모드가 아닐때 {method} 함수를 사용하는건 불가능합니다") { }
    }



    public class NotInitialLoadEndMethodException : Exception
    {
        /// <summary>
        /// Initial loading was not finished, but I tried to use a function that needs loading
        /// 초기 로딩이 안끝났는데 로딩이 필요한 함수를 사용하려 했습니다
        /// </summary>
        public NotInitialLoadEndMethodException() : base("Initial loading was not finished, but I tried to use a function that needs loading\n초기 로딩이 안끝났는데 로딩이 필요한 함수를 사용하려 했습니다") { }

        /// <summary>
        /// Initial loading was not finished, but I tried to use a {method} function that needs loading
        /// 초기 로딩이 안끝났는데 로딩이 필요한 {method} 함수를 사용하려 했습니다
        /// </summary>
        public NotInitialLoadEndMethodException(string method) : base($"Initial loading was not finished, but I tried to use a {method} function that needs loading\n초기 로딩이 안끝났는데 로딩이 필요한 {method} 함수를 사용하려 했습니다") { }
    }



    public class NullResourceObjectException : Exception
    {
        /// <summary>
        /// No object in resource folder
        /// 리소스 폴더에 오브젝트가 없습니다
        /// </summary>
        public NullResourceObjectException() : base("No object in resource folder\n리소스 폴더에 오브젝트가 없습니다") { }

        /// <summary>
        /// Object {objectName} does not exist in resource folder
        /// 리소스 폴더에 {objectName} 오브젝트가 없습니다
        /// </summary>
        public NullResourceObjectException(string objectName) : base($"Object {objectName} does not exist in resource folder\n리소스 폴더에 {objectName} 오브젝트가 없습니다") { }
    }

    public class NullSceneException : Exception
    {
        /// <summary>
        /// no scene
        /// 씬이 없습니다
        /// </summary>
        public NullSceneException() : base("no scene\n씬이 없습니다") { }

        /// <summary>
        /// {sceneName} no scene
        /// {sceneName} 씬이 없습니다
        /// </summary>
        public NullSceneException(string sceneName) : base($"{sceneName} no scene\n{sceneName} 씬이 없습니다") { }
    }

    public class NullScriptMethodException : Exception
    {
        /// <summary>
        /// Failed to execute function because script does not exist
        /// 스크립트가 없어서 함수를 실행하지 못했습니다
        /// </summary>
        public NullScriptMethodException() : base("Failed to execute function because script does not exist\n스크립트가 없어서 함수를 실행하지 못했습니다") { }

        /// <summary>
        /// Failed to execute function because script asdf does not exist
        /// {script} 스크립트가 없어서 함수를 실행하지 못했습니다
        /// </summary>
        public NullScriptMethodException(string scriptName) : base($"Failed to execute function because script {scriptName} does not exist\n{scriptName} 스크립트가 없어서 함수를 실행하지 못했습니다") { }

        /// <summary>
        /// Failed to execute {methodName} function because script {scriptName} does not exist
        /// {script} 스크립트가 없어서 {method} 함수를 실행하지 못했습니다
        /// </summary>
        public NullScriptMethodException(string scriptName, string methodName) : base($"Failed to execute {methodName} function because script {scriptName} does not exist\n{scriptName} 스크립트가 없어서 {methodName} 함수를 실행하지 못했습니다") { }
    }
}