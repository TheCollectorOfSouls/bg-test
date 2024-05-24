using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Interactables.Pentagram
{
    public class Pentagram : Interactable
    {
        #region Variables

        [Header("Pentagram Settings")] 
        [SerializeField] private Transform centerPoint;

        [SerializeField] private float spawnSoulCooldown = 0.1f;

        [Header("Soul Settings")] 
        [SerializeField] private GameObject soulPrefab;

        [SerializeField] private int soulValue = 1;
        [SerializeField] private float soulMaxXOffset = 9f;
        [SerializeField] private float soulMinXOffset = -9f;
        [SerializeField] private float soulYOffset = 9f;

        private IObjectPool<Soul> _soulPool;
        private Coroutine _soulCoroutine;

        #endregion

        #region Setup

        protected override void Awake()
        {
            base.Awake();
            GeneratePool();
        }

        private void GeneratePool()
        {
            _soulPool = new LinkedPool<Soul>(CreateSoul, actionOnGet: soul => soul.gameObject.SetActive(true),
                actionOnRelease: soul => soul.gameObject.SetActive(false),
                actionOnDestroy: soul => Destroy(soul.gameObject));
        }

        private Soul CreateSoul()
        {
            var soul = Instantiate(soulPrefab).GetComponent<Soul>();
            soul.onTargetReached.AddListener(SoulReachedCenter);
            return soul;
        }

        #endregion
        
        #region Interaction Methods

        private void StartSoul()
        {
            float xOffset = Random.Range(soulMinXOffset, soulMaxXOffset);
            var position = centerPoint.position;
            var soul = _soulPool.Get();
            Vector2 startPosition = new Vector2(position.x + xOffset, position.y + soulYOffset);
            soul.MoveToTarget(centerPoint, startPosition);
        }

        private void SoulReachedCenter(Soul soul)
        {
            PInteraction.GainSouls(soulValue);
            _soulPool.Release(soul);
        }

        protected override void EndInteraction()
        {
            PInteraction.ReleaseInteraction();
            onEndInteraction?.Invoke();
        }

        protected override void BeginInteraction()
        {
            PInteraction.LockInteraction(this, PlayerStateMachine.PlayerStates.Worshipping);
            interactionUiGo.SetActive(false);

            _soulCoroutine ??= StartCoroutine(SpawnSoulRoutine());

            onBeginInteraction?.Invoke();
        }

        private IEnumerator SpawnSoulRoutine()
        {
            while (IsInteracting)
            {
                StartSoul();
                yield return new WaitForSeconds(spawnSoulCooldown);
            }

            _soulCoroutine = null;
        }

        #endregion
    }
}
