using System.Collections.Generic;
using Inventory.Item;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables.Merchant
{
    public class Merchant : Interactable
    {

        #region Variables/Properties

        [Header("Merchant Settings")] 
        [SerializeField] private List<ItemSo> items = new List<ItemSo>();
        [SerializeField] protected Canvas interactionCanvas;
        [SerializeField] private GameObject itemGrid;
        [SerializeField] private GameObject itemsContainer;
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] private float sellItemModifier = 1;
        [SerializeField] private float buyItemModifier = 0.5f;

        [SerializeField]
        private PlayerStateMachine.PlayerStates playerInteractState = PlayerStateMachine.PlayerStates.Selling;

        public float BuyItemModifier => buyItemModifier;

        #endregion

        #region Events

        public UnityEvent<float> onItemSellPriceModifierChanged;

        #endregion

        #region Setup

        protected override void Setup()
        {
            base.Setup();
            interactionCanvas.worldCamera = Camera.main;
            itemsContainer.SetActive(false);
            SetSlots();
        }

        private void SetSlots()
        {
            foreach (var item in items)
            {
                var newItemSlot = Instantiate(itemSlotPrefab, itemGrid.transform);
                ItemSlot itemSlot = newItemSlot.GetComponent<ItemSlot>();
                itemSlot.SetItem(Instantiate(item));

                itemSlot.onSlotClicked.AddListener(TrySellItem);
                itemSlot.SetItemPriceModifier(sellItemModifier);
                itemSlot.SetItemPriceModifierEvent(onItemSellPriceModifierChanged);
            }
        }
        
        #endregion

        #region Trade Methods

        private void TrySellItem(ItemSlot itemSlot)
        {
            PInteraction.TryBuyItem(itemSlot.ItemSo);
        }

        public void ChangeSellModifier(float modifier)
        {
            sellItemModifier = modifier;
            onItemSellPriceModifierChanged?.Invoke(sellItemModifier);
        }

        #endregion

        #region Interaction

        protected override void EndInteraction()
        {
            PInteraction.ReleaseInteraction();
            itemsContainer.SetActive(false);
            onEndInteraction?.Invoke();
        }

        protected override void BeginInteraction()
        {
            PInteraction.LockMerchantInteraction(this, playerInteractState);
            interactionUiGo.SetActive(false);
            itemsContainer.SetActive(true);

            onBeginInteraction?.Invoke();
        }

        #endregion
        
    }
}
