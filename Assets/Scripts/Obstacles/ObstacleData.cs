using UnityEngine;

namespace MedievalRunner.Obstacles
{
    [CreateAssetMenu(menuName = "MedievalRunner/Obstacle Data", fileName = "ObstacleData")]
    public class ObstacleData : ScriptableObject
    {
        [Header("Spawn")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private float spawnHeight = 0f;

        [Header("Movement")]
        [SerializeField] private float speed = 6f;
        [SerializeField] private float rotationSpeed = 180f;

        [Header("Gameplay")]
        [SerializeField] private int damage = 1;
        [SerializeField] private float lifetime = 8f;

        public GameObject Prefab => prefab;
        public float SpawnHeight => spawnHeight;
        public float Speed => speed;
        public float RotationSpeed => rotationSpeed;
        public int Damage => damage;
        public float Lifetime => lifetime;
    }
}
