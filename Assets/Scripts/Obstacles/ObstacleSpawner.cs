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
        [SerializeField] private int poolSizePerType = 5;

        public event Action<ObstacleBase> OnObstacleSpawned;

        private float timer;
        private bool spawningEnabled = true;
        private readonly Dictionary<ObstacleData, Queue<ObstacleBase>> _pool = new Dictionary<ObstacleData, Queue<ObstacleBase>>();

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

        private ObstacleBase GetFromPool(ObstacleData data)
        {
            if (_pool.TryGetValue(data, out Queue<ObstacleBase> queue) && queue.Count > 0)
            {
                ObstacleBase pooled = queue.Dequeue();
                if (pooled != null)
                {
                    return pooled;
                }
            }

            GameObject instance = Instantiate(data.Prefab);
            ObstacleBase obstacle = instance.GetComponent<ObstacleBase>();
            if (obstacle != null)
            {
                obstacle.OnDespawn += ReturnToPool;
            }
            return obstacle;
        }

        private void ReturnToPool(ObstacleBase obstacle)
        {
            if (obstacle == null || obstacle.Data == null)
            {
                return;
            }

            if (!_pool.TryGetValue(obstacle.Data, out Queue<ObstacleBase> queue))
            {
                queue = new Queue<ObstacleBase>();
                _pool[obstacle.Data] = queue;
            }

            if (queue.Count < poolSizePerType)
            {
                queue.Enqueue(obstacle);
            }
            else
            {
                obstacle.OnDespawn -= ReturnToPool;
                Destroy(obstacle.gameObject);
            }
        }

        private void SpawnRandomObstacle()
        {
            ObstacleData data = obstacleData[Random.Range(0, obstacleData.Count)];
            if (data == null || data.Prefab == null)
            {
                return;
            }

            Vector3 basePosition = spawnPoint != null ? spawnPoint.position : transform.position;
            Vector3 spawnPosition = basePosition + Vector3.up * data.SpawnHeight + Vector3.left * Random.Range(-3, 3);

            ObstacleBase obstacle = GetFromPool(data);
            if (obstacle != null)
            {
                obstacle.transform.SetPositionAndRotation(spawnPosition, data.Prefab.transform.rotation);
                obstacle.gameObject.SetActive(true);
                obstacle.Initialize(data, worldMover, targetHealth);
                OnObstacleSpawned?.Invoke(obstacle);
            }
        }
    }
}
