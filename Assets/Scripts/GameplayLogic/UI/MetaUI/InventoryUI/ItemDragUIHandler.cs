using System;
using System.Collections;
using System.Collections.Generic;
using Tools.Enums;
using Tzipory.Systems.UISystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tzipory.GameplayLogic.UI.MetaUI.InventoryUI
{
    public class ItemDragUIHandler : MonoBehaviour
    {

        private ItemSlotUI currentItemDragged;

        public void AssignDraggedItem(ItemSlotUI itemSlotUI)
        {
            currentItemDragged = itemSlotUI;
        }
        
    }
}