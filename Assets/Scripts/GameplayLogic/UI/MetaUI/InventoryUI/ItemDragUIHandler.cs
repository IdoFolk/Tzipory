using System.Collections;
using System.Collections.Generic;
using Tools.Enums;
using Tzipory.Systems.UISystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tzipory.GameplayLogic.UI.MetaUI.InventoryUI
{
    public class ItemDragUIHandler : BaseInteractiveUIElement
    {
        [SerializeField] private Transform gameObjectHolder;
        protected override UIGroupType UIGroup => UIGroupType.MetaUI;

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
        }
    }
}