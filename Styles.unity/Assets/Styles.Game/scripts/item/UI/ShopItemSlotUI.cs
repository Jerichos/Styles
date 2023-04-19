using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Styles.Game
{
public class ShopItemSlotUI : ItemSlotUI
{
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemValueText;
    [SerializeField] private Button _buttonBuy;

    public Button ButtonBuy => _buttonBuy;

    public void SetShopItemSlot(ItemData item)
    {
        _itemNameText.SetText(item.Name);
        _itemValueText.SetText(item.Value + "$");
        _iconImage.sprite = item.Icon;
    }
}
}