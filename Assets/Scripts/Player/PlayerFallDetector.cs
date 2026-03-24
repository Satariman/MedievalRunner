using UnityEngine;

namespace MedievalRunner.Player
{
    public class PlayerFallDetector : MonoBehaviour
    {
        [SerializeField] private float fallThresholdY = -2f;
        [SerializeField] private PlayerHealth playerHealth;

        private void Update()
        {
            if (playerHealth == null) return;

            if (transform.position.y < fallThresholdY)
            {
                playerHealth.TakeDamage(playerHealth.MaxHealth);
            }
        }
    }
}
