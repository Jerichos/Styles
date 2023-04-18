using System;
using UnityEngine;

namespace Styles.Game
{
[CreateAssetMenu(fileName = "Item", menuName = "data/Item", order = 0)]
public class ItemSO : ScriptableObject
{
    [SerializeField] private ItemData _itemData;

    public ItemData ItemData => _itemData;
    
    public virtual Item CreateItemInstance()
    {
        return new Item(this);
    }
}

[Serializable]
public struct ItemData
{
    public Sprite Icon;
    public string Name;
    public string Description;
    public int Value;
}
}