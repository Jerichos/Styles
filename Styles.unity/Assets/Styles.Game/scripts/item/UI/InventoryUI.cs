﻿using Styles.Common;
using UnityEngine;

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
    
    private void OnSlotClicked(int value)
    {
        _inventory.UseItem(value, OnUseCallback);
    }

    private void OnUseCallback(InventorySlotCallback callback)
    {
        
    }

    private void OnEnable()
    {
        for (int i = 0; i < _uiSlots.Length; i++)
            _uiSlots[i].ClickedCallback = OnSlotClicked;
        
        _inventory.EInventoryChanged += OnInventoryChanged;
        OnInventoryChanged(_inventory.Slots);
    }

    private void OnDisable()
    {
        // don't invoke click callbacks when InventoryUI is closed
        for (int i = 0; i < _uiSlots.Length; i++)
            _uiSlots[i].ClickedCallback = null;
        
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

        _uiSlots = _panelSlots.GetComponentsInChildren<InventorySlotUI>();

        if (_uiSlots.Length != _inventory.Size)
        {
            DestroyUISlots();
            _uiSlots = new InventorySlotUI[_inventory.Size];
            for (int i = 0; i < _inventory.Size; i++)
            {
                var newSlot = Instantiate(_slotPrefab, _panelSlots, true);
                newSlot.transform.localScale = Vector3.one;
                _uiSlots[i] = newSlot;
            }
        }
        
        for (int i = 0; i < _uiSlots.Length; i++)
        {
            _uiSlots[i].SetSlotID(i);
        }
    }

    private void DestroyUISlots()
    {
        gameObject.Log("destroying ui slots");
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
            if (_uiSlots[i] == null)
            {
                Debug.LogError("null");
            }
            
            _uiSlots[i].SetSlot(value[i]);
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