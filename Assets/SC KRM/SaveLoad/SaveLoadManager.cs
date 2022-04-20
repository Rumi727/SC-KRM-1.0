using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCKRM.Threads;
using SCKRM.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SCKRM.SaveLoad
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SaveLoadAttribute : Attribute
    {
        public string name { get; private set; }

        public SaveLoadAttribute(string name = "") => this.name = name;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GeneralSaveLoadAttribute : SaveLoadAttribute
    {
        public GeneralSaveLoadAttribute(string name = "") : base(name)
        {

        }
    }

    public static class SaveLoadManager
    {
        public static List<SaveLoadClass> generalSLCList { get; [Obsolete("It is managed by the Kernel class. Please do not touch it.", false)] internal set; } = new List<SaveLoadClass>();

        public class SaveLoadClass
        {
            public string name { get; }
            public Type type { get; }
            public SaveLoadVariable<PropertyInfo>[] propertyInfos { get; } = new SaveLoadVariable<PropertyInfo>[0];
            public SaveLoadVariable<FieldInfo>[] fieldInfos { get; } = new SaveLoadVariable<FieldInfo>[0];

            public SaveLoadClass(string name, Type type, SaveLoadVariable<PropertyInfo>[] propertyInfos, SaveLoadVariable<FieldInfo>[] fieldInfos)
            {
                this.name = name;
                this.type = type;
                this.propertyInfos = propertyInfos;
                this.fieldInfos = fieldInfos;
            }

            public class SaveLoadVariable<T>
            {
                public T variableInfo { get; }
                public object defaultValue { get; }

                public SaveLoadVariable(T variableInfo, object defaultValue)
                {
                    this.variableInfo = variableInfo;
                    this.defaultValue = defaultValue;
                }
            }
        }

        public static void SaveLoadClassLoad<T>(out SaveLoadClass[] result) where T : SaveLoadAttribute
        {
            List<SaveLoadClass> saveLoadClassList = new List<SaveLoadClass>();
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            for (int assemblysIndex = 0; assemblysIndex < assemblys.Length; assemblysIndex++)
            {
                Type[] types = assemblys[assemblysIndex].GetTypes();
                for (int typesIndex = 0; typesIndex < types.Length; typesIndex++)
                {
                    Type type = types[typesIndex];
                    T saveLoadAttribute = type.GetCustomAttribute(typeof(T)) as T;
                    if (saveLoadAttribute == null)
                        continue;

                    string name;
                    if (saveLoadAttribute.name != "")
                        name = saveLoadAttribute.name;
                    else
                        name = type.Name;

                    #region 경고 및 기본값 저장
                    PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
                    List<SaveLoadClass.SaveLoadVariable<PropertyInfo>> propertyInfoList = new List<SaveLoadClass.SaveLoadVariable<PropertyInfo>>();
                    List<SaveLoadClass.SaveLoadVariable<FieldInfo>> fieldInfoList = new List<SaveLoadClass.SaveLoadVariable<FieldInfo>>();
                    for (int i = 0; i < propertyInfos.Length; i++)
                    {
                        PropertyInfo propertyInfo = propertyInfos[i];
                        bool ignore = propertyInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                        if (!propertyInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                            Debug.LogWarning(type.Name + " " + propertyInfo.PropertyType + " " + propertyInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                        else if (!ignore)
                            propertyInfoList.Add(new SaveLoadClass.SaveLoadVariable<PropertyInfo>(propertyInfo, propertyInfo.GetValue(propertyInfo.PropertyType)));
                    }

                    FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                    for (int i = 0; i < fieldInfos.Length; i++)
                    {
                        FieldInfo fieldInfo = fieldInfos[i];
                        bool ignore = fieldInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                        if (!fieldInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                            Debug.LogWarning(type.Name + " " + fieldInfo.FieldType + " " + fieldInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                        else if (!ignore)
                            fieldInfoList.Add(new SaveLoadClass.SaveLoadVariable<FieldInfo>(fieldInfo, fieldInfo.GetValue(fieldInfo.FieldType)));
                    }

                    saveLoadClassList.Add(new SaveLoadClass(name, type, propertyInfoList.ToArray(), fieldInfoList.ToArray()));
                    #endregion
                }
            }

            result = saveLoadClassList.ToArray();
        }

        public static void Save(List<SaveLoadClass> saveLoadClassList, string saveDataPath)
        {
            if (saveLoadClassList == null || saveDataPath == null || saveDataPath == "")
                return;
            else if (!Directory.Exists(saveDataPath))
                Directory.CreateDirectory(saveDataPath);

            for (int i = 0; i < saveLoadClassList.Count; i++)
            {
                SaveLoadClass saveLoadClass = saveLoadClassList[i];
                JObject jObject = new JObject();
                for (int j = 0; j < saveLoadClass.propertyInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<PropertyInfo> propertyInfo = saveLoadClass.propertyInfos[j];
                    jObject.Add(propertyInfo.variableInfo.Name, JToken.FromObject(propertyInfo.variableInfo.GetValue(propertyInfo.variableInfo.PropertyType)));
                }

                for (int j = 0; j < saveLoadClass.fieldInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<FieldInfo> fieldInfo = saveLoadClass.fieldInfos[j];
                    jObject.Add(fieldInfo.variableInfo.Name, JToken.FromObject(fieldInfo.variableInfo.GetValue(fieldInfo.variableInfo.FieldType)));
                }

                File.WriteAllText(PathTool.Combine(saveDataPath, saveLoadClass.name) + ".json", jObject.ToString());
            }
        }

        public static void Load(List<SaveLoadClass> saveLoadClassList, string loadDataPath)
        {
            if (saveLoadClassList == null || loadDataPath == null || loadDataPath == "")
                return;
            else if (!Directory.Exists(loadDataPath))
                Directory.CreateDirectory(loadDataPath);

            for (int i = 0; i < saveLoadClassList.Count; i++)
            {
                SaveLoadClass saveLoadClass = saveLoadClassList[i];
                string path = PathTool.Combine(loadDataPath, saveLoadClass.name) + ".json";
                if (!File.Exists(path))
                    continue;

                #region null 설정
                for (int j = 0; j < saveLoadClass.propertyInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<PropertyInfo> propertyInfo = saveLoadClass.propertyInfos[j];
                    propertyInfo.variableInfo.SetValue(propertyInfo.variableInfo.PropertyType, null);
                }

                for (int j = 0; j < saveLoadClass.fieldInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<FieldInfo> fieldInfo = saveLoadClass.fieldInfos[j];
                    fieldInfo.variableInfo.SetValue(fieldInfo.variableInfo.FieldType, null);
                }
                #endregion

                JsonConvert.DeserializeObject(File.ReadAllText(path), saveLoadClass.type);

                #region json을 로드 했는데도 null이면 기본값으로 되돌리기
                for (int j = 0; j < saveLoadClass.propertyInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<PropertyInfo> propertyInfo = saveLoadClass.propertyInfos[j];
                    if (propertyInfo.variableInfo.GetValue(propertyInfo.variableInfo.PropertyType) == null)
                        propertyInfo.variableInfo.SetValue(propertyInfo.variableInfo.PropertyType, propertyInfo.defaultValue);
                }

                for (int j = 0; j < saveLoadClass.fieldInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<FieldInfo> fieldInfo = saveLoadClass.fieldInfos[j];
                    if (fieldInfo.variableInfo.GetValue(fieldInfo.variableInfo.FieldType) == null)
                        fieldInfo.variableInfo.SetValue(fieldInfo.variableInfo.FieldType, fieldInfo.defaultValue);
                }
                #endregion
            }
        }
    }
}