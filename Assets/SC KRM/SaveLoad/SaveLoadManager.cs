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
    public sealed class SaveLoadAttribute : Attribute
    {
        public string name { get; private set; }

        public SaveLoadAttribute(string name = "") => this.name = name;
    }

    public static class SaveLoadManager
    {
        public static List<SaveLoadClass> generalSLCList { get; } = new List<SaveLoadClass>();

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

        public static void VariableInfoLoad()
        {
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

                    generalSLCList.Add(new SaveLoadClass(name, type, propertyInfoList.ToArray(), fieldInfoList.ToArray()));
                    #endregion
                }
            }
        }

        public static void Save()
        {
            if (!Directory.Exists(Kernel.saveDataPath))
                Directory.CreateDirectory(Kernel.saveDataPath);

            for (int i = 0; i < generalSLCList.Count; i++)
            {
                SaveLoadClass variableType = generalSLCList[i];
                JObject jObject = new JObject();
                for (int j = 0; j < variableType.propertyInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<PropertyInfo> propertyInfo = variableType.propertyInfos[j];
                    jObject.Add(propertyInfo.variableInfo.Name, JToken.FromObject(propertyInfo.variableInfo.GetValue(propertyInfo.variableInfo.PropertyType)));
                }

                for (int j = 0; j < variableType.fieldInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<FieldInfo> fieldInfo = variableType.fieldInfos[j];
                    jObject.Add(fieldInfo.variableInfo.Name, JToken.FromObject(fieldInfo.variableInfo.GetValue(fieldInfo.variableInfo.FieldType)));
                }

                File.WriteAllText(PathTool.Combine(Kernel.saveDataPath, variableType.name) + ".json", jObject.ToString());
            }
        }

        public static void Load()
        {
            if (!Directory.Exists(Kernel.saveDataPath))
                Directory.CreateDirectory(Kernel.saveDataPath);

            for (int i = 0; i < generalSLCList.Count; i++)
            {
                SaveLoadClass variableType = generalSLCList[i];
                string path = PathTool.Combine(Kernel.saveDataPath, variableType.name) + ".json";
                if (!File.Exists(path))
                    continue;

                #region null 설정
                for (int j = 0; j < variableType.propertyInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<PropertyInfo> propertyInfo = variableType.propertyInfos[j];
                    propertyInfo.variableInfo.SetValue(propertyInfo.variableInfo.PropertyType, null);
                }

                for (int j = 0; j < variableType.fieldInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<FieldInfo> fieldInfo = variableType.fieldInfos[j];
                    fieldInfo.variableInfo.SetValue(fieldInfo.variableInfo.FieldType, null);
                }
                #endregion

                JsonConvert.DeserializeObject(File.ReadAllText(path), variableType.type);

                #region json을 로드 했는데도 null이면 기본값으로 되돌리기
                for (int j = 0; j < variableType.propertyInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<PropertyInfo> propertyInfo = variableType.propertyInfos[j];
                    if (propertyInfo.variableInfo.GetValue(propertyInfo.variableInfo.PropertyType) == null)
                        propertyInfo.variableInfo.SetValue(propertyInfo.variableInfo.PropertyType, propertyInfo.defaultValue);
                }

                for (int j = 0; j < variableType.fieldInfos.Length; j++)
                {
                    SaveLoadClass.SaveLoadVariable<FieldInfo> fieldInfo = variableType.fieldInfos[j];
                    if (fieldInfo.variableInfo.GetValue(fieldInfo.variableInfo.FieldType) == null)
                        fieldInfo.variableInfo.SetValue(fieldInfo.variableInfo.FieldType, fieldInfo.defaultValue);
                }
                #endregion
            }
        }
    }
}