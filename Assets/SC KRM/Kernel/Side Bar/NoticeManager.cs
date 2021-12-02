using Cysharp.Threading.Tasks;
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

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        public static async void Notice(string name, string info, Type type = Type.none)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(Notice));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(Notice));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(Notice));

            await UniTask.WaitUntil(() => Kernel.isInitialLoadEnd);
            Notice notice = ObjectPoolingSystem.ObjectCreate("notice_manager.notice", instance.noticeList).GetComponent<Notice>();
            notice.transform.SetAsFirstSibling();
            notice.nameText.nameSpace = "sc-krm";
            notice.infoText.nameSpace = "sc-krm";
            notice.nameText.path = name;
            notice.infoText.path = info;

            notice.nameText.ResourceReload();
            notice.infoText.ResourceReload();
            notice.nameText.Rerender();
            notice.infoText.Rerender();

            if (type != Type.none)
            {
                notice.icon.gameObject.SetActive(true);

                notice.icon.nameSpace = "sc-krm";
                notice.icon.type = "gui/sidebar/notice/icon";
                notice.icon.path = type.ToString();
                notice.icon.ResourceReload();
                notice.icon.Rerender();

                notice.setSizeAsChildRectTransform.min = 70;
                notice.verticalLayout.padding.left = 70;
            }
        }

        public enum Type
        {
            none,
            error
        }
    }
}