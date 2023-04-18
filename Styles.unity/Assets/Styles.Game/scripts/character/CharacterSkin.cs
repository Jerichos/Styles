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
    
    public GenericDelegate<Dictionary<GarmentSlot, Garment>> EOutfitChanged;

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
        {
            GarmentRenderers.Add(GarmentSlot.Head, new []{_headGarment});
            Debug.Log("headGarment renderer added");
        }
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
        foreach (var garment in Garments)
        {
            Debug.Log("update garment of slot " + garment.Key.ToString());
            
            if (!GarmentRenderers.ContainsKey(garment.Key))
            {
                Debug.LogError($"missing slot key in GarmentRenderers of slot {garment.Key.ToString()}");
                continue;
            }
            
            if (!GarmentRenderers[garment.Key][0])
            {
                Debug.LogError($"missing SpriteRenderer in GarmentRenderers of slot {garment.Key.ToString()}");
                continue;
            }
            
            if (garment.Value == null)
            {
                foreach (var renderers in GarmentRenderers[garment.Key])
                {
                    renderers.sprite = null;
                    renderers.gameObject.SetActive(false);
                }
                continue;
            }

            if (garment.Value.ItemSo is GarmentSO garmentSO)
            {
                GarmentRenderers[garment.Key][0].gameObject.SetActive(true);
                GarmentRenderers[garment.Key][0].sprite = garmentSO.GarmentData.GetSprite(facing);
                Debug.Log(garmentSO.GarmentData.GetSprite(facing).ToString());
            }
            else if (garment.Value.ItemSo is GarmentDoubleVariantSO garmentDoubleSO)
            {
                GarmentRenderers[garment.Key][0].gameObject.SetActive(true);
                GarmentRenderers[garment.Key][0].sprite = garmentDoubleSO.GarmentData.GetLeftSprite(facing);
                
                // check if there is renderer for right variant sprite
                if (GarmentRenderers[garment.Key].Length <= 1 || !GarmentRenderers[garment.Key][1])
                {
                    Debug.LogError($"missing right variant SpriteRenderer in GarmentRenderers of slot {garment.Key.ToString()}");
                    continue;
                }
                
                GarmentRenderers[garment.Key][1].gameObject.SetActive(true);
                GarmentRenderers[garment.Key][1].sprite = garmentDoubleSO.GarmentData.GetRightSprite(facing);
                
                Debug.Log(garmentDoubleSO.GarmentData.GetLeftSprite(facing).ToString());
                Debug.Log(garmentDoubleSO.GarmentData.GetRightSprite(facing).ToString());
            }
        }
    }

    private void InitializeDefaultOutfit()
    {
        if (!_defaultOutfit)
        {
            Debug.Log("default outfit not set");
            DestroyAllClothing();
            return;
        }

        foreach (GarmentSlot slot in Enum.GetValues(typeof(GarmentSlot)))
        {
            Debug.Log($" set default {slot.ToString()}" );

            if (!_defaultOutfit.GetPieceItemSO(slot))
            {
                Debug.Log($"there is no default outfit garment for slot {slot.ToString()}");
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