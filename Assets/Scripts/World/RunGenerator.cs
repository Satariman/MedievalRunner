using System.Collections.Generic;
using UnityEngine;

namespace MedievalRunner.World
{
    public class RunGenerator : MonoBehaviour
    {
        [SerializeField] private WorldMover worldMover;
        [SerializeField] private List<RunSegment> segments = new List<RunSegment>();
        [SerializeField] private float recycleZ = -30f;

        private readonly List<RunSegment> _recycleBuffer = new List<RunSegment>();

        private void Update()
        {
            if (worldMover == null || segments.Count == 0)
            {
                return;
            }

            float speed = worldMover.CurrentSpeed;
            if (speed == 0f)
            {
                return;
            }

            float delta = speed * Time.deltaTime;
            float maxZ = float.MinValue;
            _recycleBuffer.Clear();

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

                if (position.z > maxZ)
                {
                    maxZ = position.z;
                }

                if (position.z <= recycleZ)
                {
                    _recycleBuffer.Add(segment);
                }
            }

            for (int i = 0; i < _recycleBuffer.Count; i++)
            {
                RunSegment segment = _recycleBuffer[i];
                Vector3 position = segment.transform.position;
                position.z = maxZ + segment.Length;
                segment.transform.position = position;
                maxZ = position.z;
            }
        }
    }
}
