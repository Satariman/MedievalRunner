using MedievalRunner.Player;
using UnityEngine;

namespace MedievalRunner.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Animator animator;
        [SerializeField] private Renderer playerRenderer;

        [Header("Color Settings")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color damagedColor = new Color(1f, 0.2f, 0.2f);
        [SerializeField] private Color bonusPickupColor = new Color(1f, 0.9f, 0.2f);
        [SerializeField] private Color invulnerableColor = new Color(0.2f, 0.6f, 1f);

        private static readonly int DamageTrigger = Animator.StringToHash("Damage");
        private static readonly int BonusTrigger = Animator.StringToHash("Bonus");
        private static readonly int IsInvulnerable = Animator.StringToHash("IsInvulnerable");

        private MaterialPropertyBlock _propBlock;

        private void Awake()
        {
            _propBlock = new MaterialPropertyBlock();
        }

        private void OnEnable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnDamageTaken += HandleDamageTaken;
                playerHealth.OnHealed += HandleHealed;
                playerHealth.OnInvulnerabilityGranted += HandleInvulnerabilityGranted;
            }
        }

        private void OnDisable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnDamageTaken -= HandleDamageTaken;
                playerHealth.OnHealed -= HandleHealed;
                playerHealth.OnInvulnerabilityGranted -= HandleInvulnerabilityGranted;
            }
        }

        private void Update()
        {
            if (animator == null || playerHealth == null)
            {
                return;
            }

            bool invulnerable = playerHealth.IsInvulnerable;
            animator.SetBool(IsInvulnerable, invulnerable);
            
            if (invulnerable)
            {
                float t = (Mathf.Sin(Time.time * 8f) + 1f) * 0.5f;
                SetColor(Color.Lerp(invulnerableColor, normalColor, t));
            }
        }

        private void HandleDamageTaken(int damage)
        {
            if (animator != null)
            {
                animator.SetTrigger(DamageTrigger);
            }
            FlashColor(damagedColor, 0.2f);
        }

        private void HandleHealed(int amount)
        {
            if (animator != null)
            {
                animator.SetTrigger(BonusTrigger);
            }
            FlashColor(bonusPickupColor, 0.3f);
        }

        private void HandleInvulnerabilityGranted(float duration)
        {
            if (animator != null)
            {
                animator.SetTrigger(BonusTrigger);
            }
            FlashColor(bonusPickupColor, 0.3f);
        }

        private void FlashColor(Color color, float duration)
        {
            StopAllCoroutines();
            StartCoroutine(FlashCoroutine(color, duration));
        }

        private System.Collections.IEnumerator FlashCoroutine(Color flashColor, float duration)
        {
            SetColor(flashColor);
            yield return new WaitForSeconds(duration);
            SetColor(normalColor);
        }

        private void SetColor(Color color)
        {
            if (playerRenderer == null)
            {
                return;
            }

            playerRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_BaseColor", color);
            playerRenderer.SetPropertyBlock(_propBlock);
        }
    }
}
