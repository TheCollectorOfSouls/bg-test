using Player;
using Player.Managers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    #region Variables
    
    [SerializeField] private float movementSpeed = 5f;

    private PlayerController _playerController;
    private bool _canMove = true;
    private bool _isMoving;
    private Vector2 _movementDirection;
    
    #endregion

    #region Properties
    
    private Rigidbody2D Rb2d => GetComponent<Rigidbody2D>();
    private InputReceiverManager Input => InputReceiverManager.Instance;
    
    #endregion

    #region Setup

    private void Start()
    {
        SetListeners();
    }
    
    private void SetListeners()
    {
        if (Input != null)
        {
            Input.onMovementInput.AddListener(MoveInput);
        }
    }
    
    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
    }

    #endregion
    
    #region Movement
    
    private void FixedUpdate()
    {
        if(!_canMove) return;
        ApplyMovement();
    }

    private void MoveInput(Vector2 value)
    {
        if (value.x != 0 && value.y != 0)
        {
            if (_movementDirection == Vector2.zero)
            {
                _movementDirection = new Vector2(value.x, 0);
            }
            return;
        }
        
        _movementDirection = value;
    }

    public void ToggleMovement(bool value)
    {
        _canMove = value;

        if (!_canMove)
        {
            _isMoving = false;
            _playerController.PlayerAnimator.ToggleMovingAnimation(_isMoving);
        }
    }
    
    private void ApplyMovement()
    {
        Rb2d.MovePosition(Rb2d.position + _movementDirection * (movementSpeed * Time.fixedDeltaTime));
        
        if (_movementDirection == Vector2.zero)
        {
            _isMoving = false;
        }
        else
        {
            _isMoving = true;
            
            if (_playerController)
            {
                _playerController.PlayerAnimator.SetMovementAnimation(_movementDirection);
            }
        }
        
        if (_playerController)
        {
            _playerController.PlayerAnimator.ToggleMovingAnimation(_isMoving);
        }
    }
    
    #endregion
}
