using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables.Pentagram
{
    public class Soul : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float tweenSpeed = 10f;
        [SerializeField] private bool speedBased = true;
        [SerializeField] private Ease ease = Ease.Linear;

        private Tween _tween;

        #endregion

        #region Events

        public UnityEvent<Soul> onTargetReached;

        #endregion

        #region Methods

        public void MoveToTarget(Transform target, Vector2 startPosition)
        {
            transform.position = startPosition;
            _tween?.Kill();
            gameObject.SetActive(true);
            _tween = transform.DOMove(target.position, tweenSpeed).SetSpeedBased(speedBased).SetEase(ease);
            _tween.OnComplete(() => { onTargetReached?.Invoke(this); });
        }

        #endregion
    }
}
