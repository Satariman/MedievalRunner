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
        public bool IsInvulnerable => _invulnerabilityTimer > 0f;

        public event Action<int, int> OnHealthChanged;
        public event Action<int> OnDamageTaken;
        public event Action OnDeath;

        private int _currentHealth;
        private float _invulnerabilityTimer;
        private bool _isDead;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        private void Start()
        {
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        private void Update()
        {
            if (_invulnerabilityTimer > 0f)
            {
                _invulnerabilityTimer -= Time.deltaTime;
            }
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || _isDead || IsInvulnerable)
            {
                return;
            }

            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            _invulnerabilityTimer = invulnerabilityDuration;

            OnDamageTaken?.Invoke(damage);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);

            if (_currentHealth <= 0 && !_isDead)
            {
                _isDead = true;
                OnDeath?.Invoke();
            }
        }

        public void ResetHealth()
        {
            _isDead = false;
            _currentHealth = maxHealth;
            _invulnerabilityTimer = 0f;
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }
    }
}
