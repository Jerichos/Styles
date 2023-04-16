using System;
using UnityEngine;

namespace Styles.Game
{
[CreateAssetMenu(fileName = "CharacterSkin", menuName = "data/CharacterSkin", order = 0)]
public class CharacterSkinSO : ScriptableObject
{
    [SerializeField] private CharacterSkinData _skinData;

    public CharacterSkinData SkinData => _skinData;
}

[Serializable]
public struct CharacterSkinData
{
    public SpriteSlot Head;
    public SpriteSlot Body;
    public SpriteSlot HandR;
    public SpriteSlot HandL;
    public SpriteSlot FootR;
    public SpriteSlot FootL;
}

[Serializable]
public struct SpriteSlot
{
    public Sprite Front;
    public Sprite Back;
    public Sprite Side;

    public Sprite GetSprite(Facing facing)
    {
        return facing switch
        {
            Facing.Front => Front,
            Facing.Back => Back,
            Facing.Right => Side,
            Facing.Left => Side,
            _ => throw new ArgumentOutOfRangeException(nameof(facing), facing, null)
        };
    }
}
}