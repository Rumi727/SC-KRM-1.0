using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Threads;
using SCKRM.Tool;
using SCKRM.UI.StatusBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.SideBar
{
    [AddComponentMenu(""), RequireComponent(typeof(RectTransform))]
    public static class SideBarManager
    {
        public static List<SideBarAni> showedSideBars { get; } = new List<SideBarAni>();
        public static bool isSideBarShow => showedSideBars.Count > 0;

        public static void AllHide()
        {
            for (int i = 0; i < showedSideBars.Count; i++)
                showedSideBars[i].Hide();
        }
    }
}