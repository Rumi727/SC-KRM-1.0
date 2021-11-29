using SCKRM.Language;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.TaskBar
{
    [RequireComponent(typeof(TMP_Text))]
    public class NoticeText : MonoBehaviour
    {
        [SerializeField, HideInInspector] TMP_Text _text;
        public TMP_Text text
        {
            get
            {
                if (_text == null)
                    _text = GetComponent<TMP_Text>();

                return _text;
            }
        }

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