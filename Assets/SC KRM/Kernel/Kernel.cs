using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
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

namespace SCKRM
{
    [AddComponentMenu("커널/커널")]
    public sealed class Kernel : MonoBehaviour
    {
        [ProjectSetting("Setting")]
        public sealed class Data
        {
            [JsonProperty] public static float standardFPS { get; set; }



            [JsonProperty] public static int notFocusFpsLimit { get; set; }
            [JsonProperty] public static int afkFpsLimit { get; set; }

            [JsonProperty] public static float afkTimerLimit { get; set; }
        }

        [SaveLoad("Kernel")]
        public sealed class SaveData
        {
            [JsonProperty] public static int fpsLimit { get; set; } = 300;
            [JsonProperty] public static float guiSize { get; set; } = 1;
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






        public static int mainVolume { get; set; } = 100;
        public static float gameSpeed { get; set; } = 1;

        public static bool isInitialLoadStart { get; private set; } = false;
        public static bool isInitialLoadEnd { get; private set; } = false;



        [SerializeField] Image _splashScreenBackground;
        public Image splashScreenBackground => _splashScreenBackground;

        void OnEnable()
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

            InitialLoad();
        }

#if !UNITY_EDITOR
        IEnumerator Start()
        {
            while (true)
            {
                if (isInitialLoadEnd && InputManager.GetKeyDown("kernel.full_screen"))
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

        void Update()
        {
            fps = 1f / deltaTime;
            deltaTime = Time.deltaTime;
            fpsDeltaTime = deltaTime * Data.standardFPS;
            unscaledDeltaTime = Time.unscaledDeltaTime;
            fpsUnscaledDeltaTime = unscaledDeltaTime * Data.standardFPS;

            SaveData.fpsLimit = SaveData.fpsLimit.Clamp(1);
            Data.notFocusFpsLimit = Data.notFocusFpsLimit.Clamp(1);
            Data.afkFpsLimit = Data.afkFpsLimit.Clamp(1);
            Data.afkTimerLimit = Data.afkTimerLimit.Clamp(0);

            //FPS Limit
            if (!isAFK && (Application.isFocused || Application.isEditor))
                Application.targetFrameRate = SaveData.fpsLimit;
            else if (!isAFK && !Application.isFocused)
                Application.targetFrameRate = Data.notFocusFpsLimit;
            else
                Application.targetFrameRate = Data.afkFpsLimit;

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

            if (mainVolume > 200)
                mainVolume = 200;
            else if (mainVolume < 0)
                mainVolume = 0;

            gameSpeed = gameSpeed.Clamp(0, 100);
            Time.timeScale = gameSpeed;
        }

        public static event Action InitialLoadStart;
        public static event Action InitialLoadEnd;
        public static event Action InitialLoadEndSceneMove;
        async void InitialLoad()
        {
            if (!isInitialLoadStart)
            {
                isInitialLoadStart = true;

                InitialLoadStart?.Invoke();

                splashScreenBackground.color = new Color(splashScreenBackground.color.r, splashScreenBackground.color.g, splashScreenBackground.color.b, 1);

                ThreadManager.Create(() => ThreadManager.ThreadAutoRemove(true), "Thread Auto Remove", true);

#if UNITY_EDITOR
                Scene scene = SceneManager.GetActiveScene();
                int index = scene.buildIndex;
                if (index != 0)
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
                    ThreadMetaData threadMetaData = ThreadManager.Create(ProjectSettingManager.Load, "Project Setting Load");
                    await UniTask.WaitUntil(() => threadMetaData.thread == null);
                    threadMetaData = ThreadManager.Create(SaveLoadManager.Load, "Save Data Load");
                    await UniTask.WaitUntil(() => threadMetaData.thread == null);
                }

                {
                    Debug.Log("Kernel: Waiting for resource to load...");
                    await ResourceManager.ResourceRefesh();
                }

                {
                    if (index == 0)
                    {
                        Debug.Log("Kernel: Waiting for scene animation...");
                        await UniTask.WaitUntil(() => !SplashScreen.isAniPlayed);
                    }
                }

                SceneManager.sceneLoaded += LoadedSceneEvent;

                {
                    isInitialLoadEnd = true;
                    InitialLoadEnd?.Invoke();

                    Debug.Log("Kernel: Initial loading finished!");
                }

#if UNITY_EDITOR
                if (index != 0)
                    SceneManager.LoadScene(index);
                else
                    SceneManager.LoadScene(index + 1);
#else
                SceneManager.LoadScene(index + 1);
#endif
                InitialLoadEndSceneMove?.Invoke();

                while (splashScreenBackground.color.a > 0)
                {
                    splashScreenBackground.color = new Color(splashScreenBackground.color.r, splashScreenBackground.color.g, splashScreenBackground.color.b, splashScreenBackground.color.a - 0.05f * fpsDeltaTime);
                    await UniTask.DelayFrame(1);
                }
            }
        }

        static void LoadedSceneEvent(Scene scene, LoadSceneMode mode) => RendererManager.AllRerender();


        public static event Action AllRefreshStart;
        public static event Action AllRefreshEnd;
        public static async void AllRefresh(bool onlyText = false)
        {
            AllRefreshStart?.Invoke();

            if (onlyText)
                RendererManager.AllTextRerender();
            else
            {
                await ResourceManager.ResourceRefesh();
                RendererManager.AllRerender();
                SoundManager.SoundRefresh();
            }

            AllRefreshEnd?.Invoke();
        }

        void OnApplicationQuit()
        {
            ThreadManager.AllThreadRemove();
            SaveLoadManager.Save();
        }
    }

