using SCKRM.Editor;
using SCKRM.UI;
using UnityEngine;

namespace SCKRM.Input.UI
{
    [AddComponentMenu("커널/Input/입력 리스트/입력 리스트 설정", 0)]
    public class ControlsListManager : MonoBehaviour
    {
        [SerializeField, SetName("Exit 키를 눌렀을때 보여질 오브젝트")]
        GameObject visibleGameObject;

        void OnEnable()
        {
            KernelCanvas.backEventList.Add(Exit);
            KernelCanvas.homeEvent += Exit;
        }

        void OnDisable()
        {
            KernelCanvas.backEventList.Remove(Exit);
            KernelCanvas.homeEvent -= Exit;
        }

        public void Exit()
        {
            gameObject.SetActive(false);
            if (visibleGameObject != null)
                visibleGameObject?.SetActive(true);
        }
    }
}