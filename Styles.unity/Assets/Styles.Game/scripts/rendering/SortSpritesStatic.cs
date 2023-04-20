using System;
using UnityEngine;

namespace Styles.Game.rendering
{
public class SortSpritesStatic : MonoBehaviour
{
    [SerializeField] protected SpriteSortingSettings _sortingSettings;
    
    protected SpriteRenderer[] _renderers;
    protected int[] _defaultOrders;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>(true);
        _defaultOrders = new int[_renderers.Length];

        for (int i = 0; i < _defaultOrders.Length; i++)
        {
            _defaultOrders[i] = _renderers[i].sortingOrder;
        }
        
        SortSprites(transform.position.y);
    }
    
    protected void SortSprites(float y)
    {
        int sortingOrder = Mathf.CeilToInt(y * _sortingSettings.StepsPerUnit) * _sortingSettings.Step;

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].sortingOrder = _defaultOrders[i] - sortingOrder;
        }
    }
}
}