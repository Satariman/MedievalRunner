using System;
using System.Collections.Generic;
using MedievalRunner.Player;
using MedievalRunner.World;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MedievalRunner.Obstacles
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private List<ObstacleData> obstacleData = new List<ObstacleData>();
        [SerializeField] private WorldMover worldMover;
        [SerializeField] private PlayerHealth targetHealth;

        public event Action<ObstacleBase> OnObstacleSpawned;

        private float timer;
        private bool spawningEnabled = true;

        private void Update()
        {
            if (!spawningEnabled || obstacleData.Count == 0)
            {
                return;
            }

            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                SpawnRandomObstacle();
            }
        }

        public void SetSpawningEnabled(bool enabled)
        {
            spawningEnabled = enabled;
        }

        private void SpawnRandomObstacle()
        {
            ObstacleData data = obstacleData[UnityEngine.Random.Range(0, obstacleData.Count)];
            if (data == null || data.Prefab == null)
            {
                return;
            }

            Vector3 basePosition = spawnPoint != null ? spawnPoint.position : transform.position;
            Vector3 spawnPosition = basePosition + Vector3.up * data.SpawnHeight + Vector3.left * Random.Range(-3, 3);

            GameObject instance = Instantiate(data.Prefab, spawnPosition, data.Prefab.transform.rotation);
            ObstacleBase obstacle = instance.GetComponent<ObstacleBase>();
            if (obstacle != null)
            {
                obstacle.Initialize(data, worldMover, targetHealth);
                OnObstacleSpawned?.Invoke(obstacle);
            }
        }
    }
}
