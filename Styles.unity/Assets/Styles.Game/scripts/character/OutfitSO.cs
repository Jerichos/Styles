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

    public OutfitPiece CreatePieceInstance(OutfitSlot slot)
    {
        var item = slot switch
        {
            OutfitSlot.Head => _outfitData.Head.CreateItemInstance(),
            OutfitSlot.Body => _outfitData.Body.CreateItemInstance(),
            OutfitSlot.Hands => _outfitData.Hands.CreateItemInstance(),
            OutfitSlot.Feet => _outfitData.Feet.CreateItemInstance(),
            _ => throw new ArgumentOutOfRangeException(nameof(slot), slot, null)
        };

        return (OutfitPiece) item;
    }
    
    public OutfitPieceSO GetOutfitPieceSO(OutfitSlot slot)
    {
        return slot switch
        {
            OutfitSlot.Head => _outfitData.Head,
            OutfitSlot.Body => _outfitData.Body,
            OutfitSlot.Hands => _outfitData.Hands,
            OutfitSlot.Feet => _outfitData.Feet,
            _ => throw new ArgumentOutOfRangeException(nameof(slot), slot, null)
        };
    }

    private void OnValidate()
    {
        if (_outfitData.Head && _outfitData.Head.Slot != OutfitSlot.Head)
        {
            Debug.LogError($"you cannot attach {_outfitData.Head.Slot.ToString()} to {OutfitSlot.Head.ToString()} slot");
            _outfitData.Head = null;
        }
        
        if (_outfitData.Body && _outfitData.Body.Slot != OutfitSlot.Body)
        {
            Debug.LogError($"you cannot attach {_outfitData.Body.Slot.ToString()} to {OutfitSlot.Body.ToString()} slot");
            _outfitData.Body = null;
        }
        
        if (_outfitData.Hands && _outfitData.Hands.Slot != OutfitSlot.Hands)
        {
            Debug.LogError($"you cannot attach {_outfitData.Hands.Slot.ToString()} to {OutfitSlot.Hands.ToString()} slot");
            _outfitData.Hands = null;
        }
        
        if (_outfitData.Feet && _outfitData.Feet.Slot != OutfitSlot.Feet)
        {
            Debug.LogError($"you cannot attach {_outfitData.Feet.Slot.ToString()} to {OutfitSlot.Head.ToString()} slot");
            _outfitData.Feet = null;
        }
    }
}

[Serializable]
public struct OutfitData
{
    [FormerlySerializedAs("Hat")] public OutfitPieceSO Head;
    public OutfitPieceSO Body;
    public OutfitPieceDoubleVariantSo Hands;
    public OutfitPieceDoubleVariantSo Feet;
}
}