    public static class KernelMethod
    {
        #region Mathf
        public static int Abs(this int value)
        {
            if (value < 0)
                return -value;
            else
                return value;
        }

        public static float Abs(this float value)
        {
            if (value < 0)
                return -value;
            else
                return value;
        }

        public static double Abs(this double value)
        {
            if (value < 0)
                return -value;
            else
                return value;
        }

        public static int Sign(this int value)
        {
            if (value < 0)
                return -1;
            else
                return 1;
        }

        public static int Sign(this float value)
        {
            if (value < 0)
                return -1;
            else
                return 1;
        }

        public static int Sign(this double value)
        {
            if (value < 0)
                return -1;
            else
                return 1;
        }

        public static int Clamp(this int value, int min, int max = int.MaxValue)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static float Clamp(this float value, float min, float max = float.PositiveInfinity)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static double Clamp(this double value, double min, double max = double.PositiveInfinity)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static int Clamp01(this int value)
        {
            if (value < 0)
                return 0;
            else if (value > 1)
                return 1;
            else
                return value;
        }

        public static float Clamp01(this float value)
        {
            if (value < 0)
                return 0;
            else if (value > 1)
                return 1;
            else
                return value;
        }

        public static double Clamp01(this double value)
        {
            if (value < 0)
                return 0;
            else if (value > 1)
                return 1;
            else
                return value;
        }

        public static int Lerp(this int current, int target, float t, bool unclamped = false)
        {
            if (!unclamped) t = t.Clamp01();
            return (int)(((1 - t) * current) + (target * t));
        }

        public static int Lerp(this int current, int target, double t, bool unclamped = false)
        {
            if (!unclamped) t = t.Clamp01();
            return (int)(((1 - t) * current) + (target * t));
        }

        public static float Lerp(this float current, float target, float t, bool unclamped = false)
        {
            if (!unclamped) t = t.Clamp01();
            return ((1 - t) * current) + (target * t);
        }

        public static double Lerp(this double current, double target, double t, bool unclamped = false)
        {
            if (!unclamped) t = t.Clamp01();
            return ((1 - t) * current) + (target * t);
        }

        public static Vector2 Lerp(this Vector2 current, Vector2 target, float t, bool unclamped = false)
        {
            if (!unclamped) t = t.Clamp01();
            return new Vector2(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t);
        }

        public static Vector3 Lerp(this Vector3 current, Vector3 target, float t, bool unclamped = false)
        {
            if (!unclamped) t = t.Clamp01();
            return new Vector3(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t, current.z + (target.z - current.z) * t);
        }

        public static Vector4 Lerp(this Vector4 current, Vector4 target, float t, bool unclamped = false)
        {
            if (!unclamped) t = t.Clamp01();
            return new Vector4(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t, current.z + (target.z - current.z) * t, current.w + (target.w - current.w) * t);
        }

