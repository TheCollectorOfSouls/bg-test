using Unity.VisualScripting;
using UnityEngine;

namespace Player.PlayerStates
{
	[CreateAssetMenu (menuName = "Player/PlayerStates/PlayerMovableStateSo")] 
	public class PlayerMovableStateSo : PlayerStateSo
	{

		#region State Methods

		public override void StartState()
		{
			base.StartState();
			Player.PlayerAnimator.SetIdleTriggerAnimation();
			Player.ToggleMovement(true);
		}
		
		public override void EndState()
		{
			Player.ToggleMovement(false);
			base.EndState();
		}

		#endregion
	}
}