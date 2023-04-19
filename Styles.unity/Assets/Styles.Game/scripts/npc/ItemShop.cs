using System;
using Styles.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Styles.Game.npc
{
public class ItemShop : MonoBehaviour, IInteractable
{
    public SAttribute<ShopItem[]> ShopItems;

    public void PurchaseItem(Wallet wallet, int shopItemID, GenericDelegate<PurchaseCallback> callback)
    {
        var shopItems = ShopItems.Value;
        
        if (shopItemID >= shopItems.Length)
        {
            Debug.LogError("out of range of available items!");
            callback?.Invoke(new PurchaseCallback());
            return;
        }

        if (shopItems[shopItemID].Amount <= 0)
        {
            Debug.Log("No item left on the store.");
            callback?.Invoke(new PurchaseCallback {ReturnCode = PurchaseReturnCode.NoItemLeft});
            return;
        }

        if (shopItems[shopItemID].Item.ItemData.Value > wallet.Money.Value)
        {
            Debug.Log("no enough money");
            callback?.Invoke(new PurchaseCallback {ReturnCode = PurchaseReturnCode.NotEnoughMoney});
            return;
        }

        if (wallet.Owner && wallet.Owner.Inventory && wallet.Owner.Inventory.IsInventoryFull())
        {
            Debug.Log("inventory full");
            callback?.Invoke(new PurchaseCallback {ReturnCode = PurchaseReturnCode.InventoryFull});
            return;
        }

        var money = wallet.Money.Value;
        money -= shopItems[shopItemID].Item.ItemData.Value;
        wallet.Money.Value = money;

        shopItems[shopItemID].Amount -= 1;
        ShopItems.Value = shopItems;

        PurchaseCallback purchase = new() {Item = shopItems[shopItemID].Item.CreateItemInstance(), ReturnCode = PurchaseReturnCode.Success };
        callback.Invoke(purchase);
        wallet.OnItemPurchasedHandler(purchase);
    }

    public void Interact(MonoBehaviour actor)
    {
        if (actor is not CharacterManager character)
            return;

        if (!character.Wallet)
        {
            character.gameObject.LogWarning("This character does not have a wallet!");
            return;
        }
        
        character.Wallet.StartShopping(this);
    }
}

public struct PurchaseCallback
{
    public Item Item;
    public PurchaseReturnCode ReturnCode;
}

[Serializable]
public struct ShopItem
{
    public ItemSO Item;
    public int Amount;
}

public enum PurchaseReturnCode
{
    Rejected,
    Success,
    NotEnoughMoney,
    NoItemLeft,
    InventoryFull,
}
}