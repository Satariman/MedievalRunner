using System;
using UnityEngine;

namespace MedievalRunner.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private float invulnerabilityDuration = 1.25f;

        public int MaxHealth => maxHealth;
        public int CurrentHealth => _currentHealth;
        public bool IsInvulnerable => _invulnerabilityEndTime > Time.time;

        public event Action<int, int> OnHealthChanged;
        public event Action<int> OnDamageTaken;
        public event Action OnDeath;
        public event Action<int> OnHealed;
        public event Action<float> OnInvulnerabilityGranted;

        private int _currentHealth;
        private float _invulnerabilityEndTime;
        private bool _isDead;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        private void Start()
        {
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || _isDead || IsInvulnerable)
            {
                return;
            }

            _currentHealth = Mathf.Max(0, _currentHealth - damage);

            OnDamageTaken?.Invoke(damage);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);

            if (_currentHealth <= 0 && !_isDead)
            {
                _isDead = true;
                OnDeath?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || _isDead)
            {
                return;
            }

            int prev = _currentHealth;
            _currentHealth = Mathf.Min(maxHealth, _currentHealth + amount);
            if (_currentHealth != prev)
            {
                OnHealed?.Invoke(amount);
                OnHealthChanged?.Invoke(_currentHealth, maxHealth);
            }
        }

        public void GrantInvulnerability(float duration)
        {
            if (duration <= 0f || _isDead)
            {
                return;
            }

            float newEnd = Time.time + duration;
            if (newEnd > _invulnerabilityEndTime)
            {
                _invulnerabilityEndTime = newEnd;
            }
            OnInvulnerabilityGranted?.Invoke(duration);
        }

        public void ResetHealth()
        {
            _isDead = false;
            _currentHealth = maxHealth;
            _invulnerabilityEndTime = 0f;
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }
    }
}
