using System;
using UnityEngine;

namespace Styles.Game.scripts
{

[RequireComponent(typeof(CharacterPhysics2D))]
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private CharacterPhysics2D _physics;

    public CharacterPhysics2D Physics => _physics;
    
    private void OnValidate()
    {
        _physics = GetComponent<CharacterPhysics2D>();

        if (!_physics)
        {
            Debug.LogWarning($"missing required component {nameof(CharacterPhysics2D)}. Adding one.");
            _physics = gameObject.AddComponent<CharacterPhysics2D>();
        }
    }
}
}