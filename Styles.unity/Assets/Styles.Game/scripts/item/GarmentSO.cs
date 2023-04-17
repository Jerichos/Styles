using System;
using UnityEngine;

namespace Styles.Game
{
[CreateAssetMenu(fileName = "Garment", menuName = "data/Garment", order = 0)]
public class GarmentSO : ItemSO
{
    [SerializeField] private GarmentData _garmentData;

    public GarmentData GarmentData => _garmentData;
    
    public new Garment CreateItemInstance()
    {
        return new Garment(this);
    }
}

[Serializable]
public partial struct GarmentData
{
    public GarmentSlot Slot;
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