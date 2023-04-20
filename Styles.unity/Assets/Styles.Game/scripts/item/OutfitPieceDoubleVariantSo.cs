using System;
using System.Linq;
using UnityEngine;

namespace Styles.Game
{
[CreateAssetMenu(fileName = "Garment", menuName = "data/Garment", order = 0)]
public class OutfitPieceDoubleVariantSo : OutfitPieceSO
{
    [SerializeField] private SpriteVariants _rightSpriteVariants;

    public SpriteVariants RightSpriteVariants => _rightSpriteVariants;

    private static readonly OutfitSlot[] ALLOWED_SLOTS = {OutfitSlot.Hands, OutfitSlot.Feet};

    private void OnValidate()
    {
        if (!ALLOWED_SLOTS.Contains(_slot))
        {
            Debug.LogError("Double variant garment only support these slots: " + string.Join(", ", ALLOWED_SLOTS));
            _slot = ALLOWED_SLOTS[0];
        }
    }
}
}