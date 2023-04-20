using System;
using Styles.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Styles.Game.npc
{

[RequireComponent(typeof(Collider2D))]
public class ItemShop : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemShop _sharedShop;
    
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
            callback?.Invoke(new PurchaseCallback {ReturnCode = ShopReturnCode.NoItemLeft});
            return;
        }

        if (shopItems[shopItemID].Item.ItemData.Value > wallet.Money.Value)
        {
            Debug.Log("no enough money");
            callback?.Invoke(new PurchaseCallback {ReturnCode = ShopReturnCode.NotEnoughMoney});
            return;
        }

        if (wallet.Owner && wallet.Owner.Inventory && wallet.Owner.Inventory.IsInventoryFull())
        {
            Debug.Log("inventory full");
            callback?.Invoke(new PurchaseCallback {ReturnCode = ShopReturnCode.InventoryFull});
            return;
        }

        var money = wallet.Money.Value;
        money -= shopItems[shopItemID].Item.ItemData.Value;
        wallet.Money.Value = money;

        shopItems[shopItemID].Amount -= 1;
        ShopItems.Value = shopItems;

        PurchaseCallback purchase = new() {Item = shopItems[shopItemID].Item.CreateItemInstance(), ReturnCode = ShopReturnCode.Success };
        callback.Invoke(purchase);
        wallet.OnItemPurchasedHandler(purchase);
    }

    public void SellItem(Wallet wallet, Item item, GenericDelegate<SellCallback> callback)
    {
        if(item == null)
            return;

        wallet.Money.Value += item.ItemSO.ItemData.Value / 2;
        callback?.Invoke(new SellCallback{Item = item, ReturnCode = ShopReturnCode.Success});
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

    private void OnValidate()
    {
        if (_sharedShop)
        {
            Debug.Log($"this shop has shared shop with {_sharedShop.transform}. You can only change shop items in references shared shop.");
            ShopItems = _sharedShop.ShopItems;
        }
    }
}

public struct SellCallback
{
    public Item Item;
    public ShopReturnCode ReturnCode;
}

public struct PurchaseCallback
{
    public Item Item;
    public ShopReturnCode ReturnCode;
}

[Serializable]
public struct ShopItem
{
    public ItemSO Item;
    public int Amount;
}

public enum ShopReturnCode
{
    Rejected,
    Success,
    NotEnoughMoney,
    NoItemLeft,
    InventoryFull,
}
}