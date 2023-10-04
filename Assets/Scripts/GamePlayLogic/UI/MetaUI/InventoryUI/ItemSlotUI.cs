using TMPro;
using Tzipory.Systems.InventorySystem;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : BaseInteractiveUIElement , IInitialization<ISlotItem>
{
   [SerializeField] private Image _image;
   [SerializeField] private TMP_Text _itemName;
   [SerializeField] private TMP_Text _itemAmount;
   
   private ISlotItem _item;
   
   private Vector3  _startPosition;

   public ISlotItem Item => _item;

   public bool IsInitialization { get; private set; }
   
   public override void OnBeginDrag(PointerEventData eventData)
   {
      base.OnBeginDrag(eventData);
      _startPosition  = transform.position;
   }

   public override void OnDrag(PointerEventData eventData)
   {
      base.OnDrag(eventData);
      transform.position = eventData.position;
   }

   public override void OnEndDrag(PointerEventData eventData)
   {
      base.OnEndDrag(eventData);
      transform.position  = _startPosition;
   }

   public void Init(ISlotItem parameter)
   {
      _image.sprite = parameter.ItemSlotSprite;
      _itemName.text = parameter.ItemSlotName;
      _itemAmount.text = parameter.ItemAmount.ToString();
      _item = parameter;
      IsInitialization = true;
   }
}
