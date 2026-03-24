using System;
using MedievalRunner.Player;
using MedievalRunner.World;
using UnityEngine;

namespace MedievalRunner.Bonuses
{
    [RequireComponent(typeof(Collider))]
    public abstract class BonusBase : MonoBehaviour
    {
        [SerializeField] private BonusData data;

        public event Action<BonusBase> OnCollected;
        public BonusData Data => data;

        protected PlayerHealth TargetHealth { get; private set; }
        protected WorldMover WorldMover { get; private set; }

        private float _lifetimeTimer;
        private GameObject _targetRootObject;
        private Renderer _renderer;
        private MaterialPropertyBlock _propBlock;

        public void Initialize(BonusData bonusData, WorldMover worldMover, PlayerHealth targetHealth)
        {
            data = bonusData;
            WorldMover = worldMover;
            TargetHealth = targetHealth;
            _targetRootObject = targetHealth != null ? targetHealth.gameObject : null;
            _lifetimeTimer = 0f;
        }

        protected virtual void Awake()
        {
            _renderer = GetComponentInChildren<Renderer>();
            _propBlock = new MaterialPropertyBlock();
        }

        protected virtual void Update()
        {
            if (data == null)
            {
                return;
            }

            _lifetimeTimer += Time.deltaTime;
            if (_lifetimeTimer >= data.Lifetime)
            {
                Despawn();
                return;
            }

            Move();
            Pulse();
        }

        private void Move()
        {
            float multiplier = WorldMover != null ? WorldMover.SpeedMultiplier : 1f;
            float speed = data.Speed * multiplier;
            transform.position += Vector3.back * (speed * Time.deltaTime);
        }

        private void Pulse()
        {
            if (_renderer == null)
            {
                return;
            }

            float t = (Mathf.Sin(Time.time * data.PulseSpeed) + 1f) * 0.5f;
            float alpha = Mathf.Lerp(data.MinAlpha, data.MaxAlpha, t);
            Color color = data.GlowColor;
            color.a = alpha;

            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_BaseColor", color);
            _renderer.SetPropertyBlock(_propBlock);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_targetRootObject == null)
            {
                return;
            }

            if (other.gameObject == _targetRootObject || other.transform.IsChildOf(_targetRootObject.transform))
            {
                ApplyEffect(TargetHealth);
                OnCollected?.Invoke(this);
                gameObject.SetActive(false);
            }
        }

        protected abstract void ApplyEffect(PlayerHealth health);

        private void Despawn()
        {
            OnCollected?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
