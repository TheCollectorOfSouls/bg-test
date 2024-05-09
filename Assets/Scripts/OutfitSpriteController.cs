using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitSpriteController : MonoBehaviour
{
    [SerializeField] private OutfitSprite _headOutfitSprite;
    [SerializeField] private OutfitSprite _bodyOutfitSprite;
    
    PlayerManager PlayerManager => PlayerManager.Instance;
    
    // Start is called before the first frame update
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
}
