using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Styles.Game
{

public enum BodySlot
{
    Head,
    Body,
    HandL,
    HandR,
    FootL,
    FootR,
}

public enum GarmentSlot
{
    Head,
    Top,
    Hands,
    Feet,
}

public enum Facing
{
    Front,
    Back,
    Right,
    Left
}

public class CharacterSkin : MonoBehaviour
{
    [SerializeField] private CharacterSkinSO _characterSkin;
    [SerializeField] private Facing _facing;
    
    [Header("body renderers")]
    [SerializeField] private SpriteRenderer _head;
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private SpriteRenderer _handR;
    [SerializeField] private SpriteRenderer _handL;
    [SerializeField] private SpriteRenderer _footR;
    [SerializeField] private SpriteRenderer _footL;

    [Header("Cloth renderers")] 
    [SerializeField] private SpriteRenderer _hat;
    [SerializeField] private SpriteRenderer _eyes;
    [SerializeField] private SpriteRenderer _top;
    [SerializeField] private SpriteRenderer _gloveL;
    [SerializeField] private SpriteRenderer _gloveR;
    [SerializeField] private SpriteRenderer _bootL;
    [SerializeField] private SpriteRenderer _bootR;
    
    private readonly Dictionary<BodySlot, SpriteRenderer> BodyRenderers = new();
    private readonly Dictionary<GarmentSlot, SpriteRenderer> ClothRenderers = new();

    private void Awake()
    {
        Initialize();
        SetSkin(Facing.Front);
    }

    private void Initialize()
    {
        // initialize skin renderers dictionary
        if(!BodyRenderers.ContainsKey(BodySlot.Body))
            BodyRenderers.Add(BodySlot.Body, _body);
        if(!BodyRenderers.ContainsKey(BodySlot.Head))
            BodyRenderers.Add(BodySlot.Head, _head);
        if(!BodyRenderers.ContainsKey(BodySlot.HandR))
            BodyRenderers.Add(BodySlot.HandR, _handR);
        if(!BodyRenderers.ContainsKey(BodySlot.HandL))
            BodyRenderers.Add(BodySlot.HandL, _handL);
        if(!BodyRenderers.ContainsKey(BodySlot.FootL))
            BodyRenderers.Add(BodySlot.FootL, _footL);
        if(!BodyRenderers.ContainsKey(BodySlot.FootR))
            BodyRenderers.Add(BodySlot.FootR, _footR);
    }

    public void SetSkin(Facing facing)
    {
        if (!_characterSkin)
        {
            Debug.LogError("characterSkin was not set");
            return;
        }
        
        BodyRenderers[BodySlot.Head].sprite = _characterSkin.SkinData.Head.GetSprite(facing);
        BodyRenderers[BodySlot.Body].sprite = _characterSkin.SkinData.Body.GetSprite(facing);
        BodyRenderers[BodySlot.HandL].sprite = _characterSkin.SkinData.HandL.GetSprite(facing);
        BodyRenderers[BodySlot.HandR].sprite = _characterSkin.SkinData.HandR.GetSprite(facing);
        BodyRenderers[BodySlot.FootL].sprite = _characterSkin.SkinData.FootL.GetSprite(facing);
        BodyRenderers[BodySlot.FootR].sprite = _characterSkin.SkinData.FootR.GetSprite(facing);
        
        if(facing == Facing.Left)
        {
            var localScale = transform.localScale;
            localScale.x = - Math.Abs(localScale.x);
            transform.localScale = localScale;
        }
        else if (facing == Facing.Right)
        {
            var localScale = transform.localScale;
            localScale.x = Math.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }

    private void OnValidate()
    {
        Initialize();
        SetSkin(_facing);
    }
}

[Serializable]
public struct SkinData
{
    public BodySlot Slot;
    public Sprite Front;
    public Sprite Side;
    public Sprite Back;
}

[Serializable]
public struct BodyData
{
    public Sprite Head;
    public Sprite Body;
    public Sprite HandL;
    public Sprite HandR;
    public Sprite FootL;
    public Sprite FootR;
}

[Serializable]
public struct ClothData
{
    public Sprite Hat;
    public Sprite Top;
}
}