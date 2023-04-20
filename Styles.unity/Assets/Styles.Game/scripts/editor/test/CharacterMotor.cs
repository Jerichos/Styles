using System;
using Styles.Game.scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Styles.Game.editor
{
public class CharacterMotor : MonoBehaviour
{
    [SerializeField] private CharacterPhysics2D _characterPhysics;

    [SerializeField] private Vector2 _velocity;

    private void Update()
    {
        _characterPhysics.Move(_velocity);
    }
}
}
