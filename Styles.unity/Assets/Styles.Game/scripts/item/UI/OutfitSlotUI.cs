using System;

namespace Styles.Game
{
public class OutfitSlotUI : ItemSlotUI
{
    public GarmentSlot Slot;

    private void OnValidate()
    {
        gameObject.name = $"{Slot} slot";
    }
}
}