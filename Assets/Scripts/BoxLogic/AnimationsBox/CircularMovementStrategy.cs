using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    public class CircularMovementStrategy : IMovementStrategy
    {
        private Box[] boxes;
        private Vector3[] initialPositions;
        private List<BoxMovementData> boxData = new List<BoxMovementData>();
        private Vector3 circleCenter;
        private bool clockwise = true;
        private float directionChangeTimer = 0f;
        private bool isChangingDirection = false;
        private CircularMovementSettings settings;
        private List<Tween> boxTweens = new List<Tween>();
        private Tween directionChangeTween;

        public void Initialize(Box[] boxes, Vector3[] initialPositions, MovementStrategySettings settings)
        {
            this.boxes = boxes;
            this.initialPositions = initialPositions;
            this.settings = (CircularMovementSettings)settings;

            CalculateCircleCenter();
            InitializeBoxData();
            StartCircularMovement();
        }

        public void Update()
        {
            if (isChangingDirection) return;

            directionChangeTimer += Time.deltaTime;
            if (directionChangeTimer >= settings.directionChangeInterval)
            {
                directionChangeTimer = 0f;
                clockwise = !clockwise;
                SmoothChangeDirection();
            }
        }

        private void CalculateCircleCenter()
        {
            circleCenter = Vector3.zero;
            int count = 0;
            foreach (var box in boxes)
            {
                if (box != null)
                {
                    circleCenter += box.transform.position;
                    count++;
                }
            }
            circleCenter /= count;
        }

        private void InitializeBoxData()
        {
            foreach (var box in boxes)
            {
                if (box != null)
                {
                    boxData.Add(new BoxMovementData
                    {
                        box = box,
                        initialPosition = box.transform.position,
                        isGravityEnabled = false,
                        currentAngle = CalculateInitialAngle(box.transform.position)
                    });
                }
            }
        }

        private float CalculateInitialAngle(Vector3 position)
        {
            Vector3 direction = position - circleCenter;
            return Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        }

        private void StartCircularMovement()
        {
            CleanupTweens();

            foreach (var data in boxData)
            {
                if (data.box != null && !data.isGravityEnabled)
                {
                    StartSingleCircularMovement(data);
                }
            }
        }

        private void StartSingleCircularMovement(BoxMovementData data)
        {
            if (data.box == null || data.isGravityEnabled) return;

            float direction = clockwise ? 1f : -1f;
            float targetAngle = data.currentAngle + 360f * direction;

            data.movementTween = DOTween.To(() => data.currentAngle, x =>
            {
                data.currentAngle = x;
                UpdateCircularPosition(data);
            }, targetAngle, settings.circleDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .OnPlay(() => {
            
            UpdateCubeOrientation(data);
            });

            boxTweens.Add(data.movementTween);
        }

        private void UpdateCircularPosition(BoxMovementData data)
        {
            if (data.box == null || data.isGravityEnabled) return;

            float rad = data.currentAngle * Mathf.Deg2Rad;
            float x = Mathf.Cos(rad) * settings.circleRadius;
            float z = Mathf.Sin(rad) * settings.circleRadius;

            Vector3 newPosition = circleCenter + new Vector3(x, data.initialPosition.y, z);
            data.box.transform.position = newPosition;

            UpdateCubeOrientation(data);
        }

        private void UpdateCubeOrientation(BoxMovementData data)
        {
            if (data.box == null) return;

            Vector3 directionToCenter = circleCenter - data.box.transform.position;

            if (directionToCenter != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToCenter);

                data.box.transform.rotation = Quaternion.Slerp(
                    data.box.transform.rotation,
                    targetRotation,
                    Time.deltaTime * 5f
                );
            }
        }

        private void SmoothChangeDirection()
        {
            if (isChangingDirection) return;
            isChangingDirection = true;

            foreach (var tween in boxTweens)
            {
                if (tween != null && tween.IsActive())
                {
                    tween.timeScale = 0.3f;
                }
            }

            directionChangeTween = DOVirtual.DelayedCall(0.5f, () =>
            {
                CleanupTweens();

                clockwise = !clockwise;
                StartCircularMovement();

                foreach (var tween in boxTweens)
                {
                    if (tween != null && tween.IsActive())
                    {
                        tween.timeScale = 1.5f;

                        DOVirtual.DelayedCall(0.3f, () => {
                            if (tween != null && tween.IsActive())
                            {
                                tween.timeScale = 1f;
                            }
                        });
                    }
                }

                isChangingDirection = false;
            });
        }

        private void CleanupTweens()
        {
            foreach (var tween in boxTweens)
            {
                if (tween != null && tween.IsActive())
                {
                    tween.Kill();
                }
            }
            boxTweens.Clear();

            if (directionChangeTween != null && directionChangeTween.IsActive())
            {
                directionChangeTween.Kill();
            }
        }

        public void OnBoxHit(Box hitBox)
        {
            EnableGravityForBox(hitBox);
        }

        public void EnableGravityForBox(Box box)
        {
            var boxDataItem = boxData.Find(data => data.box == box);
            if (boxDataItem != null && !boxDataItem.isGravityEnabled)
            {
                boxDataItem.isGravityEnabled = true;

                if (boxDataItem.movementTween != null && boxDataItem.movementTween.IsActive())
                {
                    boxDataItem.movementTween.Kill();
                }

                EnablePhysicsForBox(box);
            }
        }

        public void EnableGravityForAllBoxes()
        {
            CleanupTweens();

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
            if (box == null) return;

            Rigidbody rb = box.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }

        public void Cleanup()
        {
            CleanupTweens();
            boxData.Clear();
        }
    }
}
