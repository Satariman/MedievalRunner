using UnityEngine;

namespace MedievalRunner.Save
{
    public class SaveManager : MonoBehaviour
    {
        private const string HighScoreKey = "HighScore";

        public int HighScore { get; private set; }

        private void Awake()
        {
            HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        }

        public bool TryUpdateHighScore(int score)
        {
            if (score <= HighScore)
            {
                return false;
            }

            HighScore = score;
            PlayerPrefs.SetInt(HighScoreKey, score);
            PlayerPrefs.Save();
            return true;
        }
    }
}
