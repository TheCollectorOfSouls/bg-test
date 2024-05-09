using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputReceiver : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputActionReference movementActionReference;
    [SerializeField] private InputActionReference interactActionReference;

    private InputAction _movementAction;
    private InputAction _interactAction;
    private Vector2 _inputMovementValue;

    public UnityEvent<Vector2> onMovementInput;
    public UnityEvent onInteractInput;

    private void Awake()
    {
        SetActions();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void SetActions()
    {
        _movementAction = playerInput.actions.FindAction(movementActionReference.action.id);
        if(_movementAction != null)
        {
            _movementAction.performed += MoveInput;
            _movementAction.canceled += MoveInput;
        }
        
        _interactAction = playerInput.actions.FindAction(interactActionReference.action.id);
        if(_interactAction != null)
            _interactAction.performed += InteractInput;
    }

    private void RemoveListeners()
    {
        if(_movementAction != null)
        {
            _movementAction.performed -= MoveInput;
            _movementAction.canceled -= MoveInput;
        }
        if(_interactAction != null)
            _interactAction.performed -= InteractInput;
    }

    private void MoveInput(InputAction.CallbackContext context)
    {
        _inputMovementValue = context.ReadValue<Vector2>();
        onMovementInput?.Invoke(_inputMovementValue);
    }
    
    private void InteractInput(InputAction.CallbackContext context)
    {
        onInteractInput?.Invoke();
    }
    
}
