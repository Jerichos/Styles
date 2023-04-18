using Styles.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Styles.Game
{
public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _iconImage;
    
    private int _slotID;

    public int SlotID => _slotID;

    public GenericDelegate<int> ESlotClicked;

    public virtual void SetSlot(Sprite iconSprite)
    {
        if (iconSprite == null)
        {
            _iconImage.sprite = null;
            _iconImage.enabled = false;
            return;
        }

        _iconImage.sprite = iconSprite;
        _iconImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"slot clicked {_slotID} {eventData.button}");
        ESlotClicked?.Invoke(_slotID);
    }
}
}