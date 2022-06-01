using Newtonsoft.Json;
using SCKRM.ProjectSetting;
using SCKRM.Renderer;
using SCKRM.Threads;
using System;
using System.Collections.Generic;
using UCompile;
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
            public List<string> objectKey = new List<string>();
            public List<(MonoBehaviour monoBehaviour, IObjectPooling objectPooling)> objectPooling = new List<(MonoBehaviour, IObjectPooling)>();
        }



        void Awake() => SingletonCheck(this);

        /// <summary>
        /// 오브젝트를 미리 생성합니다
        /// </summary>
        /// <param name="objectKey">미리 생성할 오브젝트 키</param>
        public static void ObjectAdvanceCreate(string objectKey)
        {
            MonoBehaviour monoBehaviour = Resources.Load<MonoBehaviour>(Data.prefabList[objectKey]);
            IObjectPooling objectPooling = (IObjectPooling)monoBehaviour;
            if (objectPooling == null)
                return;

            ObjectAdd(objectKey, monoBehaviour, objectPooling);
        }

        /// <summary>
        /// 오브젝트를 리스트에 추가합니다
        /// </summary>
        /// <param name="objectKey">추가할 오브젝트의 키</param>
        /// <param name="monoBehaviour">추가할 오브젝트</param>
        public static void ObjectAdd(string objectKey, MonoBehaviour monoBehaviour, IObjectPooling objectPooling)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(ObjectAdvanceCreate));
            if (!Kernel.isPlaying)
                throw new NotPlayModeMethodException(nameof(ObjectAdvanceCreate));
            if (!InitialLoadManager.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(ObjectAdvanceCreate));
            if (instance == null)
                throw new NullScriptMethodException(nameof(ObjectPoolingSystem), nameof(ObjectRemove));
            if (monoBehaviour == null)
                throw new NullReferenceException(nameof(monoBehaviour));
            if (objectPooling == null)
                throw new NullReferenceException(nameof(objectPooling));

            ObjectRemove(objectKey, Instantiate(monoBehaviour, instance.transform), objectPooling);
        }

        /// <summary>
        /// 오브젝트가 리스트에 있는지 감지합니다 (리소스 폴더에 있는 프리팹은 알아서 감지하고 생성하니, 이 함수를 쓸 필요가 없습니다)
        /// </summary>
        /// <param name="objectKey">감지할 오브젝트 키</param>
        /// <returns></returns>
        public static bool ObjectContains(string objectKey) => objectList.objectKey.Contains(objectKey);

        /// <summary>
        /// 오브젝트를 생성합니다
        /// </summary>
        /// <param name="objectKey">생성할 오브젝트 키</param>
        /// <param name="parent">생성할 오브젝트가 자식으로갈 오브젝트</param>
        /// <returns></returns>
        public static (MonoBehaviour monoBehaviour, IObjectPooling objectPooling) ObjectCreate(string objectKey, Transform parent = null, bool autoRefresh = true)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(ObjectCreate));
            if (!Kernel.isPlaying)
                throw new NotPlayModeMethodException(nameof(ObjectCreate));
            if (!InitialLoadManager.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(ObjectCreate));
            if (instance == null)
                throw new NullScriptMethodException(nameof(ObjectPoolingSystem), nameof(ObjectCreate));

            if (objectList.objectKey.Contains(objectKey))
            {
                (MonoBehaviour monoBehaviour, IObjectPooling objectPooling) objectPoolingTuple = objectList.objectPooling[objectList.objectKey.IndexOf(objectKey)];

                IObjectPooling objectPooling = objectPoolingTuple.objectPooling;
                MonoBehaviour monoBehaviour = objectPoolingTuple.monoBehaviour;
                monoBehaviour.transform.SetParent(parent, false);
                monoBehaviour.gameObject.SetActive(true);

                objectPooling.objectKey = objectKey;
                
                {
                    int i = objectList.objectKey.IndexOf(objectKey);
                    objectList.objectKey.RemoveAt(i);
                    objectList.objectPooling.RemoveAt(i);
                }

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                objectPooling.isActived = true;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                if (autoRefresh)
                    RendererManager.Refresh(objectPooling.refreshableObjects, false);

                objectPooling.OnCreate();
                return objectPoolingTuple;
            }
            else if (Data.prefabList.ContainsKey(objectKey))
            {
                MonoBehaviour monoBehaviour = Instantiate(Resources.Load<MonoBehaviour>(Data.prefabList[objectKey]), parent);
                IObjectPooling objectPooling = (IObjectPooling)monoBehaviour;
                if (objectPooling == null)
                    return (null, null);

                monoBehaviour.name = objectKey;
                objectPooling.objectKey = objectKey;

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                objectPooling.isActived = true;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                if (autoRefresh)
                    RendererManager.Refresh(objectPooling.refreshableObjects, false);

                objectPooling.OnCreate();
                return (monoBehaviour, objectPooling);
            }

            return (null, null);
        }

        /// <summary>
        /// 오브젝트를 삭제합니다
        /// </summary>
        /// <param name="objectKey">지울 오브젝트 키</param>
        /// <param name="objectPooling">지울 오브젝트</param>
        public static bool ObjectRemove(string objectKey, MonoBehaviour monoBehaviour, IObjectPooling objectPooling)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(ObjectRemove));
            if (!Kernel.isPlaying)
                throw new NotPlayModeMethodException(nameof(ObjectRemove));
            if (!InitialLoadManager.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(ObjectCreate));
            if (instance == null)
                throw new NullScriptMethodException(nameof(ObjectPoolingSystem), nameof(ObjectRemove));
            if (monoBehaviour == null)
                throw new NullReferenceException(nameof(monoBehaviour));
            if (objectPooling == null)
                throw new NullReferenceException(nameof(objectPooling));

            monoBehaviour.gameObject.SetActive(false);
            monoBehaviour.transform.SetParent(instance.transform);

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            objectPooling.isActived = false;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

            objectList.objectKey.Add(objectKey);
            objectList.objectPooling.Add((monoBehaviour, objectPooling));

            return true;
        }
    }

    public class ObjectPooling : MonoBehaviour, IObjectPooling
    {
        public string objectKey { get; set; }

        public bool isRemoved => !isActived;

        public bool isActived { get; private set; }
        bool IObjectPooling.isActived { get => isActived; set => isActived = value; }



        IRefresh[] _refreshableObjects;
        public IRefresh[] refreshableObjects => _refreshableObjects = this.GetComponentsInChildrenFieldSave(_refreshableObjects, true);



        /// <summary>
        /// Please put base.OnCreate() when overriding
        /// </summary>
        public virtual void OnCreate() => IObjectPooling.OnCreateDefault(transform, this);

        /// <summary>
        /// Please put base.Remove() when overriding
        /// </summary>
        public virtual bool Remove() => IObjectPooling.RemoveDefault(this, this);
    }
}