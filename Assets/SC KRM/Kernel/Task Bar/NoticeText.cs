using SCKRM.Language;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.TaskBar
{
    public class NoticeText : MonoBehaviour
    {
        [SerializeField] Text text;

        static string am = "";
        static string pm = "";
        static int tempMinute = -1;

        void Start()
        {
            Kernel.AllRefreshEnd += LanguageChange;
            LanguageChange();
        }

        DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
        void Update()
        {
            DateTime dateTime = DateTime.Now;
            if (dateTime.Minute != tempMinute)
            {
                dateTimeFormatInfo.AMDesignator = am;
                dateTimeFormatInfo.PMDesignator = pm;
                text.text = DateTime.Now.ToString("tt h:mm\nyyyy-MM-dd", dateTimeFormatInfo);
                tempMinute = dateTime.Minute;
            }
        }

        void LanguageChange()
        {
            am = LanguageManager.TextLoad("gui.am");
            pm = LanguageManager.TextLoad("gui.pm");
            tempMinute = -1;
        }
    }
}