using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.Object;
using SCKRM.UI.Layout;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SCKRM.Resource.UI
{
    public class ResourcePackList : MonoBehaviour
    {
        [SerializeField] VerticalLayout _selectedResourcePacksContentLayout; public VerticalLayout selectedResourcePacksContentLayout => _selectedResourcePacksContentLayout;
        [SerializeField] VerticalLayout _availableResourcePacksContentLayout; public VerticalLayout availableResourcePacksContentLayout => _availableResourcePacksContentLayout;
        [SerializeField] RectTransform _selectedResourcePacksContent; public RectTransform selectedResourcePacksContent => _selectedResourcePacksContent;
        [SerializeField] RectTransform _availableResourcePacksContent; public RectTransform availableResourcePacksContent => _availableResourcePacksContent;

        void OnDisable() => ChildRemove();

        void OnApplicationFocus(bool focus)
        {
            if (InitialLoadManager.isInitialLoadEnd)
                Refresh();
        }

        public void ChildRemove()
        {
            ResourcePack[] resourcePacks = GetComponentsInChildren<ResourcePack>(true);
            for (int i = 0; i < resourcePacks.Length; i++)
                resourcePacks[i].Remove();

            createdResourcePacks.Clear();
        }

        List<ResourcePack> createdResourcePacks = new List<ResourcePack>();
        public void Refresh()
        {
            ChildRemove();

            ResourcePackLoad(ResourceManager.SaveData.resourcePacks.ToArray(), _selectedResourcePacksContent, false).Forget();
            ResourcePackLoad(Directory.GetDirectories(Kernel.resourcePackPath), _availableResourcePacksContent, true).Forget();

            async UniTaskVoid ResourcePackLoad(string[] resourcePackPaths, Transform transform, bool available)
            {
                for (int i = 0; i < resourcePackPaths.Length; i++)
                {
                    string resourcePackPath = resourcePackPaths[i].Replace("\\", "/");
                    if (available && ResourceManager.SaveData.resourcePacks.Contains(resourcePackPath))
                        continue;

                    string jsonPath = PathTool.Combine(resourcePackPath, "pack.json");
                    if (File.Exists(jsonPath))
                    {
                        try
                        {
                            ResourcePackJson resourcePackJson = JsonConvert.DeserializeObject<ResourcePackJson>(File.ReadAllText(jsonPath));
                            if (resourcePackJson != null)
                            {
                                Texture2D texture = await ResourceManager.GetTexture(PathTool.Combine(resourcePackPath, "pack"), false, new TextureMetaData() { filterMode = FilterMode.Bilinear });
                                Sprite sprite = ResourceManager.GetSprite(texture);
                                ResourcePack resourcePack = (ResourcePack)ObjectPoolingSystem.ObjectCreate("resource_pack_list.resource_pack", transform);
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                                resourcePack.resourcePackList = this;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                                createdResourcePacks.Add(resourcePack);

                                resourcePack.nameText.text = resourcePackJson.name.ConstEnvironmentVariable();
                                resourcePack.descriptionText.text = resourcePackJson.description.ConstEnvironmentVariable();

                                resourcePack.resourcePackPath = resourcePackPath;
                                resourcePack.resourcePackIndex = i;

                                resourcePack.selected = !available;

                                if (sprite != null)
                                {
                                    resourcePack.icon.gameObject.SetActive(true);
                                    resourcePack.icon.sprite = sprite;

                                    resourcePack.setSizeAsChildRectTransform.min = 70;
                                    resourcePack.verticalLayout.padding.left = 70;
                                }
                                else
                                {
                                    resourcePack.setSizeAsChildRectTransform.min = 40;
                                    resourcePack.verticalLayout.padding.left = 10;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }
            }

            _selectedResourcePacksContentLayout.LayoutRefresh();
            _availableResourcePacksContentLayout.LayoutRefresh();
            _selectedResourcePacksContentLayout.SizeUpdate(false);
            _availableResourcePacksContentLayout.SizeUpdate(false);
        }

        class ResourcePackJson
        {
            public string name = "";
            public string description = "";
        }
    }
}
