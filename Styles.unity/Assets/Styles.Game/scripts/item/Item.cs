using System;

namespace Styles.Game
{
[Serializable]
public class Item
{
    protected ItemSO _itemSO;

    public ItemSO ItemSO
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