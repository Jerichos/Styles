using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Styles.Game
{
[CreateAssetMenu(fileName = "Garment", menuName = "data/Garment", order = 0)]
public class OutfitPieceSO : ItemSO
{
    [SerializeField] protected OutfitSlot _slot;
    [SerializeField] private SpriteVariants _spriteVariants;

    public OutfitSlot Slot => _slot;
    public SpriteVariants SpriteVariants => _spriteVariants;
    
    private static readonly OutfitSlot[] ALLOWED_SLOTS = {OutfitSlot.Head, OutfitSlot.Body};
    
    public override Item CreateItemInstance()
    {
        return new OutfitPiece(this);
    }
    
    private void OnValidate()
    {
        if (!ALLOWED_SLOTS.Contains(_slot))
        {
            Debug.LogError("Single variant outfit piece only support these slots: " + string.Join(", ", ALLOWED_SLOTS));
            _slot = ALLOWED_SLOTS[0];
        }
    }
}

[Serializable]
public struct SpriteVariants
{
    public Sprite Front;
    public Sprite Back;
    public Sprite Side;
    
    public Sprite GetSprite(Facing facing)
    {
        return facing switch
        {
            Facing.Front => Front,
            Facing.Back => Back,
            Facing.Right => Side,
            Facing.Left => Side,
            _ => throw new ArgumentOutOfRangeException(nameof(facing), facing, null)
        };
    }
}
}