using System;
using Styles.Common;
using Styles.Game.extensions;
using Styles.Game.npc;
using Styles.Game.scripts;
using UnityEngine;

namespace Styles.Game
{

[RequireComponent(typeof(CharacterPhysics2D))]
[RequireComponent(typeof(CharacterInteractions))]
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private CharacterPhysics2D _physics;
    [SerializeField] private CharacterInteractions _interactions;
    [SerializeField] private CharacterSkin _skin;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Wallet _wallet;

    public CharacterPhysics2D Physics => _physics;
    public CharacterInteractions Interactions => _interactions;
    public CharacterSkin Skin => _skin;
    public Inventory Inventory => _inventory;
    public Wallet Wallet => _wallet;

    private void Awake()
    {
        if (_wallet)
            _wallet.Owner = this;
    }

    private Facing _facing;
    private Facing SetFacing
    {
        set
        {
            if (_facing == value)
                return;

            _facing = value;
            _skin.UpdateSkin(_facing);
            _skin.UpdateGarments(_facing);
        }
    }

    public void AddItemToInventory(Item item, GenericDelegate<InventorySlotCallback> callback)
    {
        _inventory.AddItem(item, callback);
    }
    
    private void OnValidate()
    {
        _physics = GetComponent<CharacterPhysics2D>();
        _interactions = GetComponent<CharacterInteractions>();

        if (!_physics)
        {
            Debug.LogWarning($"missing required component {nameof(CharacterPhysics2D)}. Adding one.");
            _physics = gameObject.AddComponent<CharacterPhysics2D>();
        }
        
        if (!_interactions)
        {
            Debug.LogWarning($"missing required component {nameof(CharacterInteractions)}. Adding one.");
            _interactions = gameObject.AddComponent<CharacterInteractions>();
        }
    }

    public void Interact()
    {
        _interactions.TryInteract(_facing.GetDirection(), OnInteraction);
    }

    private void OnInteraction(IInteractable value)
    {
        value?.Interact(this);
    }

    public void Move(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            SetFacing = direction.GetFacing();
        }
        
        _wallet.StopShopping();
        _physics.Move(direction);
    }
    
    private void OnSlotUsed(int slotID, InventorySlot slot, GenericDelegate<InventorySlotCallback> callback)
    {
        // if slot item is outfit piece, Equip it!
        if (slot.Item is Garment garment)
        {
            var previousItem = _skin.EquipItem(garment);
            callback?.Invoke(new InventorySlotCallback{SlotID = slotID, ReturnCode = InventoryReturnCode.RemoveItem, ReturningItem = previousItem});
        }
        else
        {
            Debug.LogWarning("not handled item type");
        }
    }
    
    private void OnItemPurchased(Item value)
    {
        Debug.Log($"!!! item purchased {value}");
        _inventory.AddItem(value, OnAddToInventory);
    }

    private void OnAddToInventory(InventorySlotCallback value)
    {
        Debug.Log(value.ReturnCode.ToString());
    }

    private void OnEnable()
    {
        _inventory.SlotUsedCallback = OnSlotUsed;
        _wallet.OnItemPurchased += OnItemPurchased;
    }

    private void OnDisable()
    {
        _inventory.SlotUsedCallback = null;
        _wallet.OnItemPurchased -= OnItemPurchased;
    }
}
}