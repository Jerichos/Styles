using System;
using System.Linq;
using UnityEngine;

namespace Styles.Game
{
[CreateAssetMenu(fileName = "Garment", menuName = "data/Garment", order = 0)]
public class GarmentDoubleVariantSO : ItemSO
{
    [SerializeField] private GarmentDoubleVariantData _garmentData;

    public GarmentDoubleVariantData GarmentData => _garmentData;

    public static readonly GarmentSlot[] ALLOWED_GARMENT_SLOTS = {GarmentSlot.Hands, GarmentSlot.Feet};

    public new Garment CreateItemInstance()
    {
        return new Garment(this);
    }

    private void OnValidate()
    {
        if (!ALLOWED_GARMENT_SLOTS.Contains(_garmentData.Slot))
        {
            Debug.LogError("Double variant garment only support these slots: " + string.Join(", ", ALLOWED_GARMENT_SLOTS));
            _garmentData.Slot = ALLOWED_GARMENT_SLOTS[0];
        }
    }
}

[Serializable]
public struct GarmentDoubleVariantData
{
    public GarmentSlot Slot;
    
    [Header("Left")]
    public Sprite FrontLeft;
    public Sprite BackLeft;
    public Sprite SideLeft;
    
    [Header("Right")]
    public Sprite FrontRight;
    public Sprite BackRight;
    public Sprite SideRight;
    
    public Sprite GetLeftSprite(Facing facing)
    {
        return facing switch
        {
            Facing.Front => FrontLeft,
            Facing.Back => BackLeft,
            Facing.Right => SideLeft,
            Facing.Left => SideLeft,
            _ => throw new ArgumentOutOfRangeException(nameof(facing), facing, null)
        };
    }

    public Sprite GetRightSprite(Facing facing)
    {
        return facing switch
        {
            Facing.Front => FrontRight,
            Facing.Back => BackRight,
            Facing.Right => SideRight,
            Facing.Left => SideRight,
            _ => throw new ArgumentOutOfRangeException(nameof(facing), facing, null)
        };
    }
}

}