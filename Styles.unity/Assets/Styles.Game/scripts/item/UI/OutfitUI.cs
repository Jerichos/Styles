using System;
using System.Collections.Generic;
using Styles.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Styles.Game
{
public class OutfitUI : UIPanel
{
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
            
            _outfitSlots[i].SetSlot(outfit[_outfitSlots[i].Slot].ItemSo.ItemData.Icon);
        }
    }
    
    private void OnSlotClicked(int slotID)
    {
        // unequip item
        _characterSkin.Unequip(_outfitSlots[slotID].Slot);
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