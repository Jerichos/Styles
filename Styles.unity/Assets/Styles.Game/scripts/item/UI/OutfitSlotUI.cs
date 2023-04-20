using System;

namespace Styles.Game
{
public class OutfitSlotUI : ItemSlotUI
{
    public OutfitSlot Slot;

    private void OnValidate()
    {
        gameObject.name = $"{Slot} slot";
    }
}
}