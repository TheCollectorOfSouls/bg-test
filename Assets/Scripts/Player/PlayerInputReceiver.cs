using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputReceiver : MonoBehaviour
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

    #region Properties
    
    public static PlayerInputReceiver Instance { get; private set; }
    
    #endregion

    #region Events
    
    public UnityEvent<Vector2> onMovementInput;
    public UnityEvent onInteractInput;
    public UnityEvent onInventoryInput;
    
    #endregion

    private void Awake()
    {
        Instance = this;
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
        
        _inventoryAction = playerInput.actions.FindAction(inventoryActionReference.action.id);
        if(_inventoryAction != null)
            _inventoryAction.performed += InventoryInput;
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
        
        if(_inventoryAction != null)
            _inventoryAction.performed -= InventoryInput;
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

    private void InventoryInput(InputAction.CallbackContext context)
    {
        onInventoryInput?.Invoke();
    }
    
}
