using Interactables.Merchant;
using Player.Interaction;
using Player.Managers;
using UnityEngine;

namespace Player
{
	public class PlayerController : MonoBehaviour
	{
		#region Variables/Properties

		[SerializeField] private PlayerStateMachine playerStateMachine;
		[SerializeField] private PlayerMovement playerMovement;
		[SerializeField] private PlayerAnimator playerAnimator;
		[SerializeField] private PlayerInteraction playerInteraction;
		
		public PlayerInteraction PlayerInteraction => playerInteraction;
		public PlayerMovement PlayerMovement => playerMovement;
		public PlayerAnimator PlayerAnimator => playerAnimator;
		public PlayerStateMachine PlayerStateMachine => playerStateMachine;
		private PlayerManager PManager => PlayerManager.Instance;

		#endregion

		#region Setup

		private void Awake()
		{ 
			AwakeSetup();
		}
		
		private void Start()
		{
			StartSetup();
		}

		private void AwakeSetup()
		{
			playerMovement.Initialize(this);
			playerInteraction.Initialize(this);
			playerStateMachine.Initialize(this);
		}

		private void StartSetup()
		{
			if(PManager)
				PManager.SetPlayerController(this);
			
			playerStateMachine.Begin();
		}
		
		#endregion

		#region Methods

		public void ToggleMovement(bool value)
		{
			if(playerMovement)
				playerMovement.ToggleMovement(value);
		}

		public void ChangeState(PlayerStateMachine.PlayerStates state)
		{
			if(playerStateMachine)
				playerStateMachine.ChangeState(state);
		}

		public void SetMerchant(Merchant merchant)
		{
			playerStateMachine.SetMerchant(merchant);
		}
		
		#endregion
	}
}