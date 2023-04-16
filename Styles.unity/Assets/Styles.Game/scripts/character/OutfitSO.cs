using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Styles.Game
{
[CreateAssetMenu(fileName = "Outfit", menuName = "data/outfit", order = 0)]
public class OutfitSO : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private OutfitData _outfitData;

    public OutfitData OutfitData => _outfitData;

    private void OnValidate()
    {
        if (_outfitData.Head && _outfitData.Head.GarmentData.Slot != GarmentSlot.Head)
        {
            Debug.LogError($"you cannot attach {_outfitData.Head.GarmentData.Slot.ToString()} to {GarmentSlot.Head.ToString()} slot");
            _outfitData.Head = null;
        }
        
        if (_outfitData.Body && _outfitData.Body.GarmentData.Slot != GarmentSlot.Body)
        {
            Debug.LogError($"you cannot attach {_outfitData.Body.GarmentData.Slot.ToString()} to {GarmentSlot.Body.ToString()} slot");
            _outfitData.Body = null;
        }
        
        if (_outfitData.Hands && _outfitData.Hands.GarmentData.Slot != GarmentSlot.Hands)
        {
            Debug.LogError($"you cannot attach {_outfitData.Hands.GarmentData.Slot.ToString()} to {GarmentSlot.Hands.ToString()} slot");
            _outfitData.Hands = null;
        }
        
        if (_outfitData.Feet && _outfitData.Feet.GarmentData.Slot != GarmentSlot.Feet)
        {
            Debug.LogError($"you cannot attach {_outfitData.Feet.GarmentData.Slot.ToString()} to {GarmentSlot.Head.ToString()} slot");
            _outfitData.Feet = null;
        }
    }
}

[Serializable]
public struct OutfitData
{
    [FormerlySerializedAs("Hat")] public GarmentSO Head;
    public GarmentSO Body;
    public GarmentSO Hands;
    public GarmentSO Feet;
}
}