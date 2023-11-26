using Tzipory.GameplayLogic.UI.MetaUI.InventoryUI;
using UnityEngine;

public class ItemDragUIManager : MonoBehaviour
{
    [SerializeField] private ItemDragUIHandler _itemDragUIHandlerAsset;
    [SerializeField] private Transform _inventoryItemSlotContainer;
    private static ItemDragUIHandler _itemDragUIHandler;
    private static Transform _inventoryItemSlotContainerRef;

    private void Awake()
    {
        _itemDragUIHandler = Instantiate(_itemDragUIHandlerAsset,transform);
        _inventoryItemSlotContainerRef = _inventoryItemSlotContainer;
    }

    public static void AssignDraggedItem(ItemSlotUI itemSlotUI)
    {
        _itemDragUIHandler.BeginDragItem(itemSlotUI);
    }

    public static void UnassignDraggedItem()
    {
        _itemDragUIHandler.EndDragItem();
    }

   
}
