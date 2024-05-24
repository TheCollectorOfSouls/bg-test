using Player.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Player.PlayerStates
{
	public class PlayerStateSo : ScriptableObject
	{
		#region Variables/Properties

		protected PlayerStateMachine StateMachine;
		protected PlayerController Player;
		protected PlayerManager PManager => PlayerManager.Instance;

		#endregion

		#region Events

		public event UnityAction OnEndState;

		#endregion

		#region State Methods

		public virtual void Initialize(PlayerStateMachine playerStateMachine ,PlayerController playerController)
		{
			StateMachine = playerStateMachine;
			Player = playerController;
		}
		
		public virtual void StartState()
		{
		}
		
		public virtual void EndState()
		{
			ResetValues();
			OnEndState?.Invoke();
		}

		protected virtual void ResetValues()
		{
		}

		#endregion
	}
}