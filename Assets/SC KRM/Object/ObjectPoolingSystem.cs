using Newtonsoft.Json;
using SCKRM.ProjectSetting;
using SCKRM.Renderer;
using SCKRM.Threads;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Object
{
    [AddComponentMenu("커널/Object/오브젝트 풀링 시스템", 0)]
    public sealed class ObjectPoolingSystem : MonoBehaviour
    {
        [ProjectSetting("PrefabList")]
        public sealed class Data
        {
            [JsonProperty] public static Dictionary<string, string> prefabList { get; set; } = new Dictionary<string, string>();
        }

        public static ObjectPoolingSystem instance { get; private set; }

        static ObjectList objectList { get; } = new ObjectList();
        class ObjectList
        {
            public List<string> ObjectKey = new List<string>();
            public List<GameObject> Object = new List<GameObject>();
        }



        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        /// <summary>
        /// 오브젝트를 생성합니다
        /// </summary>
        /// <param name="ObjectKey">생성할 오브젝트 키</param>
        /// <returns></returns>
        public static GameObject ObjectCreate(string ObjectKey) => ObjectCreate(ObjectKey, null);

        /// <summary>
        /// 오브젝트를 생성합니다
        /// </summary>
        /// <param name="ObjectKey">생성할 오브젝트 키</param>
        /// <param name="Parent">생성할 오브젝트가 자식으로갈 오브젝트</param>
        /// <returns></returns>
        public static GameObject ObjectCreate(string ObjectKey, Transform Parent)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(ObjectCreate));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(ObjectCreate));
#endif
            if (instance == null)
                throw new NullScriptMethodException(nameof(ObjectPoolingSystem), nameof(ObjectCreate));

            if (objectList.ObjectKey.Contains(ObjectKey))
            {
                GameObject gameObject = objectList.Object[objectList.ObjectKey.IndexOf(ObjectKey)];
                gameObject.transform.SetParent(Parent, false);
                gameObject.SetActive(true);

                ObjectPooling objectPooling = gameObject.GetComponent<ObjectPooling>();
                if (objectPooling == null)
                    objectPooling = gameObject.AddComponent<ObjectPooling>();

                objectPooling.objectKey = ObjectKey;
                
                {
                    int i = objectList.ObjectKey.IndexOf(ObjectKey);
                    objectList.ObjectKey.RemoveAt(i);
                    objectList.Object.RemoveAt(i);
                }

                RendererManager.Rerender(gameObject.GetComponentsInChildren<CustomAllRenderer>(), false);

                objectPooling.OnCreate();
                return gameObject;
            }
            else if (Data.prefabList.ContainsKey(ObjectKey))
            {
                GameObject gameObject = Instantiate(Resources.Load<GameObject>(Data.prefabList[ObjectKey]), Parent);
                gameObject.name = ObjectKey;

                ObjectPooling objectPooling = gameObject.GetComponent<ObjectPooling>();
                if (objectPooling == null)
                    objectPooling = gameObject.AddComponent<ObjectPooling>();

                objectPooling.objectKey = ObjectKey;

                RendererManager.Rerender(gameObject.GetComponentsInChildren<CustomAllRenderer>(), false);

                objectPooling.OnCreate();
                return gameObject;
            }

            return null;
        }

        /// <summary>
        /// 오브젝트를 삭제합니다
        /// </summary>
        /// <param name="ObjectKey">지울 오브젝트 키</param>
        /// <param name="gameObject">지울 오브젝트</param>
        public static void ObjectRemove(string ObjectKey, GameObject gameObject)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(ObjectRemove));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(ObjectRemove));
#endif
            if (instance == null)
                throw new NullScriptMethodException(nameof(ObjectPoolingSystem), nameof(ObjectRemove));

            gameObject.SetActive(false);
            gameObject.transform.SetParent(instance.transform);

            objectList.ObjectKey.Add(ObjectKey);
            objectList.Object.Add(gameObject);
        }
    }

    /// <summary>
    /// Please put base.Remove() when overriding
    /// </summary>
    public class ObjectPooling : MonoBehaviour
    {
        public string objectKey { get; set; } = "";

        public virtual void OnCreate()
        {

        }

        public virtual void Remove()
        {
            ObjectPoolingSystem.ObjectRemove(objectKey, gameObject);
            gameObject.name = objectKey;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
            StopAllCoroutines();
        }
    }
}