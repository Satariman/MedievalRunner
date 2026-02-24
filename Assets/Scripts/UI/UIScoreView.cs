using MedievalRunner.Score;
using TMPro;
using UnityEngine;

namespace MedievalRunner.UI
{
    public class UIScoreView : MonoBehaviour
    {
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private string prefix = "Score: ";

        private void OnEnable()
        {
            if (scoreManager != null)
            {
                scoreManager.OnScoreChanged += HandleScoreChanged;
            }
        }

        private void OnDisable()
        {
            if (scoreManager != null)
            {
                scoreManager.OnScoreChanged -= HandleScoreChanged;
            }
        }

        private void HandleScoreChanged(int score)
        {
            if (scoreText == null)
            {
                return;
            }

            scoreText.text = $"{prefix}{score}";
        }
    }
}
