using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Styles.Game
{
[CreateAssetMenu(fileName = "Outfit", menuName = "data/outfit", order = 0)]
public class OutfitSO : ScriptableObject
{
    [SerializeField] private OutfitData _outfitData;

    public OutfitData OutfitData => _outfitData;

    public Garment CreatePieceInstance(GarmentSlot slot)
    {
        return slot switch
        {
            GarmentSlot.Head => _outfitData.Head.CreateItemInstance(),
            GarmentSlot.Body => _outfitData.Body.CreateItemInstance(),
            GarmentSlot.Hands => _outfitData.Hands.CreateItemInstance(),
            GarmentSlot.Feet => _outfitData.Feet.CreateItemInstance(),
            _ => throw new ArgumentOutOfRangeException(nameof(slot), slot, null)
        };
    }
    
    public ItemSO GetPieceItemSO(GarmentSlot slot)
    {
        return slot switch
        {
            GarmentSlot.Head => _outfitData.Head,
            GarmentSlot.Body => _outfitData.Body,
            GarmentSlot.Hands => _outfitData.Hands,
            GarmentSlot.Feet => _outfitData.Feet,
            _ => throw new ArgumentOutOfRangeException(nameof(slot), slot, null)
        };
    }

    private void OnValidate()
    {
        if (_outfitData.Head && _outfitData.Head.Slot != GarmentSlot.Head)
        {
            Debug.LogError($"you cannot attach {_outfitData.Head.Slot.ToString()} to {GarmentSlot.Head.ToString()} slot");
            _outfitData.Head = null;
        }
        
        if (_outfitData.Body && _outfitData.Body.Slot != GarmentSlot.Body)
        {
            Debug.LogError($"you cannot attach {_outfitData.Body.Slot.ToString()} to {GarmentSlot.Body.ToString()} slot");
            _outfitData.Body = null;
        }
        
        if (_outfitData.Hands && _outfitData.Hands.Slot != GarmentSlot.Hands)
        {
            Debug.LogError($"you cannot attach {_outfitData.Hands.Slot.ToString()} to {GarmentSlot.Hands.ToString()} slot");
            _outfitData.Hands = null;
        }
        
        if (_outfitData.Feet && _outfitData.Feet.Slot != GarmentSlot.Feet)
        {
            Debug.LogError($"you cannot attach {_outfitData.Feet.Slot.ToString()} to {GarmentSlot.Head.ToString()} slot");
            _outfitData.Feet = null;
        }
    }
}

[Serializable]
public struct OutfitData
{
    [FormerlySerializedAs("Hat")] public GarmentSO Head;
    public GarmentSO Body;
    public GarmentDoubleVariantSO Hands;
    public GarmentDoubleVariantSO Feet;
}
}