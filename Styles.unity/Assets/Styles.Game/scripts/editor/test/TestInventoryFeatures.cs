using Styles.Common;
using UnityEngine;

namespace Styles.Game.editor
{
public class TestInventoryFeatures : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryUI _inventoryUI;

    [SerializeField] private ItemSO[] _testItems;
    
    public void AddRandomItem()
    {
        if (_testItems.Length <= 0)
        {
            gameObject.LogError("no _testItems were added in the inspector");
            return;
        }
        
        ItemSO randomItem = _testItems[Random.Range(0, _testItems.Length)];
        _inventory.AddItem(randomItem.CreateItemInstance(), OnAddItemCallback);
    }

    private void OnAddItemCallback(AddItemCallback value)
    {
        Debug.Log($"OnAddItemCallback {value.ReturnCode.ToString()}");
    }

    public void RemoveItem(int id)
    {
        _inventory.RemoveItem(0);
        _inventory.SortItems();
    }

    public void OpenInventory()
    {
        _inventoryUI.Open();
    }

    public void CloseInventory()
    {
        _inventoryUI.Close();
    }

    public void ToggleInventory()
    {
        _inventoryUI.Toggle();
    }
}
}