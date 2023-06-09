﻿using System;
using Styles.Game.scripts.physics;
using UnityEngine;

namespace Styles.Game
{

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterPhysics2D : MonoBehaviour
{
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
            _transform.position += Vector3.right * horizontalSnap;
            _velocityDelta.x = 0;
        }

        if (verticalCollision)
        {
            var verticalSnap = Mathf.Sign(_velocityDelta.y) * verticalHit.distance;
            _transform.position += Vector3.up * verticalSnap;
            _velocityDelta.y = 0;
        }
        
        _transform.position += (Vector3)_velocityDelta;
    }

    public void Move(Vector2 direction)
    {
        _velocity =  direction * _baseMoveSpeed;
    }

    public void Teleport(Vector2 teleportPosition)
    {
        _transform.position = teleportPosition;
    }

    private void OnDrawGizmos()
    {
        _boxPhysics?.DrawGizmo(transform.position);
    }
}
}