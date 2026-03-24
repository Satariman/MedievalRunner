using MedievalRunner.Core;
using MedievalRunner.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MedievalRunner.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text developerText;
        [SerializeField] private TMP_Text highScoreText;

        [Header("Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        [Header("Dependencies")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SaveManager saveManager;

        private void OnEnable()
        {
            if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
            if (quitButton != null) quitButton.onClick.AddListener(OnQuitClicked);
            UpdateHighScore();
        }

        private void OnDisable()
        {
            if (playButton != null) playButton.onClick.RemoveListener(OnPlayClicked);
            if (quitButton != null) quitButton.onClick.RemoveListener(OnQuitClicked);
        }

        public void UpdateHighScore()
        {
            if (highScoreText != null && saveManager != null)
            {
                highScoreText.text = $"High Score: {saveManager.HighScore}";
            }
        }

        private void OnPlayClicked()
        {
            if (gameManager != null)
            {
                gameManager.StartGame();
            }
        }

        private void OnQuitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
