using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Object
{
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