using System;
using UnityEngine;

namespace Styles.Game
{
public class CharacterTeleport : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _teleportTo;
    private Vector2 _teleportPosition;

    private void Awake()
    {
        _teleportPosition = _teleportTo.position;
    }

    public void Interact(MonoBehaviour actor)
    {
        if (actor is not CharacterManager character)
            return;

        character.Physics.Teleport(_teleportPosition);
    }
    
    
}
}