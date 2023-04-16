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

    private Vector2 _facing;
    public Vector2 Facing => _facing;
    
    public CharacterPhysics2D Physics => _physics;
    public CharacterInteractions Interactions => _interactions;
    
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
        _interactions.TryInteract(_facing, OnInteraction);
    }

    private void OnInteraction(IInteractable value)
    {
        value?.Interact(this);
    }

    public void Move(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _facing = direction;
        }
        
        _physics.Move(direction);
    }
}
}