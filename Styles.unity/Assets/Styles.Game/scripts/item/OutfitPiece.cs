namespace Styles.Game
{
public class OutfitPiece : Item
{
    public new OutfitPieceSO ItemSO => _itemSO as OutfitPieceSO;
    public OutfitPiece(ItemSO itemSO) : base(itemSO)
    {
    }
}
}