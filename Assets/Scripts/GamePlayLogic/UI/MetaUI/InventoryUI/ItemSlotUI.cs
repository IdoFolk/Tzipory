using System;
using Sirenix.Utilities;
using TMPro;
using Tools.Enums;
using Tzipory.Systems.InventorySystem;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tzipory.GameplayLogic.UI.MetaUI.InventoryUI
{
   public class ItemSlotUI : BaseInteractiveUIElement, IInitialization<ISlotItem>, ICopy<ItemSlotUI>
   {
      [SerializeField] private Image _image;
      [SerializeField] private TMP_Text _itemName;
      [SerializeField] private TMP_Text _itemAmount;
      [SerializeField] private RectTransform _rectTransform;
      [SerializeField] private GameObject _holder;

      private ISlotItem _item;

      private Vector3 _startPosition;
      protected override UIGroupType UIGroup => UIGroupType.MetaUI;

      public ISlotItem Item => _item;

      public bool IsInitialization { get; private set; }

      private void OnValidate()
      {
         _rectTransform ??= GetComponent<RectTransform>();
      }

      public override void OnBeginDrag(PointerEventData eventData)
      {
         base.OnBeginDrag(eventData);
         _startPosition = transform.position;
         ItemDragUIManager.AssignDraggedItem(this);
      }

      public override void OnEndDrag(PointerEventData eventData)
      {
         base.OnEndDrag(eventData);
         ItemDragUIManager.UnassignDraggedItem();
      }

      public void Init(ISlotItem parameter)
      {
         _image.sprite = parameter.ItemSlotSprite;
         _itemName.text = parameter.ItemSlotName;
         _itemAmount.text = parameter.ItemAmount.ToString();
         _item = parameter;
         IsInitialization = true;
      }

      public ItemSlotUI Copy()
      {
         ItemSlotUI itemSlotCopy = Instantiate(this);
         itemSlotCopy._image = _image;
         itemSlotCopy._itemName = _itemName;
         itemSlotCopy._itemAmount = _itemAmount;
         itemSlotCopy._item = _item;
         itemSlotCopy._rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);
         itemSlotCopy._startPosition = _startPosition;
         return itemSlotCopy;
      }

      public void ToggleVisual(bool state)
      {
         _holder.SetActive(state);
      }
      
   }
}