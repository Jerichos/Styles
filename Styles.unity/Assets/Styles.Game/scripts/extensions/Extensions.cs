
using System;
using UnityEngine;

namespace Styles.Game.extensions
{
public static class Extensions
{
    public static Facing GetFacing(this Vector2 direction)
    {
        if (direction.x > 0)
            return Facing.Right;
        if (direction.x < 0)
            return Facing.Left;
        if (direction.y > 0)
            return Facing.Back;
        if (direction.y < 0)
            return Facing.Front;

        return Facing.Front;
    }

    public static Vector2 GetDirection(this Facing facing)
    {
        return facing switch
        {
            Facing.Front => Vector2.down,
            Facing.Back => Vector2.up,
            Facing.Right => Vector2.right,
            Facing.Left => Vector2.left,
            _ => throw new ArgumentOutOfRangeException(nameof(facing), facing, null)
        };
    }
}
}