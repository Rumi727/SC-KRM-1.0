using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SCKRM.UI.Setting
{
    [AddComponentMenu("SC KRM/UI/Setting/Name Drag (Save file linkage)")]
    public class SettingNameDrag : MonoBehaviour, IDragHandler
    {
        [SerializeField] UnityEvent _onDrag = new UnityEvent();
        public UnityEvent onDrag { get => _onDrag; }

        public void OnDrag(PointerEventData eventData) => onDrag.Invoke();
    }
}