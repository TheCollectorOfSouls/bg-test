using System;
using Player.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.Item
{
    public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        #region Variables/Properties

        [SerializeField] private Image itemIconImage;
        [SerializeField] private GameObject itemPriceTooltip;
        [SerializeField] private TextMeshProUGUI itemPriceText;
        private bool _playerSlot;
        private bool _isHovering = false;
        private float _currentPriceModifier = 1f;
        
        public bool IsEmpty => ItemSo == null;
        public ItemSo ItemSo { get; private set; }

        private PlayerManager PManager => PlayerManager.Instance;

        #endregion

        #region Events

        public UnityEvent<ItemSlot> onSlotClicked;
        public UnityEvent<ItemSlot> onItemPriceHoovered;
        private UnityEvent<float> _onItemPriceModifierEvent;

        #endregion

        #region Trade Check Methods

        private void Start()
        {
            SetListeners();       
        }
        
        private void SetListeners()
        {
            if(PManager)
                PManager.OnTradeStatusChangedAction += TradeStatusChanged;
        }

        private void TradeStatusChanged(bool enable)
        {
            switch (enable)
            {
                case false when _isHovering:
                    itemPriceTooltip.SetActive(false);
                    break;
                case true when _isHovering && ItemSo != null:
                    itemPriceTooltip.SetActive(true);
                    break;
            }
        }

        #endregion

        #region Item Methods
        
        public void SetItem(ItemSo itemSo)
        {
            ItemSo = itemSo;
            itemIconImage.sprite = itemSo.UIIcon;
            itemIconImage.gameObject.SetActive(true);
            SetItemPriceModifier(_currentPriceModifier);
        }

        public void SetItemPriceModifierEvent(UnityEvent<float> itemPriceModifierEvent)
        {
            _onItemPriceModifierEvent?.RemoveListener(SetItemPriceModifier);
            _onItemPriceModifierEvent = itemPriceModifierEvent;
            _onItemPriceModifierEvent.AddListener(SetItemPriceModifier);
        }

        public void SetItemPriceModifier(float modifier)
        {
            _currentPriceModifier = modifier;

            if (ItemSo == null) return;
            var price = Mathf.RoundToInt(ItemSo.BasePrice * _currentPriceModifier);
            SetItemPrice(price);
            SetPriceText(ItemSo.CurrentPrice);
        }

        private void SetItemPrice(int price)
        {
            if (ItemSo == null) return;
            ItemSo.SetPrice(price);
            SetPriceText(ItemSo.CurrentPrice);
        }

        public int GetItemCurrentPrice()
        {
            if (ItemSo == null) return 0;
            return ItemSo.CurrentPrice;
        }

        public int GetItemBasePrice()
        {
            if (ItemSo == null) return 0;
            return ItemSo.BasePrice;
        }

        private void SetPriceText(int price)
        {
            itemPriceText.text = $": {price}";
        }

        public void RemoveItem()
        {
            ItemSo = null;
            itemIconImage.gameObject.SetActive(false);
            itemPriceTooltip.SetActive(false);
        }

        #endregion

        #region Pointer

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovering = true;
            if (!PManager) return;
            if (!PManager.InTrade()) return;

            if (!ItemSo) return;
            onItemPriceHoovered?.Invoke(this);
            itemPriceTooltip.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            itemPriceTooltip.SetActive(false);
            _isHovering = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onSlotClicked?.Invoke(this);
        }

        #endregion

        #region Cleanup

        private void OnDisable()
        {
            itemPriceTooltip.SetActive(false);
            _isHovering = false;
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
        
        private void RemoveListeners()
        {
            if(PManager)
                PManager.OnTradeStatusChangedAction -= TradeStatusChanged;
        }

        #endregion

    }
}
