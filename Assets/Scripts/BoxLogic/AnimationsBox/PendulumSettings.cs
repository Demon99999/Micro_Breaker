using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    [System.Serializable]
    public class PendulumSettings : MovementStrategySettings
    {
        [Header("Pendulum Settings")]
        [Range(5f, 75f)]
        public float swingAngle = 45f;

        [Header("Timing Settings")]
        public float swingDuration = 2f;
        public float pauseBetweenSwings = 0.3f;

        [Header("Animation Settings")]
        public bool usePhysicsCurve = true;
        public AnimationCurve swingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }
}