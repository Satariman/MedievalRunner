using MedievalRunner.Player;
using TMPro;
using UnityEngine;

namespace MedievalRunner.UI
{
    public class UIHealthView : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private string prefix = "Health: ";

        private void OnEnable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged += HandleHealthChanged;
            }
        }

        private void OnDisable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged -= HandleHealthChanged;
            }
        }

        private void HandleHealthChanged(int current, int max)
        {
            if (healthText == null)
            {
                return;
            }

            healthText.text = $"{prefix}{current}/{max}";
        }
    }
}
