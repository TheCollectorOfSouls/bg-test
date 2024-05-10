using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    #region Variables
    
    [SerializeField] PlayerInputReceiver playerInputReceiver;
    [SerializeField] private Animator animator;

    private bool _canInteract = true;
    private bool _isInteracting = false;
    private Interactable _currentInteraction;
    private Interactable _closestInteractable;
    private List<Interactable> _interactableList = new List<Interactable>();
    private static readonly int IsWorshiping = Animator.StringToHash("IsWorshiping");
    private static readonly int SetIdle = Animator.StringToHash("SetIdle");

    #endregion

    #region Properties
    
    private PlayerManager PlayerManager => PlayerManager.Instance;
    
    #endregion

    #region Setup
    
    private void Awake()
    {
        SetListeners();
    }

    private void SetListeners()
    {
        if (playerInputReceiver != null)
        {
            playerInputReceiver.onInteractInput.AddListener(Interact);
        }
    }
    
    #endregion

    #region Interactions

    private void Update()
    {
        CheckForClosestInteractable();
    }

    private void CheckForClosestInteractable()
    {
        if (!_canInteract || _isInteracting) return;
        
        if (_interactableList.Count <= 0)
        {
            _closestInteractable = null;
            return;
        }
        
        if (_interactableList.Count > 1)
        {
            _interactableList = _interactableList.OrderBy(x => Vector2.Distance(transform.position, x.transform.position)).ToList();
        }

        if (_closestInteractable != _interactableList[0])
        {
            if(_closestInteractable)
                _closestInteractable.ClosestInteractable(false);
            
            _closestInteractable = _interactableList[0];
        }
        _closestInteractable.ClosestInteractable(true);
    }

    private void Interact()
    {
        if (!_canInteract) return;

        if (_currentInteraction != null)
        {
            _currentInteraction.Interact(this);
        }
        else if(_closestInteractable != null)
        {
            _closestInteractable.Interact(this);
        }
    }

    public void LockInteraction(Interactable interactable)
    {
        PlayerManager.EnableMovement(false);
        _isInteracting = true;
        _currentInteraction = interactable;
    }

    public void ReleaseInteraction()
    {
        PlayerManager.EnableMovement(true);
        _isInteracting = false;
        SetWorshippingAnimation(false);
        _currentInteraction = null;
    }

    public void SetWorshippingAnimation(bool value)
    {
        animator.SetBool(IsWorshiping, value);
    }

    public void SetIdleAnimation()
    {
        animator.SetTrigger(SetIdle);
    }
    
    #endregion

    #region ColliderExit

    private void OnTriggerEnter2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable == null) return;
        if (_interactableList.Contains(interactable)) return;
        _interactableList.Add(interactable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable == null) return;
        if (!_interactableList.Contains(interactable)) return;
        _interactableList.Remove(interactable);

        if (_closestInteractable == interactable)
        {
            _closestInteractable.ClosestInteractable(false);
            _closestInteractable = null;
        }
    }
    
    #endregion
    
}
