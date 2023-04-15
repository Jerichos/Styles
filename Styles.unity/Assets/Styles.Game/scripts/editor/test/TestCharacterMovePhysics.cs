using System;
using Styles.Game.scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Styles.Game.editor
{
public class TestCharacterMovePhysics : MonoBehaviour
{
    [SerializeField] private CharacterPhysics2D _characterPhysics;

    [SerializeField] private Vector2 _minVelocity;
    [SerializeField] private Vector2 _maxVelocity;

    [SerializeField] private Vector2 _minPosition;
    [SerializeField] private Vector2 _maxPosition;

    private void Update()
    {
        float x = Random.Range(_minVelocity.x, _maxVelocity.x);
        float y = Random.Range(_minVelocity.y, _maxVelocity.y);
        
        _characterPhysics.Move(new Vector2(x, y));

    }

    private void LateUpdate()
    {
        var position = _characterPhysics.transform.position;

        if ((position.x < _minPosition.x || position.x > _maxPosition.x) 
            || (position.y < _minPosition.y || position.y > _maxPosition.y))
        {
            Debug.LogError("character went thru wall!");
            Debug.Break();
        }
    }
}
}
