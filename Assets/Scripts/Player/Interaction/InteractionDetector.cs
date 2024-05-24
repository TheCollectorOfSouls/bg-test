using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interactables;
using UnityEngine;

namespace Player.Interaction
{
    public class InteractionDetector : MonoBehaviour
    {
        #region Variables/Properties

        [SerializeField] private float detectionCheckTimer = 0.2f;
        private Interactable _closestInteractable;
        private List<Interactable> _interactableList = new List<Interactable>();
        private bool _isActive;
        private Coroutine _detectCoroutine;

        public Interactable ClosestInteractable => _closestInteractable;
        public bool IsActive => _isActive;

        #endregion

        #region Detection

        public void StartDetecting()
        {
            _isActive = true;

            _detectCoroutine ??= StartCoroutine(DetectRoutine());
        }

        public void StopDetecting()
        {
            _isActive = false;
        }

        private void CheckForClosestInteractable()
        {
            if (_interactableList.Count <= 0)
            {
                _closestInteractable = null;
                return;
            }

            if (_interactableList.Count > 1)
            {
                _interactableList = _interactableList.OrderBy(x => Vector2.SqrMagnitude(
                    transform.position - x.transform.position)).ToList();
            }

            if (_closestInteractable != _interactableList[0])
            {
                if (_closestInteractable)
                    _closestInteractable.ClosestInteractable(false);

                _closestInteractable = _interactableList[0];
            }

            _closestInteractable.ClosestInteractable(true);
        }

        private IEnumerator DetectRoutine()
        {
            while (_isActive)
            {
                CheckForClosestInteractable();
                yield return new WaitForSeconds(detectionCheckTimer);
            }

            _detectCoroutine = null;
        }

        #endregion

        #region Triggers

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
}
