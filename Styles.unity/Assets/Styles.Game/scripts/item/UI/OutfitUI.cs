using System;
using System.Collections.Generic;
using Styles.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Styles.Game
{
public class OutfitUI : UIPanel
{
    [SerializeField] private CharacterManager _character;
    [SerializeField] private CharacterSkin _characterSkin;
    [SerializeField] private OutfitSlotUI[] _outfitSlots;

    private void Awake()
    {
        for (int i = 0; i < _outfitSlots.Length; i++)
            _outfitSlots[i].SetSlotID(i);
    }

    private void OnOutfitChanged(Dictionary<GarmentSlot, Garment> outfit)
    {
        Debug.Log("OnOutFitChanged");
        for (int i = 0; i < _outfitSlots.Length; i++)
        {
            if (outfit[_outfitSlots[i].Slot] == null)
            {
                _outfitSlots[i].SetSlot(null);
                continue;
            }
            
            _outfitSlots[i].SetSlot(((Item) outfit[_outfitSlots[i].Slot]).ItemSO.ItemData.Icon);
        }
    }
    
    private void OnSlotClicked(int slotID)
    {
        var item = _characterSkin.Garments[_outfitSlots[slotID].Slot];
        if(item == null)
            return;
        
        if (!_character)
        {
            _characterSkin.Unequip(_outfitSlots[slotID].Slot);
            return;
        }

        // if there is reference to a character try to add it to character's inventory
        _character.AddItemToInventory(item, OnAddItemToInventory);
    }

    private void OnAddItemToInventory(AddItemCallback callbackValue)
    {
        if (callbackValue.ReturnCode != InventoryReturnCode.ItemAdded)
        {
            Debug.Log(callbackValue.ReturnCode);
            return;
        }
        
        if(callbackValue.Item is not Garment garment)
            return;
        
        Debug.Log(garment.ToString());
        Debug.Log(garment.ItemSO.ToString());
        Debug.Log(garment.ItemSO.SpriteVariants);
        
        _characterSkin.Unequip(garment.ItemSO.Slot);
    }

    private void SubscribeToSlots()
    {
        for (int i = 0; i < _outfitSlots.Length; i++)
        {
            _outfitSlots[i].ClickedCallback = OnSlotClicked;
        }
    }

    private void UnsubscribeFromSlots()
    {
        for (int i = 0; i < _outfitSlots.Length; i++)
        {
            _outfitSlots[i].ClickedCallback = OnSlotClicked;
        }
    }
    
    private void OnEnable()
    {
        SubscribeToSlots();
        
        _characterSkin.EOutfitChanged += OnOutfitChanged;
        OnOutfitChanged(_characterSkin.Garments);
    }

    private void OnDisable()
    {
        UnsubscribeFromSlots();
        
        _characterSkin.EOutfitChanged -= OnOutfitChanged;
        OnOutfitChanged(_characterSkin.Garments);
    }
}
}