using SCKRM.Object;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Language.UI
{
    [AddComponentMenu("커널/Language/언어 설정 버튼", 2)]
    public class LanguageButton : ObjectPooling
    {
        [SerializeField] Image image;
        [SerializeField] internal Text text;
        
        internal string language = "";

        void Update()
        {
            if (LanguageManager.SaveData.currentLanguage == language)
                image.color = Color.white;
            else
                image.color = Color.clear;
        }

        public void ChangeLanguage()
        {
            LanguageManager.SaveData.currentLanguage = language;
            LanguageManager.LanguageChangeEventInvoke();
            image.color = Color.white;

            Kernel.AllRefresh(true);
        }

        public override void Remove()
        {
            base.Remove();
            language = "";
        }
    }
}