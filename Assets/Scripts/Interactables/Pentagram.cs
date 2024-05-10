
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pentagram : Interactable
{
    #region Variables
    
    [SerializeField] private Soul soul;
    [SerializeField] private float soulMaxXOffset = 9f;
    [SerializeField] private float soulMinXOffset = -9f;
    [SerializeField] private float soulYOffset = 9f;
    [SerializeField] private Transform centerPoint;
    
    #endregion
    
    #region Methods

    protected override void Awake()
    {
        base.Awake();
        SetListeners();
    }

    private void SetListeners()
    {
        if(soul)
            soul.onTargetReached.AddListener(SoulReachedCenter);
    }

    private void StartSoul()
    {
        float xOffset = Random.Range(soulMinXOffset, soulMaxXOffset);
        var position = centerPoint.position;
        soul.transform.position = new Vector3(position.x + xOffset, position.y + soulYOffset, 0f);
        soul.MoveToTarget(centerPoint);
    }

    private void SoulReachedCenter()
    {
        PlayerManager.Instance.AddSouls(1);
        if(_isInteracting)
            StartSoul();
    }
    

    protected override void EndInteraction()
    {
        _playerInteraction.ReleaseInteraction();
        onEndInteraction?.Invoke();
    }

    protected override void BeginInteraction()
    {
        _playerInteraction.LockInteraction(this);
        _playerInteraction.SetWorshippingAnimation(true);
        _interactionCanvas.SetActive(false);
        
        if(!soul.IsMoving)
            StartSoul();
        
        onBeginInteraction?.Invoke();
    }
    
    #endregion
}
