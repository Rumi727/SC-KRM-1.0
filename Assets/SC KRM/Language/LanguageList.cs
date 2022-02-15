using Cysharp.Threading.Tasks;
using SCKRM.Renderer;
using SCKRM.Resource;
using SCKRM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Language.UI
{
    public class LanguageList : MonoBehaviour
    {
        [SerializeField] Dropdown dropdown;

        public void LanguageRefresh()
        {
            LanguageManager.LanguageChangeEventInvoke();
            RendererManager.AllTextRerender(true);
        }

        async UniTask Start()
        {
            await UniTask.WaitUntil(() => Kernel.isInitialLoadEnd);

            Refresh();
            Kernel.AllRefreshEnd += Refresh;
        }

        void OnDestroy() => Kernel.AllRefreshEnd -= Refresh;

        public void Refresh()
        {
            LanguageManager.Language[] languages = LanguageManager.GetLanguages();
            List<string> options = new List<string>();
            List<string> customLabel = new List<string>();

            for (int i = 0; i < languages.Length; i++)
            {
                LanguageManager.Language language = languages[i];

                if (!options.Contains(language.language))
                {
                    options.Add(language.language);
                    customLabel.Add($"{language.languageName} ({language.languageRegion})");
                }
            }

            dropdown.options = options.ToArray();
            dropdown.customLabel = customLabel.ToArray();
        }
    }
}