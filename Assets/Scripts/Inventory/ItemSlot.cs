using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    #region Variables
    
    [SerializeField] private Image itemIconImage;
    [SerializeField] private GameObject itemPriceTooltip;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private Button itemButton;
    private ItemSo _itemSo;
    private bool _playerSlot;
    
    #endregion
    
    #region Properties
    
    public bool IsEmpty => _itemSo == null;
    public ItemSo ItemSo => _itemSo;
    PlayerManager PlayerManager => PlayerManager.Instance;
    
    #endregion

    #region Events

    public UnityEvent<ItemSlot> onSlotClicked;
    
    #endregion

    #region Methods

    private void Awake()
    {
        SetListeners();
    }

    public void SetOwner(bool player)
    {
        _playerSlot = player;
    }

    private void SetListeners()
    {
        itemButton.onClick.AddListener(Clicked);
    }

    private void Clicked()
    {
        onSlotClicked?.Invoke(this);
    }

    public void SetItem(ItemSo itemSo)
    {
        _itemSo = itemSo;
        itemIconImage.sprite = itemSo.UIIcon;
        itemIconImage.gameObject.SetActive(true);
        
        SetPriceText(_playerSlot ? _itemSo.SellPrice : _itemSo.BuyPrice);
    }
    
    private void SetPriceText(int price)
    {
        itemPriceText.text = $": {price}";
    }
    
    public void RemoveItem()
    {
        _itemSo = null;
        itemIconImage.gameObject.SetActive(false);
        itemPriceTooltip.SetActive(false);
    }
    
    #endregion

    #region Pointer

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PlayerManager.InVendorInteraction)
        {
            if(_itemSo)
                itemPriceTooltip.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemPriceTooltip.SetActive(false);
    }
    
    #endregion
}
