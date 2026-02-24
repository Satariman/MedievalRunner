using System.Collections.Generic;
using UnityEngine;

namespace MedievalRunner.World
{
    public class RunGenerator : MonoBehaviour
    {
        [SerializeField] private WorldMover worldMover;
        [SerializeField] private List<RunSegment> segments = new List<RunSegment>();
        [SerializeField] private float recycleZ = -30f;

        private void Update()
        {
            if (worldMover == null || segments.Count == 0)
            {
                return;
            }

            float speed = worldMover.CurrentSpeed;
            if (Mathf.Approximately(speed, 0f))
            {
                return;
            }

            float delta = speed * Time.deltaTime;
            for (int i = 0; i < segments.Count; i++)
            {
                RunSegment segment = segments[i];
                if (segment == null)
                {
                    continue;
                }

                Vector3 position = segment.transform.position;
                position.z -= delta;
                segment.transform.position = position;

                if (position.z <= recycleZ)
                {
                    RecycleSegment(segment);
                }
            }
        }

        private void RecycleSegment(RunSegment segment)
        {
            float maxZ = float.MinValue;
            for (int i = 0; i < segments.Count; i++)
            {
                RunSegment other = segments[i];
                if (other == null)
                {
                    continue;
                }

                maxZ = Mathf.Max(maxZ, other.transform.position.z);
            }

            Vector3 position = segment.transform.position;
            position.z = maxZ + segment.Length;
            segment.transform.position = position;
        }
    }
}
