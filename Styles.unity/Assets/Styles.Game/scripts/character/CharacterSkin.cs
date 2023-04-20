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

public enum GarmentSlot
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
    [SerializeField] private SpriteRenderer _headGarment;
    [SerializeField] private SpriteRenderer _bodyGarment;
    [SerializeField] private SpriteRenderer _handGarmentL;
    [SerializeField] private SpriteRenderer _handGarmentR;
    [SerializeField] private SpriteRenderer _footGarmentL;
    [SerializeField] private SpriteRenderer _footGarmentR;
    
    private readonly Dictionary<BodySlot, SpriteRenderer> BodyRenderers = new();
    private readonly Dictionary<GarmentSlot, SpriteRenderer[]> GarmentRenderers = new();

    // TODO: change Garments to outfit (more understandable)
    public readonly Dictionary<GarmentSlot, Garment> Garments = new();
    
    public event GenericDelegate<Dictionary<GarmentSlot, Garment>> EOutfitChanged;
    public event GenericDelegate<Garment> EItemUnquiped;

    private Facing _facing;
    
    private void Awake()
    {
        InitializeRenderersAndSlots();
        UpdateSkin(Facing.Front);
        InitializeDefaultOutfit();
        UpdateGarments(Facing.Front);
    }

    private void InitializeRenderersAndSlots()
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
        
        // initialize garment renderers
        if (!GarmentRenderers.ContainsKey(GarmentSlot.Head))
            GarmentRenderers.Add(GarmentSlot.Head, new []{_headGarment});
        if(!GarmentRenderers.ContainsKey(GarmentSlot.Body))
            GarmentRenderers.Add(GarmentSlot.Body, new []{_bodyGarment});
        if (!GarmentRenderers.ContainsKey(GarmentSlot.Hands))
            GarmentRenderers.Add(GarmentSlot.Hands, new []{_handGarmentL, _handGarmentR});
        if(!GarmentRenderers.ContainsKey(GarmentSlot.Feet))
            GarmentRenderers.Add(GarmentSlot.Feet, new []{_footGarmentL, _footGarmentR});
        
        // initialize garment slots
        if(!Garments.ContainsKey(GarmentSlot.Head))
            Garments.Add(GarmentSlot.Head, null);
        
        if(!Garments.ContainsKey(GarmentSlot.Body))
            Garments.Add(GarmentSlot.Body, null);
        
        if(!Garments.ContainsKey(GarmentSlot.Hands))
            Garments.Add(GarmentSlot.Hands, null);
        
        if(!Garments.ContainsKey(GarmentSlot.Feet))
            Garments.Add(GarmentSlot.Feet, null);
        
        EOutfitChanged?.Invoke(Garments);
    }
    
    public void UpdateSkin(Facing facing)
    {
        _facing = facing;
        
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

    public void UpdateGarments(Facing facing)
    {
        _facing = facing;
        
        foreach (var garment in Garments)
        {
            UpdateGarmentSlot(garment.Key, facing);
        }
    }
    
    public void Unequip(GarmentSlot slot)
    {
        Debug.Log($"unequip {slot}");
        if (Garments[slot] == null)
        {
            Debug.Log("nothing to unquip");
            return;
        }
        
        var garment = Garments[slot];
        Garments[slot] = null;
        UpdateGarmentSlot(slot, _facing);
        EOutfitChanged?.Invoke(Garments);
    }

    private void UpdateGarmentSlot(GarmentSlot slot, Facing facing)
    {
        _facing = facing;
        
        if (!GarmentRenderers.ContainsKey(slot))
        {
            Debug.LogError($"missing slot key in GarmentRenderers of slot {slot.ToString()}");
            return;
        }
        
        if (!GarmentRenderers[slot][0])
        {
            Debug.LogError($"missing SpriteRenderer in GarmentRenderers of slot {slot.ToString()}");
            return;
        }

        var garment = Garments[slot];
        
        if (garment == null)
        {
            foreach (var renderers in GarmentRenderers[slot])
            {
                renderers.sprite = null;
                renderers.gameObject.SetActive(false);
            }
            
            return;
        }

        if (((Item) garment).ItemSO is GarmentSO garmentSO)
        {
            GarmentRenderers[slot][0].gameObject.SetActive(true);
            GarmentRenderers[slot][0].sprite = garmentSO.SpriteVariants.GetSprite(facing);
        }
        
        if (((Item) garment).ItemSO is GarmentDoubleVariantSO garmentDoubleSO)
        {
            // check if there is renderer for right variant sprite
            if (GarmentRenderers[slot].Length <= 1 || !GarmentRenderers[slot][1])
            {
                Debug.LogError($"missing right variant SpriteRenderer in GarmentRenderers of slot {slot.ToString()}");
                return;
            }
            
            GarmentRenderers[slot][1].gameObject.SetActive(true);
            GarmentRenderers[slot][1].sprite = garmentDoubleSO.RightSpriteVariants.GetSprite(facing);
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

        foreach (GarmentSlot slot in Enum.GetValues(typeof(GarmentSlot)))
        {
            if (!_defaultOutfit.GetOutfitPieceSO(slot))
            {
                Debug.LogWarning($"there is no default outfit garment for slot {slot.ToString()}");
                Garments[slot] = null;
                continue;
            }

            if (!Garments.ContainsKey(slot))
            {
                Debug.LogError($"there is no key slot defined in Garments. {slot.ToString()}");
                continue;
            }
            
            Garments[slot] = _defaultOutfit.CreatePieceInstance(slot);
        }
        
        EOutfitChanged?.Invoke(Garments);
    }

    private void DestroyAllClothing()
    {
        Garments.Keys.ToList().ForEach(x => Garments[x] = null);
        EOutfitChanged?.Invoke(Garments);
    }

    private void OnValidate()
    {
        InitializeRenderersAndSlots();
        UpdateSkin(Facing.Front);
        InitializeDefaultOutfit();
        UpdateGarments(Facing.Front);
    }

    public Garment EquipItem(Garment garment)
    {
        Debug.Log("3 equip item");
        var previousGarment = Garments[garment.ItemSO.Slot];
        Garments[garment.ItemSO.Slot] = garment;
        UpdateGarmentSlot(garment.ItemSO.Slot, _facing);
        EOutfitChanged?.Invoke(Garments);
        return previousGarment;
    }
}
}