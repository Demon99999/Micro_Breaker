using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    public class CirclePlatformMovementStrategy : IMovementStrategy
    {
        private Box[] boxes;
        private Vector3[] initialPositions;
        private List<BoxMovementData> boxData = new List<BoxMovementData>();
        private Vector3 circleCenter;
        private CirclePlatformSettings settings;
        private Transform platform;
        private Vector3 platformStartPosition;
        private float radiusMultiplier = 0.94f;

        public void Initialize(Box[] boxes, Vector3[] initialPositions, MovementStrategySettings settings)
        {
            this.boxes = boxes;
            this.initialPositions = initialPositions;
            this.settings = (CirclePlatformSettings)settings;

            CalculateCircleCenter();
            InitializeBoxData();
            FindPlatform();

            if (platform != null)
            {
                platformStartPosition = platform.position;
            }
        }

        public void Update()
        {
            if (platform == null) return;

            RotateCircleWithPlatform();
            UpdateBoxesPositions();
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
            return Mathf.Atan2(direction.z, direction.x) * Mathf.Deg2Rad;
        }

        private void FindPlatform()
        {
            GameObject platformObj = GameObject.FindGameObjectWithTag("Platform");
            if (platformObj != null)
            {
                platform = platformObj.transform;
            }
        }

        private void RotateCircleWithPlatform()
        {
            Vector3 platformMovement = platform.position - platformStartPosition;

            Vector3 horizontalMovement = new Vector3(platformMovement.x, 0f, platformMovement.z);

            if (horizontalMovement.magnitude > 0.1f)
            {
                float platformAngle = Mathf.Atan2(horizontalMovement.x, horizontalMovement.z) * Mathf.Rad2Deg;

                float targetAngle = platformAngle * settings.movementDamping;

                RotateAllBoxes(targetAngle);
            }
        }

        private void RotateAllBoxes(float targetAngle)
        {
            foreach (var data in boxData)
            {
                if (data.box != null && !data.isGravityEnabled)
                {
                    Vector3 toInitialPosition = data.initialPosition - circleCenter;
                    toInitialPosition *= radiusMultiplier;
                    Vector3 rotatedPosition = RotateAroundY(toInitialPosition, targetAngle);
                    Vector3 targetPosition = circleCenter + rotatedPosition;

                    data.box.transform.position = Vector3.Lerp(
                        data.box.transform.position,
                        targetPosition,
                        settings.followSpeed * Time.deltaTime
                    );

                    UpdateCubeOrientation(data);
                }
            }
        }

        private Vector3 RotateAroundY(Vector3 point, float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            return new Vector3(
                point.x * cos - point.z * sin,
                point.y,
                point.x * sin + point.z * cos
            );
        }

        private void UpdateBoxesPositions()
        {
            foreach (var data in boxData)
            {
                if (data.box != null && !data.isGravityEnabled)
                {
                    UpdateCubeOrientation(data);
                }
            }
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
                    Time.deltaTime * settings.followSpeed
                );
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
                EnablePhysicsForBox(box);
            }
        }

        public void EnableGravityForAllBoxes()
        {
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
                    Random.Range(-2f, 2f),
                    Random.Range(3f, 6f),
                    Random.Range(-1f, 1f)
                );
                rb.AddForce(randomForce, ForceMode.Impulse);
            }
        }

        public void Cleanup()
        {
            boxData.Clear();
        }
    }
}