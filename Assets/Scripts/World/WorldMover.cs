using MedievalRunner.Input;
using UnityEngine;

namespace MedievalRunner.World
{
    public class WorldMover : MonoBehaviour
    {
        [SerializeField] private float worldSpeed = 6f;
        [SerializeField] private float boostMultiplier = 2f;
        [SerializeField] private PlayerInputHandler inputHandler;

        private float speedMultiplier = 1f;
        private bool movementEnabled = true;

        public float CurrentSpeed => movementEnabled ? worldSpeed * speedMultiplier : 0f;
        public float SpeedMultiplier => speedMultiplier;

        private void OnEnable()
        {
            if (inputHandler != null)
            {
                inputHandler.OnBoostStateChanged += HandleBoostChanged;
            }
        }

        private void OnDisable()
        {
            if (inputHandler != null)
            {
                inputHandler.OnBoostStateChanged -= HandleBoostChanged;
            }
        }

        public void SetBoostActive(bool active)
        {
            speedMultiplier = active ? boostMultiplier : 1f;
        }

        public void SetMovementEnabled(bool enabled)
        {
            movementEnabled = enabled;
        }

        private void HandleBoostChanged(bool active)
        {
            SetBoostActive(active);
        }
    }
}
