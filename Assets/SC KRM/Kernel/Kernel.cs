using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.Tool;
using SCKRM.ProjectSetting;
using SCKRM.Renderer;
using SCKRM.Resource;
using SCKRM.SaveLoad;
using SCKRM.Sound;
using SCKRM.Splash;
using SCKRM.Threads;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SCKRM.UI.SideBar;
using SCKRM.UI.StatusBar;
using SCKRM.Json;
using K4.Threading;
using System.Collections;
using SCKRM.Input;
using SCKRM.Window;

namespace SCKRM
{
    [AddComponentMenu("커널/커널")]
    public sealed class Kernel : Manager<Kernel>
    {
        [ProjectSetting]
        public sealed class Data
        {
            [JsonProperty] public static float standardFPS = 60;



            [JsonProperty] public static int notFocusFpsLimit = 30;



            [JsonProperty] public static string splashScreenPath = "Assets/SC KRM/Splash Screen";
            [JsonProperty] public static string splashScreenName = "Splash Screen";
        }

        [SaveLoad("default")]
        public sealed class SaveData
        {
            [JsonProperty] public static JColor systemColor = new JColor(0.5137255f, 0.1019608f, 0.627451f);

            [JsonProperty] public static int mainVolume = 100;
            [JsonProperty] public static int bgmVolume = 100;
            [JsonProperty] public static int soundVolume = 100;

            [JsonProperty] public static bool vSync = true;
            [JsonProperty] public static int fpsLimit = 480;
            [JsonProperty] public static float guiSize = 1;
            [JsonProperty] public static float fixedGuiSize = 1;
            [JsonProperty] public static bool fixedGuiSizeEnable = true;
        }

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



