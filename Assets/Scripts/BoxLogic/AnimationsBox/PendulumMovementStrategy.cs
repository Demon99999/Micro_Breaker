using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    public class PendulumMovementStrategy : IMovementStrategy
    {
        private Box[] boxes;
        private Vector3[] initialPositions;
        private List<BoxMovementData> boxData = new List<BoxMovementData>();
        private PendulumSettings settings;
        private Vector3 pivotPoint;
        private float currentTiltAngle = 0f;
        private bool isTiltingRight = true;
        private Tween tiltTween;
        private bool isInitialized = false;
        private bool isGravityEnabledForAll = false;

        public void Initialize(Box[] boxes, Vector3[] initialPositions, MovementStrategySettings settings)
        {
            this.boxes = boxes;
            this.initialPositions = initialPositions;
            this.settings = (PendulumSettings)settings;

            CalculatePivotPoint();
            InitializeBoxData();
            StartTiltingMovement();

            isInitialized = true;
        }

        public void Update()
        {
            if (isGravityEnabledForAll) return;
            UpdateColumnTilt();
        }

        private void CalculatePivotPoint()
        {
            if (boxes == null || boxes.Length == 0) return;

            float minY = float.MaxValue;
            foreach (var box in boxes)
            {
                if (box != null && box.transform.position.y < minY)
                {
                    minY = box.transform.position.y;
                    pivotPoint = box.transform.position;
                }
            }
        }

        private void InitializeBoxData()
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                if (boxes[i] != null)
                {
                    Rigidbody rb = boxes[i].GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.useGravity = false;
                    }

                    boxData.Add(new BoxMovementData
                    {
                        box = boxes[i],
                        initialPosition = boxes[i].transform.position,
                        isGravityEnabled = false
                    });
                }
            }
        }

        private void StartTiltingMovement()
        {
            if (isGravityEnabledForAll) return;
            TiltToDirection();
        }

        private void TiltToDirection()
        {
            if (isGravityEnabledForAll) return;

            float targetAngle = isTiltingRight ? settings.swingAngle : -settings.swingAngle;

            tiltTween = DOTween.To(() => currentTiltAngle, x =>
            {
                currentTiltAngle = x;
            }, targetAngle, settings.swingDuration);

            if (settings.usePhysicsCurve)
            {
                tiltTween.SetEase(Ease.InOutSine);
            }
            else
            {
                tiltTween.SetEase(settings.swingCurve);
            }

            tiltTween.OnComplete(() =>
            {
                if (isGravityEnabledForAll) return;

                DOVirtual.DelayedCall(settings.pauseBetweenSwings, () =>
                {
                    isTiltingRight = !isTiltingRight;
                    TiltToDirection();
                });
            });
        }

        private void UpdateColumnTilt()
        {
            if (isGravityEnabledForAll) return;

            Quaternion tiltRotation = Quaternion.Euler(0, 0, currentTiltAngle);

            for (int i = 0; i < boxData.Count; i++)
            {
                if (boxData[i].box != null && !boxData[i].isGravityEnabled)
                {
                    UpdateBoxTiltPosition(boxData[i], tiltRotation);
                }
            }
        }

        private void UpdateBoxTiltPosition(BoxMovementData data, Quaternion tiltRotation)
        {
            Vector3 offsetFromPivot = data.initialPosition - pivotPoint;
            Vector3 rotatedOffset = tiltRotation * offsetFromPivot;
            Vector3 newPosition = pivotPoint + rotatedOffset;
            data.box.transform.position = newPosition;
            data.box.transform.rotation = tiltRotation;
        }

        public void OnBoxHit(Box hitBox)
        {
            EnableGravityForAllBoxes();
        }

        public void EnableGravityForBox(Box box)
        {
            EnableGravityForAllBoxes();
        }

        public void EnableGravityForAllBoxes()
        {
            if (isGravityEnabledForAll) return;

            isGravityEnabledForAll = true;

            if (tiltTween != null && tiltTween.IsActive())
            {
                tiltTween.Kill();
            }

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

                Vector3 randomForce = new Vector3(
                    Random.Range(-5f, 5f),
                    Random.Range(3f, 8f),
                    Random.Range(-3f, 3f)
                );
                rb.AddForce(randomForce, ForceMode.Impulse);

                Vector3 randomTorque = new Vector3(
                    Random.Range(-10f, 10f),
                    Random.Range(-10f, 10f),
                    Random.Range(-10f, 10f)
                );
                rb.AddTorque(randomTorque, ForceMode.Impulse);
            }
        }

        public void Cleanup()
        {
            if (tiltTween != null && tiltTween.IsActive())
            {
                tiltTween.Kill();
            }
            boxData.Clear();
        }
    }
}
