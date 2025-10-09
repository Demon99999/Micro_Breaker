using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    public class MorphingLineMovementStrategy : IMovementStrategy
    {
        private Box[] boxes;
        private Vector3[] initialPositions;
        private List<BoxMovementData> boxData = new List<BoxMovementData>();
        private MorphingLineSettings settings;
        private Vector3 formationCenter;
        private bool isInitialized = false;
        private bool isMorphing = false;
        private float morphTimer = 0f;
        private bool isFolded = false;

        public void Initialize(Box[] boxes, Vector3[] initialPositions, MovementStrategySettings settings)
        {
            this.boxes = boxes;
            this.initialPositions = initialPositions;
            this.settings = (MorphingLineSettings)settings;

            CalculateFormationCenter();
            InitializeBoxData();
            StartMorphing();

            isInitialized = true;
        }

        public void Update()
        {
            if (!isInitialized || isMorphing) return;

            morphTimer += Time.deltaTime;

            if (!isFolded && morphTimer >= settings.morphDuration)
            {
                morphTimer = 0f;
                StartFoldAnimation();
            }
            else if (isFolded && morphTimer >= settings.pauseBetweenMorphs)
            {
                morphTimer = 0f;
                StartUnfoldAnimation();
            }
        }

        private void CalculateFormationCenter()
        {
            formationCenter = Vector3.zero;
            int count = 0;

            foreach (var box in boxes)
            {
                if (box != null)
                {
                    formationCenter += box.transform.position;
                    count++;
                }
            }

            formationCenter /= count;
        }

        private void InitializeBoxData()
        {
            List<Box> sortedBoxes = new List<Box>();
            List<Vector3> sortedPositions = new List<Vector3>();

            for (int i = 0; i < boxes.Length; i++)
            {
                if (boxes[i] != null)
                {
                    sortedBoxes.Add(boxes[i]);
                    sortedPositions.Add(initialPositions[i]);
                }
            }

            if (settings.snakeType == SnakeType.Left)
            {
                // Ћева€ зме€: сортируем по убыванию X (голова справа)
                for (int i = 0; i < sortedBoxes.Count - 1; i++)
                {
                    for (int j = i + 1; j < sortedBoxes.Count; j++)
                    {
                        if (sortedBoxes[i].transform.position.x < sortedBoxes[j].transform.position.x)
                        {
                            // swap
                            var tempBox = sortedBoxes[i];
                            sortedBoxes[i] = sortedBoxes[j];
                            sortedBoxes[j] = tempBox;

                            var tempPos = sortedPositions[i];
                            sortedPositions[i] = sortedPositions[j];
                            sortedPositions[j] = tempPos;
                        }
                    }
                }
            }
            else
            {
                // ѕрава€ зме€: сортируем по возрастанию X (голова слева)
                for (int i = 0; i < sortedBoxes.Count - 1; i++)
                {
                    for (int j = i + 1; j < sortedBoxes.Count; j++)
                    {
                        if (sortedBoxes[i].transform.position.x > sortedBoxes[j].transform.position.x)
                        {
                            // swap
                            var tempBox = sortedBoxes[i];
                            sortedBoxes[i] = sortedBoxes[j];
                            sortedBoxes[j] = tempBox;

                            var tempPos = sortedPositions[i];
                            sortedPositions[i] = sortedPositions[j];
                            sortedPositions[j] = tempPos;
                        }
                    }
                }
            }

            boxData.Clear();
            for (int i = 0; i < sortedBoxes.Count; i++)
            {
                boxData.Add(new BoxMovementData
                {
                    box = sortedBoxes[i],
                    initialPosition = sortedPositions[i],
                    isGravityEnabled = false
                });
            }

            CalculateFormationCenter();

        }

        private void StartMorphing()
        {
            isFolded = false;
            morphTimer = 0f;
            UpdateLinePositions();
        }

        private void StartFoldAnimation()
        {
            isMorphing = true;
            float foldDuration = settings.morphDuration;

            int completedAnimations = 0;
            int totalAnimations = boxData.Count;

            for (int i = 0; i < boxData.Count; i++)
            {
                if (boxData[i].box != null && !boxData[i].isGravityEnabled)
                {
                    // √олова (первый куб) остаетс€ на месте
                    if (i == 0)
                    {
                        completedAnimations++;
                        if (completedAnimations >= totalAnimations)
                        {
                            isMorphing = false;
                            isFolded = true;
                            morphTimer = 0f;
                        }
                        continue;
                    }

                    Vector3 letterCPosition = CalculateLetterCPosition(i);
                    float delay = CalculateFoldDelayFromHead(i, foldDuration);

                    boxData[i].box.transform.DOMove(letterCPosition, foldDuration * 0.8f)
                        .SetDelay(delay)
                        .SetEase(settings.morphCurve)
                        .OnComplete(() =>
                        {
                            completedAnimations++;
                            if (completedAnimations >= totalAnimations)
                            {
                                isMorphing = false;
                                isFolded = true;
                                morphTimer = 0f;
                            }
                        });

                    float zRotation = CalculateZRotation(i);
                    Vector3 targetRotation = new Vector3(0, 0, zRotation);
                    boxData[i].box.transform.DORotate(targetRotation, foldDuration * 0.8f)
                        .SetDelay(delay)
                        .SetEase(settings.morphCurve);
                }
            }
        }

        private void StartUnfoldAnimation()
        {
            isMorphing = true;
            float unfoldDuration = settings.morphDuration;

            int completedAnimations = 0;
            int totalAnimations = boxData.Count;

            for (int i = 0; i < boxData.Count; i++)
            {
                if (boxData[i].box != null && !boxData[i].isGravityEnabled)
                {
                    if (i == 0)
                    {
                        completedAnimations++;
                        if (completedAnimations >= totalAnimations)
                        {
                            isMorphing = false;
                            isFolded = false;
                            morphTimer = 0f;
                        }
                        continue;
                    }

                    Vector3 linePosition = CalculateLinePosition(i);
                    float delay = CalculateUnfoldDelayFromHead(i, unfoldDuration);

                    boxData[i].box.transform.DOMove(linePosition, unfoldDuration * 0.8f)
                        .SetDelay(delay)
                        .SetEase(settings.morphCurve)
                        .OnComplete(() =>
                        {
                            completedAnimations++;
                            if (completedAnimations >= totalAnimations)
                            {
                                isMorphing = false;
                                isFolded = false;
                                morphTimer = 0f;
                            }
                        });

                    boxData[i].box.transform.DORotate(Vector3.zero, unfoldDuration * 0.8f)
                        .SetDelay(delay)
                        .SetEase(settings.morphCurve);
                }
            }
        }

        private float CalculateFoldDelayFromHead(int index, float totalDuration)
        {
            float distanceFromHead = index;
            float maxDelay = totalDuration * 0.6f;
            return (distanceFromHead / (boxData.Count - 1)) * maxDelay;
        }

        private float CalculateUnfoldDelayFromHead(int index, float totalDuration)
        {
            float distanceFromHead = index;
            float maxDelay = totalDuration * 0.6f;
            return ((boxData.Count - 1 - distanceFromHead) / (boxData.Count - 1)) * maxDelay;
        }

        private Vector3 CalculateLetterCPosition(int index)
        {
            Vector3 headPosition = boxData[0].box.transform.position;

            if (index == 0) return headPosition;

            if (settings.snakeType == SnakeType.Left)
            {
                Vector3[] leftPositions = new Vector3[]
                {
                    new Vector3(-1.59f, 0f, 0f),      
                    new Vector3(-3.3f, 0.79f, 0f),    
                    new Vector3(-4.4f, 2.46f, 0f), 
                    new Vector3(-4.52f, 4.34f, 0f),
                    new Vector3(-3.35f, 6.1f, 0f),
                    new Vector3(-1.59f, 6.67f, 0f)
                };

                if (index < leftPositions.Length)
                {
                    float headX = leftPositions[0].x;
                    Vector3 offset = leftPositions[index] - new Vector3(headX, 0f, 0f);
                    return headPosition + offset;
                }
            }
            else
            {
                Vector3[] rightPositions = new Vector3[]
                {
                    new Vector3(0f, 0f, 0f),          
                    new Vector3(1.62f, 0.86f, 0f),    
                    new Vector3(2.74f, 2.36f, 0f),
                    new Vector3(2.94f, 4.17f, 0f),
                    new Vector3(1.98f, 5.96f, 0f),
                    new Vector3(0f, 6.67f, 0f)
                };

                if (index < rightPositions.Length)
                {
                    return headPosition + rightPositions[index];
                }
            }

            return headPosition;
        }

        private float CalculateZRotation(int index)
        {
            if (index == 0) return 0f;

            if (settings.snakeType == SnakeType.Left)
            {
                float[] leftRotations = new float[] { 0f, -40f, -65f, -95f, -60f, -90f };
                if (index < leftRotations.Length) return leftRotations[index];
            }
            else
            {
                float[] rightRotations = new float[] { 0f, 40f, 65f, 95f, 60f, 90f };
                if (index < rightRotations.Length) return rightRotations[index];
            }

            return 0f;
        }

        private void UpdateLinePositions()
        {
            for (int i = 0; i < boxData.Count; i++)
            {
                if (boxData[i].box != null && !boxData[i].isGravityEnabled)
                {
                    Vector3 linePosition = CalculateLinePosition(i);
                    boxData[i].box.transform.position = linePosition;
                    boxData[i].box.transform.rotation = Quaternion.identity;
                }
            }
        }

        private Vector3 CalculateLinePosition(int index)
        {
            return boxData[index].initialPosition;
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
            foreach (var data in boxData)
            {
                if (data.box != null)
                {
                    data.box.transform.DOKill();
                }
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
                    Random.Range(-3f, 3f),
                    Random.Range(2f, 5f),
                    Random.Range(-2f, 2f)
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
        }
    }
}