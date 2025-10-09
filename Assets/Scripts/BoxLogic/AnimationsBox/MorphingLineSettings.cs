using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    [System.Serializable]
    public class MorphingLineSettings : MovementStrategySettings
    {
        [Header("Snake Type")]
        public SnakeType snakeType = SnakeType.Left;

        [Header("Snake Settings")]
        public float snakeLength = 4f;

        [Header("Letter C Settings")]
        public float letterRadius = 3f;

        [Header("Animation Settings")]
        public float morphDuration = 2.5f;
        public float pauseBetweenMorphs = 1.0f;
        public AnimationCurve morphCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }

    public enum SnakeType
    {
        Left,
        Right
    }
}