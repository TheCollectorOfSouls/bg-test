using System;
using Interactables.Merchant;
using Player.PlayerStates;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
	public class PlayerStateMachine : MonoBehaviour
	{
		#region Variables / Properties

		[SerializeField] private PlayerStates startingState = PlayerStates.Movable;
		[SerializeField] private PlayerMovableStateSo movableStateSo;
		[SerializeField] private PlayerWorshippingStateSo worshippingStateSo;
		[SerializeField] private PlayerSellingStateSo sellingStateSo;
		
		private PlayerStates _nextState;
		private PlayerStateSo _currentStateSoInstance = null;
		private PlayerMovableStateSo _movableStateSoInstance;
		private PlayerWorshippingStateSo _worshippingStateSoInstance;
		private PlayerSellingStateSo _sellingStateSoInstance;
		
		public PlayerStates CurrentState { get; private set; } = PlayerStates.None;

		public PlayerStateSo CurrentStateSoInstance => _currentStateSoInstance;
		
		public event UnityAction<PlayerStates> OnStateChanged;

		#endregion

		#region Enum

		public enum PlayerStates
		{
			Movable,
			Worshipping,
			Selling,
			None
		}

		#endregion

		#region Setup

		public void Initialize(PlayerController playerController)
		{
			_worshippingStateSoInstance = Instantiate(worshippingStateSo);
			_worshippingStateSoInstance.Initialize(this, playerController);
			
			_movableStateSoInstance = Instantiate(movableStateSo);
			_movableStateSoInstance.Initialize(this, playerController);
			
			_sellingStateSoInstance = Instantiate(sellingStateSo);
			_sellingStateSoInstance.Initialize(this, playerController);
		}

		public void Begin()
		{
			ChangeState(startingState);
		}

		#endregion

		#region States

		public void ChangeState(PlayerStates state)
		{
			_nextState = state;
			if(_currentStateSoInstance)
				_currentStateSoInstance.EndState();
			else
			{
				BeginNewState(state);
			}
		}

		private void CurrentStateEnded()
		{
			_currentStateSoInstance.OnEndState -= CurrentStateEnded;
			BeginNewState(_nextState);
		}

		private void BeginNewState(PlayerStates state)
		{
			_currentStateSoInstance = GetStateInstance(state);
			if(!_currentStateSoInstance) return;
			CurrentState = state;
			OnStateChanged?.Invoke(_nextState);
			_currentStateSoInstance.StartState();
			_currentStateSoInstance.OnEndState += CurrentStateEnded;
		}

		#endregion

		#region Setters

		public void SetMerchant(Merchant merchant)
		{
			if (CurrentState == PlayerStates.Selling)
			{
				var playerSellingStateSo =	_currentStateSoInstance as PlayerSellingStateSo;
				if (playerSellingStateSo != null) playerSellingStateSo.SetMerchant(merchant);
			}
			else
			{
				_sellingStateSoInstance.SetMerchant(merchant);
			}
		}

		#endregion

		#region Getters

		private PlayerStateSo GetStateInstance(PlayerStates states)
		{
			return states switch
			{
				PlayerStates.Movable => _movableStateSoInstance,
				PlayerStates.Worshipping => _worshippingStateSoInstance,
				PlayerStates.Selling => _sellingStateSoInstance,
				_ => throw new ArgumentOutOfRangeException(nameof(states), states, null)
			};
		}

		#endregion
	}
}