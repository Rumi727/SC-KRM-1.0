using SCKRM.UI;
using UnityEngine;

namespace SCKRM.Input.UI
{
    [AddComponentMenu("커널/Input/입력 리스트/입력 리스트 설정", 0)]
    public class ControlsListManager : MonoBehaviour
    {
        [SerializeField]
        GameObject visibleGameObject;

        void OnEnable()
        {
            UIManager.BackEventAdd(Exit);
            UIManager.homeEvent += Exit;
        }

        void OnDisable()
        {
            UIManager.BackEventRemove(Exit);
            UIManager.homeEvent -= Exit;
        }

        public void Exit()
        {
            gameObject.SetActive(false);
            if (visibleGameObject != null)
                visibleGameObject?.SetActive(true);
        }
    }
}