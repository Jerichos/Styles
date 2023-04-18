namespace Styles.Game
{
public class Garment : Item
{
    public new GarmentSO ItemSO => _itemSO as GarmentSO;
    public Garment(ItemSO itemSO) : base(itemSO)
    {
    }
}
}