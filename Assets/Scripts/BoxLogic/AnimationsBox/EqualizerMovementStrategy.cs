using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    public class EqualizerMovementStrategy : IMovementStrategy
    {
        private Box[] boxes;
        private Vector3[] initialPositions;
        private List<BoxMovementData> boxData = new List<BoxMovementData>();

        private float[] targetDepths;
        private float[] currentDepths;
        private float[] velocity;
        private float equalizerTimer = 0f;
        private float waveTime = 0f;

        private EqualizerMovementSettings settings;

        public void Initialize(Box[] boxes, Vector3[] initialPositions, MovementStrategySettings settings)
        {
            this.boxes = boxes;
            this.initialPositions = initialPositions;
            this.settings = (EqualizerMovementSettings)settings;

            InitializeBoxData();
            InitializeEqualizer();
        }

        public void Update()
        {
            UpdateEqualizer();
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
                        isGravityEnabled = false
                    });
                }
            }
        }

        private void InitializeEqualizer()
        {
            if (boxes == null || boxes.Length == 0) return;

            targetDepths = new float[boxes.Length];
            currentDepths = new float[boxes.Length];
            velocity = new float[boxes.Length];

            for (int i = 0; i < boxes.Length; i++)
            {
                if (boxes[i] != null)
                {
                    currentDepths[i] = 0f;
                    targetDepths[i] = 0f;
                    velocity[i] = 0f;
                }
            }
        }

        private void UpdateEqualizer()
        {
            waveTime += Time.deltaTime * settings.waveSpeed;
            equalizerTimer += Time.deltaTime;

            if (equalizerTimer >= settings.equalizerUpdateRate)
            {
                equalizerTimer = 0f;
                GenerateWavePattern();
            }

            for (int i = 0; i < boxes.Length; i++)
            {
                if (boxes[i] != null && !boxData[i].isGravityEnabled)
                {
                    currentDepths[i] = Mathf.SmoothDamp(
                        currentDepths[i],
                        targetDepths[i],
                        ref velocity[i],
                        settings.equalizerSmoothTime
                    );

                    UpdateBoxPosition(i);
                }
            }
        }

        private void UpdateBoxPosition(int index)
        {
            Vector3 newPosition = boxData[index].initialPosition;
            Vector3 movementOffset = GetEqualizerMovementDirection() * (currentDepths[index] * settings.equalizerDepth);
            newPosition += movementOffset;

            boxes[index].transform.position = newPosition;
        }

        private void GenerateWavePattern()
        {
            if (targetDepths == null) return;

            for (int i = 0; i < targetDepths.Length; i++)
            {
                float position = (float)i / (boxes.Length - 1);
                float waveValue = CalculateWaveValue(position, waveTime);
                targetDepths[i] = waveValue * settings.waveAmplitude;
            }
        }

        private float CalculateWaveValue(float position, float time)
        {
            float x = position * settings.waveFrequency * Mathf.PI * 2f;
            float t = time * settings.waveFrequency;

            switch (settings.waveType)
            {
                case WaveType.Sine:
                    return (Mathf.Sin(x + t) + 1f) * 0.5f;

                case WaveType.Square:
                    return Mathf.Sin(x + t) > 0 ? 1f : 0f;

                case WaveType.Triangle:
                    float triangle = Mathf.PingPong(x + t, Mathf.PI) / Mathf.PI;
                    return triangle;

                case WaveType.Bounce:
                    float bounce = Mathf.Abs(Mathf.Sin((x + t) * 2f));
                    return bounce * bounce;

                case WaveType.RandomWave:
                    float randomBase = Mathf.PerlinNoise(x * 0.5f, t * 0.3f);
                    float sineMod = Mathf.Sin(x + t) * 0.3f;
                    return Mathf.Clamp01(randomBase + sineMod);

                default:
                    return (Mathf.Sin(x + t) + 1f) * 0.5f;
            }
        }

        private Vector3 GetEqualizerMovementDirection()
        {
            switch (settings.equalizerDirection)
            {
                case EqualizerDirection.ForwardBack: return Vector3.forward;
                case EqualizerDirection.LeftRight: return Vector3.right;
                case EqualizerDirection.UpDown: return Vector3.up;
                default: return Vector3.forward;
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

                int index = System.Array.IndexOf(boxes, box);
                if (index >= 0)
                {
                    targetDepths[index] = 0f;
                    currentDepths[index] = 0f;
                }

                EnablePhysicsForBox(box);
            }
        }

        public void EnableGravityForAllBoxes()
        {
            for (int i = 0; i < boxData.Count; i++)
            {
                if (!boxData[i].isGravityEnabled)
                {
                    boxData[i].isGravityEnabled = true;
                    targetDepths[i] = 0f;
                    currentDepths[i] = 0f;
                    EnablePhysicsForBox(boxData[i].box);
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
            boxData.Clear();
            targetDepths = null;
            currentDepths = null;
            velocity = null;
        }
    }
}