using System;

namespace Styles.Game
{
[Serializable]
public class Item
{
    private ItemSO _itemSO;

    public ItemSO ItemSo
    {
        get => _itemSO;
        set => _itemSO = value;
    }

    public Item(ItemSO itemSO)
    {
        _itemSO = itemSO;
    }
}
}