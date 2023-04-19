using System;
using Styles.Game.npc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Styles.Game
{
public class ShopItemSlotUI : ItemSlotUI
{
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemValueText;
    [SerializeField] private TMP_Text _itemAmountText;
    [SerializeField] private Button _buttonBuy;
    [SerializeField] private CanvasGroup _canvasGroup;

    public Button ButtonBuy => _buttonBuy;

    public void SetShopItemSlot(ShopItem item)
    {
        _itemNameText.SetText(item.Item.ItemData.Name);
        _itemAmountText.SetText(item.Amount.ToString());
        _itemValueText.SetText(item.Item.ItemData.Value + "$");
        _iconImage.sprite = item.Item.ItemData.Icon;

        // if items are sold, disable interaction
        _canvasGroup.interactable = item.Amount > 0;
    }
}
}