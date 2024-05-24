using Player.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        #region Variables

        [Header("Base Interactable Settings")] [SerializeField]
        protected bool isInteractable = true;

        [SerializeField] protected GameObject interactionUiGo;

        protected bool IsInteracting = false;
        protected PlayerInteraction PInteraction;

        #endregion

        #region Events

        public UnityEvent onEndInteraction;
        public UnityEvent onBeginInteraction;

        #endregion

        #region Base Interaction

        protected virtual void Awake()
        {
            Setup();
        }

        protected virtual void Setup()
        {
            interactionUiGo.SetActive(false);
        }

        public virtual void Interact(PlayerInteraction playerInteraction)
        {
            if (!isInteractable) return;

            if (!IsInteracting)
            {
                IsInteracting = true;
                PInteraction = playerInteraction;
                BeginInteraction();
            }
            else
            {
                EndInteraction();
                IsInteracting = false;
            }
        }

        public virtual void ClosestInteractable(bool value)
        {
            if (!isInteractable || IsInteracting)
            {
                interactionUiGo.SetActive(false);
                return;
            }

            interactionUiGo.SetActive(value);
        }

        #endregion

        #region Abstract

        protected abstract void EndInteraction();
        protected abstract void BeginInteraction();

        #endregion
    }
}
