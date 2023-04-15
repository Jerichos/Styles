using System;

namespace Styles.Game
{
[Serializable]
public struct InventorySlot
{
    public Item Item;
    public int Count;

    public bool Empty => Item == null;
}
}