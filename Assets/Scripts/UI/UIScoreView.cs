using System;
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

        private readonly char[] _buffer = new char[32];
        private int _prefixLength;

        private void Awake()
        {
            _prefixLength = prefix.Length;
            prefix.CopyTo(0, _buffer, 0, _prefixLength);
        }

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

            if (score.TryFormat(new Span<char>(_buffer, _prefixLength, _buffer.Length - _prefixLength), out int charsWritten))
            {
                scoreText.SetCharArray(_buffer, 0, _prefixLength + charsWritten);
            }
        }
    }
}
