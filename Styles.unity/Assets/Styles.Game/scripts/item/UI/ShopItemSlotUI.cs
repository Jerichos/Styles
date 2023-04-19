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

    private void OnEnable()
    {
        throw new NotImplementedException();
    }

    private void OnDisable()
    {
        throw new NotImplementedException();
    }
}
}