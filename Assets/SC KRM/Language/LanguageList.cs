using Cysharp.Threading.Tasks;
using SCKRM.Renderer;
using SCKRM.Resource;
using SCKRM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Language.UI
{
    [AddComponentMenu("언어 리스트")]
    public class LanguageList : SCKRM.UI.UI
    {
        [SerializeField] Dropdown dropdown;

        public void LanguageRefresh()
        {
            LanguageManager.LanguageChangeEventInvoke();
            RendererManager.AllTextRerender(true);
        }

        async UniTaskVoid Start()
        {
            if (await UniTask.WaitUntil(() => Kernel.isInitialLoadEnd, cancellationToken: this.GetCancellationTokenOnDestroy()).SuppressCancellationThrow())
                return;

            ListRefresh();
            Kernel.AllRefreshEnd += ListRefresh;
        }

        void OnDestroy() => Kernel.AllRefreshEnd -= ListRefresh;

        public void ListRefresh()
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