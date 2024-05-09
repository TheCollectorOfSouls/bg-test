using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Soul : MonoBehaviour
{
    [SerializeField] private float tweenSpeed = 10f;
    [SerializeField] private Ease ease = Ease.Linear;
    private bool _isMoving = false;
    private Tween _tween;
    
    public bool IsMoving => _isMoving;

    public UnityEvent onTargetReached;
    
    public void MoveToTarget(Transform target)
    {
        _tween?.Kill();    
        _isMoving = true;
        gameObject.SetActive(true);
        _tween = transform.DOMove(target.position, tweenSpeed).SetSpeedBased(true).SetEase(ease);
        _tween.OnComplete(() =>
        {
            gameObject.SetActive(false);
            _isMoving = false;
            onTargetReached?.Invoke();
        });
    }
}
