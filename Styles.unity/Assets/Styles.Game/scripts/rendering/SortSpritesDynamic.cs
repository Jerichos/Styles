using System;
using UnityEngine;

namespace Styles.Game.rendering
{
public class SortSpritesDynamic : SortSpritesStatic
{
    private float _y;
    private float Y
    {
        set
        {
            if(_y == value)
                return;

            _y = value;
            SortSprites(_y);
        }
    }

    private void Update()
    {
        Y = transform.position.y;
    }
}
}