        void Awake()
        {
            if (SingletonCheck(this))
            {
                DontDestroyOnLoad(instance);
                InitialLoad().Forget();
            }
        }

#if !UNITY_EDITOR
        IEnumerator Start()
        {
            while (true)
            {
                if (isInitialLoadEnd && InputManager.GetKey("kernel.full_screen", InputType.Down, "all"))
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
            //유니티의 내장 변수들은 테스트 결과, 약간의 성능을 더 먹는것으로 확인되었기 때문에
            //이렇게 관리 스크립트가 변수를 할당하고 다른 스크립트가 그 변수를 가져오는것이 성능에 더 도움 되는것을 확인하였습니다
            fps = 1f / deltaTime;
            deltaTime = Time.deltaTime;
            fpsDeltaTime = deltaTime * Data.standardFPS;
            unscaledDeltaTime = Time.unscaledDeltaTime;
            fpsUnscaledDeltaTime = unscaledDeltaTime * Data.standardFPS;

            VariableUpdate();

            //기념일
            //초기로딩이 끝나야 알림을 띄울수 있으니 조건을 걸어둡니다
            //최적화를 위해 년, 월, 일이 변경되어야 실행됩니다
            DateTime dateTime = DateTime.Now;
            if (isInitialLoadEnd && (tempYear != dateTime.Year || tempMonth != dateTime.Month || tempDay != dateTime.Day))
            {
                //음력 날짜를 정합니다
                DateTime dateTimeLunar = dateTime.ToLunarDate();

                if (dateTime.Month == 7 && dateTime.Day == 1) //7월이라면...
                    NoticeManager.Notice("notice.school_live.birthday.title", "notice.school_live.birthday.description", "%value%", (dateTime.Year - 2012).ToString());
                else if (dateTime.Month == 7 && dateTime.Day == 9) //7월 9일이라면...
                    NoticeManager.Notice("notice.school_live_ani.birthday.title", "notice.school_live_ani.birthday.description", "%value%", (dateTime.Year - 2015).ToString()); 
                else if (dateTime.Month == 8 && dateTime.Day == 7) //8월 7일이라면...
                    NoticeManager.Notice("notice.ebisuzawa_kurumi_chan.birthday.title", "notice.ebisuzawa_kurumi_chan.birthday.description");
                else if (dateTime.Month == 4 && dateTime.Day == 5) //4월 5일이라면...
                    NoticeManager.Notice("notice.takeya_yuki.birthday.title", "notice.takeya_yuki.birthday.description");
                else if (dateTime.Month == 10 && dateTime.Day == 11) //10월 11일이라면...
                    NoticeManager.Notice("notice.wakasa_yuri.birthday.title", "notice.wakasa_yuri.birthday.description");
                else if (dateTime.Month == 12 && dateTime.Day == 10) //12월 10일이라면...
                    NoticeManager.Notice("notice.naoki_miki.birthday.title", "notice.naoki_miki.birthday.description");
                else if (dateTime.Month == 3 && dateTime.Day == 10) //3월 10일이라면...
                    NoticeManager.Notice("notice.sakura_megumi.birthday.title", "notice.sakura_megumi.birthday.description");
                else if (dateTime.Month == 2 && dateTime.Day == 9) //2월 9일이라면...
                    NoticeManager.Notice("notice.onell0.birthday.title", "notice.onell0.birthday.description", "%value%", (dateTime.Year - 2010).ToString());
                else if (dateTimeLunar.Month == 1 && dateTimeLunar.Day == 1) //음력으로 1월 1일이라면...
                    NoticeManager.Notice("notice.korean_new_year.title", "notice.korean_new_year.description");

                tempYear = dateTime.Year;
                tempMonth = dateTime.Month;
                tempDay = dateTime.Day;
            }
        }

        //변수 업데이트
        void VariableUpdate()
        {
            //현제 해상도의 가로랑 1920을 나눠서 모든 해상도에도 가로 픽셀 크기는 똑같이 유지되게 함
            float defaultGuiSize = (float)Screen.width / 1920;

            //변수들의 최소, 최대 수치를 지정합니다
            SaveData.mainVolume.ClampRef(0, 200);
            SaveData.bgmVolume.ClampRef(0, 200);
            SaveData.soundVolume.ClampRef(0, 200);

            SaveData.fpsLimit.ClampRef(1);
            SaveData.fixedGuiSize.ClampRef(defaultGuiSize * 0.5f, defaultGuiSize * 4f);
            SaveData.guiSize.ClampRef(0.5f, 4);
            Data.notFocusFpsLimit.ClampRef(0);

            //게임 속도를 0에서 100 사이로 정하고, 타임 스케일을 게임 속도로 정합니다
            gameSpeed = gameSpeed.Clamp(0, 100);
            Time.timeScale = gameSpeed;

            //GUI 크기 설정
            //고정 GUI 크기가 꺼져있다면 화면 크기에 따라 유동적으로 GUI 크기가 변경됩니다
            if (!SaveData.fixedGuiSizeEnable)
                guiSize = SaveData.guiSize * defaultGuiSize;
            else //고정 GUI 크기가 켜져있다면 GUI 크기를 고정시킵니다
                guiSize = SaveData.fixedGuiSize;

            //FPS Limit
            //앱이 포커스 상태이거나 에디터 상태라면 사용자가 지정한 프레임으로 고정시킵니다
            if (Application.isFocused || Application.isEditor)
                Application.targetFrameRate = SaveData.fpsLimit;
            else //앱이 포커스 상태가 아니라면 프로젝트에서 설정한 포커스가 아닌 프레임으로 고정시킵니다
                Application.targetFrameRate = Data.notFocusFpsLimit;

            //수직동기화
            if (!SaveData.vSync)
                QualitySettings.vSyncCount = 0;
            else
                QualitySettings.vSyncCount = 1;

            //볼륨을 사용자가 설정한 볼륨으로 조정시킵니다. 사용자가 설정한 볼륨은 int 0 ~ 200 이기 때문에 0.01을 곱해주어야 하고,
            //100 ~ 200 볼륨이 먹혀야하기 때문에 0.5로 볼륨을 낮춰야하기 때문에 0.005를 곱합니다
            AudioListener.volume = SaveData.mainVolume * 0.005f;
#if UNITY_EDITOR
            Cursor.visible = InputManager.mousePosition.x < 0 || InputManager.mousePosition.x > Screen.width || InputManager.mousePosition.y < 0 || InputManager.mousePosition.y > Screen.height;
#endif
        }

        public static event Action InitialLoadStart = delegate { };
        public static event Action InitialLoadEnd = delegate { };
        public static event Action InitialLoadEndSceneMove = delegate { };

        static async UniTaskVoid InitialLoad()
        {
            //이미 초기로딩이 시작되었으면 더 이상 초기로딩을 진행하면 안되기 때문에 조건문을 걸어줍니다
            if (!isInitialLoadStart)
            {
                try
                {
                    //이 함수는 어떠한 경우에도 메인스레드가 아닌 스레드에서 실행되면 안됩니다
                    if (!ThreadManager.isMainThread)
                        throw new NotMainThreadMethodException(nameof(InitialLoad));
#if UNITY_EDITOR
                    //이 함수는 어떠한 경우에도 앱이 플레이중이 아닐때 실행되면 안됩니다
                    if (!Application.isPlaying)
                        throw new NotPlayModeMethodException(nameof(InitialLoad));
#endif

                    //초기로딩이 시작됬습니다
                    isInitialLoadStart = true;
                    InitialLoadStart();

#if !UNITY_EDITOR
                    Cursor.visible = false;
#endif

                    StatusBarManager.allowStatusBarShow = false;

                    //ThreadManager.Create(() => ThreadManager.ThreadAutoRemove(true), "notice.running_task.thread_auto_remove.name", "notice.running_task.thread_auto_remove.info", true);
                    //스레드를 자동 삭제해주는 함수를 작동시킵니다
                    ThreadManager.ThreadAutoRemove().Forget();

#if UNITY_EDITOR
                    //에디터에선 스플래시 씬에서 시작하지 않기 때문에
                    //시작한 씬의 인덱스를 구하고
                    //인덱스가 0번이 아니면 스플래시 씬을 로딩합니다
                    Scene scene = SceneManager.GetActiveScene();
                    int startedSceneIndex = scene.buildIndex;
                    if (startedSceneIndex != 0)
                        SceneManager.LoadScene(0);
#endif
                    //빌드된곳에선 스플래시 씬에서 시작하기 때문에
                    //아무런 조건문 없이 바로 시작합니다

                    //다른 스레드에서 이 값을 설정하기 전에
                    //미리 설정합니다
                    //(참고: 이 변수는 프로퍼티고 변수가 비어있다면 Application를 호출합니다)
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
                        //프로젝트 설정을 다른 스레드에서 로딩합니다
                        if (await UniTask.RunOnThreadPool(ProjectSettingManager.Load, cancellationToken: AsyncTaskManager.cancel).SuppressCancellationThrow())
                            return;

                        //세이브 데이터의 기본값과 변수들을 다른 스레드에서 로딩합니다
                        if (await UniTask.RunOnThreadPool(SaveLoadManager.VariableInfoLoad, cancellationToken: AsyncTaskManager.cancel).SuppressCancellationThrow())
                            return;

                        //세이브 데이터를 다른 스레드에서 로딩합니다
                        if (await UniTask.RunOnThreadPool(SaveLoadManager.Load, cancellationToken: AsyncTaskManager.cancel).SuppressCancellationThrow())
                            return;

                    }

                    {
                        //리소스를 로딩합니다
                        Debug.Log("Kernel: Waiting for resource to load...");
                        await ResourceManager.ResourceRefresh();

#if UNITY_EDITOR
                        if (!Application.isPlaying)
                            return;
#endif
                    }

                    {
                        //초기 로딩이 끝났습니다
                        isInitialLoadEnd = true;
                        InitialLoadEnd();

                        //리소스를 로딩했으니 모든 렌더러를 전부 재렌더링합니다
                        RendererManager.AllRerender();

                        Debug.Log("Kernel: Initial loading finished!");
                    }

#if UNITY_EDITOR
                    if (startedSceneIndex == 0)
#endif
                    {
                        //씬 애니메이션이 끝날때까지 기다립니다
                        Debug.Log("Kernel: Waiting for scene animation...");
                        if (await UniTask.WaitUntil(() => !SplashScreen.isAniPlayed, cancellationToken: AsyncTaskManager.cancel).SuppressCancellationThrow())
                            return;
                    }

                    StatusBarManager.allowStatusBarShow = true;

                    //씬이 바뀌었을때 렌더러를 재 렌더링해야하기때문에 이벤트를 걸어줍니다
                    SceneManager.sceneLoaded += LoadedSceneEvent;

                    //GC를 호출합니다
                    GC.Collect();

#if UNITY_EDITOR
                    //씬을 이동합니다
                    if (startedSceneIndex != 0)
                        SceneManager.LoadScene(startedSceneIndex);
                    else
                        SceneManager.LoadScene(1);
#else
                    SceneManager.LoadScene(1);
#endif

                    //씬을 이동했으면 이벤트를 호출합니다
                    InitialLoadEndSceneMove();
                }
                catch (Exception e)
                {
                    //예외를 발견하면 앱을 강제 종료합니다
                    //에디터 상태라면 플레이 모드를 종료합니다
                    Debug.LogException(e);

                    if (!isInitialLoadEnd)
                    {
                        Debug.LogError("Kernel: Initial loading failed");
#if UNITY_EDITOR
                        if (Application.isPlaying)
                        {
                            //플레이 모드가 바로 종료되지 않기 때문에
                            //다른 예외가 날 가능성이 있어서 먼저 모든 게임 오브젝트를 지웁니다
                            GameObject[] gameObjects = FindObjectsOfType<GameObject>(true);
                            int length = gameObjects.Length;
                            for (int i = 0; i < length; i++)
                                DestroyImmediate(gameObjects[i]);

                            UnityEditor.EditorApplication.isPlaying = false;
                        }
                        else
                        {
                            //예외가 났는데 플레이 모드가 종료된 상태면
                            //십중팔구로 초기로딩이 끝나지 않은상태에서
                            //플레이 모드를 종료한거기 때문에 경고만 띄워줍니다
                            Debug.LogWarning("Kernel: Do not exit play mode during initial loading");
                        }
#else

                        WindowManager.MessageBox(e.GetType().Name + ": " + e.Message + "\n\n" + e.StackTrace.Substring(5), "Kernel: Initial loading failed", WindowManager.MessageBoxButtons.OK, WindowManager.MessageBoxIcon.Error);
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
            AsyncTaskManager.AllAsyncTaskCancel(false);
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