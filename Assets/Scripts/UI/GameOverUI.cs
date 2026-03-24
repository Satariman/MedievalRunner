using MedievalRunner.Core;
using MedievalRunner.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MedievalRunner.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SaveManager saveManager;

        private void OnEnable()
        {
            if (restartButton != null) restartButton.onClick.AddListener(OnRestartClicked);
            if (mainMenuButton != null) mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        private void OnDisable()
        {
            if (restartButton != null) restartButton.onClick.RemoveListener(OnRestartClicked);
            if (mainMenuButton != null) mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        }

        public void Show(int finalScore)
        {
            if (finalScoreText != null)
            {
                finalScoreText.text = $"Score: {finalScore}";
            }
            if (highScoreText != null && saveManager != null)
            {
                highScoreText.text = $"High Score: {saveManager.HighScore}";
            }
        }

        private void OnRestartClicked()
        {
            if (gameManager != null)
            {
                gameManager.StartGame();
            }
        }

        private void OnMainMenuClicked()
        {
            if (gameManager != null)
            {
                gameManager.GoToMenu();
            }
        }
    }
}