        public static Rect Lerp(this Rect current, Rect target, float t, bool unclamped = false)
        {
            if (!unclamped) t = t.Clamp01();
            return new Rect(current.x + (target.x - current.x) * t, current.y + (target.y - current.y) * t, current.width + (target.width - current.width) * t, current.height + (target.height - current.height) * t);
        }

        public static Color Lerp(this Color current, Color target, float t, bool unclamped = false)
        {
            if (!unclamped) t = t.Clamp01();
            return new Color(current.r + (target.r - current.r) * t, current.g + (target.g - current.g) * t, current.b + (target.b - current.b) * t, current.b + (target.b - current.b) * t);
        }

        public static int MoveTowards(this int current, int target, int maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
                return target;

            return current + (target - current).Sign() * maxDelta;
        }

        public static float MoveTowards(this float current, float target, float maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
                return target;

            return current + (target - current).Sign() * maxDelta;
        }

        public static double MoveTowards(this double current, double target, double maxDelta)
        {
            if ((target - current).Abs() <= maxDelta)
                return target;

            return current + (target - current).Sign() * maxDelta;
        }

        public static Vector2 MoveTowards(this Vector2 current, Vector2 target, float maxDistanceDelta)
        {
            Vector2 a = target - current;
            float magnitude = a.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;

            return current + a / magnitude * maxDistanceDelta;
        }
        public static Vector3 MoveTowards(this Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            Vector3 a = target - current;
            float magnitude = a.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;

            return current + a / magnitude * maxDistanceDelta;
        }
        public static Vector4 MoveTowards(this Vector4 current, Vector4 target, float maxDistanceDelta)
        {
            Vector4 a = target - current;
            float magnitude = a.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;

            return current + a / magnitude * maxDistanceDelta;
        }
        public static Rect MoveTowards(this Rect current, Rect target, float maxDistanceDelta)
        {
            Rect a = new Rect(target.x - current.x, target.y - current.y, target.width - current.width, target.height - current.height);
            float magnitude = Mathf.Sqrt((a.x * a.x) + (a.y * a.y) + (a.width * a.width) + (a.height * a.height));
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;

            return new Rect(current.x + (a.x / magnitude * maxDistanceDelta), current.y + (a.y / magnitude * maxDistanceDelta), current.width + (a.width / magnitude * maxDistanceDelta), current.height + (a.height / magnitude * maxDistanceDelta));
        }
        public static Color MoveTowards(this Color current, Color target, float maxDistanceDelta)
        {
            Color a = target - current;
            float magnitude = Mathf.Sqrt((a.r * a.r) + (a.g * a.g) + (a.b * a.b) + (a.a * a.a));
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;
            
            return current + a / magnitude * maxDistanceDelta;
        }
        #endregion

        #region List
        public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            T temp = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, temp);
        }

        public static void Change<T>(this List<T> list, int oldIndex, int newIndex)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            T temp = list[newIndex];
            list[newIndex] = list[oldIndex];
            list[oldIndex] = temp;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static int CloseValue(this List<int> list, int target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static float CloseValue(this List<float> list, float target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="list">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static double CloseValue(this List<double> list, double target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(this List<double> list, double target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.IndexOf(list.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(this List<double> list, double target)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.BinarySearch(list.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y));

            return 0;
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                    throw new InvalidOperationException("Empty sequence");

                var comparer = Comparer<TKey>.Default;
                TSource min = sourceIterator.Current;
                TKey minKey = selector(min);

                while (sourceIterator.MoveNext())
                {
                    TSource current = sourceIterator.Current;
                    TKey currentKey = selector(current);

                    if (comparer.Compare(currentKey, minKey) >= 0)
                        continue;

                    min = current;
                    minKey = currentKey;
                }

                return min;
            }
        }
        #endregion

        #region Array
        public static T[] Add<T>(this T[] array, T item)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T[] tempArray = new T[array.Length + 1];
            
            for (int i = 0; i < array.Length; i++)
                tempArray[i] = array[i];
            tempArray[tempArray.Length - 1] = item;

            return tempArray;
        }

        public static T[] Insert<T>(this T[] array, int index, T item)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T[] tempArray = new T[array.Length + 1];
            bool insert = false;
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (i != index)
                {
                    if (!insert)
                        tempArray[i] = array[i];
                    else
                        tempArray[i] = array[i - 1];
                }
                else
                {
                    tempArray[i] = item;
                    insert = true;
                }
            }

