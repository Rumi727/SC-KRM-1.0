using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Resource.UI
{
    public class ResourcePackSettingButton : MonoBehaviour
    {
        [SerializeField] Button button;

        void Update() => button.interactable = !ResourceManager.isResourceRefesh;
    }
}
