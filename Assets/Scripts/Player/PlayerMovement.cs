using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputReceiver playerInputReceiver;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Animator animator;
    

    private bool _canMove = true;
    private bool _isMoving;
    private Vector2 _movementDirection;
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private Rigidbody2D Rb2d => GetComponent<Rigidbody2D>();
    private PlayerManager PlayerManager => PlayerManager.Instance; 

    private void Awake()
    {
        SetListeners();
    }
    
    private void FixedUpdate()
    {
        if(!_canMove) return;
        ApplyMovement();
    }
    
    private void SetListeners()
    {
        if (playerInputReceiver != null)
        {
            playerInputReceiver.onMovementInput.AddListener(MoveInput);
        }

        if (PlayerManager)
        {
            PlayerManager.onToggleMovement.AddListener(ToggleMovement);
        }
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

    private void ToggleMovement(bool value)
    {
        _canMove = value;
        
        if(!_canMove)
            _isMoving = false;
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
            
            if (animator)
            {
                animator.SetFloat(Horizontal, _movementDirection.x);
                animator.SetFloat(Vertical, _movementDirection.y);
            }
        }
        
        if (animator)
        {
            animator.SetBool(IsMoving, _isMoving);
        }
    }
}