            return tempArray;
        }

        public static T[] Remove<T>(this T[] array, T item)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T[] tempArray = new T[array.Length - 1];
            int remove = 0;
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (!item.Equals(tempArray[i]))
                    tempArray[i - remove] = array[i];
                else
                {
                    tempArray[i] = array[i + 1];
                    remove++;
                }
            }

            return tempArray;
        }

        public static T[] RemoveAt<T>(this T[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T[] tempArray = new T[array.Length - 1];
            
            bool remove = false;
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (i != index)
                {
                    if (!remove)
                        tempArray[i] = array[i];
                    else
                        tempArray[i - 1] = array[i];
                }
                else
                {
                    tempArray[i] = array[i + 1];
                    remove = true;
                }
            }

            return tempArray;
        }

        public static T[] Move<T>(this T[] array, int oldIndex, int newIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T temp = array[oldIndex];
            array = array.RemoveAt(oldIndex);
            return array.Insert(newIndex, temp);
        }

        public static T[] Change<T>(this T[] array, int oldIndex, int newIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array == null)
                throw new ArgumentNullException(nameof(array));

            T temp = array[newIndex];
            array[newIndex] = array[oldIndex];
            array[oldIndex] = temp;
            return array;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// int
        /// </returns>
        public static int CloseValue(this int[] array, int target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// float
        /// </returns>
        public static float CloseValue(this float[] array, float target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="array">
        /// 리스트
        /// </param>
        /// <param name="target">
        /// 기준
        /// </param>
        /// <returns>
        /// double
        /// </returns>
        public static double CloseValue(this double[] array, double target)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length > 0)
                return array.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y);

            return 0;
        }
        #endregion

        #region String
        /// <summary>
        /// (paths = ("asdf", "asdf")) = "asdf/asdf" (Path.Combine is "asdf\asdf")
        /// </summary>
        /// <param name="paths">경로</param>
        /// <returns></returns>
        public static string PathCombine(params string[] paths)
        {
            if (paths == null || paths.Length < 0)
                throw new NullReferenceException();

            string path = "";
            if (paths.Length == 1)
                path = paths[0];
            else
            {
                for (int i = 0; i < paths.Length - 1; i++)
                {
                    string tempPath = paths[i];
                    string tempPath2 = paths[i + 1];
                    if (tempPath == null || tempPath == "")
                        continue;
                    else if (tempPath2 == null || tempPath2 == "")
                        continue;


                    if (tempPath[tempPath.Length - 1] == '/' && tempPath2[0] == '/')
                    {
                        if (i == 0)
                            path += tempPath;
                        path += tempPath2.Substring(1, tempPath2.Length - 2);
                    }
                    else if (tempPath[tempPath.Length - 1] == '/' && tempPath2[0] != '/')
                    {
                        if (i == 0)
                            path += tempPath;
                        path += tempPath2;
                    }
                    else if (tempPath[tempPath.Length - 1] != '/' && tempPath2[0] == '/')
                    {
                        if (i == 0)
                            path += tempPath;
                        path += tempPath2;
                    }
                    else if (tempPath[tempPath.Length - 1] != '/' && tempPath2[0] != '/')
                    {
                        if (i == 0)
                            path += tempPath;
                        path += "/" + tempPath2;
                    }
                }
            }

            if (!(path == null || path == "") && path[path.Length - 1] == '/')
                path = path.Substring(1, path.Length - 2);

            return path;
        }

        public static string EnvironmentVariable(this string value)
        {
            if (value == null)
                return null;

            value = value.Replace("%DataPath%", Kernel.dataPath);
            value = value.Replace("%StreamingAssetsPath%", Kernel.streamingAssetsPath);
            value = value.Replace("%PersistentDataPath%", Kernel.persistentDataPath);

            value = value.Replace("%CompanyName%", Kernel.companyName);
            value = value.Replace("%ProductName%", Kernel.productName);
            value = value.Replace("%Version%", Kernel.version);

            value = value.Replace("%Platform%", Kernel.platform.ToString());

            return value;
        }

        /// <summary>
        /// (text = "AddSpacesToSentence") = "Add Spaces To Sentence"
        /// </summary>
        /// <param name="text">텍스트</param>
        /// <param name="preserveAcronyms">약어 보존</param>
        /// <returns></returns>
        public static string AddSpacesToSentence(this string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) || (preserveAcronyms && char.IsUpper(text[i - 1]) && i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }

        /// <summary>
        /// (keyCode = KeyCode.RightArrow) = "→"
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public static string KeyCodeToString(this KeyCode keyCode)
        {
            string text;
            if (keyCode == KeyCode.Escape)
                text = "None";
            else if (keyCode == KeyCode.Return)
                text = "␣";
            else if (keyCode == KeyCode.Alpha0)
                text = "0";
            else if (keyCode == KeyCode.Alpha1)
                text = "1";
            else if (keyCode == KeyCode.Alpha2)
                text = "2";
            else if (keyCode == KeyCode.Alpha3)
                text = "3";
            else if (keyCode == KeyCode.Alpha4)
                text = "4";
            else if (keyCode == KeyCode.Alpha5)
                text = "5";
            else if (keyCode == KeyCode.Alpha6)
                text = "6";
            else if (keyCode == KeyCode.Alpha7)
                text = "7";
            else if (keyCode == KeyCode.Alpha8)
                text = "8";
            else if (keyCode == KeyCode.Alpha9)
                text = "9";
            else if (keyCode == KeyCode.AltGr)
                text = "AG";
            else if (keyCode == KeyCode.Ampersand)
                text = "&";
            else if (keyCode == KeyCode.Asterisk)
                text = "*";
            else if (keyCode == KeyCode.At)
                text = "@";
            else if (keyCode == KeyCode.BackQuote)
                text = "`";
            else if (keyCode == KeyCode.Backslash)
                text = "\\";
            else if (keyCode == KeyCode.Caret)
                text = "^";
            else if (keyCode == KeyCode.Colon)
                text = ":";
            else if (keyCode == KeyCode.Comma)
                text = ",";
            else if (keyCode == KeyCode.Dollar)
                text = "$";
            else if (keyCode == KeyCode.DoubleQuote)
                text = "\"";
            else if (keyCode == KeyCode.Equals)
                text = "=";
            else if (keyCode == KeyCode.Exclaim)
                text = "!";
            else if (keyCode == KeyCode.Greater)
                text = ">";
            else if (keyCode == KeyCode.Hash)
                text = "#";
            else if (keyCode == KeyCode.Keypad0)
                text = "0";
            else if (keyCode == KeyCode.Keypad1)
                text = "1";
            else if (keyCode == KeyCode.Keypad2)
                text = "2";
            else if (keyCode == KeyCode.Keypad3)
                text = "3";
            else if (keyCode == KeyCode.Keypad4)
                text = "4";
            else if (keyCode == KeyCode.Keypad5)
                text = "5";
            else if (keyCode == KeyCode.Keypad6)
                text = "6";
            else if (keyCode == KeyCode.Keypad7)
                text = "7";
            else if (keyCode == KeyCode.Keypad8)
                text = "8";
            else if (keyCode == KeyCode.Keypad9)
                text = "9";
            else if (keyCode == KeyCode.KeypadDivide)
                text = "/";
            else if (keyCode == KeyCode.KeypadEnter)
                text = "↵";
            else if (keyCode == KeyCode.KeypadEquals)
                text = "=";
            else if (keyCode == KeyCode.KeypadMinus)
                text = "-";
            else if (keyCode == KeyCode.KeypadMultiply)
                text = "*";
            else if (keyCode == KeyCode.KeypadPeriod)
                text = ".";
            else if (keyCode == KeyCode.KeypadPlus)
                text = "+";
            else if (keyCode == KeyCode.LeftApple)
                text = "Left Command";
            else if (keyCode == KeyCode.LeftBracket)
                text = "[";
            else if (keyCode == KeyCode.LeftCurlyBracket)
                text = "{";
            else if (keyCode == KeyCode.LeftParen)
                text = "(";
            else if (keyCode == KeyCode.Less)
                text = "<";
            else if (keyCode == KeyCode.Minus)
                text = "-";
            else if (keyCode == KeyCode.Mouse0)
                text = "LM";
            else if (keyCode == KeyCode.Mouse1)
                text = "RM";
            else if (keyCode == KeyCode.Mouse2)
                text = "MM";
            else if (keyCode == KeyCode.Mouse3)
                text = "3M";
            else if (keyCode == KeyCode.Mouse4)
                text = "4M";
            else if (keyCode == KeyCode.Mouse5)
                text = "5M";
            else if (keyCode == KeyCode.Mouse6)
                text = "6M";
            else if (keyCode == KeyCode.Percent)
                text = "%";
            else if (keyCode == KeyCode.Period)
                text = ".";
            else if (keyCode == KeyCode.Pipe)
                text = "|";
            else if (keyCode == KeyCode.Plus)
                text = "+";
            else if (keyCode == KeyCode.Question)
                text = "?";
            else if (keyCode == KeyCode.Quote)
                text = "'";
            else if (keyCode == KeyCode.RightApple)
                text = "Right Command";
            else if (keyCode == KeyCode.RightBracket)
                text = "]";
            else if (keyCode == KeyCode.RightCurlyBracket)
                text = "}";
            else if (keyCode == KeyCode.RightParen)
                text = ")";
            else if (keyCode == KeyCode.Semicolon)
                text = ";";
            else if (keyCode == KeyCode.Slash)
                text = "/";
            else if (keyCode == KeyCode.SysReq)
                text = "Print Screen";
            else if (keyCode == KeyCode.Tilde)
                text = "~";
            else if (keyCode == KeyCode.Underscore)
                text = "_";
            else if (keyCode == KeyCode.UpArrow)
                text = "↑";
            else if (keyCode == KeyCode.DownArrow)
                text = "↓";
            else if (keyCode == KeyCode.LeftArrow)
                text = "←";
            else if (keyCode == KeyCode.RightArrow)
                text = "→";
            else if (keyCode == KeyCode.LeftControl)
                text = "LC";
            else if (keyCode == KeyCode.RightControl)
                text = "RC";
            else if (keyCode == KeyCode.LeftAlt)
                text = "LA";
            else if (keyCode == KeyCode.RightAlt)
                text = "RA";
            else if (keyCode == KeyCode.LeftShift)
                text = "L⇧";
            else if (keyCode == KeyCode.RightShift)
                text = "R⇧";
            else if (keyCode == KeyCode.Backspace)
                text = "B←";
            else if (keyCode == KeyCode.Delete)
                text = "D←";
            else if (keyCode == KeyCode.PageUp)
                text = "P↑";
            else if (keyCode == KeyCode.PageDown)
                text = "P↓";
            else
                text = keyCode.ToString();

            return text.AddSpacesToSentence();
        }

        /// <summary>
        /// (second = 70) = "1:10"
        /// </summary>
        /// <param name="second">초</param>
        /// <param name="minuteAlwayShow">분 단위 항상 표시</param>
        /// <param name="hourAlwayShow">시간, 분 단위 항상 표시</param>
        /// <param name="dayAlwayShow">하루, 시간, 분 단위 항상 표시</param>
        /// <returns></returns>
        public static string ToTime(this int second, bool minuteAlwayShow = false, bool hourAlwayShow = false, bool dayAlwayShow = false)
        {
            try
            {
                int secondAbs = second.Abs();
                if (second <= TimeSpan.MinValue.TotalSeconds)
                    return TimeSpan.MinValue.ToString(@"d\:hh\:mm\:ss");
                else if (second > TimeSpan.MaxValue.TotalSeconds)
                    return TimeSpan.MaxValue.ToString(@"d\:h\:mm\:ss");
                else if (secondAbs >= 86400 || dayAlwayShow)
                    return TimeSpan.FromSeconds(second).ToString(@"d\:hh\:mm\:ss");
                else if (secondAbs >= 3600 || hourAlwayShow)
                    return TimeSpan.FromSeconds(second).ToString(@"h\:mm\:ss");
                else if (secondAbs >= 60 || minuteAlwayShow)
                    return TimeSpan.FromSeconds(second).ToString(@"m\:ss");
                else
                    return TimeSpan.FromSeconds(second).ToString(@"s");
            }
            catch (Exception) { return "--:--"; }
        }

        /// <summary>
        /// (second = 70.1f) = "1:10.1"
        /// </summary>
        /// <param name="second">초</param>
        /// <param name="decimalShow">소수 표시</param>
        /// <param name="minuteAlwayShow">분 단위 항상 표시</param>
        /// <param name="hourAlwayShow">시간, 분 단위 항상 표시</param>
        /// <param name="dayAlwayShow">하루, 시간, 분 단위 항상 표시</param>
        /// <returns></returns>
        public static string ToTime(this float second, bool decimalShow = true, bool minuteAlwayShow = false, bool hourAlwayShow = false, bool dayAlwayShow = false) => ToTime(second, decimalShow, minuteAlwayShow, hourAlwayShow, dayAlwayShow);

        /// <summary>
        /// (second = 70.1) = "1:10.1"
        /// </summary>
        /// <param name="second">초</param>
        /// <param name="decimalShow">소수 표시</param>
        /// <param name="minuteAlwayShow">분 단위 항상 표시</param>
        /// <param name="hourAlwayShow">시간, 분 단위 항상 표시</param>
        /// <param name="dayAlwayShow">하루, 시간, 분 단위 항상 표시</param>
        /// <returns></returns>
        public static string ToTime(this double second, bool decimalShow = true, bool minuteAlwayShow = false, bool hourAlwayShow = false, bool dayAlwayShow = false)
        {
            try
            {
                double secondAbs = second.Abs();
                if (decimalShow)
                {
                    if (second <= TimeSpan.MinValue.TotalSeconds || secondAbs == float.NegativeInfinity)
                        return TimeSpan.MinValue.ToString(@"d\:hh\:mm\:ss\.ff");
                    else if (second > TimeSpan.MaxValue.TotalSeconds || secondAbs == float.PositiveInfinity)
                        return TimeSpan.FromSeconds(TimeSpan.MaxValue.TotalSeconds).ToString(@"d\:hh\:mm\:ss\.ff");
                    else if (secondAbs >= 86400 || dayAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"d\:hh\:mm\:ss\.ff");
                    else if (secondAbs >= 3600 || hourAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"h\:mm\:ss\.ff");
                    else if (secondAbs >= 60 || minuteAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"m\:ss\.ff");
                    else
                        return TimeSpan.FromSeconds(second).ToString(@"s\.ff");
                }
                else
                {
                    if (second <= TimeSpan.MinValue.TotalSeconds || secondAbs == float.NegativeInfinity)
                        return TimeSpan.MinValue.ToString(@"d\:hh\:mm\:ss");
                    else if (second > TimeSpan.MaxValue.TotalSeconds || secondAbs == float.PositiveInfinity)
                        return TimeSpan.FromSeconds(TimeSpan.MaxValue.TotalSeconds).ToString(@"d\:h\:mm\:ss");
                    else if (secondAbs >= 86400 || dayAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"d\:hh\:mm\:ss");
                    else if (secondAbs >= 3600 || hourAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"h\:mm\:ss");
                    else if (secondAbs >= 60 || minuteAlwayShow)
                        return TimeSpan.FromSeconds(second).ToString(@"m\:ss");
                    else
                        return TimeSpan.FromSeconds(second).ToString(@"s");
                }
            }
            catch (Exception) { return "--:--"; }
        }



        /// <summary>
        /// (value = 5, max = 10, length = 10) = "■■■■■□□□□□"
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string IntToBar(this int value, int max, int length, string fill = "■", string half = "▣", string empty = "□") => DoubleToBar(value, max, length, fill, half, empty);

        /// <summary>
        /// (value = 5.5, max = 10, length = 10) = "■■■■■▣□□□□"
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string FloatToBar(this float value, float max, int length, string fill = "■", string half = "▣", string empty = "□") => DoubleToBar(value, max, length, fill, half, empty);

        /// <summary>
        /// (value = 5.5, max = 10, length = 10) = "■■■■■▣□□□□"
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string DoubleToBar(this double value, double max, int length, string fill = "■", string half = "▣", string empty = "□")
        {
            if (fill == null)
                fill = "";
            if (half == null)
                half = "";
            if (empty == null)
                empty = "";

            string text = "";

            for (double i = 0.5; i < length + 0.5; i++)
            {
                if (value / max >= i / length)
                    text += fill;
                else
                {
                    if (value / max >= (i - 0.5) / length)
                        text += half;
                    else
                        text += empty;
                }
            }
            return text;
        }
        #endregion
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