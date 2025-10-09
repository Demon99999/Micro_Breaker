using System;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    [Serializable]
    public class CircularMovementSettings : MovementStrategySettings
    {
        [Header("Circular Movement Settings")]
        public float circleRadius = 3f;
        public float circleDuration = 3f;
        public float directionChangeInterval = 5f;
    }
}