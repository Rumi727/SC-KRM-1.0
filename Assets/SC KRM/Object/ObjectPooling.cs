using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Object
{
    [AddComponentMenu("SC KRM/Object/Object Pooling")]
    public class ObjectPooling : MonoBehaviour, IObjectPooling
    {
        public string objectKey { get; set; }

        public bool isRemoved => !isActived;

        public bool isActived { get; private set; }
        bool IObjectPooling.isActived { get => isActived; set => isActived = value; }



        IRefreshable[] _refreshableObjects;
        public IRefreshable[] refreshableObjects => _refreshableObjects = this.GetComponentsInChildrenFieldSave(_refreshableObjects, true);



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
