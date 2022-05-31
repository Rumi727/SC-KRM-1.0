using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Object;
using SCKRM.Renderer;
using SCKRM.Threads;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SCKRM.UI.SideBar
{
    [AddComponentMenu("")]
    public sealed class NoticeManager : ManagerUI<NoticeManager>
    {
        [SerializeField] Transform _noticeListTransform; public Transform noticeListTransform => _noticeListTransform;
        [SerializeField] SideBarAni noticeBar;
        [SerializeField] UnityEvent _noticeAdd;

        public static List<Notice> noticeList { get; } = new List<Notice>();
        public static event Action noticeAdd = () => { };

        protected override void Awake()
        {
            if (SingletonCheck(this))
                noticeAdd += _noticeAdd.Invoke;
        }

        void Update()
        {
            if (InitialLoadManager.isInitialLoadEnd)
            {
                if (noticeBar.isShow && noticeList.Count > 0 && InputManager.GetKey("notice_manager.notice_remove", InputType.Down, "all"))
                {
                    noticeList[noticeList.Count - 1].Remove();
                    noticeList.RemoveAt(noticeList.Count - 1);
                }

                if (noticeBar.isShow && noticeList.Count > 0 && InputManager.GetKey("notice_manager.notice_clear_all", InputType.Down, "all"))
                    Clear();
            }
        }

        public void Clear()
        {
            for (int i = 0; i < noticeList.Count; i++)
                noticeList[i].Remove();
        }

        public void AllAsyncTaskCancel() => AsyncTaskManager.AllAsyncTaskCancel();

        public static void Notice(string name, string info) => notice(name, info, null, Type.none);
        public static void Notice(string name, string info, Type type) => notice(name, info, null, type);
        public static void Notice(string name, string info, ReplaceOldNewPair replace) => notice(name, info, new ReplaceOldNewPair[] { replace }, Type.none);
        public static void Notice(string name, string info, ReplaceOldNewPair replace, Type type) => notice(name, info, new ReplaceOldNewPair[] { replace }, type);
        public static void Notice(string name, string info, ReplaceOldNewPair[] replace) => notice(name, info, replace, Type.none);
        public static void Notice(string name, string info, ReplaceOldNewPair[] replace, Type type) => notice(name, info, replace, type);

        static void notice(string name, string info, ReplaceOldNewPair[] replace, Type type)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(Notice));
            if (!Kernel.isPlaying)
                throw new NotPlayModeMethodException(nameof(Notice));
            if (!InitialLoadManager.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(Notice));

            Notice notice = (Notice)ObjectPoolingSystem.ObjectCreate("notice_manager.notice", instance.noticeListTransform);
            notice.transform.SetAsFirstSibling();
            notice.nameText.nameSpace = "sc-krm";
            notice.infoText.nameSpace = "sc-krm";
            notice.nameText.path = name;
            notice.infoText.path = info;
            notice.nameText.replace = replace;
            notice.infoText.replace = replace;

            notice.nameText.Refresh();
            notice.infoText.Refresh();

            noticeList.Add(notice);

            if (type != Type.none)
            {
                notice.icon.gameObject.SetActive(true);

                notice.icon.nameSpace = "sc-krm";
                notice.icon.type = "gui";
                notice.icon.path = "notice_icon_" + type;
                notice.icon.Refresh();

                notice.setSizeAsChildRectTransform.min = 70;
                notice.verticalLayout.padding.left = 70;
            }

            noticeAdd?.Invoke();
        }

        public enum Type
        {
            none,
            warning,
            error
        }
    }
}