using UnityEngine;
using System.Collections.Generic;

namespace SCKRM.Renderer
{
    [AddComponentMenu("")]
    public class CustomAllRenderer : MonoBehaviour
    {
        [SerializeField] string _nameSpace = "";
        public string nameSpace { get => _nameSpace; set => _nameSpace = value; }

        [SerializeField] string _path = "";
        public string path { get => _path; set => _path = value; }


        protected Queue<object> queue { get; } = new Queue<object>();

        /// <summary>
        /// Please put base.Update() when overriding
        /// </summary>
        protected virtual void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            if (queue.Count >= 1)
                Rerender();
        }

        /// <summary>
        /// CustomRendererManager에서 다른 스레드가 호출 할 수 있는 함수입니다. 유니티 API를 사용해선 안됩니다. (플레이 모드가 아닐땐 제외)
        /// You should not use the Unity API (Except when not in play mode)
        /// Please put base.ResourceReload() when overriding
        /// </summary>
        public virtual void ResourceReload() => Clear();

        public virtual void Rerender()
        {
            
        }

        /// <summary>
        /// Please put base.Clear() when overriding
        /// </summary>
        public virtual void Clear() => queue.Clear();
    }
}