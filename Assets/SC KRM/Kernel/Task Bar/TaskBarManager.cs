using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.Input;
using SCKRM.SaveLoad;
using SCKRM.Tool;
using SCKRM.UI.SideBar;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.UI.TaskBar
{
    [AddComponentMenu(""), RequireComponent(typeof(RectTransform)), RequireComponent(typeof(Image))]
    public sealed class TaskBarManager : MonoBehaviour
    {
        [SaveLoad("task_bar")]
        public sealed class SaveData
        {
            [JsonProperty] public static bool bottomMode { get; set; } = false;
        }

        public static TaskBarManager instance { get; private set; }
        public static bool allowTaskBarShow { get; set; } = false;
        public static bool backButtonShow { get; set; } = true;
        public static bool isTaskBarShow { get; private set; } = true;

        static bool _cropTheScreen = true; public static bool cropTheScreen
        {
            get => _cropTheScreen;
            set
            {
                if (allowTaskBarShow)
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
        [SerializeField] Image _background; public Image background => _background;
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

        static bool tabAllow = false;
        static GameObject oldSelectedObject;
        static bool tempTopMode;
        static bool tempCropTheScreen;
        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                {
                    bool selectedTaskBar = eventSystem.currentSelectedGameObject?.GetComponentInParent<KernelCanvas>() != null || SideBarManager.isSideBarShow || SettingBarManager.isSettingBarShow;
                    isTaskBarShow = allowTaskBarShow || selectedTaskBar;
                    tabAllow = oldSelectedObject == null || !oldSelectedObject.activeInHierarchy || oldSelectedObject.GetComponentInParent<KernelCanvas>() == null;

                    InputManager.SetInputLock("taskbar", selectedTaskBar);

                    if (selectedTaskBar)
                    {
                        oldSelectedObject = eventSystem.currentSelectedGameObject;

                        if (!background.gameObject.activeSelf)
                            background.gameObject.SetActive(true);

                        background.color = background.color.Lerp(new Color(0, 0, 0, 0.5f), 0.2f * Kernel.fpsDeltaTime);
                    }
                    else
                    {
                        if (background.gameObject.activeSelf)
                        {
                            if (background.color.a <= 0.01f)
                            {
                                background.color = new Color(0, 0, 0, 0);
                                background.gameObject.SetActive(false);
                            }
                            else
                                background.color = background.color.Lerp(Color.clear, 0.2f * Kernel.fpsDeltaTime);
                        }
                    }
                    
                    if ((!selectedTaskBar || (selectedTaskBar && tabAllow)) && InputManager.GetKeyDown("gui.tab", "all"))
                        Tab();
                    else if (selectedTaskBar && InputManager.GetKeyDown("gui.back", "all"))
                        eventSystem.SetSelectedGameObject(null);

                    if (InputManager.GetKeyDown("sidebar_manager.show", "all"))
                        SideBarToggle();

                    if (InputManager.GetKeyDown("setting_manager.show", "all"))
                        SettingBarToggle();
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

                    if (SideBarManager.isSideBarShow)
                    {
                        GameObject gameObject = SideBarManager.instance.gameObject;

                        if (!gameObject.activeSelf)
                            gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject gameObject = SideBarManager.instance.gameObject;
                        RectTransform rectTransform = SideBarManager.instance.rectTransform;

                        if (gameObject.activeSelf && rectTransform.anchoredPosition.x >= rectTransform.sizeDelta.x - 0.01f)
                            gameObject.SetActive(false);
                    }

                    if (SettingBarManager.isSettingBarShow)
                    {
                        GameObject gameObject = SettingBarManager.instance.gameObject;

                        if (!gameObject.activeSelf)
                            gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject gameObject = SettingBarManager.instance.gameObject;
                        RectTransform rectTransform = SettingBarManager.instance.rectTransform;

                        if (gameObject.activeSelf && rectTransform.anchoredPosition.x <= (-rectTransform.sizeDelta.x) + 0.01f)
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
                    if (allowTaskBarShow)
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
            if (SettingBarManager.isSettingBarShow)
            {
                SettingBarManager.isSettingBarShow = false;
                Tab();
            }
            else if (SideBarManager.isSideBarShow)
            {
                SideBarManager.isSideBarShow = false;
                Tab();
            }
            else
                eventSystem.SetSelectedGameObject(null);
        }

        public void SideBarToggle()
        {
            SideBarManager.isSideBarShow = !SideBarManager.isSideBarShow;
            SettingBarManager.isSettingBarShow = false;
        }

        public void SettingBarToggle()
        {
            SettingBarManager.isSettingBarShow = !SettingBarManager.isSettingBarShow;
            SideBarManager.isSideBarShow = false;
        }

        public static void Tab()
        {
            if (!instance.layout.activeSelf)
                instance.layout.SetActive(true);
            
            if (tabAllow)
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