using Styles.Common;
using UnityEngine;

namespace Styles.Game
{
public class PlayerUIManager : Singleton<PlayerUIManager>
{
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private OutfitUI _outfitUI;
    [SerializeField] private ControlsUI _controlsUI;
    [SerializeField] private MainMenuUI _mainMenuUI;

    public void ToggleInventoryUI()
    {
        _inventoryUI.Toggle();
    }

    public void ToggleOutfitUI()
    {
        _outfitUI.Toggle();
    }

    public void ToggleControlsUI()
    {
        _controlsUI.Toggle();
    }

    public void ToggleMainMenuUI()
    {
        _mainMenuUI.Toggle();
    }
}
}