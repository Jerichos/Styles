using System;
using System.Collections.Generic;
using Styles.Common;
using TMPro;
using UnityEngine;

namespace Styles.Game
{

public enum InventoryReturnCode
{
    ItemAdded,
    ItemRemoved,
    InventoryFull,
    RemoveItem,
}

[DefaultExecutionOrder(-1)]
public class Inventory : MonoBehaviour
{
    [SerializeField] private int _size = 6;
    [SerializeField] private ItemStack[] _defaultItems;
    
    private InventorySlot[] _slots;
    public InventorySlot[] Slots => _slots;
    
    public int Size => _size;

    public event GenericDelegate<InventorySlot[]> EInventoryChanged;
    public event GenericDelegate<int, InventorySlot, GenericDelegate<InventorySlotCallback>> ESlotUsed;

    public SAttribute<bool> IsShopping;

    private void Awake()
    {
        _slots = new InventorySlot[_size];
        IsShopping = false;
        
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

    private void AddItem(Item item, int slotID)
    {
        if (!_slots[slotID].Empty)
        {
            Debug.Log($"slot {slotID} is not empty");
            return;
        }

        _slots[slotID].Item = item;
        EInventoryChanged?.Invoke(_slots);
    }

    public void AddItem(Item item, GenericDelegate<InventorySlotCallback> callback)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (!_slots[i].Empty) 
                continue;
            
            _slots[i].Item = item;
            callback?.Invoke(new InventorySlotCallback { SlotID = i, ItemSlot = _slots[i], ReturnCode = InventoryReturnCode.ItemAdded });
            EInventoryChanged?.Invoke(_slots);
            Debug.Log("item added to inventory");
            return;
        }
        
        Debug.Log("inventory full");
        callback?.Invoke(new InventorySlotCallback { ReturnCode = InventoryReturnCode.InventoryFull });
    }

    public Item RemoveItem(int slotID)
    {
        Debug.Log($"5 RemoveItem {slotID}");
        if(_slots[slotID].Empty)
            return null;

        Debug.Log("6 equip item");
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

    public void UseItem(int slotID, GenericDelegate<InventorySlotCallback> callback)
    {
        var itemSlot = Slots[slotID];
        if (itemSlot.Empty)
        {
            Debug.Log($"slot is empty {slotID}");
            return;
        }
        
        ESlotUsed?.Invoke(slotID, _slots[slotID], OnItemUsedCallback);
    }

    public bool IsInventoryFull()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].Empty)
                return false;
        }

        return true;
    }

    private void OnItemUsedCallback(InventorySlotCallback callback)
    {
        if (callback.ReturnCode == InventoryReturnCode.RemoveItem)
        {
            RemoveItem(callback.SlotID);
            
            if(callback.ReturningItem != null)
                AddItem(callback.ReturningItem, callback.SlotID);
        }
    }
}

public struct InventorySlotCallback
{
    public int SlotID;
    public InventorySlot ItemSlot;
    public Item ReturningItem;
    public InventoryReturnCode ReturnCode;
}

[Serializable]
public class ItemStack
{
    public ItemSO Item;
    public int Count;
}
}