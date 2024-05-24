using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Item;
using Player;
using Player.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {

        #region Variables / Properties

        [SerializeField] private ItemSlot headSlot;
        [SerializeField] private ItemSlot bodySlot;
        [SerializeField] private GameObject gearContainer;
        [SerializeField] private GameObject inventorySlotHolder;

        private List<ItemSlot> _inventorySlots = new List<ItemSlot>();

        public ItemSlot HeadSlot => headSlot;
        public ItemSlot BodySlot => bodySlot;
        PlayerManager PManager => PlayerManager.Instance;
        InputReceiverManager InputReceiverManager => InputReceiverManager.Instance;

        #endregion

        #region Events

        [HideInInspector] public UnityEvent<float> onTradePriceModified; 

        #endregion

        #region Setup

        private void Awake()
        {
            SetInventorySlots();
        }

        private void Start()
        {
            Setup();
        }

        private async void Setup()
        {
            if (PManager)
            {
                PManager.SetInventoryController(this);
            }
            
            SetListeners();
            SetupSlots();
        }

        private void SetInventorySlots()
        {
            _inventorySlots = inventorySlotHolder.GetComponentsInChildren<ItemSlot>().ToList();
        }

        private void SetupSlots()
        {
            
            headSlot.onSlotClicked.AddListener(GearClicked);
            SetSlot(headSlot);
            
            bodySlot.onSlotClicked.AddListener(GearClicked);
            SetSlot(bodySlot);
            
            foreach (var slot in _inventorySlots)
            {
                slot.onSlotClicked.AddListener(InventorySlotClicked);
                SetSlot(slot);
            }
        }

        private void SetSlot(ItemSlot itemSlot)
        {
            itemSlot.SetItemPriceModifierEvent(onTradePriceModified);
            if (PManager.InTrade())
                itemSlot.SetItemPriceModifier(PManager.GetMerchantBuyModifier());
        }
        
        private void UpdatedActionTradePriceModifier(float modifier)
        {
            onTradePriceModified?.Invoke(modifier);
        }

        #endregion

        #region GearInventoryContainer

        private void ToggleGearContainer()
        {
            if (gearContainer.activeSelf)
                gearContainer.SetActive(false);
            else
            {
                gearContainer.SetActive(true);
            }
        }

        #endregion

        #region HandleItemSlots

        private void GearClicked(ItemSlot itemSlot)
        {
            if (itemSlot.ItemSo == null) return;

            if (PManager.GetPlayerState() == PlayerStateMachine.PlayerStates.Selling)
            {
                PManager.AddSouls(itemSlot.ItemSo.CurrentPrice);
                PManager.RaiseGearItemRemoved(itemSlot.ItemSo);
                itemSlot.RemoveItem();
                return;
            }

            bool success = TryAddToInventory(itemSlot.ItemSo);
            if (success)
            {
                PManager.RaiseGearItemRemoved(itemSlot.ItemSo);
                itemSlot.RemoveItem();
            }
        }

        private void InventorySlotClicked(ItemSlot itemSlot)
        {
            if (itemSlot.ItemSo == null) return;

            if (PManager.GetPlayerState() == PlayerStateMachine.PlayerStates.Selling)
            {
                PManager.AddSouls(itemSlot.ItemSo.CurrentPrice);
                itemSlot.RemoveItem();
                return;
            }

            ItemSo.Slot slot = itemSlot.ItemSo.ItemSlot;
            ItemSo itemSo = itemSlot.ItemSo;

            switch (slot)
            {
                case ItemSo.Slot.Head:
                    var oldHeadItem = headSlot.ItemSo;
                    if (oldHeadItem)
                    {
                        headSlot.RemoveItem();
                        headSlot.SetItem(itemSo);
                        PManager.RaiseGearItemChanged(headSlot.ItemSo);
                        itemSlot.SetItem(oldHeadItem);
                    }
                    else
                    {
                        headSlot.SetItem(itemSo);
                        PManager.RaiseGearItemChanged(headSlot.ItemSo);
                        itemSlot.RemoveItem();
                    }

                    break;
                case ItemSo.Slot.Body:
                    var oldBodyItem = bodySlot.ItemSo;
                    if (oldBodyItem)
                    {
                        bodySlot.RemoveItem();
                        bodySlot.SetItem(itemSo);
                        PManager.RaiseGearItemChanged(bodySlot.ItemSo);
                        itemSlot.SetItem(oldBodyItem);
                    }
                    else
                    {
                        bodySlot.SetItem(itemSlot.ItemSo);
                        PManager.RaiseGearItemChanged(bodySlot.ItemSo);
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
                    var item = Instantiate(itemSo);
                    slot.SetItem(item);
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Listeners/Cleanup

        private void SetListeners()
        {
            if (InputReceiverManager)
                InputReceiverManager.onInventoryInput.AddListener(ToggleGearContainer);
            
            if(PManager)
                PManager.OnMerchantBuyModifierUpdatedAction += UpdatedActionTradePriceModifier;
        }

        private void RemoveListeners()
        {
            if(PManager)
                PManager.OnMerchantBuyModifierUpdatedAction -= UpdatedActionTradePriceModifier;
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        #endregion
    }
}
