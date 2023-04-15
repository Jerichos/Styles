using System;
using Styles.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Styles.Game
{
public class InventoryUI : UIPanel
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventorySlotUI _slotPrefab;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Transform _panelSlots;

    [SerializeField] private InventorySlotUI[] _uiSlots;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        _inventory.EInventoryChanged += OnInventoryChanged;
        OnInventoryChanged(_inventory.Slots);
    }

    private void OnDisable()
    {
        _inventory.EInventoryChanged -= OnInventoryChanged;
    }

    private void Initialize()
    {
        if (!_inventory)
        {
            gameObject.Log("_inventory has not been set");
            DestroyUISlots();
            return;
        }

        var currentSlots = _panelSlots.GetComponentsInChildren<InventorySlotUI>();
        if (currentSlots.Length != _inventory.Size)
        {
            DestroyUISlots();

            for (int i = 0; i < _inventory.Size; i++)
            {
                var newSlot = Instantiate(_slotPrefab, _panelSlots, true);
                newSlot.transform.localScale = Vector3.one;
                newSlot.InitSlot(i);
                _uiSlots[i] = newSlot;
                Debug.Log("add ui slot");
            }
        }
        else
        {
            for (int i = 0; i < currentSlots.Length; i++)
            {
                currentSlots[i].InitSlot(i);
            }
        }
    }

    private void DestroyUISlots()
    {
        var currentSlots = _panelSlots.GetComponentsInChildren<InventorySlotUI>();
        for (int i = 0; i < currentSlots.Length; i++)
        {
#if UNITY_EDITOR
            DestroyImmediate(currentSlots[i].gameObject);
#else
                Destroy(currentSlots[i].gameObject);
#endif
        }
    }

    private void OnInventoryChanged(InventorySlot[] value)
    {
        if (value.Length != _uiSlots.Length)
        {
            Debug.LogError("inventory size changed");
        }
        
        Debug.Log("inventory changed");
        for (int i = 0; i < value.Length; i++)
        {
            //Debug.Log($"{i} {(value[i].Item != null? value[i].Item.ItemSo.ItemData.Name : "empty")}");

            if (_uiSlots[i] == null)
            {
                Debug.LogError("null");
            }
            
            _uiSlots[i].UpdateSlot(value[i]);
        }
    }

#if UNITY_EDITOR
    public void UpdateEditor()
    {
        Initialize();
    }

    public void ForceUpdateEditor()
    {
        _uiSlots = _panelSlots.GetComponentsInChildren<InventorySlotUI>();
        
        Debug.Log($"inventorySize {_inventory.Size} uiSlots.Length {_uiSlots.Length}");
        for (int i = 0; i < _uiSlots.Length; i++)
        {
            Debug.Log($"destroying slot {i}");
            DestroyImmediate(_uiSlots[i].gameObject);
        }
        
        Initialize();
    }
#endif
}
}