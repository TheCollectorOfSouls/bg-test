using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : Interactable
{
    [SerializeField] List<ItemSo> items = new List<ItemSo>();
    [SerializeField] private GameObject itemGrid;
    [SerializeField] private GameObject itemsContainer;
    [SerializeField] private GameObject itemSlotPrefab;
    
    PlayerManager PlayerManager => PlayerManager.Instance;

    protected override void Awake()
    {
        base.Awake();
        itemsContainer.SetActive(false);
        SetSlots();
    }

    private void SetSlots()
    {
        foreach (var item in items)
        {
            var newItemSlot = Instantiate(itemSlotPrefab, itemGrid.transform);
            ItemSlot itemSlot = newItemSlot.GetComponent<ItemSlot>();
            itemSlot.SetItem(item);
            itemSlot.onSlotClicked.AddListener(TrySellItem);
            itemSlot.SetOwner(false);
        }
    }

    private void TrySellItem(ItemSlot itemSlot)
    {
        PlayerManager.TryBuyItem(itemSlot.ItemSo);
    }

    protected override void EndInteraction()
    {
        _playerInteraction.ReleaseInteraction();
        itemsContainer.SetActive(false);
        PlayerManager.ToggleVendorInteraction(false);
        onEndInteraction?.Invoke();
    }

    protected override void BeginInteraction()
    {
        _playerInteraction.LockInteraction(this);
        _interactionCanvas.SetActive(false);
        itemsContainer.SetActive(true);
        PlayerManager.ToggleVendorInteraction(true);
        onBeginInteraction?.Invoke();
    }
}
