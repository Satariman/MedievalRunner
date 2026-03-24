using MedievalRunner.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MedievalRunner.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private GameManager gameManager;

        private void OnEnable()
        {
            if (resumeButton != null) resumeButton.onClick.AddListener(OnResumeClicked);
            if (mainMenuButton != null) mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        private void OnDisable()
        {
            if (resumeButton != null) resumeButton.onClick.RemoveListener(OnResumeClicked);
            if (mainMenuButton != null) mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        }

        private void OnResumeClicked()
        {
            if (gameManager != null)
            {
                gameManager.ResumeGame();
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
