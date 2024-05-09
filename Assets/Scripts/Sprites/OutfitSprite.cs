using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class OutfitSprite : MonoBehaviour
{

    #region Variables
    
    [SerializeField]private CharacterSpriteChangeDetector spriteChangeDetector;
    
    private ItemSo _itemSo;
    
    #endregion

    #region Properties
    
    private SpriteRenderer SpRenderer => GetComponent<SpriteRenderer>();
    
    #endregion

    #region Methods
    
    private void Awake()
    {
        SetListeners();
        Setup();
    }

    private void Setup()
    {
        if (!_itemSo)
            SpRenderer.sprite = null;
        else
        {
            ChangeFrame(spriteChangeDetector.CurrentFrame);
        }
    }

    private void SetListeners()
    {
        spriteChangeDetector.onFrameChanged.AddListener(ChangeFrame);
    }
    
    public void SetItem(ItemSo itemSo)
    {
        _itemSo = itemSo;
        if (!_itemSo)
            SpRenderer.sprite = null;
        else
        {
            ChangeFrame(spriteChangeDetector.CurrentFrame);
        }
    }

    private void ChangeFrame(int frame)
    {
        if(!_itemSo) return;
        
        SpRenderer.sprite = _itemSo.AnimationSprites[frame];
    }
    
    #endregion
}
