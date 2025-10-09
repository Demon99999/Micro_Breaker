using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    [System.Serializable]
    public class FollowPlatformSettings : MovementStrategySettings
    {
        [Header("Follow Settings")]
        public float followSpeed = 3f;
        public float movementDamping = 0.5f;
        public float verticalOffset = 0.5f;
        public float horizontalSpacing = 1f;
        public AnimationCurve followCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Platform Detection")]
        public LayerMask platformLayerMask = 1;
        public float detectionRadius = 2f;
    }
}
