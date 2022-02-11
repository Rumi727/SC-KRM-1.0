using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Object;
using SCKRM.Threads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI.SideBar
{
    public sealed class NoticeManager : MonoBehaviour
    {
        public static NoticeManager instance { get; private set; }

        [SerializeField] Transform _noticeList;
        public Transform noticeList => _noticeList;

        async void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(this);
                return;
            }

            await UniTask.WaitUntil(() => Kernel.isInitialLoadEnd);
        }

        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                if (SideBarManager.isSideBarShow && noticeList.transform.childCount > 0 && InputManager.GetKeyDown("notice_manager.notice_remove", "statusbar", "sidebar"))
                    noticeList.transform.GetChild(0).GetComponent<Notice>().Remove();
            }
        }

        public static void Notice(string name, string info) => notice(name, info, null, null, Type.none);
        public static void Notice(string name, string info, Type type) => notice(name, info, null, null, type);
        public static void Notice(string name, string info, string replaceOld, string replaceNew) => notice(name, info, new string[] { replaceOld }, new string[] { replaceNew }, Type.none);
        public static void Notice(string name, string info, string replaceOld, string replaceNew, Type type) => notice(name, info, new string[] { replaceOld }, new string[] { replaceNew }, type);
        public static void Notice(string name, string info, string[] replaceOld, string[] replaceNew) => notice(name, info, replaceOld, replaceNew, Type.none);
        public static void Notice(string name, string info, string[] replaceOld, string[] replaceNew, Type type) => notice(name, info, replaceOld, replaceNew, type);

        static void notice(string name, string info, string[] replaceOld, string[] replaceNew, Type type)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(Notice));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(Notice));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(Notice));

            Notice notice = ObjectPoolingSystem.ObjectCreate("notice_manager.notice", instance.noticeList).GetComponent<Notice>();
            notice.transform.SetAsFirstSibling();
            notice.nameText.nameSpace = "sc-krm";
            notice.infoText.nameSpace = "sc-krm";
            notice.nameText.path = name;
            notice.infoText.path = info;
            notice.nameText.replaceOld = replaceOld;
            notice.infoText.replaceOld = replaceOld;
            notice.nameText.replaceNew = replaceNew;
            notice.infoText.replaceNew = replaceNew;

            notice.nameText.ResourceReload();
            notice.infoText.ResourceReload();
            notice.nameText.Rerender();
            notice.infoText.Rerender();

            if (type != Type.none)
            {
                notice.icon.gameObject.SetActive(true);

                notice.icon.nameSpace = "sc-krm";
                notice.icon.type = "gui";
                notice.icon.path = "notice_icon_" + type;
                notice.icon.ResourceReload();
                notice.icon.Rerender();

                notice.setSizeAsChildRectTransform.min = 70;
                notice.verticalLayout.padding.left = 70;
            }

            SideBarManager.isSideBarShow = true;
            SettingBarManager.isSettingBarShow = false;
        }

        public enum Type
        {
            none,
            warning,
            error
        }
    }
}