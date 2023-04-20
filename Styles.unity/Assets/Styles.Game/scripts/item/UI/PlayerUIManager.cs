using Styles.Common;
using UnityEngine;

namespace Styles.Game
{
public class PlayerUIManager : Singleton<PlayerUIManager>
{
    [SerializeField] private InventoryUI _inventory;
    [SerializeField] private OutfitUI _outfitUI;
    [SerializeField] private ControlsUI _controlsUI;

    public void ToggleInventoryUI()
    {
        _inventory.Toggle();
    }

    public void ToggleOutfitUI()
    {
        _outfitUI.Toggle();
    }

    public void ToggleControlsUI()
    {
        _controlsUI.Toggle();
    }
}
}