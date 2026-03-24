using UnityEngine;

namespace MedievalRunner.Core
{
    public class PerformanceInitializer : MonoBehaviour
    {
        [SerializeField] private int targetQualityLevel = 2;
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private int vSyncCount = 0;

        private void Awake()
        {
            QualitySettings.SetQualityLevel(targetQualityLevel, true);
            QualitySettings.vSyncCount = vSyncCount;
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
