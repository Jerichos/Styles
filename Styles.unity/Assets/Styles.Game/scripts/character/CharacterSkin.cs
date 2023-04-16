using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Facing _facing;
    
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
    private readonly Dictionary<GarmentSlot, SpriteRenderer> GarmentRenderers = new();
    private readonly Dictionary<GarmentSlot, Garment> Garments = new();

    private void Awake()
    {
        InitializeRenderersAndSlots();
        UpdateSkin(Facing.Front);
        InitializeDefaultGarments();
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
            GarmentRenderers.Add(GarmentSlot.Head, _headGarment);
            Debug.Log("headGarment renderer added");
        }
        if(!GarmentRenderers.ContainsKey(GarmentSlot.Body))
            GarmentRenderers.Add(GarmentSlot.Body, _bodyGarment);
        if (!GarmentRenderers.ContainsKey(GarmentSlot.Hands))
            GarmentRenderers.Add(GarmentSlot.Hands, _handGarmentL);
        if(!GarmentRenderers.ContainsKey(GarmentSlot.Feet))
            GarmentRenderers.Add(GarmentSlot.Feet, _footGarmentL);
        
        // initialize garment slots
        if(!Garments.ContainsKey(GarmentSlot.Head))
            Garments.Add(GarmentSlot.Head, null);
        
        if(!Garments.ContainsKey(GarmentSlot.Body))
            Garments.Add(GarmentSlot.Body, null);
        
        if(!Garments.ContainsKey(GarmentSlot.Hands))
            Garments.Add(GarmentSlot.Hands, null);
        
        if(!Garments.ContainsKey(GarmentSlot.Feet))
            Garments.Add(GarmentSlot.Feet, null);
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
            
            if (!GarmentRenderers[garment.Key])
            {
                Debug.LogError($"missing SpriteRenderer in GarmentRenderers of slot {garment.Key.ToString()}");
                continue;
            }
            
            if (garment.Value == null)
            {
                Debug.Log("garment not set");
                GarmentRenderers[garment.Key].sprite = null;
                GarmentRenderers[garment.Key].gameObject.SetActive(false);
                continue;
            }

            GarmentRenderers[garment.Key].gameObject.SetActive(true);
            Debug.Log(garment.Value.ItemSo.GarmentData.GetSprite(facing).ToString());
            GarmentRenderers[garment.Key].sprite = garment.Value.ItemSo.GarmentData.GetSprite(facing);
        }
    }

    private void InitializeDefaultGarments()
    {
        if (!_defaultOutfit)
        {
            Debug.Log("default outfit not set");
            DestroyAllClothing();
            return;
        }
        
        if (_defaultOutfit.OutfitData.Head != null)
        {
            var bodyGarment = _defaultOutfit.OutfitData.Head.CreateItemInstance();
            Garments[GarmentSlot.Head] = bodyGarment;
        }
        
        if (_defaultOutfit.OutfitData.Body != null)
        {
            var bodyGarment = _defaultOutfit.OutfitData.Body.CreateItemInstance();
            Garments[GarmentSlot.Body] = bodyGarment;
        }
        
        if (_defaultOutfit.OutfitData.Hands != null)
        {
            var bodyGarment = _defaultOutfit.OutfitData.Hands.CreateItemInstance();
            Garments[GarmentSlot.Hands] = bodyGarment;
        }
        
        if (_defaultOutfit.OutfitData.Feet != null)
        {
            var bodyGarment = _defaultOutfit.OutfitData.Feet.CreateItemInstance();
            Garments[GarmentSlot.Feet] = bodyGarment;
        }
    }

    private void DestroyAllClothing()
    {
        Garments.Keys.ToList().ForEach(x => Garments[x] = null);
    }

    private void OnValidate()
    {
        InitializeRenderersAndSlots();
        UpdateSkin(Facing.Front);
        InitializeDefaultGarments();
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