using Styles.Common;
using Styles.Game.npc;
using UnityEngine;

namespace Styles.Game
{
public class Wallet : MonoBehaviour
{
    public SAttribute<int> Money = 100;

    public event GenericDelegate<ItemShop> OnShopping;
    public event GenericDelegate<Item> OnItemPurchased;

    public CharacterManager Owner;

    public void OnItemPurchasedHandler(PurchaseCallback value)
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
        OnShopping?.Invoke(shop);
    }

    public void StopShopping()
    {
        OnShopping?.Invoke(null);
    }
}
}