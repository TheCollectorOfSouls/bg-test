using Interactables.Merchant;
using UnityEngine;

namespace Player.PlayerStates
{
	[CreateAssetMenu (menuName = "Player/PlayerStates/PlayerSellingStateSo")]
	public class PlayerSellingStateSo : PlayerStateSo
	{
		#region Variables/Properties

		public Merchant CurrentMerchant { get; private set; }

		#endregion

		#region Setter

		public void SetMerchant(Merchant merchant)
		{
			CurrentMerchant = merchant;
			
			if(StateMachine.CurrentState == PlayerStateMachine.PlayerStates.Selling)
				PManager.UpdateMerchantTradeModifier(merchant.BuyItemModifier);
		}

		#endregion

		#region State Methods

		public override void StartState()
		{
			base.StartState();
			Player.PlayerAnimator.SetIdleTriggerAnimation();
			PManager.ToggleTrade(true);
			
			if(CurrentMerchant)
				PManager.UpdateMerchantTradeModifier(CurrentMerchant.BuyItemModifier);
		}
		
		public override void EndState()
		{
			CurrentMerchant = null;
			PManager.ToggleTrade(false);
			base.EndState();
		}

		#endregion
		
	}
}