using System;
using System.Collections.Generic;
using Styles.Common;
using UnityEngine;

namespace Styles.Game
{

public enum InventoryReturnCode
{
    ItemAdded,
    ItemRemoved,
    InventoryFull
}

[DefaultExecutionOrder(-1)]
public class Inventory : MonoBehaviour
{
    [SerializeField] private int _size = 6;
    [SerializeField] private ItemStack[] _defaultItems;

    private InventorySlot[] _slots;
    public InventorySlot[] Slots => _slots;
    
    public int Size => _size;

    public GenericDelegate<InventorySlot[]> EInventoryChanged;

    private void Awake()
    {
        _slots = new InventorySlot[_size];
        
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < _defaultItems.Length && _defaultItems[i].Item)
                _slots[i] = new InventorySlot { Item = _defaultItems[i].Item.CreateItemInstance(), Count = _defaultItems[i].Count};
        }
    }

    private void OnValidate()
    {
        if (_defaultItems.Length != _size)
        {
            var items = _defaultItems;

            if (_size < _defaultItems.Length)
            {
                for (int i = _size; i < items.Length; i++)
                {
                    if(items[i].Item)
                        Debug.LogWarning($"{items[i].Item.ItemData.Name} removed from inventory. New size is less than previous specified _defaultItems");
                }
            }
            
            _defaultItems = new ItemStack[_size];

            for (int i = 0; i < _size; i++)
            {
                if(i >= items.Length)
                    break;
                
                _defaultItems[i] = items[i];
            }
        }
    }

    public void AddItem(Item item, int slotID)
    {
        EInventoryChanged?.Invoke(_slots);
    }

    public void AddItem(Item item, GenericDelegate<InventoryReturnCode> callback)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (!_slots[i].Empty) 
                continue;
            
            _slots[i].Item = item;
            callback?.Invoke(InventoryReturnCode.ItemAdded);
            EInventoryChanged?.Invoke(_slots);
            return;
        }
        
        callback?.Invoke(InventoryReturnCode.InventoryFull);
    }

    public Item RemoveItem(int slotID)
    {
        if(_slots[slotID].Empty)
            return null;

        var item = _slots[slotID].Item;
        _slots[slotID].Item = null;
        _slots[slotID].Count = 0;
        
        EInventoryChanged?.Invoke(_slots);
        return item;
    }

    public void SortItems()
    {
        List<InventorySlot> items = new();

        for (int i = 0; i < _slots.Length; i++)
        {
            if(_slots[i].Empty)
                continue;

            items.Add(_slots[i]);
            _slots[i].Item = null;
            _slots[i].Count = 0;
        }
        
        if(items.Count == 0)
            return;

        for (int i = 0; i < items.Count; i++)
            _slots[i] = items[i];
        
        EInventoryChanged?.Invoke(_slots);
    }
}

[Serializable]
public class ItemStack
{
    public ItemSO Item;
    public int Count;
}
}