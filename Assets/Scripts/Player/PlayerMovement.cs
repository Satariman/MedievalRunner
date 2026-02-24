using UnityEngine;

namespace MedievalRunner.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private float airControlMultiplier = 0.6f;

        private CharacterController characterController;
        private Vector2 moveInput;
        private float verticalVelocity;

        public bool IsGrounded => characterController != null && characterController.isGrounded;
        public float VerticalVelocity => verticalVelocity;
        public float Gravity => gravity;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            ApplyGravity();
            MoveCharacter();
        }

        public void SetMoveInput(Vector2 input)
        {
            moveInput = input;
        }

        public void SetVerticalVelocity(float velocity)
        {
            verticalVelocity = velocity;
        }

        public void AddVerticalVelocity(float velocityDelta)
        {
            verticalVelocity += velocityDelta;
        }

        private void ApplyGravity()
        {
            if (IsGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            verticalVelocity += gravity * Time.deltaTime;
        }

        private void MoveCharacter()
        {
            Vector3 horizontal = new Vector3(moveInput.x, moveInput.y, 0f);
            float controlMultiplier = IsGrounded ? 1f : airControlMultiplier;
            Vector3 horizontalVelocity = horizontal * (moveSpeed * controlMultiplier);
            Vector3 velocity = horizontalVelocity;
            velocity.y = verticalVelocity;

            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
