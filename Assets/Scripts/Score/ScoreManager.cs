using System;
using MedievalRunner.Input;
using UnityEngine;

namespace MedievalRunner.Score
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private float baseScorePerSecond = 10f;
        [SerializeField] private float boostMultiplier = 2f;
        [SerializeField] private PlayerInputHandler inputHandler;

        public event Action<int> OnScoreChanged;

        private float scoreMultiplier = 1f;
        private float scoreAccumulator;
        private int currentScore;
        private bool scoringEnabled = true;

        private void OnEnable()
        {
            if (inputHandler != null)
            {
                inputHandler.OnBoostStateChanged += HandleBoostChanged;
            }
        }

        private void OnDisable()
        {
            if (inputHandler != null)
            {
                inputHandler.OnBoostStateChanged -= HandleBoostChanged;
            }
        }

        private void Update()
        {
            if (!scoringEnabled)
            {
                return;
            }

            scoreAccumulator += baseScorePerSecond * scoreMultiplier * Time.deltaTime;
            int newScore = Mathf.FloorToInt(scoreAccumulator);
            if (newScore != currentScore)
            {
                currentScore = newScore;
                OnScoreChanged?.Invoke(currentScore);
            }
        }

        public void SetScoringEnabled(bool enabled)
        {
            scoringEnabled = enabled;
        }

        public void SetBoostActive(bool active)
        {
            scoreMultiplier = active ? boostMultiplier : 1f;
        }

        private void HandleBoostChanged(bool active)
        {
            SetBoostActive(active);
        }
    }
}
