using System;
using System.Collections.Generic;
using System.Linq;
using Styles.Common;
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

public enum OutfitSlot
{
    Head,
    Body,
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
    [SerializeField] private OutfitSO _defaultOutfit;
    
    [Header("body renderers")]
    [SerializeField] private SpriteRenderer _head;
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private SpriteRenderer _handR;
    [SerializeField] private SpriteRenderer _handL;
    [SerializeField] private SpriteRenderer _footR;
    [SerializeField] private SpriteRenderer _footL;

    [Header("Cloth renderers")] 
    [FormerlySerializedAs("_headGarment")] [SerializeField] private SpriteRenderer _headOutfitRenderer;
    [FormerlySerializedAs("_bodyGarment")] [SerializeField] private SpriteRenderer _bodyOutfitRenderer;
    [FormerlySerializedAs("_handGarmentL")] [SerializeField] private SpriteRenderer _handOutfitRendererL;
    [FormerlySerializedAs("_handGarmentR")] [SerializeField] private SpriteRenderer _handOutfitRendererR;
    [FormerlySerializedAs("_footGarmentL")] [SerializeField] private SpriteRenderer _footOutfitRendererL;
    [FormerlySerializedAs("_footGarmentR")] [SerializeField] private SpriteRenderer _footOutfitRendererR;
    
    private readonly Dictionary<BodySlot, SpriteRenderer> BodyRenderersMap = new();
    private readonly Dictionary<OutfitSlot, SpriteRenderer[]> OutfitRenderersMap = new();

    public readonly Dictionary<OutfitSlot, OutfitPiece> Outfit = new();
    
    public event GenericDelegate<Dictionary<OutfitSlot, OutfitPiece>> EOutfitChanged;

    private Facing _facing;
    
    private void Awake()
    {
        InitializeRenderersAndSlots();
        UpdateSkin(Facing.Front);
        InitializeDefaultOutfit();
        UpdateOutfit(Facing.Front);
    }

    private void InitializeRenderersAndSlots()
    {
        // initialize skin renderers dictionary
        if(!BodyRenderersMap.ContainsKey(BodySlot.Body))
            BodyRenderersMap.Add(BodySlot.Body, _body);
        if(!BodyRenderersMap.ContainsKey(BodySlot.Head))
            BodyRenderersMap.Add(BodySlot.Head, _head);
        if(!BodyRenderersMap.ContainsKey(BodySlot.HandR))
            BodyRenderersMap.Add(BodySlot.HandR, _handR);
        if(!BodyRenderersMap.ContainsKey(BodySlot.HandL))
            BodyRenderersMap.Add(BodySlot.HandL, _handL);
        if(!BodyRenderersMap.ContainsKey(BodySlot.FootL))
            BodyRenderersMap.Add(BodySlot.FootL, _footL);
        if(!BodyRenderersMap.ContainsKey(BodySlot.FootR))
            BodyRenderersMap.Add(BodySlot.FootR, _footR);
        
        // initialize garment renderers
        if (!OutfitRenderersMap.ContainsKey(OutfitSlot.Head))
            OutfitRenderersMap.Add(OutfitSlot.Head, new []{_headOutfitRenderer});
        if(!OutfitRenderersMap.ContainsKey(OutfitSlot.Body))
            OutfitRenderersMap.Add(OutfitSlot.Body, new []{_bodyOutfitRenderer});
        if (!OutfitRenderersMap.ContainsKey(OutfitSlot.Hands))
            OutfitRenderersMap.Add(OutfitSlot.Hands, new []{_handOutfitRendererL, _handOutfitRendererR});
        if(!OutfitRenderersMap.ContainsKey(OutfitSlot.Feet))
            OutfitRenderersMap.Add(OutfitSlot.Feet, new []{_footOutfitRendererL, _footOutfitRendererR});
        
        // initialize garment slots
        if(!Outfit.ContainsKey(OutfitSlot.Head))
            Outfit.Add(OutfitSlot.Head, null);
        
        if(!Outfit.ContainsKey(OutfitSlot.Body))
            Outfit.Add(OutfitSlot.Body, null);
        
        if(!Outfit.ContainsKey(OutfitSlot.Hands))
            Outfit.Add(OutfitSlot.Hands, null);
        
        if(!Outfit.ContainsKey(OutfitSlot.Feet))
            Outfit.Add(OutfitSlot.Feet, null);
        
        EOutfitChanged?.Invoke(Outfit);
    }
    
