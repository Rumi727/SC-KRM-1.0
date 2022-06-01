using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using SCKRM.Resource;

namespace SCKRM.Renderer
{
    [AddComponentMenu("")]
    public class CustomAllRenderer : MonoBehaviour
    {
        public NameSpacePathPair nameSpacePathPair
        {
            get => new NameSpacePathPair(nameSpace, path);
            set
            {
                nameSpace = value.nameSpace;
                path = value.path;
            }
        }

        [SerializeField] string _nameSpace = "";
        public string nameSpace { get => _nameSpace; set => _nameSpace = value; }

        [SerializeField] string _path = "";
        public string path { get => _path; set => _path = value; }

        /// <summary>
        /// CustomRendererManager에서 다른 스레드가 호출 할 수 있는 함수입니다. 유니티 API를 사용해선 안됩니다. (플레이 모드가 아닐땐 제외)
        /// You should not use the Unity API (Except when not in play mode)
        /// </summary>
        public virtual void Refresh()
        {
            
        }
    }

    public struct NameSpacePathPair
    {
        public string path;
        public string nameSpace;

        public NameSpacePathPair(string path, string nameSpace = "")
        {
            this.path = path;
            this.nameSpace = nameSpace;
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