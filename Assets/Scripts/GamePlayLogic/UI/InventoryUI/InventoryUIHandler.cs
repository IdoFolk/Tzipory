using SerializeData.InventorySerializeData;
using Systems.UISystem;
using Tzipory.Tools.Interface;

public class InventoryUIHandler : BaseUIElement , IInitialization<InventorySerializeData>
{
    public bool IsInitialization { get; }
    public void Init(InventorySerializeData parameter)
    {
        throw new System.NotImplementedException();
    }
}
