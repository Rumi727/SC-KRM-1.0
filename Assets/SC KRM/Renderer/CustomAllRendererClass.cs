using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using SCKRM.Resource;

namespace SCKRM.Renderer
{
    [AddComponentMenu("")]
    public abstract class CustomAllRenderer : MonoBehaviour, IRefresh
    {
        [SerializeField] string _nameSpace = "";
        public string nameSpace { get => _nameSpace; set => _nameSpace = value; }

        [SerializeField] string _path = "";
        public string path { get => _path; set => _path = value; }

        public NameSpacePathPair nameSpacePathPair
        {
            get => new NameSpacePathPair(nameSpace, path);
            set
            {
                nameSpace = value.nameSpace;
                path = value.path;
            }
        }

        public abstract void Refresh();
    }

    public struct NameSpacePathPair
    {
        public string path;
        public string nameSpace;

        public NameSpacePathPair(string path)
        {
            nameSpace = "";
            this.path = path;
        }

        public NameSpacePathPair(string nameSpace, string path)
        {
            this.nameSpace = nameSpace;
            this.path = path;
        }

        public static implicit operator string(NameSpacePathPair value) => value.ToString();

        public static implicit operator NameSpacePathPair(string value)
        {
            string nameSpace = ResourceManager.GetNameSpace(value, out value);
            return new NameSpacePathPair(value, nameSpace);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(nameSpace))
                return path;
            else
                return nameSpace + ":" + path;
        }
    }
}