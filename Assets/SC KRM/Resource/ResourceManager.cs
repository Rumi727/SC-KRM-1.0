using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SCKRM.Json;
using SCKRM.Language;
using SCKRM.ProjectSetting;
using SCKRM.SaveLoad;
using SCKRM.Threads;
using SCKRM.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace SCKRM.Resource
{
    public static class ResourceManager
    {
        [ProjectSetting]
        public sealed class Data
        {
            [JsonProperty] public static List<string> nameSpaces { get; set; } = new List<string>() { "sc-krm", "minecraft", "school-live" };
        }

        [SaveLoad("resource")]
        public sealed class SaveData
        {
            [JsonProperty] public static List<string> resourcePacks { get; set; } = new List<string>() { Kernel.streamingAssetsPath };
            [JsonProperty] public static List<string> nameSpaces { get; set; } = new List<string>();
        }



        public const string defaultNameSpace = "sc-krm";

        public const string assetsPath = "assets/%NameSpace%";
        public const string texturePath = "assets/%NameSpace%/textures";
        public const string soundPath = "assets/%NameSpace%/sounds";
        public const string nbsPath = "assets/%NameSpace%/nbs";
        public const string languagePath = "assets/%NameSpace%/lang";
        public const string settingsPath = "projectSettings";

        public static List<string> nameSpaces { get => Data.nameSpaces; }

        public static string[] textureExtension { get; } = new string[] { "png", "jpg", "tga" };
        public static string[] textExtension { get; } = new string[] { "txt", "html", "htm", "xml", "bytes", "json", "csv", "yaml", "fnt" };
        public static string[] audioExtension { get; } = new string[] { "ogg", "mp3", "mp2", "wav", "aif", "xm", "mod", "it", "vag", "xma", "s3m" };



        /// <summary>
        /// Texture2D = allTextures[nameSpace][type];
        /// </summary>
        static Dictionary<string, Dictionary<string, Texture2D>> packTextures { get; } = new Dictionary<string, Dictionary<string, Texture2D>>();
        /// <summary>
        /// Rect = allTextureRects[nameSpace][type][fileName];
        /// </summary>
        static Dictionary<string, Dictionary<string, Dictionary<string, Rect>>> packTextureRects { get; } = new Dictionary<string, Dictionary<string, Dictionary<string, Rect>>>();
        /// <summary>
        /// string = allTexturePaths[nameSpace][type][fileName];
        /// </summary>
        static Dictionary<string, Dictionary<string, Dictionary<string, string>>> packTexturePaths { get; } = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        /// <summary>
        /// string = allTexturePaths[nameSpace][type];
        /// </summary>
        static Dictionary<string, Dictionary<string, string>> packTextureTypePaths { get; } = new Dictionary<string, Dictionary<string, string>>();
        /// <summary>
        /// Sprite = allTextureSprites[nameSpace][type][fileName];
        /// </summary>
        static Dictionary<string, Dictionary<string, Dictionary<string, Sprite[]>>> allTextureSprites { get; } = new Dictionary<string, Dictionary<string, Dictionary<string, Sprite[]>>>();



        static List<AudioClip> garbages = new List<AudioClip>();
        /// <summary>
        /// SoundData = allTextureSounds[nameSpace][key];
        /// </summary>
        static Dictionary<string, Dictionary<string, SoundData>> allSounds { get; } = new Dictionary<string, Dictionary<string, SoundData>>();



        /// <summary>
        /// string = allLanguages[nameSpace][language][key];
        /// </summary>
        static Dictionary<string, Dictionary<string, Dictionary<string, string>>> allLanguages { get; } = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();



        public static bool isInitialLoadPackTexturesEnd { get; private set; } = false;
        public static bool isInitialLoadSpriteEnd { get; private set; } = false;
        public static bool isInitialLoadAudioEnd { get; private set; } = false;
        public static bool isInitialLoadLanguageEnd { get; private set; } = false;
        public static bool isResourceRefesh { get; private set; } = false;

        /// <summary>
        /// 리소스 새로고침 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Resource refresh (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <returns></returns>
        public static async UniTask ResourceRefresh()
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(ResourceRefresh));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(ResourceRefresh));
#endif
            if (isResourceRefesh)
                return;

            Data.nameSpaces = Data.nameSpaces.Union(SaveData.nameSpaces).ToList();

            isResourceRefesh = true;

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = "notice.running_task.resource_pack_refresh.name";
            threadMetaData.autoRemoveDisable = true;

            ThreadManager.runningThreads.Add(threadMetaData);

            try
            {
                Debug.Log("ResourceManager: Resource refresh start!");
                Debug.Log("ResourceManager: Waiting for pack textures to set...");
                await SetPackTextures();

                threadMetaData.progress = 1f / 4f;
                Debug.Log("ResourceManager: Waiting for sprite to set...");
                await SetSprite();

                threadMetaData.progress = 2f / 4f;
                Debug.Log("ResourceManager: Waiting for language to set...");
                ThreadMetaData threadMetaData2 = ThreadManager.Create(SetLanguage, "notice.running_task.language_refresh.name");
                await UniTask.WaitUntil(() => threadMetaData2.thread == null);
                isInitialLoadLanguageEnd = true;

                threadMetaData.progress = 3f / 4f;
                Debug.Log("ResourceManager: Waiting for audio to set...");
                await SetAudio();

                threadMetaData.progress = 1;
                Debug.Log("ResourceManager: Resource refresh finished!");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogError("ResourceManager: Resource refresh failed");

                if (!Kernel.isInitialLoadEnd)
#if UNITY_EDITOR
                {
                    Debug.LogError("Kernel: Initial loading failed");
                    if (Application.isPlaying)
                    {
                        GameObject[] gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>(true);
                        int length = gameObjects.Length;
                        for (int i = 0; i < length; i++)
                            UnityEngine.Object.DestroyImmediate(gameObjects[i]);

                        UnityEditor.EditorApplication.isPlaying = false;
                        while (true)
                            await UniTask.DelayFrame(1);
                    }
                    else
                        Debug.LogWarning("Kernel: Do not exit play mode during initial loading");
                }
#else
                    Application.Quit(1);
#endif
            }

            threadMetaData.autoRemoveDisable = false;
            isResourceRefesh = false;
        }

        /// <summary>
        /// 리소스팩의 텍스쳐를 전부 하나로 합칩니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Combine all textures from resource packs into one (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <returns></returns>
        static async UniTask SetPackTextures()
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(SetPackTextures));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(SetPackTextures));
#endif
            packTextures.Clear();
            packTextureRects.Clear();
            packTexturePaths.Clear();
            packTextureTypePaths.Clear();
            Dictionary<string, Dictionary<string, Texture2D[]>> nameSpace_type_textures = new Dictionary<string, Dictionary<string, Texture2D[]>>();
            Dictionary<string, Dictionary<string, List<string>>> nameSpace_type_textureNames = new Dictionary<string, Dictionary<string, List<string>>>();

            //모든 리소스팩을 돌아다닙니다
            for (int i = 0; i < SaveData.resourcePacks.Count; i++)
            {
                string resourcePack = SaveData.resourcePacks[i];
                //연결된 네임스페이스를 돌아다닙니다 (리소스팩에 있는 네임스페이스를 감지하지 않습니다!)
                for (int j = 0; j < Data.nameSpaces.Count; j++)
                {
                    string nameSpace = Data.nameSpaces[j];
                    string resourcePackTexturePath = PathTool.Combine(resourcePack, texturePath.Replace("%NameSpace%", nameSpace));

                    if (!Directory.Exists(resourcePackTexturePath))
                        continue;

                    string[] types = Directory.GetDirectories(resourcePackTexturePath, "*", SearchOption.AllDirectories);

                    //리소스팩 폴더 안에 있는 텍스쳐 타입 폴더를 돌아다닙니다 (타입 폴더 안의 폴더도 타입으로 간주합니다 즉 파일 경로가 "assets/sc-krm/textures/asdf/asdf2/asdf3.png" 이라면 타입은 "asdf/asdf2"가 됩니다)
                    for (int k = 0; k < types.Length; k++)
                    {
                        string typePath = types[k].Replace("\\", "/");
                        string type = typePath.Substring(resourcePackTexturePath.Length + 1, typePath.Length - resourcePackTexturePath.Length - 1);
                        List<Texture2D> textures = new List<Texture2D>();
                        List<string> textureNames = new List<string>();
                        Dictionary<string, string> fileName_texturePaths = new Dictionary<string, string>();
                        TextureMetaData textureMetaData = JsonManager.JsonRead<TextureMetaData>(typePath + ".json", true);
                        if (textureMetaData == null)
                            textureMetaData = new TextureMetaData();

                        List<string> paths = new List<string>();

                        for (int l = 0; l < textureExtension.Length; l++)
                            paths.AddRange(Directory.GetFiles(typePath, "*." + textureExtension[l]));

                        if (paths.Count <= 0)
                            continue;

                        //타입 폴더 안의 모든 이미지를 돌아다닙니다 (타입 폴더 안의 폴더 안의... 이미지는 타입으로 취급하기 때문에 감지하지 않습니다)
                        for (int l = 0; l < paths.Count; l++)
                        {
                            string path = paths[l].Replace("\\", "/");
                            Texture2D texture = GetTexture(path, true, textureMetaData);

                            if (textureNames.Contains(texture.name))
                                continue;
                            
                            if (!nameSpace_type_textureNames.ContainsKey(nameSpace) || !nameSpace_type_textureNames[nameSpace].ContainsKey(type))
                            {
                                fileName_texturePaths.Add(texture.name, path);
                                textureNames.Add(texture.name);
                                textures.Add(texture);
                            }
                            else
                            {
                                //상위 리소스팩에서 이미 텍스쳐를 감지했다면, 감지한 텍스쳐를 무시합니다
                                if (!nameSpace_type_textureNames[nameSpace][type].Contains(texture.name))
                                {
                                    fileName_texturePaths.Add(texture.name, path);
                                    textureNames.Add(texture.name);
                                    textures.Add(texture);
                                }
                            }

                            await UniTask.DelayFrame(1, PlayerLoopTiming.Initialization);
                        }
                        
                        if (!packTextureTypePaths.ContainsKey(nameSpace))
                        {
                            packTextureTypePaths.Add(nameSpace, new Dictionary<string, string>());
                            packTextureTypePaths[nameSpace].Add(type, typePath);
                        }
                        else if (!packTextureTypePaths[nameSpace].ContainsKey(type))
                            packTextureTypePaths[nameSpace].Add(type, typePath);

                        if (!packTexturePaths.ContainsKey(nameSpace))
                        {
                            packTexturePaths.Add(nameSpace, new Dictionary<string, Dictionary<string, string>>());
                            packTexturePaths[nameSpace].Add(type, fileName_texturePaths);
                        }
                        else if (!packTexturePaths[nameSpace].ContainsKey(type))
                            packTexturePaths[nameSpace].Add(type, fileName_texturePaths);
                        else
                            packTexturePaths[nameSpace][type] = packTexturePaths[nameSpace][type].Concat(fileName_texturePaths).ToDictionary(a => a.Key, b => b.Value);

                        if (!nameSpace_type_textureNames.ContainsKey(nameSpace))
                        {
                            nameSpace_type_textureNames.Add(nameSpace, new Dictionary<string, List<string>>());
                            nameSpace_type_textureNames[nameSpace].Add(type, textureNames);
                        }
                        else if (!nameSpace_type_textureNames[nameSpace].ContainsKey(type))
                            nameSpace_type_textureNames[nameSpace].Add(type, textureNames);
                        else
                            nameSpace_type_textureNames[nameSpace][type] = nameSpace_type_textureNames[nameSpace][type].Concat(textureNames).ToList();

                        if (!nameSpace_type_textures.ContainsKey(nameSpace))
                        {
                            nameSpace_type_textures.Add(nameSpace, new Dictionary<string, Texture2D[]>());
                            nameSpace_type_textures[nameSpace].Add(type, textures.ToArray());
                        }
                        else if (!nameSpace_type_textures[nameSpace].ContainsKey(type))
                            nameSpace_type_textures[nameSpace].Add(type, textures.ToArray());
                        else
                            nameSpace_type_textures[nameSpace][type] = nameSpace_type_textures[nameSpace][type].Concat(textures).ToArray();
                    }
                }
            }

            foreach (var nameSpace in nameSpace_type_textures)
            {
                /*allTextureRects*/ Dictionary<string, Dictionary<string, Rect>> type_fileName = new Dictionary<string, Dictionary<string, Rect>>();
                /*allTextures*/ Dictionary<string, Texture2D> type_texture = new Dictionary<string, Texture2D>();
                foreach (var type in nameSpace.Value)
                {
                    Texture2D[] textures = type.Value;
                    Texture2D[] textures2 = new Texture2D[textures.Length];
                    string[] textureNames = new string[textures.Length];
                    int width = 0;
                    int height = 0;
                    for (int i = 0; i < textures.Length; i++)
                    {
                        Texture2D texture = textures[i];
                        textures2[i] = texture;
                        textureNames[i] = texture.name;
                        width += texture.width + 10;
                        height += texture.height + 10;

                        if (i == textures.Length - 1)
                        {
                            width -= 10;
                            height -= 10;
                        }
                    }
                    
                    /*allTextureRects*/
                    TextureMetaData textureMetaData = JsonManager.JsonRead<TextureMetaData>(packTextureTypePaths[nameSpace.Key][type.Key] + ".json", true);
                    if (textureMetaData == null)
                        textureMetaData = new TextureMetaData();

                    Texture2D background = new Texture2D(width, height);
                    Dictionary<string, Rect> fileName_rect = new Dictionary<string, Rect>();
                    
                    Rect[] rects = background.PackTextures(textures2, 10);
                    background.filterMode = textureMetaData.filterMode;

                    if (textureMetaData.compressionType == TextureMetaData.CompressionType.normal)
                        background.Compress(false);
                    else if (textureMetaData.compressionType == TextureMetaData.CompressionType.highQuality)
                        background.Compress(true);

                    for (int i = 0; i < rects.Length; i++)
                        fileName_rect.Add(textureNames[i], rects[i]);
                    type_fileName.Add(type.Key, fileName_rect);

                    /*allTextures*/
                    type_texture.Add(type.Key, background);

                    for (int j = 0; j < textures.Length; j++)
                    {
                        Texture2D texture = textures[j];
                        UnityEngine.Object.Destroy(texture);
                    }
                }
                /*allTextureRects*/ packTextureRects.Add(nameSpace.Key, type_fileName);
                /*allTextures*/ packTextures.Add(nameSpace.Key, type_texture);
            }

            isInitialLoadPackTexturesEnd = true;
        }

        /// <summary>
        /// 합친 텍스쳐를 전부 스프라이트로 만들어서 리스트에 넣습니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Put all the combined textures into sprites and put them in the list (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <returns></returns>
        static async UniTask SetSprite()
        {
            if (!isInitialLoadPackTexturesEnd)
                throw new NotInitialLoadEndMethodException(nameof(SetSprite));
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(SetSprite));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(SetSprite));
#endif

            allTextureSprites.Clear();

            foreach (var nameSpace in packTextureRects)
            {
                foreach (var type in nameSpace.Value)
                {
                    foreach (var fileName in type.Value)
                    {
                        Texture2D background = SearchPackTexture(type.Key, nameSpace.Key);
                        Rect rect = fileName.Value;
                        rect = new Rect(rect.x * background.width, rect.y * background.height, rect.width * background.width, rect.height * background.height);

                        SpriteMetaData[] spriteMetaDatas = JsonManager.JsonRead<SpriteMetaData[]>(SearchTexturePath(type.Key, fileName.Key, nameSpace.Key) + ".json", true);
                        if (spriteMetaDatas == null)
                            spriteMetaDatas = new SpriteMetaData[1];

                        for (int i = 0; i < spriteMetaDatas.Length; i++)
                        {
                            SpriteMetaData spriteMetaData = spriteMetaDatas[i];
                            if (spriteMetaData == null)
                            {
                                spriteMetaData = new SpriteMetaData();
                                spriteMetaData.RectMinMax(rect.width, rect.height);
                                spriteMetaDatas[i] = spriteMetaData;
                            }

                            spriteMetaDatas[i].rect = new JRect(rect.x + spriteMetaData.rect.x, rect.y + spriteMetaData.rect.y, rect.width - (rect.width - spriteMetaData.rect.width), rect.height - (rect.height - spriteMetaData.rect.height));
                        }
                        Sprite[] sprites = GetSprites(background, spriteMetaDatas);
                        
                        if (!allTextureSprites.ContainsKey(nameSpace.Key))
                        {
                            allTextureSprites.Add(nameSpace.Key, new Dictionary<string, Dictionary<string, Sprite[]>>());
                            allTextureSprites[nameSpace.Key].Add(type.Key, new Dictionary<string, Sprite[]>());
                            allTextureSprites[nameSpace.Key][type.Key].Add(fileName.Key, sprites);
                        }
                        else if (!allTextureSprites[nameSpace.Key].ContainsKey(type.Key))
                        {
                            allTextureSprites[nameSpace.Key].Add(type.Key, new Dictionary<string, Sprite[]>());
                            allTextureSprites[nameSpace.Key][type.Key].Add(fileName.Key, sprites);
                        }
                        else
                            allTextureSprites[nameSpace.Key][type.Key].Add(fileName.Key, sprites);

                        await UniTask.DelayFrame(1, PlayerLoopTiming.Initialization);
                    }
                }
            }

            isInitialLoadSpriteEnd = true;
        }

        /// <summary>
        /// 리소스팩의 sounds.json에서 오디오를 가져옵니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Get audio from sounds.json in resource pack (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <returns></returns>
        static async UniTask SetAudio()
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(SetAudio));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(SetAudio));
#endif

            foreach (var item in allSounds)
            {
                foreach (var item2 in item.Value)
                {
                    for (int i = 0; i < item2.Value.sounds.Length; i++)
                        garbages.Add(item2.Value.sounds[i].audioClip);
                }
            }

            allSounds.Clear();

            for (int i = 0; i < SaveData.resourcePacks.Count; i++)
            {
                string resourcePack = SaveData.resourcePacks[i];
                for (int j = 0; j < nameSpaces.Count; j++)
                {
                    string nameSpace = nameSpaces[j];
                    string path = PathTool.Combine(resourcePack, soundPath.Replace("%NameSpace%", nameSpace));

                    if (!Directory.Exists(path))
                        continue;
                    
                    Dictionary<string, SoundData> soundDatas = JsonManager.JsonRead<Dictionary<string, SoundData>>(path + ".json", true);
                    if (soundDatas == null)
                        continue;
                    
                    foreach (var soundData in soundDatas)
                    {
                        if (soundData.Value.sounds == null)
                            continue;

                        List<SoundMetaData> soundMetaDatas = new List<SoundMetaData>();
                        for (int k = 0; k < soundData.Value.sounds.Length; k++)
                        {
                            SoundMetaData sound = soundData.Value.sounds[k];

                            string soundPath = PathTool.Combine(path, sound.path);
                            AudioClip audioClip = await GetAudio(soundPath, sound.stream);

                            if (audioClip != null)
                                soundMetaDatas.Add(new SoundMetaData(sound.path, sound.stream, sound.pitch, sound.tempo, audioClip));
                        }

                        if (!allSounds.ContainsKey(nameSpace))
                        {
                            allSounds.Add(nameSpace, new Dictionary<string, SoundData>());
                            allSounds[nameSpace].Add(soundData.Key, new SoundData(soundData.Value.category, soundData.Value.subtitle, soundData.Value.isBGM, soundMetaDatas.ToArray()));
                        }
                        else if (!allSounds[nameSpace].ContainsKey(soundData.Key))
                            allSounds[nameSpace].Add(soundData.Key, new SoundData(soundData.Value.category, soundData.Value.subtitle, soundData.Value.isBGM, soundMetaDatas.ToArray()));
                    }
                }
            }

            isInitialLoadAudioEnd = true;
        }

        /// <summary>
        /// 리소스팩의 sounds.json에서 오디오를 가져옵니다
        /// Get audio from sounds.json in resource pack
        /// </summary>
        /// <returns></returns>
        static void SetLanguage(ThreadMetaData threadMetaData)
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(SetLanguage));
#endif
            int l = 0;
            LanguageManager.Language[] languages = LanguageManager.GetLanguages();
            for (int i = 0; i < SaveData.resourcePacks.Count; i++)
            {
                string resourcePack = SaveData.resourcePacks[i];
                for (int j = 0; j < nameSpaces.Count; j++)
                {
                    string nameSpace = nameSpaces[j];
                    for (int k = 0; k < languages.Length; k++)
                    {
                        threadMetaData.progress = l / (float)(SaveData.resourcePacks.Count * nameSpaces.Count * languages.Length);
                        l++;

                        LanguageManager.Language language = languages[k];

                        Dictionary<string, string> dictionary = JsonManager.JsonRead<Dictionary<string, string>>(PathTool.Combine(resourcePack, languagePath.Replace("%NameSpace%", nameSpace), language.language) + ".json", true);
                        if (dictionary == null)
                            continue;

                        foreach (var languageDictionary in dictionary)
                        {
                            if (!allLanguages.ContainsKey(nameSpace))
                            {
                                allLanguages.Add(nameSpace, new Dictionary<string, Dictionary<string, string>>());
                                allLanguages[nameSpace].Add(language.language, new Dictionary<string, string>());
                                allLanguages[nameSpace][language.language].Add(languageDictionary.Key, languageDictionary.Value);
                            }
                            else if (!allLanguages[nameSpace].ContainsKey(language.language))
                            {
                                allLanguages[nameSpace].Add(language.language, new Dictionary<string, string>());
                                allLanguages[nameSpace][language.language].Add(languageDictionary.Key, languageDictionary.Value);
                            }
                            else if (!allLanguages[nameSpace][language.language].ContainsKey(languageDictionary.Key))
                                allLanguages[nameSpace][language.language].Add(languageDictionary.Key, languageDictionary.Value);
                        }
                    }
                }
            }
        }


        public static void AudioGarbageRemoval()
        {
            for (int i = 0; i < garbages.Count; i++)
                UnityEngine.Object.Destroy(garbages[i]);
        }


        #region Search Method
        /// <summary>
        /// 합쳐진 텍스쳐의 경로를 찾아서 반환합니다
        /// Finds the path of the merged texture and returns it
        /// </summary>
        /// <param name="type">
        /// 타입
        /// Type
        /// </param>
        /// <param name="name">
        /// 이름
        /// Name
        /// </param>
        /// <param name="nameSpace">
        /// 네임스페이스
        /// Name Space
        /// </param>
        /// <returns></returns>
        public static string SearchTexturePath(string type, string name, string nameSpace = "")
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeSearchMethodException();
#endif
            if (!isInitialLoadPackTexturesEnd)
                throw new NotInitialLoadEndMethodException(nameof(SearchTexturePath));

            if (type == null)
                type = "";
            if (name == null)
                name = "";
            if (nameSpace == null)
                nameSpace = "";

            if (nameSpace == "")
                nameSpace = defaultNameSpace;

            if (packTexturePaths.ContainsKey(nameSpace) && packTexturePaths[nameSpace].ContainsKey(type) && packTexturePaths[nameSpace][type].ContainsKey(name))
                return packTexturePaths[nameSpace][type][name];

            return "";
        }

        /// <summary>
        /// 합쳐진 텍스쳐를 반환합니다
        /// Returns the merged texture
        /// </summary>
        /// <param name="type">
        /// 타입
        /// Type
        /// </param>
        /// <param name="nameSpace">
        /// 네임스페이스
        /// Name Space
        /// </param>
        /// <returns></returns>
        public static Texture2D SearchPackTexture(string type, string nameSpace = "")
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeSearchMethodException();
#endif
            if (!isInitialLoadPackTexturesEnd)
                throw new NotInitialLoadEndMethodException(nameof(SearchPackTexture));

            if (type == null)
                type = "";
            if (nameSpace == null)
                nameSpace = "";

            if (nameSpace == "")
                nameSpace = defaultNameSpace;

            if (packTextures.ContainsKey(nameSpace) && packTextures[nameSpace].ContainsKey(type))
                return packTextures[nameSpace][type];

            return null;
        }

        /// <summary>
        /// 합쳐진 텍스쳐의 Rect를 반환합니다
        /// Returns a Rect of the merged texture.
        /// </summary>
        /// <param name="type">
        /// 타입
        /// Type
        /// </param>
        /// <param name="name">
        /// 이름
        /// Name
        /// </param>
        /// <param name="nameSpace">
        /// 네임스페이스
        /// Name Space
        /// </param>
        /// <returns></returns>
        public static Rect SearchTextureRect(string type, string name, string nameSpace = "")
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeSearchMethodException();
#endif
            if (!isInitialLoadPackTexturesEnd)
                throw new NotInitialLoadEndMethodException(nameof(SearchTextureRect));

            if (type == null)
                type = "";
            if (name == null)
                name = "";
            if (nameSpace == null)
                nameSpace = "";

            if (nameSpace == "")
                nameSpace = defaultNameSpace;

            if (packTextureRects.ContainsKey(nameSpace) && packTextureRects[nameSpace].ContainsKey(type) && packTextureRects[nameSpace][type].ContainsKey(name))
                return packTextureRects[nameSpace][type][name];

            return Rect.zero;
        }



        /// <summary>
        /// 스프라이트 리스트에서 스프라이트를 찾고 반환합니다
        /// </summary>
        /// <param name="type">
        /// 타입
        /// Type
        /// </param>
        /// <param name="name">
        /// 이름
        /// Name
        /// </param>
        /// <param name="nameSpace">
        /// 네임스페이스
        /// Name Space
        /// </param>
        /// <returns></returns>
        public static Sprite[] SearchSprites(string type, string name, string nameSpace = "")
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeSearchMethodException();
#endif
            if (!isInitialLoadSpriteEnd)
                throw new NotInitialLoadEndMethodException(nameof(SearchSprites));

            if (type == null)
                type = "";
            if (name == null)
                name = "";
            if (nameSpace == null)
                nameSpace = "";

            if (nameSpace == "")
                nameSpace = defaultNameSpace;

            if (allTextureSprites.ContainsKey(nameSpace))
            {
                if (allTextureSprites[nameSpace].ContainsKey(type))
                {
                    if (allTextureSprites[nameSpace][type].ContainsKey(name))
                        return allTextureSprites[nameSpace][type][name];
                }
            }
            return null;
        }

        /// <summary>
        /// 리소스팩에서 언어를 검색하고 반환합니다
        /// It can be executed even if it is not in the initial loading and play mode
        /// </summary>
        /// <param name="key">
        /// 경로
        /// Path
        /// </param>
        /// <param name="nameSpace">
        /// 네임스페이스
        /// Name Space
        /// </param>
        /// <returns></returns>
        public static string SearchLanguage(string key, string nameSpace = "", string language = "")
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeSearchMethodException();
#endif
            if (!isInitialLoadSpriteEnd)
                throw new NotInitialLoadEndMethodException(nameof(SearchLanguage));

            if (key == null)
                key = "";
            if (nameSpace == null)
                nameSpace = "";
            if (language == null)
                language = "";

            if (nameSpace == "")
                nameSpace = defaultNameSpace;
            if (language == "")
                language = LanguageManager.SaveData.currentLanguage;

            if (allLanguages.ContainsKey(nameSpace))
            {
                if (allLanguages[nameSpace].ContainsKey(language))
                {
                    if (allLanguages[nameSpace][language].ContainsKey(key))
                        return allLanguages[nameSpace][language][key];
                }
            }

            return "";
        }

        /// <summary>
        /// 리소스팩에서 사운드 데이터를 찾고 반환합니다
        /// Finds and returns sound data from resource packs
        /// </summary>
        /// <param name="key">
        /// 키
        /// Key
        /// </param>
        /// <param name="nameSpace">
        /// 네임스페이스
        /// Name Space
        /// </param>
        /// <returns></returns>
        public static SoundData SearchSoundData(string key, string nameSpace = "")
        {
#if UNITY_EDITOR
            if (ThreadManager.isMainThread && !Application.isPlaying)
                throw new NotPlayModeSearchMethodException();
#endif
            if (!isInitialLoadAudioEnd)
                throw new NotInitialLoadEndMethodException(nameof(SearchSoundData));

            if (key == null)
                key = "";
            if (nameSpace == null)
                nameSpace = "";

            if (nameSpace == "")
                nameSpace = defaultNameSpace;
            
            if (allSounds.ContainsKey(nameSpace))
            {
                if (allSounds[nameSpace].ContainsKey(key))
                    return allSounds[nameSpace][key];
            }
            return null;
        }
        #endregion



        #region Get Resource Method
        /// <summary>
        /// 이미지 파일을 Texture2D 타입으로 가져옵니다
        /// Import image files as Texture2D type
        /// </summary>
        /// <param name="path">
        /// 파일의 경로
        /// Path
        /// </param>
        /// <param name="pathExtensionUse">
        /// 경로에 확장자 사용
        /// Use extension in path
        /// </param>
        /// <returns></returns>
        public static Texture2D GetTexture(string path, bool pathExtensionUse = false, TextureFormat textureFormat = TextureFormat.RGBA32)
        {
            TextureMetaData textureMetaData = JsonManager.JsonRead<TextureMetaData>(path + ".json", true);
            if (textureMetaData == null)
            {
                textureMetaData = new TextureMetaData();
                return GetTexture(path, pathExtensionUse, textureMetaData.filterMode, textureMetaData.mipmapUse, textureMetaData.compressionType, textureFormat);
            }
            return GetTexture(path, pathExtensionUse, FilterMode.Point, true, TextureMetaData.CompressionType.none, textureFormat);
        }

        /// <summary>
        /// 이미지 파일을 Texture2D 타입으로 가져옵니다
        /// Import image files as Texture2D type
        /// </summary>
        /// <param name="path">
        /// 파일의 경로
        /// Path
        /// </param>
        /// <param name="pathExtensionUse">
        /// 경로에 확장자 사용
        /// Use extension in path
        /// </param>
        /// <returns></returns>
        public static Texture2D GetTexture(string path, bool pathExtensionUse, TextureMetaData textureMetaData, TextureFormat textureFormat = TextureFormat.RGBA32) => GetTexture(path, pathExtensionUse, textureMetaData.filterMode, textureMetaData.mipmapUse, textureMetaData.compressionType, textureFormat);

        /// <summary>
        /// 이미지 파일을 Texture2D 타입으로 가져옵니다
        /// Import image files as Texture2D type
        /// </summary>
        /// <param name="path">
        /// 파일의 경로
        /// Path
        /// </param>
        /// <param name="pathExtensionUse">
        /// 경로에 확장자 사용
        /// Use extension in path
        /// </param>
        /// <returns></returns>
        public static Texture2D GetTexture(string path, bool pathExtensionUse, FilterMode filterMode, bool mipmapUse, TextureMetaData.CompressionType compressionType, TextureFormat textureFormat = TextureFormat.RGBA32)
        {
            if (path == null)
                path = "";

            bool exists;
            if (!pathExtensionUse)
                exists = FileExtensionExists(path, out path, textureExtension);
            else
                exists = File.Exists(path);
            
            if (exists)
            {
                if (Path.GetExtension(path).ToLower() == ".tga")
                {
                    Texture2D texture = TGALoader.LoadTGA(path);
                    texture.name = Path.GetFileNameWithoutExtension(path);
                    texture.filterMode = filterMode;

                    if (compressionType == TextureMetaData.CompressionType.normal)
                        texture.Compress(false);
                    else if (compressionType == TextureMetaData.CompressionType.highQuality)
                        texture.Compress(true);
                    
                    return texture;
                }
                else
                {
                    Texture2D texture = new Texture2D(0, 0, textureFormat, mipmapUse);

                    byte[] bytes = File.ReadAllBytes(path);
                    if (texture.LoadImage(bytes))
                    {
                        texture.name = Path.GetFileNameWithoutExtension(path);
                        texture.filterMode = filterMode;

                        if (compressionType == TextureMetaData.CompressionType.normal)
                            texture.Compress(false);
                        else if (compressionType == TextureMetaData.CompressionType.highQuality)
                            texture.Compress(true);

                        return texture;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 텍스쳐를 스프라이트로 변환합니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Convert texture to sprite (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Sprite GetSprite(Texture2D texture, SpriteMetaData spriteMetaData = null)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetSprite));

            if (texture == null)
                return null;

            if (spriteMetaData == null)
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, Vector4.zero);
            else
            {
                spriteMetaData.RectMinMax(texture.width, texture.height);
                spriteMetaData.PixelsPreUnitMinSet();

                return Sprite.Create(texture, spriteMetaData.rect, spriteMetaData.pivot, spriteMetaData.pixelsPerUnit, 0, SpriteMeshType.FullRect, spriteMetaData.border);
            }
        }

        /// <summary>
        /// 이미지 파일을 스프라이트로 가져옵니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다.)
        /// Import image files as sprites (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="resourcePackPath">
        /// 리소스팩 경로
        /// Resource Pack Path
        /// </param>
        /// <param name="type">
        /// 타입
        /// Type
        /// </param>
        /// <param name="name">
        /// 이름
        /// Name
        /// </param>
        /// <param name="nameSpace">
        /// 네임스페이스
        /// Name Space
        /// </param>
        /// <param name="textureFormat">
        /// 텍스쳐 포맷
        /// Texture Format
        /// </param>
        /// <returns></returns>
        public static Sprite[] GetSprites(string resourcePackPath, string type, string name, string nameSpace = "", TextureFormat textureFormat = TextureFormat.RGBA32)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetSprites));

            if (resourcePackPath == null || resourcePackPath == "")
                return null;
            if (type == null || type == "")
                return null;
            if (name == null || name == "")
                return null;
            if (nameSpace == null)
                nameSpace = null;

            if (nameSpace == "")
                nameSpace = defaultNameSpace;

            string path = PathTool.Combine(resourcePackPath, texturePath.Replace("%NameSpace%", nameSpace));
            string allPath = PathTool.Combine(path, type, name);
            
            TextureMetaData textureMetaData = JsonManager.JsonRead<TextureMetaData>(PathTool.Combine(path, type) + ".json", true);
            if (textureMetaData == null)
                textureMetaData = new TextureMetaData();

            Texture2D texture = GetTexture(allPath, false, textureMetaData, textureFormat);
            FileExtensionExists(allPath, out string allPath2, textureExtension);
            SpriteMetaData[] spriteMetaDatas = JsonManager.JsonRead<SpriteMetaData[]>(allPath2 + ".json", true);
            return GetSprites(texture, spriteMetaDatas);
        }

        /// <summary>
        /// 이미지 파일을 스프라이트로 가져옵니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다.)
        /// Import image files as sprites (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="path">
        /// 이미지 파일의 경로
        /// Path
        /// </param>
        /// <param name="pathExtensionUse">
        /// 경로에 확장자 사용
        /// Use extension in path
        /// </param>
        /// <returns></returns>
        public static Sprite[] GetSprites(string path, bool pathExtensionUse = false, TextureFormat textureFormat = TextureFormat.RGBA32)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetSprites));

            if (path == null)
                path = "";

            Texture2D texture = GetTexture(path, pathExtensionUse, textureFormat);
            SpriteMetaData[] spriteMetaDatas = JsonManager.JsonRead<SpriteMetaData[]>(path + ".json", true);
            return GetSprites(texture, spriteMetaDatas);
        }

        /// <summary>
        /// 텍스쳐를 스프라이트로 변환합니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Import image files as sprites (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="texture">
        /// 변환할 텍스쳐
        /// texture to convert
        /// </param>
        /// <param name="spriteMetaDatas">
        /// Sprite's metadata
        /// </param>
        /// <returns></returns>
        public static Sprite[] GetSprites(Texture2D texture, params SpriteMetaData[] spriteMetaDatas)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetSprites));

            if (texture == null)
                return null;
            if (spriteMetaDatas == null)
                spriteMetaDatas = new SpriteMetaData[] { new SpriteMetaData() };

            Sprite[] sprites = new Sprite[spriteMetaDatas.Length];
            for (int i = 0; i < spriteMetaDatas.Length; i++)
            {
                SpriteMetaData spriteMetaData = spriteMetaDatas[i];
                if (spriteMetaData == null)
                    spriteMetaData = new SpriteMetaData();

                spriteMetaData.RectMinMax(texture.width, texture.height);
                spriteMetaData.PixelsPreUnitMinSet();

                Sprite sprite = Sprite.Create(texture, spriteMetaData.rect, spriteMetaData.pivot, spriteMetaData.pixelsPerUnit, 0, SpriteMeshType.FullRect, spriteMetaData.border);
                sprite.name = texture.name;

                sprites[i] = sprite;
            }
            return sprites;
        }



        /// <summary>
        /// 텍스트 파일을 string 타입으로 가져옵니다
        /// Import text file as string type
        /// </summary>
        /// <param name="path">
        /// 파일의 경로
        /// Path
        /// </param>
        /// <param name="pathExtensionUse">
        /// 경로에 확장자 사용
        /// Use extension in path
        /// </param>
        /// <returns></returns>
        public static string GetText(string path, bool pathExtensionUse = false)
        {
            if (path == null)
                path = "";

            bool exists;
            if (!pathExtensionUse)
                exists = FileExtensionExists(path, out path, textExtension);
            else
                exists = File.Exists(path);
            
            if (exists)
                return File.ReadAllText(path);
            
            return "";
        }

        /// <summary>
        /// 오디오 파일을 오디오 클립으로 가져옵니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Import audio files as audio clips (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="path">
        /// 경로
        /// Path
        /// </param>
        /// <param name="stream">
        /// 스트림
        /// Stream
        /// </param>
        /// <returns></returns>
        public static async UniTask<AudioClip> GetAudio(string path, bool stream = false)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(GetAudio));

            if (path == null)
                path = "";
            
            AudioClip audioClip = await getSound(".ogg", AudioType.OGGVORBIS);
            if (audioClip == null) audioClip = await getSound(".mp3", AudioType.MPEG);
            if (audioClip == null) audioClip = await getSound(".mp2", AudioType.MPEG);
            if (audioClip == null) audioClip = await getSound(".wav", AudioType.WAV);
            if (audioClip == null) audioClip = await getSound(".aif", AudioType.AIFF);
            if (audioClip == null) audioClip = await getSound(".xm", AudioType.XM);
            if (audioClip == null) audioClip = await getSound(".mod", AudioType.MOD);
            if (audioClip == null) audioClip = await getSound(".it", AudioType.IT);
            if (audioClip == null) audioClip = await getSound(".vag", AudioType.VAG);
            if (audioClip == null) audioClip = await getSound(".xma", AudioType.XMA);
            if (audioClip == null) audioClip = await getSound(".s3m", AudioType.S3M);

            return audioClip;

            async UniTask<AudioClip> getSound(string extension, AudioType type)
            {
                if (File.Exists(path + extension))
                {
                    using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path + extension, type);
                    ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = stream;

                    await www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                        throw new Exception(www.error);

                    return DownloadHandlerAudioClip.GetContent(www);
                }

                return null;
            }
        }
        #endregion



        /// <summary>
        /// 파일들에 특정 확장자가 있으면 true를 반환합니다
        /// Returns true if files have a specific extension
        /// </summary>
        /// <param name="path">
        /// 파일의 경로
        /// Path
        /// </param>
        /// <param name="outPath">
        /// 검색한 확장자를 포함한 전체 경로
        /// Full path including searched extension
        /// </param>
        /// <param name="extensions">
        /// 확장자 리스트
        /// extension list
        /// </param>
        /// <returns></returns>
        public static bool FileExtensionExists(string path, out string outPath, params string[] extensions)
        {
            if (path == null)
                path = "";
            if (extensions == null)
            {
                outPath = "";
                return false;
            }

            for (int i = 0; i < extensions.Length; i++)
            {
                string extension = extensions[i];
                if (File.Exists(path + "." + extension))
                {
                    outPath = path += "." + extension;
                    return true;
                }
            }

            outPath = "";
            return false;
        }

        /// <summary>
        /// 텍스트에서 네임스페이스를 분리하고 네임스페이스를 반환합니다.
        /// Detach a namespace from text and return the namespace.
        /// </summary>
        /// <param name="text">
        /// 분리할 텍스트
        /// text to split
        /// </param>
        /// <param name="value">
        /// 분리되고 남은 텍스트
        /// Remaining Text
        /// </param>
        /// <returns></returns>
        public static string GetNameSpace(string text, out string value)
        {
            if (text == null)
            {
                value = "";
                return "";
            }

            if (text.Contains(":"))
            {
                value = text.Remove(0, text.IndexOf(":") + 1);
                return text.Substring(0, text.IndexOf(":"));
            }
            else
            {
                value = text;
                return defaultNameSpace;
            }
        }



        /// <summary>
        /// 텍스쳐의 평균 색상을 구합니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Gets the average color of a texture (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="texture">
        /// 텍스쳐
        /// Texture
        /// </param>
        /// <returns></returns>
        public static Color AverageColorFromTexture(Texture2D texture) => AverageColorFromTexture(texture, 0, 0, texture.width, texture.height);

        /// <summary>
        /// 텍스쳐의 평균 색상을 구합니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Gets the average color of a texture (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="texture">
        /// 텍스쳐
        /// Texture
        /// </param>
        /// <param name="rect">
        /// 텍스쳐 크기
        /// Texture Size
        /// </param>
        /// <returns></returns>
        public static Color AverageColorFromTexture(Texture2D texture, RectInt rect) => AverageColorFromTexture(texture, rect.x, rect.y, rect.width, rect.height);

        /// <summary>
        /// 텍스쳐의 평균 색상을 구합니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Gets the average color of a texture (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="texture">
        /// 텍스쳐
        /// Texture
        /// </param>
        /// <param name="x">
        /// X 좌표
        /// X Pos
        /// </param>
        /// <param name="y">
        /// Y 좌표
        /// Y Pos
        /// </param>
        /// <param name="width">
        /// 너비
        /// Width
        /// </param>
        /// <param name="height">
        /// 높이
        /// Height
        /// </param>
        /// <returns></returns>
        public static Color AverageColorFromTexture(Texture2D texture, int x, int y, int width, int height)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(AverageColorFromTexture));

            Color[] textureColors = texture.GetPixels(x, y, width, height);

            int length = textureColors.Length;

            float r = 0;
            float g = 0;
            float b = 0;
            float a = 0;

            for (int i = 0; i < length; i++)
            {
                Color color = textureColors[i];
                r += color.r;
                g += color.g;
                b += color.b;
                a += color.a;
            }

            return new Color(r / length, g / length, b / length, a / length);
        }

        /// <summary>
        /// 색상을 텍스쳐로 변환합니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Convert color to texture (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="color">
        /// 컬러
        /// Color
        /// </param>
        /// <param name="width">
        /// 너비
        /// Width
        /// </param>
        /// <param name="height">
        /// 높이
        /// Height
        /// </param>
        /// <param name="filterMode">
        /// 필터 모드
        /// Filter Mode
        /// </param>
        /// <returns></returns>
        public static Texture2D TextureFromColor(Color color, int width, int height, FilterMode filterMode = FilterMode.Point)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(TextureFromColor));

            if (color == null)
                color = Color.white;
            if (width < 1)
                width = 1;
            if (height < 1)
                height = 1;

            Texture2D texture = new Texture2D(width, height, TextureFormat.DXT5, false);
            texture.filterMode = filterMode;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                    texture.SetPixel(x, y, color);
            }
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// 색상을 텍스쳐로 변환합니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Convert color to texture (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="color">
        /// 색상
        /// Color
        /// </param>
        /// <param name="alpha">
        /// 알파 텍스쳐
        /// Alpha texture
        /// </param>
        /// <returns></returns>
        public static Texture2D TextureFromColor(Color color, Texture2D alpha)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(TextureFromColor));

            if (color == null)
                color = Color.white;

            Texture2D texture = new Texture2D(alpha.width, alpha.height, TextureFormat.DXT5, false);
            texture.filterMode = alpha.filterMode;
            for (int x = 0; x < alpha.width; x++)
            {
                for (int y = 0; y < alpha.height; y++)
                    texture.SetPixel(x, y, new Color(color.r, color.g, color.b, alpha.GetPixel(x, y).a));
            }
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// 색상을 스프라이트로 변환합니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Convert color to sprite (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="color">
        /// 색상
        /// Color
        /// </param>
        /// <param name="width">
        /// 너비
        /// Width
        /// </param>
        /// <param name="height">
        /// 높이
        /// Height
        /// </param>
        /// <param name="filterMode">
        /// 필터 모드
        /// Filter Mode
        /// </param>
        /// <param name="spriteMetaData">
        /// 스프라이트의 메타 데이터
        /// Sprite's metadata
        /// </param>
        /// <returns></returns>
        public static Sprite SpriteFromColor(Color color, int width = 1, int height = 1, FilterMode filterMode = FilterMode.Point, SpriteMetaData spriteMetaData = null)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(SpriteFromColor));

            if (color == null)
                color = Color.white;
            if (width < 1)
                width = 1;
            if (height < 1)
                height = 1;

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = filterMode;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                    texture.SetPixel(x, y, color);
            }
            texture.Apply();
            return GetSprite(texture, spriteMetaData);
        }

        /// <summary>
        /// 색상을 스프라이트로 변환합니다 (Unity API를 사용하기 때문에 메인 스레드에서 실행해야 합니다)
        /// Convert color to sprite (Since the Unity API is used, we need to run it on the main thread)
        /// </summary>
        /// <param name="color">
        /// 색상
        /// Color
        /// </param>
        /// <param name="alpha">
        /// 알파 텍스쳐
        /// Alpha
        /// </param>
        /// <param name="spriteMetaData">
        /// 스프라이트의 메타 데이터
        /// Sprite's metadata
        /// </param>
        /// <returns></returns>
        public static Sprite SpriteFromColor(Color color, Texture2D alpha, SpriteMetaData spriteMetaData = null)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(SpriteFromColor));

            if (color == null)
                color = Color.white;

            Texture2D texture = new Texture2D(alpha.width, alpha.height, TextureFormat.RGBA32, false);
            texture.filterMode = alpha.filterMode;
            for (int x = 0; x < alpha.width; x++)
            {
                for (int y = 0; y < alpha.height; y++)
                    texture.SetPixel(x, y, new Color(color.r, color.g, color.b, alpha.GetPixel(x, y).a));
            }
            texture.Apply();
            return GetSprite(texture, spriteMetaData);
        }
    }

    public class TextureMetaData
    {
        [JsonProperty("Filter Mode")] public FilterMode filterMode = FilterMode.Point;
        [JsonProperty("Mipmap Use")] public bool mipmapUse = true;
        [JsonProperty("Compression")] public CompressionType compressionType = CompressionType.none;

        public enum CompressionType
        {
            none,
            normal,
            highQuality
        }
    }

    public class SpriteMetaData
    {
        [JsonProperty("Rect")] public JRect rect = new JRect(float.MinValue);
        [JsonProperty("Pivot")] public JVector2 pivot = new JVector2(0.5f);
        [JsonProperty("Pixels Per Unit")] public float pixelsPerUnit = 100;
        [JsonProperty("Border")] public JVector4 border = JVector4.zero;

        public void RectMinMax(float width, float height)
        {
            if (rect.x <= float.MinValue)
            {
                rect.x = 0;
                rect.y = 0;
                rect.width = width;
                rect.height = height;
            }

            if (rect.width < 1)
                rect.width = 1;
            if (rect.width > width)
                rect.width = width;
            if (rect.height < 1)
                rect.height = 1;
            if (rect.height > height)
                rect.height = height;

            if (rect.x < 0)
                rect.x = 0;
            if (rect.x > width - rect.width)
                rect.x = width - rect.width;
            if (rect.y < 0)
                rect.y = 0;
            if (rect.y > height - rect.height)
                rect.y = height - rect.height;
        }

        public void PixelsPreUnitMinSet()
        {
            if (pixelsPerUnit < 0.01f)
                pixelsPerUnit = 0.01f;
        }
    }



    public class SoundData
    {
        public SoundData(SoundCategory category, string subtitle, bool isBGM, SoundMetaData[] sounds)
        {
            this.category = category;
            this.subtitle = subtitle;
            this.isBGM = isBGM;
            this.sounds = sounds;
        }

        public SoundCategory category { get; } = SoundCategory.master;
        public string subtitle { get; } = "";
        public bool isBGM { get; } = false;
        public SoundMetaData[] sounds { get; } = new SoundMetaData[0];

        public enum SoundCategory
        {
            master,
            music,
            record
        }
    }

    public class SoundMetaData
    {
        public SoundMetaData(string path, bool stream, float pitch, float tempo, AudioClip audioClip)
        {
            this.path = path;
            this.stream = stream;
            this.pitch = pitch;
            this.tempo = tempo;
            this.audioClip = audioClip;
        }

        public string path { get; } = "";
        public bool stream { get; } = false;
        public float pitch { get; } = 1;
        public float tempo { get; } = 1;

        [JsonIgnore] public AudioClip audioClip { get; }
    }



    public class NotPlayModeSearchMethodException : Exception
    {
        /// <summary>
        /// Search function cannot be used outside of play mode
        /// 플레이 모드가 아니면 Search 함수를 사용할 수 없습니다
        /// </summary>
        public NotPlayModeSearchMethodException() : base("Search function cannot be used outside of play mode\n플레이 모드가 아니면 Search 함수를 사용할 수 없습니다") { }
    }
}