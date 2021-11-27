using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.Input;
using SCKRM.SaveLoad;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.UI.TaskBar
{
    [AddComponentMenu("")]
    public sealed class TaskBarManager : MonoBehaviour
    {
        [SaveLoad("Task Bar")]
        public sealed class SaveData
        {
            [JsonProperty] public static bool topMode { get; set; } = true;
        }

        public static TaskBarManager instance { get; private set; }
        public static bool taskBarShow { get; set; } = true;
        public static bool isTaskBarShow { get; private set; } = true;

        static bool _cropTheScreen = true;
        public static bool cropTheScreen
        {
            get => _cropTheScreen;
            set
            {
                if (taskBarShow)
                    return;

                _cropTheScreen = value;
            }
        }


        [SerializeField] RectTransform _rectTransform;
        public RectTransform rectTransform => _rectTransform;
        [SerializeField] EventSystem _eventSystem;
        public EventSystem eventSystem => _eventSystem;

        [SerializeField] Image _image;
        public Image image => _image;
        [SerializeField] Sprite bg;
        [SerializeField] Sprite bg2;

        void OnEnable()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        static GameObject oldSelectedObject;
        static bool tempTopMode;
        static bool tempCropTheScreen;
        void Update()
        {
            bool selectedTaskBar = eventSystem.currentSelectedGameObject?.GetComponentInParent<TaskBarManager>() != null;
            isTaskBarShow = Kernel.isInitialLoadEnd && (taskBarShow || selectedTaskBar);

            if (Kernel.isInitialLoadEnd)
                InputManager.SetInputLock("taskbar", isTaskBarShow);

            if (selectedTaskBar)
                oldSelectedObject = eventSystem.currentSelectedGameObject;

            if (isTaskBarShow)
                rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(Vector2.zero, 0.2f * Kernel.fpsDeltaTime);
            else
            {
                if (!SaveData.topMode)
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(0, rectTransform.sizeDelta.y), 0.2f * Kernel.fpsDeltaTime);
                else
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(0, -rectTransform.sizeDelta.y), 0.2f * Kernel.fpsDeltaTime);
            }

            if (Kernel.isInitialLoadEnd && !selectedTaskBar && InputManager.GetKeyDown("gui.tab", "taskbar"))
            {
                if (oldSelectedObject == null || !oldSelectedObject.activeSelf)
                {
                    Transform[] transforms = GetComponentsInChildren<Transform>();
                    if (transforms.Length > 1)
                        eventSystem.SetSelectedGameObject(transforms[1].gameObject);
                    else
                        eventSystem.SetSelectedGameObject(gameObject);
                }
                else
                    eventSystem.SetSelectedGameObject(oldSelectedObject);
            }
            else if (Kernel.isInitialLoadEnd && selectedTaskBar && InputManager.GetKeyDown("gui.back", "taskbar"))
                eventSystem.SetSelectedGameObject(null);

            if (taskBarShow)
                cropTheScreen = true;

            if (tempCropTheScreen != cropTheScreen)
            {
                if (!cropTheScreen)
                    tempTopMode = !SaveData.topMode;
                else
                    image.sprite = null;

                tempCropTheScreen = cropTheScreen;
            }

            if (tempTopMode != SaveData.topMode)
            {
                if (!SaveData.topMode)
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
                tempTopMode = SaveData.topMode;
            }
        }
    }
}