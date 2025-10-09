using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    public class LinearMovementStrategy : IMovementStrategy
    {
        private Box[] boxes;
        private Vector3[] initialPositions;
        private List<BoxMovementData> boxData = new List<BoxMovementData>();
        private Tween movementTween;
        private bool isGravityEnabled = false;
        private LinearMovementSettings settings;

        public void Initialize(Box[] boxes, Vector3[] initialPositions, MovementStrategySettings settings)
        {
            this.boxes = boxes;
            this.initialPositions = initialPositions;
            this.settings = (LinearMovementSettings)settings;

            for (int i = 0; i < boxes.Length; i++)
            {
                if (boxes[i] != null)
                {
                    boxData.Add(new BoxMovementData
                    {
                        box = boxes[i],
                        initialPosition = initialPositions[i],
                        isGravityEnabled = false
                    });
                }
            }

            StartMovement();
        }

        public void Update() { }

        private void StartMovement()
        {
            Vector3 movementVector = GetMovementVector();
            Vector3 centerPosition = CalculateCenterPosition();

            movementTween = DOTween.To(() => centerPosition, x => UpdateBoxesPosition(x),
                centerPosition + movementVector, settings.moveDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private Vector3 CalculateCenterPosition()
        {
            Vector3 center = Vector3.zero;
            foreach (var data in boxData)
            {
                if (data.box != null) center += data.box.transform.position;
            }
            return center / boxData.Count;
        }

        private void UpdateBoxesPosition(Vector3 centerPosition)
        {
            Vector3 originalCenter = CalculateCenterPosition();

            foreach (var data in boxData)
            {
                if (data.box != null && !data.isGravityEnabled)
                {
                    Vector3 localOffset = data.initialPosition - originalCenter;
                    data.box.transform.position = centerPosition + localOffset;
                }
            }
        }

        private Vector3 GetMovementVector()
        {
            switch (settings.direction)
            {
                case MovementDirection.Right: return Vector3.right * settings.moveDistance;
                case MovementDirection.Left: return Vector3.left * settings.moveDistance;
                case MovementDirection.Up: return Vector3.up * settings.moveDistance;
                case MovementDirection.Down: return Vector3.down * settings.moveDistance;
                case MovementDirection.Forward: return Vector3.forward * settings.moveDistance;
                case MovementDirection.Back: return Vector3.back * settings.moveDistance;
                default: return Vector3.zero;
            }
        }

        public void OnBoxHit(Box hitBox)
        {
            if (!isGravityEnabled)
                EnableGravityForAllBoxes();
        }

        public void EnableGravityForBox(Box box)
        {
            var boxDataItem = boxData.Find(data => data.box == box);
            if (boxDataItem != null && !boxDataItem.isGravityEnabled)
            {
                boxDataItem.isGravityEnabled = true;
                EnablePhysicsForBox(box);
            }
        }

        public void EnableGravityForAllBoxes()
        {
            isGravityEnabled = true;
            movementTween?.Kill();

            foreach (var data in boxData)
            {
                if (!data.isGravityEnabled)
                {
                    data.isGravityEnabled = true;
                    EnablePhysicsForBox(data.box);
                }
            }
        }

        private void EnablePhysicsForBox(Box box)
        {
            if (box == null)
                return;

            Rigidbody rb = box.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }

        public void Cleanup()
        {
            movementTween?.Kill();
            boxData.Clear();
        }
    }
}
