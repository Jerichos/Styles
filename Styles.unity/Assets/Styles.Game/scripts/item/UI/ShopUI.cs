﻿using System;
using Styles.Common;
using Styles.Game.npc;
using TMPro;
using UnityEngine;

namespace Styles.Game
{
public class ShopUI : UIPanel
{
    [Header("character components")]
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Inventory _inventory;
    
    [Header("ui components")]
    [SerializeField] private Transform _slotsPanel;
    [SerializeField] private ShopItemSlotUI _shopItemSlotPrefab;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private InventoryUI _inventoryUI;

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
        _shop.ShopItems.EChanged += RefreshUI;
        
        InitializeSlots(_shop.ShopItems.Value);
        RefreshUI(_shop.ShopItems.Value);
    }

    private void InitializeSlots(ShopItem[] value)
    {
        // disable buttons which are not needed
        for (int i = value.Length; i < _itemSlots.Length; ++i)
        {
            _itemSlots[i].gameObject.SetActive(false);
        }

        if (value.Length > _itemSlots.Length)
        {
            Debug.LogWarning("There are more items in shop then ShopUI has already spawned! Expand it");
            ShopItemSlotUI[] newSlots = new ShopItemSlotUI[value.Length];

            // add current itemSlots to new array
            for (int i = 0; i < _itemSlots.Length; i++)
                newSlots[i] = _itemSlots[i];

            // create new item slots
            for (int i = _itemSlots.Length; i < newSlots.Length; i++)
            {
                Debug.Log($"creating new slot with ID {i}");
                var newSlot = Instantiate(_shopItemSlotPrefab, _slotsPanel, false);
                newSlots[i] = newSlot;
            }

            _itemSlots = newSlots;
        }

        for (int i = 0; i < _shop.ShopItems.Value.Length; ++i)
        {
            _itemSlots[i].gameObject.SetActive(true);
        }
    }

    private void RefreshUI(ShopItem[] value)
    {
        // update item slots
        for (int i = 0; i < _shop.ShopItems.Value.Length; ++i)
        {
            int id = i;
            _itemSlots[i].ButtonBuy.onClick.RemoveAllListeners();
            _itemSlots[i].ButtonBuy.onClick.AddListener(delegate { OnItemClicked(id); });
            _itemSlots[i].SetShopItemSlot(_shop.ShopItems.Value[i], _wallet.Money.Value);
        }
    }
    
    private void OnItemClicked(int i)
    {
        Debug.Log($"buy item clicked {i}");
        _shop.PurchaseItem(_wallet, i, OnPurchase);
    }

    private void OnPurchase(PurchaseCallback value)
    {
        Debug.Log($"OnPurchase {value.ReturnCode}");
    }

    private void OnMoneyChanged(int value)
    {
        _moneyText.SetText(value + "$");
        
        // refresh ui in case you can/can't afford items
        RefreshUI(_shop.ShopItems.Value);
    }
    
    private void OnInventoryItemUsed(int itemID, InventorySlot inventorySlot, GenericDelegate<InventorySlotCallback> value3)
    {
        _shop.SellItem(_wallet, inventorySlot.Item, OnSellCallback);
        value3?.Invoke(new InventorySlotCallback(){SlotID = itemID, ItemSlot = inventorySlot, ReturnCode = InventoryReturnCode.RemoveItem});
    }

    private void OnSellCallback(SellCallback value)
    {
        Debug.Log($"$OnSellCallback {value.ReturnCode}");
    }

    protected override void OnOpen()
    {
        Debug.Log("OnOpen");
        _panel.gameObject.SetActive(true);

        if (!_wallet)
        {
            transform.LogError("There is no wallet reference set in the inspector");
            return;
        }
        
        if(_inventoryUI)
            _inventoryUI.Open();
        else
            transform.LogWarning("missing reference to InventoryUI");
        
        _wallet.Money.EChanged += OnMoneyChanged;
        _inventory.ESlotUsed += OnInventoryItemUsed;
        _inventory.IsShopping.Value = true;
            
        OnMoneyChanged(_wallet.Money.Value);
    }

    protected override void OnClose()
    {
        Debug.Log("OnClose");
        _panel.gameObject.SetActive(false);
        
        for (int i = 0; i < _itemSlots.Length; i++)
            _itemSlots[i].ButtonBuy.onClick.RemoveAllListeners();

        _wallet.Money.EChanged -= OnMoneyChanged;
        _inventory.ESlotUsed -= OnInventoryItemUsed;
        _inventory.IsShopping.Value = false;
        
        if(_inventoryUI)
            _inventoryUI.Close();
        else
            transform.LogWarning("missing reference to InventoryUI");
        
        UnsubscribeFromShop(_shop);
    }
    
    private void UnsubscribeFromShop(ItemShop shop)
    {
        for (int i = 0; i < _itemSlots.Length; i++)
            _itemSlots[i].ButtonBuy.onClick.RemoveAllListeners();
        
        if(!shop)
            return;
        
        shop.ShopItems.EChanged -= RefreshUI;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _wallet.OnShopping += OnShopping;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _wallet.OnShopping -= OnShopping;
    }
}
}