using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitSpriteController : MonoBehaviour
{

    #region Variables

    
    [SerializeField] private OutfitSprite _headOutfitSprite;
    [SerializeField] private OutfitSprite _bodyOutfitSprite;
    
    #endregion

    #region Properties
    
    PlayerManager PlayerManager => PlayerManager.Instance;
    
    #endregion
    
    #region Methods
    
    void Start()
    {
        SetListeners();
    }

    private void SetListeners()
    {
        PlayerManager.onGearRemoved.AddListener(RemoveGear);
        PlayerManager.onGearChanged.AddListener(GearChanged);
    }

    private void GearChanged(ItemSo itemSo)
    {
        switch (itemSo.ItemSlot)
        {
            case ItemSo.Slot.Head:
                _headOutfitSprite.SetItem(itemSo);
                break;
            case ItemSo.Slot.Body:
                _bodyOutfitSprite.SetItem(itemSo);
                break;
        }
    }

    private void RemoveGear(ItemSo itemSo)
    {
        switch (itemSo.ItemSlot)
        {
            case ItemSo.Slot.Head:
                _headOutfitSprite.SetItem(null);
                break;
            case ItemSo.Slot.Body:
                _bodyOutfitSprite.SetItem(null);
                break;
        }
    }
    
    #endregion
}
