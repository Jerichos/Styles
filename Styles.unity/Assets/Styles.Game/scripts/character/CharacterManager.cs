﻿using Styles.Game.extensions;
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

    private Facing _facing;
    public Facing Facing
    {
        get => _facing;
        set
        {
            if (_facing == value)
                return;

            _facing = value;
            _skin.SetSkin(_facing);
        }
    }
    
    
    public CharacterPhysics2D Physics => _physics;
    public CharacterInteractions Interactions => _interactions;
    public CharacterSkin Skin => _skin;
    
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
            Facing = direction.GetFacing();
        }
        
        _physics.Move(direction);
    }
}
}