namespace Styles.Game
{
public class Garment : Item
{
    public new GarmentSO ItemSo => _itemSO as GarmentSO;
    
    public Garment(ItemSO itemSO) : base(itemSO)
    {
    }
}
}