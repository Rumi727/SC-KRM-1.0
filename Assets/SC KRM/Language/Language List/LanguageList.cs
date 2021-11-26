using SCKRM.Json;
using SCKRM.Object;
using SCKRM.Resource;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SCKRM.Language.UI
{
    [AddComponentMenu("커널/Language/언어 리스트/언어 리스트", 1)]
    public class LanguageList : MonoBehaviour
    {
        IEnumerator reloadCoroutine;

        public static LanguageList instance;

        void OnEnable() => Reload();

        public void Reload()
        {
            if (reloadCoroutine != null)
                StopCoroutine(reloadCoroutine);

            reloadCoroutine = ReloadCoroutine();
            StartCoroutine(reloadCoroutine);
        }

        IEnumerator ReloadCoroutine()
        {
            LanguageButton[] child = GetComponentsInChildren<LanguageButton>();
            for (int i = 0; i < child.Length; i++)
            {
                LanguageButton item = child[i];
                item.Remove();
            }

            instance = this;

            List<string> languageList = new List<string>();
            for (int packIndex = 0; packIndex < ResourceManager.resourcePacks.Count; packIndex++)
            {
                string resourcePackPath = ResourceManager.resourcePacks[packIndex];

                for (int nameSpaceIndex = 0; nameSpaceIndex < ResourceManager.nameSpaces.Count; nameSpaceIndex++)
                {
                    string nameSpace = ResourceManager.nameSpaces[nameSpaceIndex];
                    if (Directory.Exists(KernelMethod.PathCombine(resourcePackPath, ResourceManager.languagePath).Replace("%NameSpace%", nameSpace)))
                    {
                        string[] directorys = Directory.GetFiles(KernelMethod.PathCombine(resourcePackPath, ResourceManager.languagePath).Replace("%NameSpace%", nameSpace), "*.json");

                        for (int languageIndex = 0; languageIndex < directorys.Length; languageIndex++)
                        {
                            string path = directorys[languageIndex].Replace("\\", "/");
                            string language = path.Substring(path.LastIndexOf("/") + 1, path.LastIndexOf(".") - path.LastIndexOf("/") - 1);

                            if (languageList.Contains(language))
                                continue;

                            LanguageButton button = ObjectPoolingSystem.ObjectCreate("language_list.language_button", transform).GetComponent<LanguageButton>();
                            button.gameObject.name = language;
                            button.language = language;

                            languageList.Add(language);

                            Dictionary<string, string> languageFile = JsonManager.JsonRead<Dictionary<string, string>>(path, true);

                            if (languageFile.ContainsKey("language.name"))
                                button.text.text = languageFile["language.name"];
                            if (languageFile.ContainsKey("language.region"))
                                button.text.text += $" ({languageFile["language.region"]})";

                            if (button.text.text == "")
                                button.text.text = language;

                            yield return null;
                        }
                    }
                }
            }
        }
    }
}