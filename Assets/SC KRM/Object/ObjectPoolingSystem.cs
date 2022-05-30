using Newtonsoft.Json;
using SCKRM.ProjectSetting;
using SCKRM.Renderer;
using SCKRM.Threads;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SCKRM.Object
{
    [AddComponentMenu("커널/Object/오브젝트 풀링 시스템", 0)]
    public sealed class ObjectPoolingSystem : Manager<ObjectPoolingSystem>
    {
        [ProjectSettingSaveLoad]
        public sealed class Data
        {
            [JsonProperty] public static Dictionary<string, string> prefabList { get; set; } = new Dictionary<string, string>();
        }

        static ObjectList objectList { get; } = new ObjectList();
        class ObjectList
        {
            public List<string> ObjectKey = new List<string>();
            public List<ObjectPooling> Object = new List<ObjectPooling>();
        }



        void Awake() => SingletonCheck(this);

        /// <summary>
        /// 오브젝트를 미리 생성합니다
        /// </summary>
        /// <param name="objectKey">미리 생성할 오브젝트 키</param>
        public static void ObjectAdvanceCreate(string objectKey) => ObjectAdd(objectKey, Resources.Load<ObjectPooling>(Data.prefabList[objectKey]));

        /// <summary>
        /// 오브젝트를 리스트에 추가합니다
        /// </summary>
        /// <param name="objectKey">추가할 오브젝트의 키</param>
        /// <param name="gameObject">추가할 오브젝트</param>
        public static void ObjectAdd(string objectKey, ObjectPooling gameObject)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(ObjectAdvanceCreate));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(ObjectAdvanceCreate));
#endif
            if (!InitialLoadManager.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(ObjectAdvanceCreate));
            if (instance == null)
                throw new NullScriptMethodException(nameof(ObjectPoolingSystem), nameof(ObjectRemove));

            ObjectRemove(objectKey, Instantiate(gameObject, instance.transform));
        }

        /// <summary>
        /// 오브젝트가 리스트에 있는지 감지합니다 (리소스 폴더에 있는 프리팹은 알아서 감지하고 생성하니, 이 함수를 쓸 필요가 없습니다)
        /// </summary>
        /// <param name="objectKey">감지할 오브젝트 키</param>
        /// <returns></returns>
        public static bool ObjectContains(string objectKey) => objectList.ObjectKey.Contains(objectKey);

        /// <summary>
        /// 오브젝트를 생성합니다
        /// </summary>
        /// <param name="objectKey">생성할 오브젝트 키</param>
        /// <returns></returns>
        public static ObjectPooling ObjectCreate(string objectKey) => ObjectCreate(objectKey, null);

        /// <summary>
        /// 오브젝트를 생성합니다
        /// </summary>
        /// <param name="objectKey">생성할 오브젝트 키</param>
        /// <param name="parent">생성할 오브젝트가 자식으로갈 오브젝트</param>
        /// <returns></returns>
        public static ObjectPooling ObjectCreate(string objectKey, Transform parent)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(ObjectCreate));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(ObjectCreate));
#endif
            if (!InitialLoadManager.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(ObjectCreate));
            if (instance == null)
                throw new NullScriptMethodException(nameof(ObjectPoolingSystem), nameof(ObjectCreate));

            if (objectList.ObjectKey.Contains(objectKey))
            {
                ObjectPooling objectPooling = objectList.Object[objectList.ObjectKey.IndexOf(objectKey)];
                objectPooling.transform.SetParent(parent, false);
                objectPooling.gameObject.SetActive(true);
                objectPooling.objectKey = objectKey;
                
                {
                    int i = objectList.ObjectKey.IndexOf(objectKey);
                    objectList.ObjectKey.RemoveAt(i);
                    objectList.Object.RemoveAt(i);
                }

                RendererManager.Rerender(objectPooling.renderers, false);

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                objectPooling._actived = true;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                objectPooling.OnCreate();
                return objectPooling;
            }
            else if (Data.prefabList.ContainsKey(objectKey))
            {
                ObjectPooling objectPooling = Instantiate(Resources.Load<ObjectPooling>(Data.prefabList[objectKey]), parent);
                objectPooling.name = objectKey;
                objectPooling.objectKey = objectKey;

                RendererManager.Rerender(objectPooling.renderers, false);

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                objectPooling._actived = true;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                objectPooling.OnCreate();
                return objectPooling;
            }

            return null;
        }

        /// <summary>
        /// 오브젝트를 삭제합니다
        /// </summary>
        /// <param name="objectKey">지울 오브젝트 키</param>
        /// <param name="objectPooling">지울 오브젝트</param>
        public static void ObjectRemove(string objectKey, ObjectPooling objectPooling)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(ObjectRemove));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(ObjectRemove));
#endif
            if (!InitialLoadManager.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(ObjectCreate));
            if (instance == null)
                throw new NullScriptMethodException(nameof(ObjectPoolingSystem), nameof(ObjectRemove));

            objectPooling.gameObject.SetActive(false);
            objectPooling.transform.SetParent(instance.transform);

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            objectPooling._actived = false;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

            objectList.ObjectKey.Add(objectKey);
            objectList.Object.Add(objectPooling);
        }
    }

    public class ObjectPooling : MonoBehaviour
    {
        public string objectKey { get; set; } = "";

        [Obsolete("It is managed by the ObjectPoolingSystem class. Please do not touch it.", false)]
        internal bool _actived;
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        public bool actived => _actived;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

        [NonSerialized] CustomAllRenderer[] _renderers;
        public CustomAllRenderer[] renderers
        {
            get
            {
                if (_renderers == null)
                    _renderers = GetComponentsInChildren<CustomAllRenderer>(true);

                return _renderers;
            }
        }

        /// <summary>
        /// Please put base.OnCreate() when overriding
        /// </summary>
        public virtual void OnCreate()
        {
            gameObject.name = objectKey;

            transform.localPosition = Vector3.zero;

            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Please put base.Remove() when overriding
        /// </summary>
        public virtual void Remove()
        {
            if (!actived)
                return;

            ObjectPoolingSystem.ObjectRemove(objectKey, this);
            gameObject.name = objectKey;

            transform.localPosition = Vector3.zero;

            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;

            StopAllCoroutines();
        }
    }
}