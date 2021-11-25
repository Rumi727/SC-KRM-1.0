using UnityEngine;

namespace SCKRM.Font
{
    [ExecuteAlways]
    [AddComponentMenu("커널/Font/글꼴의 텍스쳐 필터 모드를 점으로 설정", 0)]
    public sealed class FontPointSetting : MonoBehaviour
    {
        [SerializeField] UnityEngine.Font[] font;

        void Awake()
        {
            for (int i = 0; i < font.Length; i++)
                font[i].material.mainTexture.filterMode = FilterMode.Point;
        }
    }
}