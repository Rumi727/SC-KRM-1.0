using SCKRM.Editor;
using UnityEngine;

namespace SCKRM.Input.UI
{
    [AddComponentMenu("커널/Input/입력 리스트/입력 리스트 설정", 0)]
    public class ControlsListManager : MonoBehaviour
    {
        [SerializeField, SetName("Exit 키를 눌렀을때 보여질 오브젝트")]
        GameObject visibleGameObject;

        void Update()
        {
            if (InputManager.GetKeyDown("gui.back"))
                Exit();
        }

        public void Exit()
        {
            gameObject.SetActive(false);
            if (visibleGameObject != null)
                visibleGameObject?.SetActive(true);
        }
    }
}