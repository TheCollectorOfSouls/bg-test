using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    private int _playerSouls = 0;
    
    public int PlayerSouls => _playerSouls;
    public static PlayerManager Instance { get; private set; }
    public UnityEvent<bool> onToggleMovement;
    public UnityEvent<int> onPlayerSoulsChanged;

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
}
