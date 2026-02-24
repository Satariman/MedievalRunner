using MedievalRunner.Input;
using UnityEngine;

namespace MedievalRunner.Player
{
    public class PlayerRoot : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler inputHandler;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerJumpController jumpController;

        private void OnEnable()
        {
            if (inputHandler == null)
            {
                return;
            }

            inputHandler.OnMove += HandleMove;
            inputHandler.OnJumpPressed += HandleJumpPressed;
        }

        private void OnDisable()
        {
            if (inputHandler == null)
            {
                return;
            }

            inputHandler.OnMove -= HandleMove;
            inputHandler.OnJumpPressed -= HandleJumpPressed;
        }

        private void HandleMove(Vector2 input)
        {
            movement?.SetMoveInput(input);
        }

        private void HandleJumpPressed()
        {
            jumpController?.HandleJumpPressed();
        }
    }
}
