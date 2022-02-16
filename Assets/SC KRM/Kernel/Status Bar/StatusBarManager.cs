using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.Input;
using SCKRM.SaveLoad;
using SCKRM.Tool;
using SCKRM.UI.SideBar;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.UI.StatusBar
{
    [AddComponentMenu(""), RequireComponent(typeof(RectTransform)), RequireComponent(typeof(Image))]
    public sealed class StatusBarManager : UI, IPointerEnterHandler, IPointerExitHandler
    {
        [SaveLoad("statusbar")]
        public sealed class SaveData
        {
            [JsonProperty] public static bool bottomMode { get; set; } = false;
            [JsonProperty] public static bool twentyFourHourSystem { get; set; } = false;
            [JsonProperty] public static bool toggleSeconds { get; set; } = false;
        }

        public static StatusBarManager instance { get; private set; }
        public static bool allowStatusBarShow { get; set; } = false;
        public static bool backButtonShow { get; set; } = true;

        public static bool selectedStatusBar { get; private set; } = false;
        public static bool isStatusBarShow { get; private set; } = true;

        static bool _cropTheScreen = true; public static bool cropTheScreen
        {
            get => _cropTheScreen;
            set
            {
                if (allowStatusBarShow)
                    return;

                _cropTheScreen = value;
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
        static bool tempSelectedStatusBar;
        static bool pointer = false;
        static float timer = 0;
        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                {
                    bool mouseYisScreenY;
                    if (SaveData.bottomMode)
                        mouseYisScreenY = InputManager.mousePosition.y <= 1;
                    else
                        mouseYisScreenY = InputManager.mousePosition.y >= (Screen.height - 1);

                    selectedStatusBar = pointer || mouseYisScreenY || SideBarManager.isSideBarShow || SettingBarManager.isSettingBarShow || eventSystem.currentSelectedGameObject?.GetComponentInParent<KernelCanvas>() != null;
                    bool statusBarShow = selectedStatusBar || timer > 0;
                    isStatusBarShow = allowStatusBarShow || statusBarShow;
                    tabAllow = oldSelectedObject == null || !oldSelectedObject.activeInHierarchy || oldSelectedObject.GetComponentInParent<KernelCanvas>() == null;

                    if (selectedStatusBar)
                        timer = 1;
                    else
                        timer -= Kernel.unscaledDeltaTime;

                    if (tempSelectedStatusBar != selectedStatusBar)
                        InputManager.SetInputLock("statusbar", selectedStatusBar);

                    tempSelectedStatusBar = selectedStatusBar;

                    if (selectedStatusBar)
                    {
                        oldSelectedObject = eventSystem.currentSelectedGameObject;

                        if (!background.gameObject.activeSelf)
                            background.gameObject.SetActive(true);

                        background.color = background.color.Lerp(new Color(0, 0, 0, 0.5f), 0.2f * Kernel.fpsUnscaledDeltaTime);
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
                                background.color = background.color.Lerp(Color.clear, 0.2f * Kernel.fpsUnscaledDeltaTime);
                        }
                    }
                    
                    if ((!selectedStatusBar || (statusBarShow && tabAllow)) && (InputManager.GetKeyDown("gui.tab", "all") || InputManager.GetKeyDown("gui.tab2", "all")))
                        Tab();
                    else if (selectedStatusBar && InputManager.GetKeyDown("gui.back", "all"))
                        eventSystem.SetSelectedGameObject(null);

                    if (InputManager.GetKeyDown("sidebar_manager.show", "all"))
                        SideBarToggle();

                    if (InputManager.GetKeyDown("setting_manager.show", "all"))
                        SettingBarToggle();
                }



                {
                    if (isStatusBarShow)
                    {
                        rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(Vector2.zero, 0.2f * Kernel.fpsUnscaledDeltaTime);

                        if (!layout.activeSelf)
                            layout.SetActive(true);
                    }
                    else
                    {
                        if (!SaveData.bottomMode)
                        {
                            rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(0, rectTransform.sizeDelta.y), 0.2f * Kernel.fpsUnscaledDeltaTime);

                            if (rectTransform.anchoredPosition.y >= rectTransform.sizeDelta.y - 0.01f)
                            {
                                if (layout.activeSelf)
                                    layout.SetActive(false);
                            }
                        }
                        else
                        {
                            rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(0, -rectTransform.sizeDelta.y), 0.2f * Kernel.fpsUnscaledDeltaTime);

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
                    if (allowStatusBarShow)
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
                            rectTransform.anchorMax = Vector2.one;
                            rectTransform.pivot = Vector2.up;

                            if (!cropTheScreen)
                                image.sprite = bg;
                            else
                                image.sprite = null;

                            if (!isStatusBarShow)
                                rectTransform.anchoredPosition = new Vector2(0, rectTransform.sizeDelta.y);
                        }
                        else
                        {
                            rectTransform.anchorMin = Vector2.zero;
                            rectTransform.anchorMax = Vector2.right;
                            rectTransform.pivot = Vector2.zero;

                            if (!cropTheScreen)
                                image.sprite = bg2;
                            else
                                image.sprite = null;

                            if (!isStatusBarShow)
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

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => pointer = true;
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => pointer = false;
    }
}