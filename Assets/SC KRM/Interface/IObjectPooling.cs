using SCKRM.Renderer;
using System;
using UnityEngine;

namespace SCKRM.Object
{
    public interface IObjectPooling : IRemove
    {
        string objectKey { get; set; }
        bool isActived { get; [Obsolete("It is managed by the ObjectPoolingSystem class. Please do not touch it.")] internal set; }

        IRefresh[] refreshableObjects { get; }

        void OnCreate();

        public static void OnCreateDefault(Transform transform, IObjectPooling objectPooling)
        {
            transform.gameObject.name = objectPooling.objectKey;

            transform.localPosition = Vector3.zero;

            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        public static bool RemoveDefault(MonoBehaviour monoBehaviour, IObjectPooling objectPooling)
        {
            if (!objectPooling.isActived)
                return false;

            ObjectPoolingSystem.ObjectRemove(objectPooling.objectKey, monoBehaviour, objectPooling);
            monoBehaviour.name = objectPooling.objectKey;

            monoBehaviour.transform.localPosition = Vector3.zero;

            monoBehaviour.transform.localEulerAngles = Vector3.zero;
            monoBehaviour.transform.localScale = Vector3.one;

            monoBehaviour.StopAllCoroutines();
            return true;
        }

        public static bool RemoveDefault(UI.UI ui, IObjectPooling objectPooling)
        {
            if (!objectPooling.isActived)
                return false;

            ObjectPoolingSystem.ObjectRemove(objectPooling.objectKey, ui, objectPooling);
            ui.name = objectPooling.objectKey;

            ui.rectTransform.anchoredPosition = Vector3.zero;

            ui.rectTransform.localEulerAngles = Vector3.zero;
            ui.rectTransform.localScale = Vector3.one;

            ui.StopAllCoroutines();
            return true;
        }
    }
}
