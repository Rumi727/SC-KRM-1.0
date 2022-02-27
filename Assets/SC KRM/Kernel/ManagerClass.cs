using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM
{
    public class Manager<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance { get; private set; }



        protected static bool SingletonCheck(T manager)
        {
            if (instance != null && instance != manager)
            {
                DestroyImmediate(manager.gameObject);
                return false;
            }

            return (instance = manager) == manager;
        }
    }
}
