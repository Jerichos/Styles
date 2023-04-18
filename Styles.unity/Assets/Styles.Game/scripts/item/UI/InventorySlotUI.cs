namespace Styles.Game
{
public class InventorySlotUI : ItemSlotUI
{
    public void SetSlot(InventorySlot inventorySlot)
    {
        if (inventorySlot.Empty)
        {
            SetSlot(null);
            return;
        }
        
        SetSlot(inventorySlot.Item.ItemSO.ItemData.Icon);
    }
}
}