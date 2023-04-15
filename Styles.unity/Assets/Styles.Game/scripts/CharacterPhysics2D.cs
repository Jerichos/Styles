using System;
using UnityEngine;

namespace Styles.Game.scripts
{
public class CharacterPhysics2D : MonoBehaviour
{
    [SerializeField] private float _baseMoveSpeed;

    private Transform _transform;
    private Vector2 _velocity;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.position += (Vector3)_velocity * Time.deltaTime;
    }

    public void Move(Vector2 direction)
    {
        _velocity =  direction * _baseMoveSpeed;
        Debug.Log($"velocity: {_velocity}");
    }
}
}