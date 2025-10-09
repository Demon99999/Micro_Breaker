using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    [System.Serializable]
    public class CirclePlatformSettings : MovementStrategySettings
    {
        [Header("Circle Settings")]
        public float circleRadius = 3f;
        public float rotationSpeed = 30f;

        [Header("Platform Follow Settings")]
        public float followSpeed = 3f;
        public float movementDamping = 0.5f;
        public float verticalOffset = 0.5f;

        [Header("Platform Detection")]
        public LayerMask platformLayerMask = 1;
        public float detectionRadius = 2f;
    }
}
