using System;
using Styles.Game.scripts.physics;
using UnityEngine;

namespace Styles.Game.scripts
{

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterPhysics2D : MonoBehaviour
{
    [SerializeField] private Vector2 _motor;
    [SerializeField] private float _baseMoveSpeed;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private LayerMask _collisionMask;

    private Transform _transform;
    private Vector2 _velocity;
    private Vector2 _velocityDelta;

    private BoxPhysics2D _boxPhysics;
    
    private void Awake()
    {
        _transform = transform;
        _boxPhysics = new BoxPhysics2D(_collider, 0.1f, _collisionMask);
    }

    private void Update()
    {
        _velocityDelta = _velocity * Time.deltaTime;
        bool horizontalCollision = _boxPhysics.CheckHorizontalCollision(transform.position, _velocityDelta.x, out var horizontalHit);
        bool verticalCollision = _boxPhysics.CheckVerticalCollision(transform.position, _velocityDelta.y, out var verticalHit);

        if (horizontalCollision)
        {
            // snap to to hit
            var horizontalSnap =  Mathf.Sign(_velocityDelta.x) * horizontalHit.distance;
            Debug.Log($"hitDistance: {horizontalHit.distance} horizontalSnap: {horizontalSnap}");
            _transform.position += Vector3.right * horizontalSnap;
            _velocityDelta.x = 0;
        }

        if (verticalCollision)
        {
            var verticalSnap = Mathf.Sign(_velocityDelta.y) * verticalHit.distance;
            Debug.Log($"hitDistance: {verticalHit.distance} verticalSnap: {verticalSnap}");
            _transform.position += Vector3.up * verticalSnap;
            _velocityDelta.y = 0;
        }
        
        _transform.position += (Vector3)_velocityDelta;
    }

    public void Move(Vector2 direction)
    {
        _velocity =  direction * _baseMoveSpeed;
        Debug.Log($"velocity: {_velocity}");
    }
}
}