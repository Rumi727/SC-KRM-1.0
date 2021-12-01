using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.Input;
using SCKRM.SaveLoad;
using SCKRM.Tool;
using SCKRM.UI.NoticeBar;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.UI.TaskBar
{
    [AddComponentMenu(""), RequireComponent(typeof(RectTransform)), RequireComponent(typeof(Image))]
    public sealed class TaskBarManager : MonoBehaviour
    {
        [SaveLoad("Task Bar")]
        public sealed class SaveData
        {
            [JsonProperty] public static bool bottomMode { get; set; } = false;
        }

        public static TaskBarManager instance { get; private set; }
        public static bool taskBarShow { get; set; } = true;
        public static bool backButtonShow { get; set; } = true;
        public static bool isTaskBarShow { get; private set; } = true;

        static bool _cropTheScreen = true; public static bool cropTheScreen
        {
            get => _cropTheScreen;
            set
            {
                if (taskBarShow)
                    return;

                _cropTheScreen = value;
            }
        }


        [SerializeField, HideInInspector] RectTransform _rectTransform; public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();

                return _rectTransform;
            }
        }
        [SerializeField, HideInInspector] Image _image; public Image image
        {
            get
            {
                if (_image == null)
                    _image = GetComponent<Image>();

                return _image;
            }
        }

        [SerializeField] EventSystem _eventSystem; public EventSystem eventSystem => _eventSystem;
        [SerializeField] GameObject _background; public GameObject background => _background;
        [SerializeField] GameObject _backButton; public GameObject backButton => _backButton;
        [SerializeField] GameObject _layout; public GameObject layout => _layout;


        [SerializeField] Sprite bg;
        [SerializeField] Sprite bg2;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        static GameObject oldSelectedObject;
        static bool tempTopMode;
        static bool tempCropTheScreen;
        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                {
                    bool selectedTaskBar = eventSystem.currentSelectedGameObject?.GetComponentInParent<KernelCanvas>() != null || NoticeBarManager.isNoticeBarShow;
                    isTaskBarShow = taskBarShow || selectedTaskBar;

                    InputManager.SetInputLock("taskbar", selectedTaskBar);

                    if (selectedTaskBar)
                    {
                        oldSelectedObject = eventSystem.currentSelectedGameObject;

                        if (!background.activeSelf)
                            background.SetActive(true);
                    }
                    else
                    {
                        if (background.activeSelf)
                            background.SetActive(false);
                    }

                    if (!selectedTaskBar && InputManager.GetKeyDown("gui.tab", "taskbar"))
                        Tab();
                    else if (selectedTaskBar && InputManager.GetKeyDown("gui.back", "taskbar"))
                        eventSystem.SetSelectedGameObject(null);
                }



                {
                    if (isTaskBarShow)
                    {
                        rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(Vector2.zero, 0.2f * Kernel.fpsDeltaTime);

                        if (!layout.activeSelf)
                            layout.SetActive(true);
                    }
                    else
                    {
                        if (!SaveData.bottomMode)
                        {
                            rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(0, rectTransform.sizeDelta.y), 0.2f * Kernel.fpsDeltaTime);

                            if (rectTransform.anchoredPosition.y >= rectTransform.sizeDelta.y - 0.01f)
                            {
                                if (layout.activeSelf)
                                    layout.SetActive(false);
                            }
                        }
                        else
                        {
                            rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(0, -rectTransform.sizeDelta.y), 0.2f * Kernel.fpsDeltaTime);

                            if (rectTransform.anchoredPosition.y <= -rectTransform.sizeDelta.y + 0.01f)
                            {
                                if (layout.activeSelf)
                                    layout.SetActive(false);
                            }
                        }
                    }

                    if (NoticeBarManager.isNoticeBarShow)
                    {
                        GameObject gameObject = NoticeBarManager.instance.gameObject;

                        if (!gameObject.activeSelf)
                            gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject gameObject = NoticeBarManager.instance.gameObject;
                        RectTransform rectTransform = NoticeBarManager.instance.rectTransform;

                        if (gameObject.activeSelf && rectTransform.anchoredPosition.x >= rectTransform.sizeDelta.x - 0.01f)
                            gameObject.SetActive(false);
                    }

                    if (backButtonShow)
                    {
                        if (!backButton.activeSelf)
                            backButton.SetActive(true);
                    }
                    else
                    {
                        if (backButton.activeSelf)
                            backButton.SetActive(false);
                    }
                }



                {
                    if (taskBarShow)
                        cropTheScreen = true;

                    if (tempCropTheScreen != cropTheScreen)
                    {
                        if (!cropTheScreen)
                            tempTopMode = !SaveData.bottomMode;
                        else
                            image.sprite = null;

                        tempCropTheScreen = cropTheScreen;
                    }

                    if (tempTopMode != SaveData.bottomMode)
                    {
                        if (!SaveData.bottomMode)
                        {
                            rectTransform.anchorMin = Vector2.up;
                            rectTransform.anchorMax = Vector2.up;
                            rectTransform.pivot = Vector2.up;

                            if (!cropTheScreen)
                                image.sprite = bg;
                            else
                                image.sprite = null;

                            if (!isTaskBarShow)
                                rectTransform.anchoredPosition = new Vector2(0, rectTransform.sizeDelta.y);
                        }
                        else
                        {
                            rectTransform.anchorMin = Vector2.zero;
                            rectTransform.anchorMax = Vector2.zero;
                            rectTransform.pivot = Vector2.zero;

                            if (!cropTheScreen)
                                image.sprite = bg2;
                            else
                                image.sprite = null;

                            if (!isTaskBarShow)
                                rectTransform.anchoredPosition = new Vector2(0, -rectTransform.sizeDelta.y);
                        }
                        tempTopMode = SaveData.bottomMode;
                    }
                }
            }
        }

        public void Hide()
        {
            if (NoticeBarManager.isNoticeBarShow)
            {
                NoticeBarManager.isNoticeBarShow = false;
                Tab();
            }
            else
                eventSystem.SetSelectedGameObject(null);
        }

        public void NoticeBarToggle() => NoticeBarManager.isNoticeBarShow = !NoticeBarManager.isNoticeBarShow;

        public static void Tab()
        {
            if (!instance.layout.activeSelf)
                instance.layout.SetActive(true);

            if (oldSelectedObject == null || !oldSelectedObject.activeSelf)
            {
                Selectable[] selectables = instance.GetComponentsInChildren<Selectable>();
                if (selectables.Length > 0)
                    instance.eventSystem.SetSelectedGameObject(selectables[0].gameObject);
                else
                    instance.eventSystem.SetSelectedGameObject(instance.gameObject);
            }
            else
                instance.eventSystem.SetSelectedGameObject(oldSelectedObject);
        }
    }
}