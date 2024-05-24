using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Managers
{
    public class InputReceiverManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputActionReference movementActionReference;
        [SerializeField] private InputActionReference interactActionReference;
        [SerializeField] private InputActionReference inventoryActionReference;

        private InputAction _movementAction;
        private InputAction _interactAction;
        private InputAction _inventoryAction;
        private Vector2 _inputMovementValue;

        #endregion

        #region Events

        public UnityEvent<Vector2> onMovementInput;
        public UnityEvent onInteractInput;
        public UnityEvent onInventoryInput;

        #endregion

        #region Singleton

        public static InputReceiverManager Instance { get; private set; }

        private void SetSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        #endregion

        #region Setup

        private void Awake()
        {
            SetSingleton();
            SetActions();
        }

        private void SetActions()
        {
            _movementAction = playerInput.actions.FindAction(movementActionReference.action.id);
            if (_movementAction != null)
            {
                _movementAction.performed += MoveInput;
                _movementAction.canceled += MoveInput;
            }

            _interactAction = playerInput.actions.FindAction(interactActionReference.action.id);
            if (_interactAction != null)
                _interactAction.performed += InteractInput;

            _inventoryAction = playerInput.actions.FindAction(inventoryActionReference.action.id);
            if (_inventoryAction != null)
                _inventoryAction.performed += InventoryInput;
        }

        #endregion


        #region Disable

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void RemoveListeners()
        {
            if (_movementAction != null)
            {
                _movementAction.performed -= MoveInput;
                _movementAction.canceled -= MoveInput;
            }

            if (_interactAction != null)
                _interactAction.performed -= InteractInput;

            if (_inventoryAction != null)
                _inventoryAction.performed -= InventoryInput;
        }

        #endregion

        #region Inputs

        private void MoveInput(InputAction.CallbackContext context)
        {
            _inputMovementValue = context.ReadValue<Vector2>();
            onMovementInput?.Invoke(_inputMovementValue);
        }

        private void InteractInput(InputAction.CallbackContext context)
        {
            onInteractInput?.Invoke();
        }

        private void InventoryInput(InputAction.CallbackContext context)
        {
            onInventoryInput?.Invoke();
        }

        #endregion

    }
}
