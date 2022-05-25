using SCKRM.UI;
using SCKRM.UI.Layout;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.Resource.UI
{
    public class ResourcePack : ObjectPoolingUI, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public ResourcePackList resourcePackList { get; [Obsolete("It is managed by the ResourcePackList class. Please do not touch it.")] internal set; }

        public int resourcePackIndex { get; set; } = 0;
        public string resourcePackPath { get; set; } = "";
        public bool selected = false;

        [SerializeField, HideInInspector] VerticalLayout _verticalLayout; public VerticalLayout verticalLayout => _verticalLayout = this.GetComponentFieldSave(_verticalLayout);
        [SerializeField, HideInInspector] SetSizeAsChildRectTransform _setSizeAsChildRectTransform; public SetSizeAsChildRectTransform setSizeAsChildRectTransform => _setSizeAsChildRectTransform = this.GetComponentFieldSave(_setSizeAsChildRectTransform);



        [SerializeField] Image _icon; public Image icon => _icon;
        [SerializeField] TMP_Text _nameText; public TMP_Text nameText => _nameText;

        [SerializeField] TMP_Text _descriptionText; public TMP_Text descriptionText => _descriptionText;

        public override void Remove()
        {
            base.Remove();

            if (icon.gameObject.activeSelf)
            {
                icon.gameObject.SetActive(false);
                Destroy(icon.sprite);
                icon.sprite = null;
            }

            nameText.text = "";
            descriptionText.text = "";
            setSizeAsChildRectTransform.min = 40;
            verticalLayout.padding.left = 10;
        }

        Vector2 posOffset = Vector2.zero;
        RectTransform[] selectedChildRectTransforms;
        List<float> selectedChildYPosList = new List<float>();
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            //기본 리소스팩이거나 리소스팩이 새로고침 중이면 무시합니다
            if ((selected && resourcePackIndex == ResourceManager.SaveData.resourcePacks.Count - 1) || ResourceManager.isResourceRefesh)
            {
                resourcePackList.Refresh();
                return;
            }

            posOffset = eventData.position - rectTransform.anchoredPosition;

            transform.SetParent(transform.parent.parent);

            selectedChildRectTransforms = new RectTransform[resourcePackList.selectedResourcePacksContent.childCount];
            for (int i = 0; i < selectedChildRectTransforms.Length; i++)
                selectedChildRectTransforms[i] = (RectTransform)resourcePackList.selectedResourcePacksContent.GetChild(i);
        }
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            //기본 리소스팩이거나 리소스팩이 새로고침 중이면 무시합니다
            if ((selected && resourcePackIndex == ResourceManager.SaveData.resourcePacks.Count - 1) || ResourceManager.isResourceRefesh)
            {
                resourcePackList.Refresh();
                return;
            }

            rectTransform.anchoredPosition = eventData.position - posOffset;

            selectedChildYPosList.Clear();

            for (int i = 0; i < selectedChildRectTransforms.Length; i++)
                selectedChildYPosList.Add(selectedChildRectTransforms[i].anchoredPosition.y);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            //기본 리소스팩이거나 리소스팩이 새로고침 중이면 무시합니다
            if ((selected && resourcePackIndex == ResourceManager.SaveData.resourcePacks.Count - 1) || ResourceManager.isResourceRefesh)
            {
                resourcePackList.Refresh();
                return;
            }

            //드래그 하는 오브젝트가 선택 할 수 있는 오브젝트일 경우
            if (!selected)
            {
                //오브젝트를 왼쪽으로 끌었을때
                if (rectTransform.anchoredPosition.x <= -115)
                {
                    selected = true;

                    resourcePackIndex = selectedChildYPosList.CloseValueIndex(rectTransform.anchoredPosition.y);
                    ResourceManager.SaveData.resourcePacks.Insert(resourcePackIndex, resourcePackPath);

                    Kernel.AllRefresh().Forget();

                    transform.SetParent(resourcePackList.selectedResourcePacksContent);
                }
                else
                    transform.SetParent(resourcePackList.availableResourcePacksContent);

            }
            //드래그 하는 오브젝트가 선택 된 오브젝트일 경우
            else if (selected)
            {
                //오브젝트를 오른쪽으로 끌었을때
                if (rectTransform.anchoredPosition.x >= 110)
                {
                    selected = false;

                    ResourceManager.SaveData.resourcePacks.RemoveAt(resourcePackIndex);
                    Kernel.AllRefresh().Forget();

                    transform.SetParent(resourcePackList.availableResourcePacksContent);
                }
                else
                {
                    int oldIndex = resourcePackIndex;
                    resourcePackIndex = selectedChildYPosList.CloseValueIndex(rectTransform.anchoredPosition.y);
                    ResourceManager.SaveData.resourcePacks.Move(oldIndex, resourcePackIndex);

                    if (oldIndex != resourcePackIndex)
                        Kernel.AllRefresh().Forget();

                    transform.SetParent(resourcePackList.selectedResourcePacksContent);
                }
            }

            transform.SetSiblingIndex(resourcePackIndex);
        }
    }
}
