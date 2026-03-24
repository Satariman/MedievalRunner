using System;
using MedievalRunner.Bonuses;
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
        [SerializeField] private GameState initialState = GameState.Menu;

        [Header("Dependencies")]
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private PlayerInputHandler inputHandler;
        [SerializeField] private WorldMover worldMover;
        [SerializeField] private ObstacleSpawner obstacleSpawner;
        [SerializeField] private BonusSpawner bonusSpawner;
        [SerializeField] private ScoreManager scoreManager;

        public GameState State { get; private set; }

        public event Action<GameState> OnStateChanged;

        private void OnEnable()
        {
            if (playerHealth != null) playerHealth.OnDeath += HandlePlayerDeath;
            if (inputHandler != null) inputHandler.OnPausePressed += HandlePausePressed;
        }

        private void OnDisable()
        {
            if (playerHealth != null) playerHealth.OnDeath -= HandlePlayerDeath;
            if (inputHandler != null) inputHandler.OnPausePressed -= HandlePausePressed;
        }

        private void Start()
        {
            // Force transition from default to initial by resetting state
            State = (GameState)(-1);
            SetState(initialState);
        }

        public void StartGame()
        {
            playerHealth?.ResetHealth();
            scoreManager?.ResetScore();
            SetState(GameState.Playing);
        }

        public void ResumeGame()
        {
            SetState(GameState.Playing);
        }

        public void GoToMenu()
        {
            SetState(GameState.Menu);
        }

        private void HandlePlayerDeath()
        {
            SetState(GameState.GameOver);
        }

        private void HandlePausePressed()
        {
            if (State == GameState.Playing)
            {
                SetState(GameState.Paused);
            }
            else if (State == GameState.Paused)
            {
                SetState(GameState.Playing);
            }
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
                    Time.timeScale = 1f;
                    inputHandler?.SetInputEnabled(true);
                    worldMover?.SetMovementEnabled(true);
                    obstacleSpawner?.SetSpawningEnabled(true);
                    bonusSpawner?.SetSpawningEnabled(true);
                    scoreManager?.SetScoringEnabled(true);
                    break;

                case GameState.Paused:
                    Time.timeScale = 0f;
                    inputHandler?.SetInputEnabled(false);
                    worldMover?.SetMovementEnabled(false);
                    obstacleSpawner?.SetSpawningEnabled(false);
                    bonusSpawner?.SetSpawningEnabled(false);
                    scoreManager?.SetScoringEnabled(false);
                    break;

                case GameState.Menu:
                    Time.timeScale = 1f;
                    inputHandler?.SetInputEnabled(false);
                    worldMover?.SetMovementEnabled(false);
                    obstacleSpawner?.SetSpawningEnabled(false);
                    bonusSpawner?.SetSpawningEnabled(false);
                    scoreManager?.SetScoringEnabled(false);
                    break;

                case GameState.GameOver:
                    Time.timeScale = 0f;
                    inputHandler?.SetInputEnabled(false);
                    worldMover?.SetMovementEnabled(false);
                    obstacleSpawner?.SetSpawningEnabled(false);
                    bonusSpawner?.SetSpawningEnabled(false);
                    scoreManager?.SetScoringEnabled(false);
                    break;
            }
        }
    }
}
