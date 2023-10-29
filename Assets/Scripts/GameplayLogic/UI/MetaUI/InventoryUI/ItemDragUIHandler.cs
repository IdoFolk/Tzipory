using System;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Unity.Entities;
using UnityEngine;

namespace Tzipory.GameplayLogic.UI.MetaUI.InventoryUI
{
    public class ItemDragUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject itemSlotPrefab;
        private ItemSlotUI _currentItemDragged;
        private ItemSlotUI _currentItemDraggedCopy;
        private bool _isItemDragged;

        public void BeginDragItem(ItemSlotUI itemSlotUI)
        {
            _currentItemDragged = itemSlotUI;
            _currentItemDraggedCopy = itemSlotUI.Copy();
            _currentItemDraggedCopy.gameObject.SetActive(true);
            _currentItemDragged.ToggleVisual(false);
            _isItemDragged = true;
            _currentItemDraggedCopy.transform.SetParent(transform);
        }

        private void Update()
        {
            if (_isItemDragged && _currentItemDraggedCopy.EnableDrag)
            {
                _currentItemDraggedCopy.transform.position = Input.mousePosition;
            }
        }

        public void EndDragItem()
        {
            _isItemDragged = false;
            _currentItemDraggedCopy.gameObject.SetActive(false);
            _currentItemDragged.ToggleVisual(true);

        }
        
    }
}