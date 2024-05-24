using Interactables;
using Interactables.Merchant;
using Inventory.Item;
using Player;
using Player.Managers;
using UnityEditor;
using UnityEngine;

namespace Player.Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {

        #region Variables

        [SerializeField] private InteractionDetector interactionDetector;

        private PlayerController _playerController;
        private bool _canInteract = true;
        private bool _isInteracting = false;
        private Interactable _currentInteraction;

        #endregion

        #region Properties

        private PlayerManager PManager => PlayerManager.Instance;
        private InputReceiverManager Input => InputReceiverManager.Instance;

        #endregion

        #region Setup

        private void Start()
        {
            SetListeners();
            StartInteractionDetection();
        }

        private void StartInteractionDetection()
        {
            if (_canInteract)
                interactionDetector.StartDetecting();
        }

        public void Initialize(PlayerController playerController)
        {
            _playerController = playerController;
        }

        private void SetListeners()
        {
            if (Input != null)
            {
                Input.onInteractInput.AddListener(Interact);
            }
        }

        #endregion

        #region Interactions

        private void Interact()
        {
            if (!_canInteract) return;

            if (_currentInteraction != null)
            {
                _currentInteraction.Interact(this);
            }
            else if (interactionDetector.ClosestInteractable != null)
            {
                interactionDetector.ClosestInteractable.Interact(this);
            }
        }

        public void LockMerchantInteraction(Merchant merchant, PlayerStateMachine.PlayerStates state)
        {
            LockInteraction(merchant, PlayerStateMachine.PlayerStates.Selling);
            _playerController.SetMerchant(merchant);
        }

        public void LockInteraction(Interactable interactable, PlayerStateMachine.PlayerStates state)
        {
            ChangePlayerState(state);
            ToggleDetection(false);
            _isInteracting = true;
            _currentInteraction = interactable;
        }

        public void ReleaseInteraction()
        {
            ChangePlayerState(PlayerStateMachine.PlayerStates.Movable);
            _isInteracting = false;
            _currentInteraction = null;
            ToggleDetection(true);
        }

        public void TryBuyItem(ItemSo itemSo)
        {
            if (PManager)
                PManager.TryBuyItem(itemSo);
        }

        public void GainSouls(int amount)
        {
            if (PManager)
                PManager.AddSouls(amount);
        }

        private void ChangePlayerState(PlayerStateMachine.PlayerStates state)
        {
            _playerController.ChangeState(state);
        }

        public void ToggleInteraction(bool toggle)
        {
            _canInteract = toggle;

            ToggleDetection(toggle);
        }

        private void ToggleDetection(bool toggle)
        {
            switch (toggle)
            {
                case false when interactionDetector.IsActive:
                    interactionDetector.StopDetecting();
                    break;
                case true when !interactionDetector.IsActive:
                    interactionDetector.StartDetecting();
                    break;
            }
        }

        #endregion
    }
}
