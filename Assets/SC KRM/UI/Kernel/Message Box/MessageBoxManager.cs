using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Object;
using SCKRM.Renderer;
using SCKRM.Threads;
using SCKRM.UI.StatusBar;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.UI.MessageBox
{
    public class MessageBoxManager : UIManager<MessageBoxManager>
    {
        [SerializeField] CanvasGroup messageBoxCanvasGroup;
        [SerializeField] Transform messageBoxButtons;
        [SerializeField] CustomAllTextRenderer messageBoxInfo;
        [SerializeField] CustomAllSpriteRenderer messageBoxIcon;

        static MessageBoxButton[] createdMessageBoxButton = new MessageBoxButton[0];
        public static bool isMessageBoxShow { get; private set; } = false;



        protected override void Awake() => SingletonCheck(this);

        void Update()
        {
            if (isMessageBoxShow)
            {
                messageBoxCanvasGroup.alpha = messageBoxCanvasGroup.alpha.Lerp(1, 0.2f * Kernel.fpsDeltaTime);
                if (messageBoxCanvasGroup.alpha > 0.99f)
                    messageBoxCanvasGroup.alpha = 1;

                messageBoxCanvasGroup.interactable = true;
                messageBoxCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                messageBoxCanvasGroup.alpha = messageBoxCanvasGroup.alpha.Lerp(0, 0.2f * Kernel.fpsDeltaTime);
                if (messageBoxCanvasGroup.alpha < 0.01f)
                    messageBoxCanvasGroup.alpha = 0;

                messageBoxCanvasGroup.interactable = false;
                messageBoxCanvasGroup.blocksRaycasts = false;
            }
        }



        public static async UniTask<int> Show(NameSpacePathReplacePair button, int defaultIndex, NameSpacePathReplacePair info, NameSpaceIndexTypePathPair icon) => await show(new NameSpacePathReplacePair[] { button }, defaultIndex, info, icon);
        public static async UniTask<int> Show(NameSpacePathReplacePair[] buttons, int defaultIndex, NameSpacePathReplacePair info, NameSpaceIndexTypePathPair icon) => await show(buttons, defaultIndex, info, icon);

        static async UniTask<int> show(NameSpacePathReplacePair[] buttons, int defaultIndex, NameSpacePathReplacePair info, NameSpaceIndexTypePathPair icon)
        {
            if (!InitialLoadManager.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException();
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException();
            if (!Kernel.isPlaying)
                throw new NotPlayModeMethodException();

            await UniTask.WaitUntil(() => instance != null);

            if (isMessageBoxShow)
                return defaultIndex;
            else if (defaultIndex < 0 || buttons.Length < defaultIndex)
                return defaultIndex;

            isMessageBoxShow = true;

            instance.messageBoxIcon.nameSpaceIndexTypePathPair = icon;
            instance.messageBoxIcon.Refresh();

            instance.messageBoxInfo.nameSpacePathReplacePair = info;
            instance.messageBoxInfo.Refresh();

            #region Button Object Create
            for (int i = 0; i < createdMessageBoxButton.Length; i++)
                createdMessageBoxButton[i].Remove();

            createdMessageBoxButton = new MessageBoxButton[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                MessageBoxButton button = (MessageBoxButton)ObjectPoolingSystem.ObjectCreate("window_manager.message_box_button", instance.messageBoxButtons).monoBehaviour;
                createdMessageBoxButton[i] = button;

                button.index = i;

                button.text.nameSpacePathReplacePair = buttons[i];
                button.text.Refresh();

                //버튼이 눌렸을때, UI를 닫기 위해서 버튼에 있는 OnClick 이벤트에 clickedIndex를 button.index로 바꾸는 action 메소드를 추가합니다
                button.button.onClick.AddListener(() => action(button));
            }

            for (int i = 0; i < createdMessageBoxButton.Length; i++)
            {
                Navigation navigation = new Navigation();
                navigation.mode = Navigation.Mode.Explicit;

                if (i > 0)
                    navigation.selectOnUp = createdMessageBoxButton[i - 1].button;
                if (i < createdMessageBoxButton.Length - 1)
                    navigation.selectOnDown = createdMessageBoxButton[i + 1].button;

                createdMessageBoxButton[i].button.navigation = navigation;
            }
            #endregion

            StatusBarManager.tabSelectGameObject = createdMessageBoxButton[defaultIndex].gameObject;

            GameObject previouslySelectedGameObject = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(createdMessageBoxButton[defaultIndex].gameObject);

            InputManager.forceInputLock = true;



            int clickedIndex = -1;

            //Back 버튼을 눌렀을때, UI를 닫기 위해 이벤트에 clickedIndex를 defaultIndex로 변경하는 BackEvent 메소드를 추가합니다
            UIManager.BackEventAdd(backEvent, true);

            GameObject oldSelectedGameObject = null;
            while (clickedIndex < 0)
            {
                /*
                 * BackEvent 함수가 호출되거나, 버튼이 눌려서 clickedIndex가 0 이상이 되거나
                 * select 함수가 호출되서 함수가 리턴 될 때까지 대기합니다
                 */

                if (EventSystem.current.currentSelectedGameObject == null)
                    EventSystem.current.SetSelectedGameObject(oldSelectedGameObject);
                else
                    oldSelectedGameObject = EventSystem.current.currentSelectedGameObject;

                if (await UniTask.DelayFrame(1, PlayerLoopTiming.Update, AsyncTaskManager.cancelToken).SuppressCancellationThrow())
                    return 0;
            }

            return select(clickedIndex);



            int select(int index)
            {
                isMessageBoxShow = false;
                StatusBarManager.tabSelectGameObject = null;
                EventSystem.current.SetSelectedGameObject(previouslySelectedGameObject);

                InputManager.forceInputLock = false;

                UIManager.BackEventRemove(backEvent, true);

                return index;
            }

            void backEvent() => clickedIndex = defaultIndex;
            void action(MessageBoxButton messageBoxButton) => clickedIndex = messageBoxButton.index;
        }
    }
}
