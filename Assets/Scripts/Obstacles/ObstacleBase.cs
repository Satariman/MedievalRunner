using System;
using MedievalRunner.Player;
using MedievalRunner.World;
using UnityEngine;

namespace MedievalRunner.Obstacles
{
    public abstract class ObstacleBase : MonoBehaviour
    {
        [SerializeField] private ObstacleData data;

        public event Action<ObstacleBase> OnDespawn;

        public ObstacleData Data => data;
        protected PlayerHealth TargetHealth { get; private set; }
        protected WorldMover WorldMover { get; private set; }

        private float lifetimeTimer;
        private GameObject _targetRootObject;

        protected virtual void Update()
        {
            if (data == null)
            {
                return;
            }

            lifetimeTimer += Time.deltaTime;
            if (lifetimeTimer >= data.Lifetime)
            {
                Despawn();
                return;
            }

            Move();
            Rotate();
        }

        public void Initialize(ObstacleData obstacleData, WorldMover worldMover, PlayerHealth targetHealth)
        {
            data = obstacleData;
            WorldMover = worldMover;
            TargetHealth = targetHealth;
            _targetRootObject = targetHealth != null ? targetHealth.gameObject : null;
            lifetimeTimer = 0f;
        }

        protected abstract void Move();

        protected virtual void Rotate()
        {
            if (data == null)
            {
                return;
            }

            transform.Rotate(Vector3.right, data.RotationSpeed * Time.deltaTime, Space.Self);
        }

        protected void TryApplyDamage(PlayerHealth health)
        {
            if (health == null || data == null)
            {
                return;
            }

            health.TakeDamage(data.Damage);
        }

        protected void Despawn()
        {
            OnDespawn?.Invoke(this);
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_targetRootObject == null)
            {
                return;
            }

            // Проверяем, является ли объект, с которым столкнулись, нашим игроком
            // Используем кэшированную ссылку вместо GetComponentInParent для избежания аллокаций
            GameObject collisionObj = collision.gameObject;
            if (collisionObj == _targetRootObject || collisionObj.transform.IsChildOf(_targetRootObject.transform))
            {
                TryApplyDamage(TargetHealth);
            }
        }
    }
}
