using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using SCKRM.Resource;
using System.Threading;

namespace SCKRM.Renderer
{
    public abstract class CustomAllRenderer : MonoBehaviour, IRefreshable
    {
        /// <summary>
        /// 렌더링 할 파일의 네임스페이스
        /// </summary>
        public string nameSpace
        {
            get
            {
                while (Interlocked.CompareExchange(ref nameSpaceLock, 1, 0) != 0)
                    Thread.Sleep(1);

                string nameSpace = _nameSpace;

                Interlocked.Decrement(ref nameSpaceLock);
                return nameSpace;
            }
            set
            {
                while (Interlocked.CompareExchange(ref nameSpaceLock, 1, 0) != 0)
                    Thread.Sleep(1);

                _nameSpace = value;
                Interlocked.Decrement(ref nameSpaceLock);
            }
        }
        int nameSpaceLock = 0;
        [SerializeField] string _nameSpace = "";

        /// <summary>
        /// 렌더링 할 파일의 경로
        /// </summary>
        public string path
        {
            get
            {
                while (Interlocked.CompareExchange(ref pathLock, 1, 0) != 0)
                    Thread.Sleep(1);

                string path = _path;

                Interlocked.Decrement(ref pathLock);
                return path;
            }
            set
            {
                while (Interlocked.CompareExchange(ref pathLock, 1, 0) != 0)
                    Thread.Sleep(1);

                _path = value;

                Interlocked.Decrement(ref pathLock);
            }
        }
        int pathLock = 0;
        [SerializeField] string _path = "";

        /// <summary>
        /// 네임스페이스랑 경로랑 동시에 설정할 수 있습니다
        /// </summary>
        public NameSpacePathPair nameSpacePathPair
        {
            get => new NameSpacePathPair(nameSpace, path);
            set
            {
                nameSpace = value.nameSpace;
                path = value.path;
            }
        }

        /// <summary>
        /// 렌더러를 새로 고칩니다
        /// </summary>
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
            return new NameSpacePathPair(nameSpace, value);
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