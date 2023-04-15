using UnityEngine;
using UnityEngine.UI;

namespace Styles.Game
{
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;

    [SerializeField] [HideInInspector] private int _slotID;

    public void InitSlot(int id)
    {
        _slotID = id;
    }

    public void UpdateSlot(InventorySlot itemSlot)
    {
        if (itemSlot.Empty)
        {
            _iconImage.enabled = false;
        }
        else
        {
            var item = itemSlot.Item;
            
            _iconImage.enabled = true;
            _iconImage.sprite = item.ItemSo.ItemData.Icon;
        }
    }
}
}