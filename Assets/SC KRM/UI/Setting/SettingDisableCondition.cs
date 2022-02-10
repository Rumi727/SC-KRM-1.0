using Cysharp.Threading.Tasks;
using SCKRM.Input;
using SCKRM.Renderer;
using SCKRM.SaveLoad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace SCKRM.UI.Setting
{
    [AddComponentMenu("커널/UI/비활성화 조건 (설정)")]
    public class SettingDisableCondition : Setting
    {
        [SerializeField] GameObject _disableGameObject; public GameObject disableGameObject { get => _disableGameObject; set => _disableGameObject = value; }

        [SerializeField] bool _reversal = false; public bool reversal { get => _reversal; set => _reversal = value; }

        public override async UniTask Awake()
        {
            await base.Awake();

            if (type != typeof(bool))
                enabled = false;
        }

        public virtual void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                if ((bool)GetValue())
                {
                    if (reversal)
                    {
                        if (disableGameObject.activeSelf)
                            disableGameObject.SetActive(false);
                    }
                    else if (!disableGameObject.activeSelf)
                        disableGameObject.SetActive(true);
                }
                else
                {
                    if (reversal)
                    {
                        if (!disableGameObject.activeSelf)
                            disableGameObject.SetActive(true);
                    }
                    else if (disableGameObject.activeSelf)
                        disableGameObject.SetActive(false);
                }
            }
        }
    }
}