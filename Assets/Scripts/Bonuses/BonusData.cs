using UnityEngine;

namespace MedievalRunner.Bonuses
{
    [CreateAssetMenu(menuName = "MedievalRunner/Bonus Data", fileName = "BonusData")]
    public class BonusData : ScriptableObject
    {
        [Header("Spawn")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private float spawnHeight = 1f;

        [Header("Movement")]
        [SerializeField] private float speed = 6f;
        [SerializeField] private float lifetime = 10f;

        [Header("Visual")]
        [SerializeField] private Color glowColor = Color.red;
        [SerializeField] private float pulseSpeed = 3f;
        [SerializeField] private float minAlpha = 0.3f;
        [SerializeField] private float maxAlpha = 0.8f;

        [Header("Effect")]
        [SerializeField] private float effectDuration = 5f;
        [SerializeField] private int healAmount = 1;

        public GameObject Prefab => prefab;
        public float SpawnHeight => spawnHeight;
        public float Speed => speed;
        public float Lifetime => lifetime;
        public Color GlowColor => glowColor;
        public float PulseSpeed => pulseSpeed;
        public float MinAlpha => minAlpha;
        public float MaxAlpha => maxAlpha;
        public float EffectDuration => effectDuration;
        public int HealAmount => healAmount;
    }
}
