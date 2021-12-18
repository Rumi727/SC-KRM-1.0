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
        public static Dictionary<string, object> defaultList { get; } = new Dictionary<string, object>();
        public static List<VariableType> variableTypeList { get; } = new List<VariableType>();

        public class VariableType
        {
            public string name { get; }
            public Type type { get; }
            public Info<PropertyInfo>[] propertyInfos { get; } = new Info<PropertyInfo>[0];
            public Info<FieldInfo>[] fieldInfos { get; } = new Info<FieldInfo>[0];

            public VariableType(string name, Type type, Info<PropertyInfo>[] propertyInfos, Info<FieldInfo>[] fieldInfos)
            {
                this.name = name;
                this.type = type;
                this.propertyInfos = propertyInfos;
                this.fieldInfos = fieldInfos;
            }

            public class Info<T>
            {
                public T variableInfo { get; }
                public object defaultValue { get; }

                public Info(T variableInfo, object defaultValue)
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
                    List<VariableType.Info<PropertyInfo>> propertyInfoList = new List<VariableType.Info<PropertyInfo>>();
                    List<VariableType.Info<FieldInfo>> fieldInfoList = new List<VariableType.Info<FieldInfo>>();
                    for (int i = 0; i < propertyInfos.Length; i++)
                    {
                        PropertyInfo propertyInfo = propertyInfos[i];
                        bool ignore = propertyInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                        if (!propertyInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                            Debug.LogWarning(type.Name + " " + propertyInfo.PropertyType + " " + propertyInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                        else if (!ignore)
                            propertyInfoList.Add(new VariableType.Info<PropertyInfo>(propertyInfo, propertyInfo.GetValue(propertyInfo.PropertyType)));
                    }

                    FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                    for (int i = 0; i < fieldInfos.Length; i++)
                    {
                        FieldInfo fieldInfo = fieldInfos[i];
                        bool ignore = fieldInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
                        if (!fieldInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Any() && !ignore)
                            Debug.LogWarning(type.Name + " " + fieldInfo.FieldType + " " + fieldInfo.Name + " 에 [JsonProperty] 어트리뷰트가 추가되어있지 않습니다.\n이 변수는 로드되지 않을것입니다.\n무시를 원하신다면 [JsonIgnore] 어트리뷰트를 추가해주세요");
                        else if (!ignore)
                            fieldInfoList.Add(new VariableType.Info<FieldInfo>(fieldInfo, fieldInfo.GetValue(fieldInfo.FieldType)));
                    }

                    variableTypeList.Add(new VariableType(name, type, propertyInfoList.ToArray(), fieldInfoList.ToArray()));
                    #endregion
                }
            }
        }

        public static void Save()
        {
            if (!Directory.Exists(Kernel.saveDataPath))
                Directory.CreateDirectory(Kernel.saveDataPath);

            for (int i = 0; i < variableTypeList.Count; i++)
            {
                VariableType variableType = variableTypeList[i];
                JObject jObject = new JObject();
                for (int j = 0; j < variableType.propertyInfos.Length; j++)
                {
                    VariableType.Info<PropertyInfo> propertyInfo = variableType.propertyInfos[j];
                    jObject.Add(propertyInfo.variableInfo.Name, JToken.FromObject(propertyInfo.variableInfo.GetValue(propertyInfo.variableInfo.PropertyType)));
                }

                for (int j = 0; j < variableType.fieldInfos.Length; j++)
                {
                    VariableType.Info<FieldInfo> fieldInfo = variableType.fieldInfos[j];
                    jObject.Add(fieldInfo.variableInfo.Name, JToken.FromObject(fieldInfo.variableInfo.GetValue(fieldInfo.variableInfo.FieldType)));
                }

                File.WriteAllText(PathTool.Combine(Kernel.saveDataPath, variableType.name) + ".json", jObject.ToString());
            }
        }

        public static void Load()
        {
            if (!Directory.Exists(Kernel.saveDataPath))
                Directory.CreateDirectory(Kernel.saveDataPath);

            for (int i = 0; i < variableTypeList.Count; i++)
            {
                VariableType variableType = variableTypeList[i];
                string path = PathTool.Combine(Kernel.saveDataPath, variableType.name) + ".json";
                if (!File.Exists(path))
                    continue;

                #region 경고 및 기본값 저장과 null 설정
                for (int j = 0; j < variableType.propertyInfos.Length; j++)
                {
                    VariableType.Info<PropertyInfo> propertyInfo = variableType.propertyInfos[j];
                    propertyInfo.variableInfo.SetValue(propertyInfo.variableInfo.PropertyType, null);
                }

                for (int j = 0; j < variableType.fieldInfos.Length; j++)
                {
                    VariableType.Info<FieldInfo> fieldInfo = variableType.fieldInfos[j];
                    fieldInfo.variableInfo.SetValue(fieldInfo.variableInfo.FieldType, null);
                }
                #endregion

                JsonConvert.DeserializeObject(File.ReadAllText(path), variableType.type);

                #region json을 로드 했는데도 null이면 기본값으로 되돌리기
                for (int j = 0; j < variableType.propertyInfos.Length; j++)
                {
                    VariableType.Info<PropertyInfo> propertyInfo = variableType.propertyInfos[j];
                    if (propertyInfo.variableInfo.GetValue(propertyInfo.variableInfo.PropertyType) == null)
                        propertyInfo.variableInfo.SetValue(propertyInfo.variableInfo.PropertyType, propertyInfo.defaultValue);
                }

                for (int j = 0; j < variableType.fieldInfos.Length; j++)
                {
                    VariableType.Info<FieldInfo> fieldInfo = variableType.fieldInfos[j];
                    if (fieldInfo.variableInfo.GetValue(fieldInfo.variableInfo.FieldType) == null)
                        fieldInfo.variableInfo.SetValue(fieldInfo.variableInfo.FieldType, fieldInfo.defaultValue);
                }
                #endregion
            }
        }
    }
}