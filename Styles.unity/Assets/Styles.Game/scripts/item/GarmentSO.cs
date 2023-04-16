using System;
using UnityEngine;

namespace Styles.Game
{
[CreateAssetMenu(fileName = "Garment", menuName = "data/Garment", order = 0)]
public class GarmentSO : ItemSO
{
    [SerializeField] private GarmentData _garmentData;

    public GarmentData GarmentData => _garmentData;

    public override Item CreateItemInstance()
    {
        return new Garment(this);
    }
}

[Serializable]
public struct GarmentData
{
    public GarmentSlot Slot;
    public Sprite Front;
    public Sprite Back;
    public Sprite Side;
}
}