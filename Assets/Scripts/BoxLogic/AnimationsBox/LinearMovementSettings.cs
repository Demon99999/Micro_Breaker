using System;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    [Serializable]
    public class LinearMovementSettings : MovementStrategySettings
    {
        [Header("Linear Movement Settings")]
        public float moveDistance = 3f;
        public float moveDuration = 2f;
        public MovementDirection direction = MovementDirection.Right;
    }
}