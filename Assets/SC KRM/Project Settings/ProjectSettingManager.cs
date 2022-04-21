using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCKRM.Resource;
using SCKRM.SaveLoad;
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
    public sealed class ProjectSettingSaveLoadAttribute : SaveLoadAttribute
    {

    }

    public static class ProjectSettingManager
    {
        public static SaveLoadClass[] projectSettingSLCList { get; [Obsolete("It is managed by the Kernel class. Please do not touch it.", false)] internal set; } = new SaveLoadClass[0];

        public static JObject Read(Type type)
        {
            string path = PathTool.Combine(Kernel.projectSettingPath, type.FullName) + ".json";
            JObject jObject = JObject.Parse(ResourceManager.GetText(path, true));
            return jObject;
        }
    }
}