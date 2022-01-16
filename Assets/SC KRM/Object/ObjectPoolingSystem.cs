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
        [ProjectSetting]
        public sealed class Data
        {
            [JsonProperty] public static Dictionary<string, string> prefabList { get; set; } = new Dictionary<string, string>();
        }

        public static ObjectPoolingSystem instance { get; private set; }

        static ObjectList objectList { get; } = new ObjectList();
        class ObjectList
        {
            public List<string> ObjectKey = new List<string>();
            public List<ObjectPooling> Object = new List<ObjectPooling>();
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
        public static ObjectPooling ObjectCreate(string ObjectKey) => ObjectCreate(ObjectKey, null);

        /// <summary>
        /// 오브젝트를 생성합니다
        /// </summary>
        /// <param name="ObjectKey">생성할 오브젝트 키</param>
        /// <param name="Parent">생성할 오브젝트가 자식으로갈 오브젝트</param>
        /// <returns></returns>
        public static ObjectPooling ObjectCreate(string ObjectKey, Transform Parent)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(ObjectCreate));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(ObjectCreate));
#endif
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(ObjectCreate));
            if (instance == null)
                throw new NullScriptMethodException(nameof(ObjectPoolingSystem), nameof(ObjectCreate));

            if (objectList.ObjectKey.Contains(ObjectKey))
            {
                ObjectPooling objectPooling = objectList.Object[objectList.ObjectKey.IndexOf(ObjectKey)];
                objectPooling.transform.SetParent(Parent, false);
                objectPooling.gameObject.SetActive(true);
                objectPooling.objectKey = ObjectKey;
                
                {
                    int i = objectList.ObjectKey.IndexOf(ObjectKey);
                    objectList.ObjectKey.RemoveAt(i);
                    objectList.Object.RemoveAt(i);
                }

                RendererManager.Rerender(objectPooling.GetComponentsInChildren<CustomAllRenderer>(), false).Forget();

                objectPooling.actived = true;
                objectPooling.OnCreate();
                return objectPooling;
            }
            else if (Data.prefabList.ContainsKey(ObjectKey))
            {
                GameObject gameObject = Instantiate(Resources.Load<GameObject>(Data.prefabList[ObjectKey]), Parent);
                gameObject.name = ObjectKey;

                ObjectPooling objectPooling = gameObject.GetComponent<ObjectPooling>();
                if (objectPooling == null)
                    objectPooling = gameObject.AddComponent<ObjectPooling>();

                objectPooling.objectKey = ObjectKey;

                RendererManager.Rerender(gameObject.GetComponentsInChildren<CustomAllRenderer>(), false).Forget();

                objectPooling.actived = true;
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
            if (!Kernel.isInitialLoadEnd)
                throw new NotInitialLoadEndMethodException(nameof(ObjectCreate));
            if (instance == null)
                throw new NullScriptMethodException(nameof(ObjectPoolingSystem), nameof(ObjectRemove));

            objectPooling.gameObject.SetActive(false);
            objectPooling.transform.SetParent(instance.transform);

            objectPooling.actived = false;

            objectList.ObjectKey.Add(objectKey);
            objectList.Object.Add(objectPooling);
        }
    }

    public class ObjectPooling : MonoBehaviour
    {
        public string objectKey { get; set; } = "";
        public bool actived { get; set; } = false;

        public virtual void OnCreate()
        {
            
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

            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
                rectTransform.anchoredPosition = Vector2.zero;
            else
                transform.localPosition = Vector3.zero;

            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;

            StopAllCoroutines();
        }
    }
}