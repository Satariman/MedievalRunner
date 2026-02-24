using System;
using MedievalRunner.Input;
using MedievalRunner.Obstacles;
using MedievalRunner.Player;
using MedievalRunner.Score;
using MedievalRunner.World;
using UnityEngine;

namespace MedievalRunner.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("State")]
        [SerializeField] private GameState initialState = GameState.Playing;

        [Header("Dependencies")]
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private PlayerInputHandler inputHandler;
        [SerializeField] private WorldMover worldMover;
        [SerializeField] private ObstacleSpawner obstacleSpawner;
        [SerializeField] private ScoreManager scoreManager;

        public GameState State { get; private set; }

        public event Action<GameState> OnStateChanged;

        private void OnEnable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnDeath += HandlePlayerDeath;
            }
        }

        private void OnDisable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnDeath -= HandlePlayerDeath;
            }
        }

        private void Start()
        {
            SetState(initialState);
        }

        private void HandlePlayerDeath()
        {
            SetState(GameState.GameOver);
        }

        private void SetState(GameState newState)
        {
            if (State == newState)
            {
                return;
            }

            State = newState;
            ApplyState(State);
            OnStateChanged?.Invoke(State);
        }

        private void ApplyState(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    inputHandler?.SetInputEnabled(true);
                    worldMover?.SetMovementEnabled(true);
                    obstacleSpawner?.SetSpawningEnabled(true);
                    scoreManager?.SetScoringEnabled(true);
                    break;
                case GameState.Menu:
                case GameState.GameOver:
                    inputHandler?.SetInputEnabled(false);
                    worldMover?.SetMovementEnabled(false);
                    obstacleSpawner?.SetSpawningEnabled(false);
                    scoreManager?.SetScoringEnabled(false);
                    break;
            }
        }
    }
}
