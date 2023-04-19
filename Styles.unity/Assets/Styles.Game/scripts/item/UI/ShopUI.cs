using System;
using Styles.Common;
using Styles.Game.npc;
using TMPro;
using UnityEngine;

namespace Styles.Game
{
public class ShopUI : UIPanel
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Transform _panel;
    [SerializeField] private Transform _slotsPanel;
    [SerializeField] private ShopItemSlotUI _shopItemSlotPrefab;
    [SerializeField] private TMP_Text _moneyText;

    private ItemShop _shop;
    private ShopItemSlotUI[] _itemSlots;

    private void Awake()
    {
        // get default slots if there are any
        _itemSlots = _slotsPanel.GetComponentsInChildren<ShopItemSlotUI>(true);
    }

    private void OnShopping(ItemShop shop)
    {
        if (!shop)
        {
            UnsubscribeFromShop(_shop);
            Close();
        }
        else
        {
            OnStartShopping(shop);
            Open();
        }

    }

    private void OnStartShopping(ItemShop shop)
    {
        // unsub from previous shop, if there still any for some reason
        UnsubscribeFromShop(_shop);
        
        _shop = shop;
        _shop.ShopItems.EChanged += OnShopItemsChanged;
        
        // disable buttons which are not needed
        for (int i = _shop.ShopItems.Value.Length; i < _itemSlots.Length; ++i)
        {
            _itemSlots[i].gameObject.SetActive(false);
        }

        if (_shop.ShopItems.Value.Length > _itemSlots.Length)
        {
            Debug.LogWarning("There are more items in shop then ShopUI has already spawned! Expand it");
            //var currenItemSlots = _itemSlots;
            ShopItemSlotUI[] newSlots = new ShopItemSlotUI[_shop.ShopItems.Value.Length];

            // add current itemSlots to new array
            for (int i = 0; i < _itemSlots.Length; i++)
                newSlots[i] = _itemSlots[i];

            // create new item slots
            for (int i = _itemSlots.Length; i < newSlots.Length; i++)
            {
                Debug.Log($"creating new slot with ID {i}");
                var newSlot = Instantiate(_shopItemSlotPrefab, _slotsPanel, false);

                // if (newSlot.transform is RectTransform rect && _shopItemSlotPrefab.transform is RectTransform rect2)
                // {
                //     Debug.Log($"rect1 {rect.sizeDelta}, rect2 {rect2.sizeDelta}");
                //     rect.sizeDelta = rect2.sizeDelta;
                // }
                
                newSlots[i] = newSlot;
            }

            _itemSlots = newSlots;
        }
        
        // update item slots
        for (int i = 0; i < _shop.ShopItems.Value.Length; ++i)
        {
            _itemSlots[i].gameObject.SetActive(true);
            int id = i;
            _itemSlots[i].ButtonBuy.onClick.AddListener(delegate { OnItemClicked(id); });
            _itemSlots[i].SetShopItemSlot(_shop.ShopItems.Value[i].Item.ItemData);
        }
    }

    private void UnsubscribeFromShop(ItemShop shop)
    {
        for (int i = 0; i < _itemSlots.Length; i++)
            _itemSlots[i].ButtonBuy.onClick.RemoveAllListeners();
        
        if(!shop)
            return;
        
        shop.ShopItems.EChanged -= OnShopItemsChanged;
    }

    private void OnItemClicked(int i)
    {
        Debug.Log($"but item clicked {i}");
    }

    private void OnShopItemsChanged(ShopItem[] value)
    {
        throw new NotImplementedException();
    }
    
    private void OnMoneyChanged(int value)
    {
        _moneyText.SetText(value + "$");
    }

    protected override void OnOpen()
    {
        _panel.gameObject.SetActive(true);

        if (!_wallet)
        {
            transform.LogError("There is no wallet reference set in the inspector");
            return;
        }
        
        _wallet.Money.EChanged += OnMoneyChanged;
        OnMoneyChanged(_wallet.Money.Value);
    }

    protected override void OnClose()
    {
        _panel.gameObject.SetActive(false);
        
        for (int i = 0; i < _itemSlots.Length; i++)
            _itemSlots[i].ButtonBuy.onClick.RemoveAllListeners();

        _wallet.Money.EChanged -= OnMoneyChanged;
    }

    private void OnEnable()
    {
        _wallet.OnShopping += OnShopping;
    }

    private void OnDisable()
    {
        _wallet.OnShopping -= OnShopping;
    }
}
}