using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    private int _playerSouls = 0;
    
    public int PlayerSouls => _playerSouls;
    public bool InVendorInteraction { get; set; } = false;
    public static PlayerManager Instance { get; private set; }
    public UnityEvent<bool> onToggleMovement;
    public UnityEvent<int> onPlayerSoulsChanged;
    public UnityEvent<ItemSo> onGearRemoved;
    public UnityEvent<ItemSo> onGearChanged;

    private void Awake()
    {
        Instance = this;
    }
    
    public void AddSouls(int amount)
    {
        _playerSouls += amount;
        onPlayerSoulsChanged?.Invoke(_playerSouls);
    }
    
    public void RemoveSouls(int amount)
    {
        _playerSouls -= amount;
        onPlayerSoulsChanged?.Invoke(_playerSouls);
    }
    
    public void EnableMovement(bool value)
    {
        onToggleMovement?.Invoke(value);
    }

    public void GearItemRemoved(ItemSo item)
    {
        onGearRemoved?.Invoke(item);
    }
    public void GearItemChanged(ItemSo item)
    {
        onGearChanged?.Invoke(item);
    }

    public void TryBuyItem(ItemSo itemSo)
    {
        if (_playerSouls < itemSo.SellPrice) return;

        var success = inventoryController.TryAddToInventory(itemSo);
        if (success) RemoveSouls(itemSo.SellPrice);
    }
    
    public void ToggleVendorInteraction(bool value)
    {
        InVendorInteraction = value;
    }
}
