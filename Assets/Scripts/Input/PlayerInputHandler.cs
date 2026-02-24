using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MedievalRunner.Input
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Actions")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference boostAction;

        public event Action<Vector2> OnMove;
        public event Action OnJumpPressed;
        public event Action<bool> OnBoostStateChanged;

        private bool inputEnabled = true;

        private void OnEnable()
        {
            RegisterCallbacks();
            SetInputEnabled(inputEnabled);
        }

        private void OnDisable()
        {
            UnregisterCallbacks();
            DisableActions();
        }

        public void SetInputEnabled(bool enabled)
        {
            inputEnabled = enabled;
            if (inputEnabled)
            {
                EnableActions();
            }
            else
            {
                DisableActions();
                OnMove?.Invoke(Vector2.zero);
                OnBoostStateChanged?.Invoke(false);
            }
        }

        private void RegisterCallbacks()
        {
            if (moveAction != null)
            {
                moveAction.action.performed += HandleMove;
                moveAction.action.canceled += HandleMove;
            }

            if (jumpAction != null)
            {
                jumpAction.action.performed += HandleJump;
            }

            if (boostAction != null)
            {
                boostAction.action.started += HandleBoostStarted;
                boostAction.action.canceled += HandleBoostCanceled;
            }
        }

        private void UnregisterCallbacks()
        {
            if (moveAction != null)
            {
                moveAction.action.performed -= HandleMove;
                moveAction.action.canceled -= HandleMove;
            }

            if (jumpAction != null)
            {
                jumpAction.action.performed -= HandleJump;
            }

            if (boostAction != null)
            {
                boostAction.action.started -= HandleBoostStarted;
                boostAction.action.canceled -= HandleBoostCanceled;
            }
        }

        private void EnableActions()
        {
            moveAction?.action.Enable();
            jumpAction?.action.Enable();
            boostAction?.action.Enable();
        }

        private void DisableActions()
        {
            moveAction?.action.Disable();
            jumpAction?.action.Disable();
            boostAction?.action.Disable();
        }

        private void HandleMove(InputAction.CallbackContext context)
        {
            if (!inputEnabled)
            {
                return;
            }

            OnMove?.Invoke(context.ReadValue<Vector2>());
        }

        private void HandleJump(InputAction.CallbackContext context)
        {
            if (!inputEnabled)
            {
                return;
            }

            OnJumpPressed?.Invoke();
        }

        private void HandleBoostStarted(InputAction.CallbackContext context)
        {
            if (!inputEnabled)
            {
                return;
            }

            OnBoostStateChanged?.Invoke(true);
        }

        private void HandleBoostCanceled(InputAction.CallbackContext context)
        {
            OnBoostStateChanged?.Invoke(false);
        }
    }
}
