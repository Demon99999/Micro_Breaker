using System;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    [Serializable]
    public class BoxMovementData
    {
        public Box box;
        public Vector3 initialPosition;
        public Tween movementTween;
        public bool isGravityEnabled = false;
        public float currentAngle = 0f;
        public float targetAngle = 0f;
        public Vector3 targetPosition;
        public bool isFollowing;
    }
}