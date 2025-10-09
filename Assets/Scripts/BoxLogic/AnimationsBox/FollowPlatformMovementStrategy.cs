using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    public class FollowPlatformMovementStrategy : IMovementStrategy
    {
        private Box[] boxes;
        private Vector3[] initialPositions;
        private List<BoxMovementData> boxData = new List<BoxMovementData>();
        private FollowPlatformSettings settings;
        private Transform platform;
        private bool isInitialized = false;
        private Vector3 platformStartPosition;
        private Vector3[] boxOffsets;

        public void Initialize(Box[] boxes, Vector3[] initialPositions, MovementStrategySettings settings)
        {
            this.boxes = boxes;
            this.initialPositions = initialPositions;
            this.settings = (FollowPlatformSettings)settings;

            InitializeBoxData();
            FindPlatform();

            if (platform != null)
            {
                platformStartPosition = platform.position;
                CalculateBoxOffsets();
            }

            isInitialized = true;
        }

        public void Update()
        {
            if (!isInitialized || platform == null) return;

            MoveBoxesWithPlatform();
        }

        private void InitializeBoxData()
        {
            boxData.Clear();

            for (int i = 0; i < boxes.Length; i++)
            {
                if (boxes[i] != null)
                {
                    boxData.Add(new BoxMovementData
                    {
                        box = boxes[i],
                        initialPosition = initialPositions[i],
                        isGravityEnabled = false,
                        isFollowing = false,
                        targetPosition = Vector3.zero
                    });
                }
            }
        }

        private void FindPlatform()
        {
            GameObject platformObj = GameObject.FindGameObjectWithTag("Platform");
            if (platformObj != null)
            {
                platform = platformObj.transform;
                return;
            }

            if (boxData.Count > 0 && boxData[0].box != null)
            {
                Collider[] colliders = Physics.OverlapSphere(boxData[0].box.transform.position, settings.detectionRadius, settings.platformLayerMask);
                if (colliders.Length > 0)
                {
                    platform = colliders[0].transform;
                }
            }
        }

        private void CalculateBoxOffsets()
        {
            boxOffsets = new Vector3[boxData.Count];
            for (int i = 0; i < boxData.Count; i++)
            {
                if (boxData[i].box != null)
                {
                    float randomX = Random.Range(-settings.horizontalSpacing, settings.horizontalSpacing);
                    float randomZ = Random.Range(-settings.horizontalSpacing * 0.5f, settings.horizontalSpacing * 0.5f);

                    boxOffsets[i] = new Vector3(randomX, settings.verticalOffset, randomZ);
                }
            }
        }

        private void MoveBoxesWithPlatform()
        {
            if (platform == null) return;

            Vector3 platformMovement = platform.position - platformStartPosition;

            Vector3 dampedMovement = platformMovement * settings.movementDamping;

            for (int i = 0; i < boxData.Count; i++)
            {
                var data = boxData[i];
                if (data.box != null && !data.isGravityEnabled)
                {
                    Vector3 targetPosition = data.initialPosition + dampedMovement + boxOffsets[i];

                    data.box.transform.position = Vector3.Lerp(
                        data.box.transform.position,
                        targetPosition,
                        settings.followSpeed * Time.deltaTime
                    );

                    float sway = Mathf.Sin(Time.time * 2f + i) * 0.03f;
                    data.box.transform.rotation = Quaternion.Euler(sway, 0f, sway);
                }
            }
        }

        public void OnBoxHit(Box hitBox)
        {
            var data = boxData.Find(d => d.box == hitBox);
            if (data != null && !data.isGravityEnabled)
            {
                hitBox.transform.DOShakePosition(0.3f, 0.2f, 10, 90f);
            }
        }

        public void EnableGravityForBox(Box box)
        {
            var data = boxData.Find(d => d.box == box);
            if (data != null && !data.isGravityEnabled)
            {
                data.isGravityEnabled = true;
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
            foreach (var data in boxData)
            {
                if (data.box != null)
                {
                    data.box.transform.DOKill();
                }
            }

            boxData.Clear();
            isInitialized = false;
        }
    }
}