    public void UpdateSkin(Facing facing)
    {
        _facing = facing;
        
        if (!_characterSkin)
        {
            Debug.LogError("characterSkin was not set");
            return;
        }
        
        BodyRenderersMap[BodySlot.Head].sprite = _characterSkin.SkinData.Head.GetSprite(facing);
        BodyRenderersMap[BodySlot.Body].sprite = _characterSkin.SkinData.Body.GetSprite(facing);
        BodyRenderersMap[BodySlot.HandL].sprite = _characterSkin.SkinData.HandL.GetSprite(facing);
        BodyRenderersMap[BodySlot.HandR].sprite = _characterSkin.SkinData.HandR.GetSprite(facing);
        BodyRenderersMap[BodySlot.FootL].sprite = _characterSkin.SkinData.FootL.GetSprite(facing);
        BodyRenderersMap[BodySlot.FootR].sprite = _characterSkin.SkinData.FootR.GetSprite(facing);
        
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

    public void UpdateOutfit(Facing facing)
    {
        _facing = facing;
        
        foreach (var outfitPiece in Outfit)
        {
            UpdateOutfitSlot(outfitPiece.Key, facing);
        }
    }
    
    public void Unequip(OutfitSlot slot)
    {
        Debug.Log($"unequip {slot}");
        if (Outfit[slot] == null)
        {
            Debug.Log("nothing to unquip");
            return;
        }
        
        Outfit[slot] = null;
        UpdateOutfitSlot(slot, _facing);
        EOutfitChanged?.Invoke(Outfit);
    }

    private void UpdateOutfitSlot(OutfitSlot slot, Facing facing)
    {
        _facing = facing;
        
        if (!OutfitRenderersMap.ContainsKey(slot))
        {
            Debug.LogError($"missing slot key in GarmentRenderers of slot {slot.ToString()}");
            return;
        }
        
        if (!OutfitRenderersMap[slot][0])
        {
            Debug.LogError($"missing SpriteRenderer in GarmentRenderers of slot {slot.ToString()}");
            return;
        }

        var outfitPiece = Outfit[slot];
        
        if (outfitPiece == null)
        {
            foreach (var renderers in OutfitRenderersMap[slot])
            {
                renderers.sprite = null;
                renderers.gameObject.SetActive(false);
            }
            
            return;
        }

        if (((Item) outfitPiece).ItemSO is OutfitPieceSO outfitPieceSO)
        {
            OutfitRenderersMap[slot][0].gameObject.SetActive(true);
            OutfitRenderersMap[slot][0].sprite = outfitPieceSO.SpriteVariants.GetSprite(facing);
        }
        
        if (((Item) outfitPiece).ItemSO is OutfitPieceDoubleVariantSo doubleOutfitPiece)
        {
            // check if there is renderer for right variant sprite
            if (OutfitRenderersMap[slot].Length <= 1 || !OutfitRenderersMap[slot][1])
            {
                Debug.LogError($"missing right variant SpriteRenderer in GarmentRenderers of slot {slot.ToString()}");
                return;
            }
            
            OutfitRenderersMap[slot][1].gameObject.SetActive(true);
            OutfitRenderersMap[slot][1].sprite = doubleOutfitPiece.RightSpriteVariants.GetSprite(facing);
        }
    }

    private void InitializeDefaultOutfit()
    {
        if (!_defaultOutfit)
        {
            transform.LogWarning("default outfit not set. Character is naked!");
            DestroyAllClothing();
            return;
        }

        foreach (OutfitSlot slot in Enum.GetValues(typeof(OutfitSlot)))
        {
            if (!_defaultOutfit.GetOutfitPieceSO(slot))
            {
                Debug.LogWarning($"there is no default outfit garment for slot {slot.ToString()}");
                Outfit[slot] = null;
                continue;
            }

            if (!Outfit.ContainsKey(slot))
            {
                Debug.LogError($"there is no key slot defined in Garments. {slot.ToString()}");
                continue;
            }
            
            Outfit[slot] = _defaultOutfit.CreatePieceInstance(slot);
        }
        
        EOutfitChanged?.Invoke(Outfit);
    }

    private void DestroyAllClothing()
    {
        Outfit.Keys.ToList().ForEach(x => Outfit[x] = null);
        EOutfitChanged?.Invoke(Outfit);
    }

    private void OnValidate()
    {
        InitializeRenderersAndSlots();
        UpdateSkin(Facing.Front);
        InitializeDefaultOutfit();
        UpdateOutfit(Facing.Front);
    }

    public OutfitPiece EquipItem(OutfitPiece outfitPiece)
    {
        Debug.Log("3 equip item");
        var previousOutfitPiece = Outfit[outfitPiece.ItemSO.Slot];
        Outfit[outfitPiece.ItemSO.Slot] = outfitPiece;
        UpdateOutfitSlot(outfitPiece.ItemSO.Slot, _facing);
        EOutfitChanged?.Invoke(Outfit);
        return previousOutfitPiece;
    }
}
}