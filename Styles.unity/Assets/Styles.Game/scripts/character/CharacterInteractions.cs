using System;
using Styles.Common;
using Styles.Game.scripts;
using UnityEngine;

namespace Styles.Game
{
public class CharacterInteractions : MonoBehaviour
{
    [SerializeField] private LayerMask _interactionLayers;
    [SerializeField] private Vector2 _originOffset;
    [SerializeField] private float _interactionDistance = 0.1f;
    
    public void TryInteract(Vector2 direction, GenericDelegate<IInteractable> callback)
    {
        Vector2 rayOrigin = transform.position + (Vector3) _originOffset;
        
        var hit = Physics2D.Raycast(rayOrigin, direction, _interactionDistance, _interactionLayers);

#if UNITY_EDITOR
        DebugRay(rayOrigin, direction, _interactionDistance);
#endif

        if (!hit)
            return;

        Debug.DrawLine(rayOrigin, hit.point, new Color(1f, 0f, 0.82f));
        var interactable = hit.transform.GetComponent<IInteractable>();
        callback?.Invoke(interactable);
        
#if UNITY_EDITOR
        DebugHit(rayOrigin, hit);
#endif
    }

    public Vector2 SetOriginOffset
    {
        set => _originOffset = value;
    }

#if UNITY_EDITOR
    [Space]
    [Header("debug")]
    [SerializeField] private float _gizmoTtl = 1f;
    private float _t;
    private Vector2 _rayOrigin;
    private Vector2 _hitOrigin;
    private Vector2 _rayDirection;
    private float _rayLength;
    private bool _isHit;
    private RaycastHit2D _hit; 

    private void DebugRay(Vector2 rayOrigin, Vector2 rayDirection, float rayLength)
    {
        _rayOrigin = rayOrigin;
        _rayDirection = rayDirection;
        _rayLength = rayLength;
        _t = Time.realtimeSinceStartup;
    }

    private void DebugHit(Vector2 hitOrigin, RaycastHit2D hit)
    {
        _isHit = true;
        _hit = hit;
        _hitOrigin = hitOrigin;
        _t = Time.realtimeSinceStartup;
    }

    private void OnDrawGizmos()
    {
        if (Time.realtimeSinceStartup > _t + _gizmoTtl)
        {
            _isHit = false;
            return;
        }
        
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_rayOrigin, _rayDirection * _rayLength);

        if(!_isHit)
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_hitOrigin, _hit.point);
        Gizmos.DrawSphere(_hitOrigin, 0.05f);
        Gizmos.DrawSphere(_hit.point, 0.025f);
    }
#endif
}

public interface IInteractable
{
    void Interact(MonoBehaviour actor);
}
}