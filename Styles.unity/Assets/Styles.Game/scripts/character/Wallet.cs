using Styles.Common;
using Styles.Game.npc;
using UnityEngine;

namespace Styles.Game
{
public class Wallet : MonoBehaviour
{
    public SAttribute<int> Money = 100;

    public GenericDelegate<Wallet, ItemShop> OnShopping;
    public GenericDelegate<Item> OnItemPurchased;

    private ItemShop _itemShop;

    public void PurchaseItem(int itemID, PurchaseCallback callback)
    {
        if (!_itemShop)
        {
            Debug.LogError("no item shop ");
            return;
        }
        
        _itemShop.PurchaseItem(this, itemID, OnItemPurchasedHandler);
    }

    private void OnItemPurchasedHandler(PurchaseCallback value)
    {
        if (value.ReturnCode != PurchaseReturnCode.Success)
        {
            Debug.Log($"item was not purchased. Reason {value.ReturnCode}");
            return;
        }
        
        OnItemPurchased?.Invoke(value.Item);
    }

    public void StartShopping(ItemShop shop)
    {
        transform.Log($"StartShopping with {shop.name}");
        _itemShop = shop;
        OnShopping?.Invoke(this, shop);
    }

    public void StopShopping()
    {
        _itemShop = null;
        OnShopping?.Invoke(null, null);
    }
}
}