using UnityEngine;

namespace MedievalRunner.Obstacles
{
    public class BoulderObstacle : ObstacleBase
    {
        protected override void Move()
        {
            if (Data == null)
            {
                return;
            }

            float multiplier = WorldMover != null ? WorldMover.SpeedMultiplier : 1f;
            float speed = Data.Speed * multiplier;
            
            // Двигаемся только назад. Гравитация Unity (Rigidbody) сама прижмет объект к полу,
            // если он заспавнен сверху, при условии что IsKinematic = false.
            transform.position += Vector3.back * (speed * Time.deltaTime);
        }
    }
}
