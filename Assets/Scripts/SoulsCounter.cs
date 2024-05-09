using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI soulsText;
    PlayerManager PlayerManager => PlayerManager.Instance;

    private void Start()
    {
        SetListeners();
        UpdateSoulsText(PlayerManager.PlayerSouls);
    }

    private void SetListeners()
    {
        PlayerManager.onPlayerSoulsChanged.AddListener(UpdateSoulsText);
    }
    
    private void UpdateSoulsText(int souls)
    {
        soulsText.text = $"Souls: {souls}";
    }
}
