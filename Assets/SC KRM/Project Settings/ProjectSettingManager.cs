using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCKRM.Resource;
using SCKRM.Threads;
using SCKRM.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SCKRM.ProjectSetting
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ProjectSettingAttribute : Attribute
    {
        public string name { get; private set; }

        public ProjectSettingAttribute(string name = "") => this.name = name;
    }

    public static class ProjectSettingManager
    {
        public static void Save()
        {
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            for (int assemblysIndex = 0; assemblysIndex < assemblys.Length; assemblysIndex++)
            {
                Type[] types = assemblys[assemblysIndex].GetTypes();
                for (int typesIndex = 0; typesIndex < types.Length; typesIndex++)
                {
                    Type type = types[typesIndex];
                    Save(type);
                }
            }
        }

        public static void Load()
        {
            if (!Directory.Exists(Kernel.projectSettingPath))
                Directory.CreateDirectory(Kernel.projectSettingPath);

            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            for (int assemblysIndex = 0; assemblysIndex < assemblys.Length; assemblysIndex++)
            {
                Type[] types = assemblys[assemblysIndex].GetTypes();
                for (int typesIndex = 0; typesIndex < types.Length; typesIndex++)
                {
                    Type type = types[typesIndex];
                    Load(type);
                }
            }
        }

        public static void Save(Type type)
        {
            ProjectSettingAttribute projectSettingAttribute = type.GetCustomAttribute(typeof(ProjectSettingAttribute)) as ProjectSettingAttribute;
            if (projectSettingAttribute == null)
                return;

            JObject jObject = new JObject();
            string name;
            if (projectSettingAttribute.name != "")
                name = projectSettingAttribute.name;
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
                        Debug.LogWarning(type.Name + " " + propertyInfo.PropertyType + " " + propertyInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 null이 될것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
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
                        Debug.LogWarning(type.Name + " " + fieldInfo.FieldType + " " + fieldInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 null이 될것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                }
            }

            File.WriteAllText(PathTool.Combine(Kernel.projectSettingPath, type.FullName) + "." + name + ".json", jObject.ToString());
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        public static void Load(Type type)
        {
            ProjectSettingAttribute projectSettingAttribute = type.GetCustomAttribute(typeof(ProjectSettingAttribute)) as ProjectSettingAttribute;
            if (projectSettingAttribute == null)
                return;

            string name;
            if (projectSettingAttribute.name != "")
                name = projectSettingAttribute.name;
            else
                name = type.Name;

            string path = PathTool.Combine(Kernel.projectSettingPath, type.FullName) + "." + name + ".json";
            if (!File.Exists(path))
                return;

            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo propertyInfo = propertyInfos[i];
                bool ignore = propertyInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                if (!propertyInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                    Debug.LogWarning(type.Name + " " + propertyInfo.PropertyType + " " + propertyInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 null이 될것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                else if (!ignore)
                    propertyInfo.SetValue(propertyInfo.PropertyType, null);
            }

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                FieldInfo fieldInfo = fieldInfos[i];

                bool ignore = fieldInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                if (!fieldInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                    Debug.LogWarning(type.Name + " " + fieldInfo.FieldType + " " + fieldInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 null이 될것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                else if (!ignore)
                    fieldInfo.SetValue(fieldInfo.FieldType, null);
            }

            JsonConvert.DeserializeObject(ResourceManager.GetText(path, true), type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">어트리뷰트를 감지 할 타입</param>
        /// <returns></returns>
        public static JObject Read(Type type)
        {
            IEnumerable<Attribute> attributes = type.GetCustomAttributes(typeof(ProjectSettingAttribute));
            foreach (var attribute in attributes)
            {
                ProjectSettingAttribute ProjectSettingAttribute = attribute as ProjectSettingAttribute;
                string name;
                if (ProjectSettingAttribute.name != "")
                    name = ProjectSettingAttribute.name;
                else
                    name = type.Name;

                string path = PathTool.Combine(Kernel.projectSettingPath, type.FullName) + "." + name + ".json";
                JObject jObject = JObject.Parse(ResourceManager.GetText(path, true));
                return jObject;
            }

            return null;
        }
    }
}