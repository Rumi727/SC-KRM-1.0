using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SCKRM.Renderer
{
    [AddComponentMenu("")]
    public class CustomAllRenderer : MonoBehaviour
    {
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
    }
}