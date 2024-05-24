using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Item;
using Player;
using Player.PlayerStates;
using UnityEngine;
using UnityEngine.Events;

namespace Player.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Variables/Properties

        public int PlayerSouls { get; private set; } = 0;
        public InventoryController InventoryController { get; private set; }
        public TradeController TradeController { get; private set; }
        public PlayerController PlayerController { get; private set; }

        #endregion

        #region Events

        public UnityEvent<int> onPlayerSoulsChanged;
        public UnityEvent<ItemSo> onGearRemoved;
        public UnityEvent<ItemSo> onGearChanged;
        public event UnityAction<float> OnMerchantBuyModifierUpdatedAction;
        public event UnityAction<bool> OnTradeStatusChangedAction;

        #endregion

        #region Singleton

        public static PlayerManager Instance { get; private set; }

        private void SetSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        #endregion

        #region Setters

        public void SetInventoryController(InventoryController inventoryController)
        {
            InventoryController = inventoryController;
        }

        public void SetTradeController(TradeController tradeController)
        {
            TradeController = tradeController;
        }

        public void SetPlayerController(PlayerController playerController)
        {
            PlayerController = playerController;
        }

        #endregion

        #region Getters

        public float GetMerchantBuyModifier()
        {
            return TradeController ? TradeController.MerchantBuyModifier : 1f;
        }

        #endregion

        

        private void Awake()
        {
            SetSingleton();
        }

        #region ManageSouls
        
        public void AddSouls(int amount)
        {
            PlayerSouls += amount;
            onPlayerSoulsChanged?.Invoke(PlayerSouls);
        }

        public void RemoveSouls(int amount)
        {
            PlayerSouls -= amount;
            onPlayerSoulsChanged?.Invoke(PlayerSouls);
        }
        
        #endregion

        #region Inventory

        public void RaiseGearItemRemoved(ItemSo item)
        {
            onGearRemoved?.Invoke(item);
        }

        public void RaiseGearItemChanged(ItemSo item)
        {
            onGearChanged?.Invoke(item);
        }
        
        public bool TryAddToInventory(ItemSo itemSo)
        {
            return InventoryController && InventoryController.TryAddToInventory(itemSo);
        }

        #endregion

        #region Trade

        public bool InTrade()
        {
            return TradeController && TradeController.InTrade;
        }

        public void TryBuyItem(ItemSo itemSo)
        {
            if (TradeController)
                TradeController.TryBuyItem(itemSo);
        }

        public void ToggleTrade(bool value)
        {
            if (TradeController)
                TradeController.ToggleTrade(value);
            
            OnTradeStatusChangedAction?.Invoke(value);
        }

        public void UpdateMerchantTradeModifier(float modifier)
        {
            if (TradeController)
                TradeController.UpdateMerchantBuyModifier(modifier);
            
            OnMerchantBuyModifierUpdatedAction?.Invoke(modifier);
        }

        #endregion

        #region States

        public PlayerStateMachine.PlayerStates GetPlayerState()
        {
            return PlayerController == null
                ? PlayerStateMachine.PlayerStates.None
                : PlayerController.PlayerStateMachine.CurrentState;
        }

        public PlayerStateSo GetPlayerStateSo()
        {
            return PlayerController == null ? null : PlayerController.PlayerStateMachine.CurrentStateSoInstance;
        }

        #endregion

    }
}
