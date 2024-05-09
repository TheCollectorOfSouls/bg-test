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

    public abstract void Interact(PlayerInteraction playerInteraction);
    public abstract void ClosestInteractable(bool value);
    protected abstract void EndInteraction();
    protected abstract void BeginInteraction();
}
