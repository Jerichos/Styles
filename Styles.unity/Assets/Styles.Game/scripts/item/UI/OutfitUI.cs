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
    
    private void OnOutfitChanged(Dictionary<GarmentSlot, Garment> outfit)
    {
        Debug.Log("OnOutFitChanged");
        for (int i = 0; i < _outfitSlots.Length; i++)
        {
            if (outfit[_outfitSlots[i].Slot] == null)
            {
                _outfitSlots[i].Image.enabled = false;
                continue;
            }
            
            _outfitSlots[i].Image.sprite = outfit[_outfitSlots[i].Slot].ItemSo.ItemData.Icon;
            _outfitSlots[i].Image.enabled = true;
        }
    }
    
    private void OnEnable()
    {
        _characterSkin.EOutfitChanged += OnOutfitChanged;
        OnOutfitChanged(_characterSkin.Garments);
    }

    private void OnDisable()
    {
        _characterSkin.EOutfitChanged -= OnOutfitChanged;
        OnOutfitChanged(_characterSkin.Garments);
    }
}

[Serializable]
public struct OutfitSlotUI
{
    public Image Image;
    public GarmentSlot Slot;
}
}