using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using SCKRM.Resource;
using System.Threading;

namespace SCKRM.Renderer
{
    [AddComponentMenu("")]
    public abstract class CustomAllRenderer : MonoBehaviour, IRefresh
    {
        int nameSpaceLock = 0;
        [SerializeField] string _nameSpace = "";
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

        int pathLock = 0;
        [SerializeField] string _path = "";
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