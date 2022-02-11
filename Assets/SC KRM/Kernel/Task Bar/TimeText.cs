using SCKRM.Language;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.StatusBar
{
    [AddComponentMenu("")]
    [RequireComponent(typeof(TMP_Text))]
    public sealed class TimeText : MonoBehaviour
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
        static int tempSecond = -1;

        static DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();

        static bool tempTwentyFourHourSystem = false;
        static bool tempToggleSeconds = false;

        void Start()
        {
            LanguageManager.currentLanguageChange += LanguageChange;
            LanguageChange();
        }

        void Update()
        {
            DateTime dateTime = DateTime.Now;
            if ((dateTime.Second != tempSecond && StatusBarManager.SaveData.toggleSeconds) || dateTime.Minute != tempMinute || StatusBarManager.SaveData.twentyFourHourSystem != tempTwentyFourHourSystem || StatusBarManager.SaveData.toggleSeconds != tempToggleSeconds)
            {
                dateTimeFormatInfo.AMDesignator = am;
                dateTimeFormatInfo.PMDesignator = pm;

                string time = "tt h:mm\nyyyy-MM-dd";

                if (StatusBarManager.SaveData.twentyFourHourSystem)
                    time = time.Replace("h", "H").Replace("tt", "");

                if (StatusBarManager.SaveData.toggleSeconds)
                    time = time.Replace("mm", "mm:ss");

                text.text = DateTime.Now.ToString(time, dateTimeFormatInfo);

                tempMinute = dateTime.Minute;
                tempTwentyFourHourSystem = StatusBarManager.SaveData.twentyFourHourSystem;
                tempToggleSeconds = StatusBarManager.SaveData.toggleSeconds;
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