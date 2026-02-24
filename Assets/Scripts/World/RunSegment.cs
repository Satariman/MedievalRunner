using UnityEngine;

namespace MedievalRunner.World
{
    public class RunSegment : MonoBehaviour
    {
        [SerializeField] private float length = 20f;

        public float Length => length;
    }
}
