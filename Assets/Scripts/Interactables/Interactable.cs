using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected bool _isInteractable = true;
    [SerializeField] protected GameObject _interactionCanvas;
    
    protected bool _isInteracting = false;
    protected PlayerInteraction _playerInteraction;
    
    public UnityEvent onEndInteraction;
    public UnityEvent onBeginInteraction;

    protected virtual void Awake()
    {
        _interactionCanvas.SetActive(false);
    }

    public virtual void Interact(PlayerInteraction playerInteraction)
    {
        if(!_isInteractable) return;

        if (!_isInteracting)
        {
            _isInteracting = true;
            _playerInteraction = playerInteraction;
            BeginInteraction();
        }
        else
        {
            EndInteraction();
            _isInteracting = false;
        }
    }

    public virtual void ClosestInteractable(bool value)
    {
        if (!_isInteractable || _isInteracting)
        {
            _interactionCanvas.SetActive(false);
            return;
        }

        _interactionCanvas.SetActive(value);
    }
    protected abstract void EndInteraction();
    protected abstract void BeginInteraction();
}
