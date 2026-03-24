using System;
using System.Collections.Generic;
using MedievalRunner.Player;
using MedievalRunner.World;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MedievalRunner.Bonuses
{
    public class BonusSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnInterval = 8f;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private List<BonusData> bonusDataList = new List<BonusData>();
        [SerializeField] private WorldMover worldMover;
        [SerializeField] private PlayerHealth targetHealth;
        [SerializeField] private int poolSizePerType = 3;

        public event Action<BonusBase> OnBonusSpawned;

        private float timer;
        private bool spawningEnabled = true;
        private readonly Dictionary<BonusData, Queue<BonusBase>> _pool = new Dictionary<BonusData, Queue<BonusBase>>();

        private void Update()
        {
            if (!spawningEnabled || bonusDataList.Count == 0)
            {
                return;
            }

            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                SpawnRandomBonus();
            }
        }

        public void SetSpawningEnabled(bool enabled)
        {
            spawningEnabled = enabled;
        }

        private BonusBase GetFromPool(BonusData data)
        {
            if (_pool.TryGetValue(data, out Queue<BonusBase> queue) && queue.Count > 0)
            {
                BonusBase pooled = queue.Dequeue();
                if (pooled != null)
                {
                    return pooled;
                }
            }

            GameObject instance = Instantiate(data.Prefab);
            BonusBase bonus = instance.GetComponent<BonusBase>();
            if (bonus != null)
            {
                bonus.OnCollected += ReturnToPool;
            }
            return bonus;
        }

        private void ReturnToPool(BonusBase bonus)
        {
            if (bonus == null || bonus.Data == null)
            {
                return;
            }

            if (!_pool.TryGetValue(bonus.Data, out Queue<BonusBase> queue))
            {
                queue = new Queue<BonusBase>();
                _pool[bonus.Data] = queue;
            }

            if (queue.Count < poolSizePerType)
            {
                queue.Enqueue(bonus);
            }
            else
            {
                bonus.OnCollected -= ReturnToPool;
                Destroy(bonus.gameObject);
            }
        }

        private void SpawnRandomBonus()
        {
            BonusData data = bonusDataList[Random.Range(0, bonusDataList.Count)];
            if (data == null || data.Prefab == null)
            {
                return;
            }

            Vector3 basePosition = spawnPoint != null ? spawnPoint.position : transform.position;
            Vector3 spawnPosition = basePosition + Vector3.up * data.SpawnHeight + Vector3.left * Random.Range(-3, 3);

            BonusBase bonus = GetFromPool(data);
            if (bonus != null)
            {
                bonus.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
                bonus.gameObject.SetActive(true);
                bonus.Initialize(data, worldMover, targetHealth);
                OnBonusSpawned?.Invoke(bonus);
            }
        }
    }
}
