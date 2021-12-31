using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCKRM.Resource;
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

            File.WriteAllText(PathTool.Combine(Kernel.projectSettingPath, type.FullName) + ".json", jObject.ToString());
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        public static void Load(Type type)
        {
            ProjectSettingAttribute projectSettingAttribute = type.GetCustomAttribute(typeof(ProjectSettingAttribute)) as ProjectSettingAttribute;
            if (projectSettingAttribute == null)
                return;

            string path = PathTool.Combine(Kernel.projectSettingPath, type.FullName) + ".json";
            if (!File.Exists(path))
                return;

            #region 경고 및 기본값 저장과 null 설정
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
            Dictionary<string, object> defaultList = new Dictionary<string, object>();
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo propertyInfo = propertyInfos[i];
                bool ignore = propertyInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                if (!propertyInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                    Debug.LogWarning(type.Name + " " + propertyInfo.PropertyType + " " + propertyInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                else
                {
                    defaultList.Add(propertyInfo.Name, propertyInfo.GetValue(propertyInfo.PropertyType));
                    propertyInfo.SetValue(propertyInfo.PropertyType, null);
                }
            }

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                FieldInfo fieldInfo = fieldInfos[i];

                bool ignore = fieldInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                if (!fieldInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                    Debug.LogWarning(type.Name + " " + fieldInfo.FieldType + " " + fieldInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                else
                {
                    defaultList.Add(fieldInfo.Name, fieldInfo.GetValue(fieldInfo.FieldType));
                    fieldInfo.SetValue(fieldInfo.FieldType, null);
                }
            }
            #endregion

            JsonConvert.DeserializeObject(File.ReadAllText(path), type);

            #region json을 로드 했는데도 null이면 기본값으로 되돌리기
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo propertyInfo = propertyInfos[i];
                bool ignore = propertyInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                if (!propertyInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                    Debug.LogWarning(type.Name + " " + propertyInfo.PropertyType + " " + propertyInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                else
                {
                    if (propertyInfo.GetValue(propertyInfo.PropertyType) == null)
                        propertyInfo.SetValue(propertyInfo.PropertyType, defaultList[propertyInfo.Name]);
                }
            }

            for (int i = 0; i < fieldInfos.Length; i++)
            {
                FieldInfo fieldInfo = fieldInfos[i];

                bool ignore = fieldInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                if (!fieldInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                    Debug.LogWarning(type.Name + " " + fieldInfo.FieldType + " " + fieldInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                else
                {
                    if (fieldInfo.GetValue(fieldInfo.FieldType) == null)
                        fieldInfo.SetValue(fieldInfo.FieldType, defaultList[fieldInfo.Name]);
                }
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">어트리뷰트를 감지 할 타입</param>
        /// <returns></returns>
        public static JObject Read(Type type)
        {
            string path = PathTool.Combine(Kernel.projectSettingPath, type.FullName) + ".json";
            JObject jObject = JObject.Parse(ResourceManager.GetText(path, true));
            return jObject;
        }
    }
}