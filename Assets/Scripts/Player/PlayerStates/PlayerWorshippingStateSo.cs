using UnityEngine;

namespace Player.PlayerStates
{
	[CreateAssetMenu (menuName = "Player/PlayerStates/PlayerWorshippingStateSo")]
	public class PlayerWorshippingStateSo : PlayerStateSo
	{
		#region State Methods

		public override void StartState()
		{
			base.StartState();
			Player.PlayerAnimator.ToggleWorshippingAnimation(true);
		}

		public override void EndState()
		{
			Player.PlayerAnimator.ToggleWorshippingAnimation(false);
			base.EndState();
		}

		#endregion
	}
}