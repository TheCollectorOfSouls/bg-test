using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemSlot headSlot;
    [SerializeField] private ItemSlot bodySlot;
    [SerializeField] private GameObject gearContainer;
    [SerializeField] private GameObject inventorySlotHolder;
    
    private List<ItemSlot> _inventorySlots = new List<ItemSlot>();
    PlayerManager PlayerManager => PlayerManager.Instance;
    PlayerInputReceiver PlayerInputReceiver => PlayerInputReceiver.Instance;
    

    private void Awake()
    {
        SetInventorySlots();
        SetupSlots();
    }

    private void Start()
    {
        SetListeners();
    }

    private void SetListeners()
    {
        PlayerInputReceiver.onInventoryInput.AddListener(ToggleGearContainer);
    }

    private void ToggleGearContainer()
    {
        if(gearContainer.activeSelf)
            gearContainer.SetActive(false);
        else
        {
            gearContainer.SetActive(true);
        }
    }

    private void SetInventorySlots()
    {
        _inventorySlots = inventorySlotHolder.GetComponentsInChildren<ItemSlot>().ToList();
    }

    private void SetupSlots()
    {
        headSlot.onSlotClicked.AddListener(GearClicked);
        headSlot.SetOwner(true);
        bodySlot.onSlotClicked.AddListener(GearClicked);
        bodySlot.SetOwner(true);
        
        foreach (var slot in _inventorySlots)
        {
            slot.onSlotClicked.AddListener(InventorySlotClicked);
            slot.SetOwner(true);
        }
    }

    private void GearClicked(ItemSlot itemSlot)
    {
        if(itemSlot.ItemSo == null) return;
        
        if (PlayerManager.InVendorInteraction)
        {
            PlayerManager.AddSouls(itemSlot.ItemSo.SellPrice);
            PlayerManager.GearItemRemoved(itemSlot.ItemSo);
            itemSlot.RemoveItem();
            return;
        }

        bool success = TryAddToInventory(itemSlot.ItemSo);
        if (success)
        {
            PlayerManager.GearItemRemoved(itemSlot.ItemSo);
            itemSlot.RemoveItem();
        }
    }

    private void InventorySlotClicked(ItemSlot itemSlot)
    {
        if(itemSlot.ItemSo == null) return;
        
        if (PlayerManager.InVendorInteraction)
        {
            PlayerManager.AddSouls(itemSlot.ItemSo.SellPrice);
            itemSlot.RemoveItem();
            return;
        }
        
        ItemSo.Slot slot = itemSlot.ItemSo.ItemSlot;
        
        switch (slot)
        {
            case ItemSo.Slot.Head:
                var oldHeadItem = headSlot.ItemSo;
                if (oldHeadItem)
                {
                    headSlot.RemoveItem();
                    headSlot.SetItem(itemSlot.ItemSo);
                    PlayerManager.GearItemChanged(headSlot.ItemSo);
                    itemSlot.SetItem(oldHeadItem);
                }
                else
                {
                    headSlot.SetItem(itemSlot.ItemSo);
                    PlayerManager.GearItemChanged(headSlot.ItemSo);
                    itemSlot.RemoveItem();
                }
                break;
            case ItemSo.Slot.Body:
                var oldBodyItem = bodySlot.ItemSo;
                if (oldBodyItem)
                {
                    bodySlot.RemoveItem();
                    bodySlot.SetItem(itemSlot.ItemSo);
                    PlayerManager.GearItemChanged(bodySlot.ItemSo);
                    itemSlot.SetItem(oldBodyItem);
                }
                else
                {
                    bodySlot.SetItem(itemSlot.ItemSo);
                    PlayerManager.GearItemChanged(bodySlot.ItemSo);
                    itemSlot.RemoveItem();
                }
                break;
        }
    }
    
    public bool TryAddToInventory(ItemSo itemSo)
    {
        foreach (var slot in _inventorySlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(itemSo);
                return true;
            }
        }
        return false;
    }
    
    
}
