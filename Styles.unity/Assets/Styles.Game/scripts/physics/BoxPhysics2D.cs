using System;
using UnityEngine;

namespace Styles.Game.scripts.physics
{
public class BoxPhysics2D
{
    private BoxCollider2D _collider;

    private int _verticalRayCount;
    private int _horizontalRayCount;

    private float _maxRayGap;
    private float _colliderHalfWidth;
    private float _colliderHalfHeight;

    private LayerMask _collisionMask;
    private Vector2 _offset;
    private Vector2 _depthSize;

    public BoxPhysics2D(BoxCollider2D collider, float maxRayGap, LayerMask collisionMask, float depth = 0.00f, float shrinkSizeMag = 0.90f)
    {
        _collisionMask = collisionMask;
        _collider = collider;
        
        var shrinkedSize = _collider.size * shrinkSizeMag;
        _depthSize = Vector2.one * (1 - shrinkSizeMag) / 2;
        
        _verticalRayCount = (int) Mathf.Ceil(shrinkedSize.y / maxRayGap) + 1;
        _horizontalRayCount = (int) Mathf.Ceil(shrinkedSize.x / maxRayGap) + 1;

        _maxRayGap = maxRayGap;

        _colliderHalfWidth = shrinkedSize.x / 2;
        _colliderHalfHeight = shrinkedSize.y / 2;

        _offset = _collider.offset;
    }

    public bool CheckHorizontalCollision(Vector2 position, float deltaX, out RaycastHit2D hit)
    {
        hit = new RaycastHit2D();
        
        if (deltaX == 0)
            return false;

        var dir = Mathf.Sign(deltaX);

        for (int i = 0; i < _horizontalRayCount; i++)
        {
            Vector2 rayOrigin = position + Vector2.up * (_maxRayGap * i) + Vector2.right * (dir * _colliderHalfWidth)- Vector2.up * (_colliderHalfHeight) + _offset;
            Vector2 rayDirection = Vector2.right * dir;
            float rayLength = Math.Abs(deltaX) + _depthSize.x;

            Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);

            hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, _collisionMask);
            
            if(hit)
            {
                hit.distance -= _depthSize.x;
                Debug.DrawLine(rayOrigin, hit.point, Color.red);
                return true;
            }
        }

        return false;
    }
    
    public bool CheckVerticalCollision(Vector2 position, float deltaY, out RaycastHit2D hit)
    {
        hit = new RaycastHit2D();
        
        if (deltaY == 0)
            return false;

        var dir = Mathf.Sign(deltaY);

        for (int i = 0; i < _verticalRayCount; i++)
        {
            Vector2 rayOrigin = position + Vector2.right * (_maxRayGap * i) - Vector2.right * (_colliderHalfWidth) 
                + Vector2.up * (dir * _colliderHalfHeight) + _offset;
            Vector2 rayDirection = Vector2.up * dir;
            float rayLength = Math.Abs(deltaY) + _depthSize.y;

            Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);

            hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, _collisionMask);
            
            if(hit)
            {
                hit.distance -= _depthSize.y;
                Debug.DrawLine(rayOrigin, hit.point, Color.red);
                return true;
            }
        }

        return false;
    }
}
}