using System;
using UnityEngine;

namespace Styles.Game.scripts.physics
{
public class BoxPhysics2D
{
    private BoxCollider2D _collider;

    private int _verticalRayCount;
    private int _horizontalRayCount;

    private float _rayGap;
    private float _colliderHalfWidth;
    private float _colliderHalfHeight;

    private LayerMask _collisionMask;
    private Vector2 _offset;
    private Vector2 _depthSize;
    private Vector2 _depthCollider;

    /// <summary>
    /// Cast rays from box sides. We expect that the BoxCollider2 is square. It must have same width and height.
    /// TODO: Add support for rectangles.
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="rayGap"></param>
    /// <param name="collisionMask"></param>
    /// <param name="skinSizeMag">1 means rays will start from the edge of the BoxCollider. At 0.90 the rays will start at edge of imaginary BoxCollider shrunk by 10%</param>
    public BoxPhysics2D(BoxCollider2D collider, float maxRayGap, LayerMask collisionMask, float skinSizeMag = 0.80f)
    {
        _collisionMask = collisionMask;
        _collider = collider;
        
        _depthCollider = _collider.size * skinSizeMag;
        _depthSize = Vector2.one * (1 - skinSizeMag) / 2;
        
        _verticalRayCount = Mathf.FloorToInt(_depthCollider.y / maxRayGap) + 2;
        _horizontalRayCount = Mathf.FloorToInt(_depthCollider.x / maxRayGap) + 2;

        _rayGap = _depthCollider.x / (_horizontalRayCount - 1);

        _colliderHalfWidth = _depthCollider.x / 2;
        _colliderHalfHeight = _depthCollider.y / 2;

        Debug.Log($"colliderSize {collider.size} depthBoxCollider: {_depthCollider} gap: {_rayGap} rays: {_verticalRayCount}");
        
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
            Vector2 rayOrigin = position + Vector2.up * (_rayGap * i) + Vector2.right * (dir * _colliderHalfWidth)- Vector2.up * (_colliderHalfHeight) + _offset;
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
            Vector2 rayOrigin = position + Vector2.right * (_rayGap * i) - Vector2.right * (_colliderHalfWidth) 
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

    public void DrawGizmo(Vector2 transformPosition)
    {
        var position = transformPosition + _collider.offset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(position, _depthCollider);
    }
}
}