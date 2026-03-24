using MedievalRunner.Core;
using MedievalRunner.Score;
using MedievalRunner.Save;
using UnityEngine;

namespace MedievalRunner.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private SaveManager saveManager;

        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject hudPanel;
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private GameObject gameOverPanel;

        private MainMenuUI _mainMenuUI;
        private GameOverUI _gameOverUI;

        private void Awake()
        {
            if (mainMenuPanel != null) _mainMenuUI = mainMenuPanel.GetComponent<MainMenuUI>();
            if (gameOverPanel != null) _gameOverUI = gameOverPanel.GetComponent<GameOverUI>();
        }

        private void OnEnable()
        {
            if (gameManager != null)
            {
                gameManager.OnStateChanged += HandleStateChanged;
            }
        }

        private void OnDisable()
        {
            if (gameManager != null)
            {
                gameManager.OnStateChanged -= HandleStateChanged;
            }
        }

        private void Start()
        {
            HandleStateChanged(gameManager != null ? gameManager.State : GameState.Menu);
        }

        private void HandleStateChanged(GameState state)
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(state == GameState.Menu);
            if (hudPanel != null) hudPanel.SetActive(state == GameState.Playing || state == GameState.Paused);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(state == GameState.Paused);
            if (gameOverPanel != null) gameOverPanel.SetActive(state == GameState.GameOver);

            if (state == GameState.Menu && _mainMenuUI != null)
            {
                _mainMenuUI.UpdateHighScore();
            }

            if (state == GameState.GameOver)
            {
                int finalScore = scoreManager != null ? scoreManager.CurrentScore : 0;
                if (saveManager != null)
                {
                    saveManager.TryUpdateHighScore(finalScore);
                }
                if (_gameOverUI != null)
                {
                    _gameOverUI.Show(finalScore);
                }
            }
        }
    }
}
