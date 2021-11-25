using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCKRM.Threads;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SCKRM.SaveLoad
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class SaveLoadAttribute : Attribute
    {
        public string name { get; private set; }

        public SaveLoadAttribute(string name = "") => this.name = name;
    }

    public static class SaveLoadManager
    {
        public static void Save()
        {
            if (!Directory.Exists(Kernel.saveDataPath))
                Directory.CreateDirectory(Kernel.saveDataPath);

            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            for (int assemblysIndex = 0; assemblysIndex < assemblys.Length; assemblysIndex++)
            {
                Type[] types = assemblys[assemblysIndex].GetTypes();
                for (int typesIndex = 0; typesIndex < types.Length; typesIndex++)
                {
                    Type type = types[typesIndex];
                    SaveLoadAttribute saveLoadAttribute = type.GetCustomAttribute(typeof(SaveLoadAttribute)) as SaveLoadAttribute;
                    if (saveLoadAttribute == null)
                        continue;

                    JObject jObject = new JObject();
                    string name;
                    if (saveLoadAttribute.name != "")
                        name = saveLoadAttribute.name;
                    else
                        name = type.Name;

                    PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
                    for (int i = 0; i < propertyInfos.Length; i++)
                    {
                        PropertyInfo propertyInfo = propertyInfos[i];
                        if (!propertyInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any())
                        {
                            jObject.Add(propertyInfo.Name, JToken.FromObject(propertyInfo.GetValue(propertyInfo.PropertyType)));

                            if (!propertyInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any())
                                Debug.LogWarning(type.Name + " " + propertyInfo.PropertyType + " " + propertyInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                        }
                    }

                    FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                    for (int i = 0; i < fieldInfos.Length; i++)
                    {
                        FieldInfo fieldInfo = fieldInfos[i];
                        if (!fieldInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any())
                        {
                            jObject.Add(fieldInfo.Name, JToken.FromObject(fieldInfo.GetValue(fieldInfo.FieldType)));

                            if (!fieldInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any())
                                Debug.LogWarning(type.Name + " " + fieldInfo.FieldType + " " + fieldInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                        }
                    }

                    File.WriteAllText(KernelMethod.PathCombine(Kernel.saveDataPath, name) + ".json", jObject.ToString());
                }
            }
        }

        public static void Load(ThreadMetaData threadMetaData)
        {
            if (!Directory.Exists(Kernel.saveDataPath))
                Directory.CreateDirectory(Kernel.saveDataPath);

            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            for (int assemblysIndex = 0; assemblysIndex < assemblys.Length; assemblysIndex++)
            {
                Type[] types = assemblys[assemblysIndex].GetTypes();
                for (int typesIndex = 0; typesIndex < types.Length; typesIndex++)
                {
                    Type type = types[typesIndex];
                    SaveLoadAttribute saveLoadAttribute = type.GetCustomAttribute(typeof(SaveLoadAttribute)) as SaveLoadAttribute;
                    if (saveLoadAttribute == null)
                        continue;

                    string name;
                    if (saveLoadAttribute.name != "")
                        name = saveLoadAttribute.name;
                    else
                        name = type.Name;

                    string path = KernelMethod.PathCombine(Kernel.saveDataPath, name) + ".json";
                    if (!File.Exists(path))
                        continue;

                    PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
                    for (int i = 0; i < propertyInfos.Length; i++)
                    {
                        PropertyInfo propertyInfo = propertyInfos[i];
                        bool ignore = propertyInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                        if (!propertyInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                            Debug.LogWarning(type.Name + " " + propertyInfo.PropertyType + " " + propertyInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                    }

                    FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                    for (int i = 0; i < fieldInfos.Length; i++)
                    {
                        FieldInfo fieldInfo = fieldInfos[i];

                        bool ignore = fieldInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                        if (!fieldInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                            Debug.LogWarning(type.Name + " " + fieldInfo.FieldType + " " + fieldInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                    }

                    JsonConvert.DeserializeObject(File.ReadAllText(path), type);
                }
            }
        }
    }
}