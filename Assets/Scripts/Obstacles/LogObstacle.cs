using UnityEngine;

namespace MedievalRunner.Obstacles
{
    public class LogObstacle : ObstacleBase
    {
        protected override void Move()
        {
            if (Data == null)
            {
                return;
            }

            float multiplier = WorldMover != null ? WorldMover.SpeedMultiplier : 1f;
            float speed = Data.Speed * multiplier;
            transform.position += Vector3.back * (speed * Time.deltaTime);
        }
    }
}
