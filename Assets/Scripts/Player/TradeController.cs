using Inventory.Item;
using Player.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
	public class TradeController : MonoBehaviour
	{
		#region Variables / Properties 

		public bool InTrade { get; private set; } = false;
		public float MerchantBuyModifier { get; private set; } = 1f;
		
		private PlayerManager PManager => PlayerManager.Instance;
		
		#endregion
		
		#region Events
		
		public event UnityAction<float> OnMerchantBuyModifierUpdated;
		public event UnityAction<bool> OnTradeStatusChanged;

		#endregion


		#region Setup

		private void Start()
		{
			Setup();
		}

		private void Setup()
		{
			if (PManager) 
			{
				PManager.SetTradeController(this);
			}
		}

		#endregion

		#region Methods

		public void ToggleTrade(bool toggle)
		{
			if (toggle == InTrade) return;
			
			InTrade = toggle;
			OnTradeStatusChanged?.Invoke(InTrade);
		}
		
		public void UpdateMerchantBuyModifier(float modifier)
		{
			MerchantBuyModifier = modifier;
			OnMerchantBuyModifierUpdated?.Invoke(MerchantBuyModifier);
		}
		
		public void TryBuyItem(ItemSo itemSo)
		{
			if(!PManager) return;
			if (PManager.PlayerSouls < itemSo.CurrentPrice) return;

			var success = PManager.TryAddToInventory(itemSo);
			if (success) PManager.RemoveSouls(itemSo.CurrentPrice);
		}
		
		#endregion
	}
}