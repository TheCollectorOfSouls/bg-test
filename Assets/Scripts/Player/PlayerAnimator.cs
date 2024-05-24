using UnityEngine;

namespace Player
{
	public class PlayerAnimator : MonoBehaviour
	{
		#region Variables

		[SerializeField] private Animator _animator;
		
		private static readonly int IsWorshiping = Animator.StringToHash("IsWorshiping");
		private static readonly int SetIdle = Animator.StringToHash("SetIdle");
		private static readonly int Horizontal = Animator.StringToHash("Horizontal");
		private static readonly int Vertical = Animator.StringToHash("Vertical");
		private static readonly int IsMoving = Animator.StringToHash("IsMoving");

		#endregion

		#region Animations

		public void ToggleWorshippingAnimation(bool value)
		{
			_animator.SetBool(IsWorshiping, value);
		}
		
		public void SetIdleTriggerAnimation()
		{
			_animator.SetTrigger(SetIdle);
		}

		public void SetMovementAnimation(Vector2 direction)
		{
			_animator.SetFloat(Horizontal, direction.x);
			_animator.SetFloat(Vertical, direction.y);
		}
		
		public void ToggleMovingAnimation(bool value)
		{
			_animator.SetBool(IsMoving, value);
		}

		#endregion
	}
}