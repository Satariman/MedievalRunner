using System;
using UnityEngine;

namespace MedievalRunner.Player
{
    public class PlayerJumpController : MonoBehaviour
    {
        [Header("Jump")]
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float gravityMultiplier = 2f;
        [SerializeField] private PlayerMovement playerMovement;

        public event Action OnLanded;

        private bool jumpConsumed;
        private bool wasGrounded;

        private void Update()
        {
            if (playerMovement == null)
            {
                return;
            }

            bool grounded = playerMovement.IsGrounded;
            if (grounded && !wasGrounded)
            {
                jumpConsumed = false;
                OnLanded?.Invoke();
            }

            ApplyExtraGravity(grounded);
            wasGrounded = grounded;
        }

        public void HandleJumpPressed()
        {
            if (playerMovement == null)
            {
                return;
            }

            if (!playerMovement.IsGrounded || jumpConsumed)
            {
                return;
            }

            playerMovement.SetVerticalVelocity(jumpForce);
            jumpConsumed = true;
        }

        private void ApplyExtraGravity(bool grounded)
        {
            if (grounded)
            {
                return;
            }

            if (playerMovement.VerticalVelocity >= 0f)
            {
                return;
            }

            float extraGravity = playerMovement.Gravity * (gravityMultiplier - 1f);
            playerMovement.AddVerticalVelocity(extraGravity * Time.deltaTime);
        }
    }
}
