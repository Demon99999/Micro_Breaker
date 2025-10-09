using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    public class ColumnMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public MovementType movementType = MovementType.Linear;

        [Header("Linear Movement Settings")]
        public LinearMovementSettings linearSettings = new LinearMovementSettings();

        [Header("Circular Movement Settings")]
        public CircularMovementSettings circularSettings = new CircularMovementSettings();

        [Header("Equalizer Settings")]
        public EqualizerMovementSettings equalizerSettings = new EqualizerMovementSettings();

        [Header("Pendulum Settings")]
        public PendulumSettings pendulumSettings = new PendulumSettings();

        [Header("Morphing Line Settings")]
        public MorphingLineSettings morphingLineSettings = new MorphingLineSettings();

        [Header("Platform Settings")]
        public FollowPlatformSettings followPlatformSettings = new FollowPlatformSettings();

        [Header("CirclePlatformSettings")]
        public CirclePlatformSettings CirclePlatformSettings = new CirclePlatformSettings();

        [Header("Assigned Boxes")]
        [SerializeField] private Box[] boxesInColumn;

        private IMovementStrategy currentStrategy;
        private List<BoxMovementData> boxMovementData = new List<BoxMovementData>();
        private Vector3[] initialPositions;

        private void Start()
        {
            InitializeBoxes();
            InitializeStrategy();
        }

        private void Update()
        {
            currentStrategy?.Update();
        }

        private void InitializeBoxes()
        {
            if (boxesInColumn == null || boxesInColumn.Length == 0) return;

            initialPositions = new Vector3[boxesInColumn.Length];

            for (int i = 0; i < boxesInColumn.Length; i++)
            {
                if (boxesInColumn[i] != null)
                {
                    initialPositions[i] = boxesInColumn[i].transform.position;

                    var data = new BoxMovementData
                    {
                        box = boxesInColumn[i],
                        initialPosition = initialPositions[i],
                        isGravityEnabled = false
                    };
                    boxMovementData.Add(data);

                    BoxCollisionHandler collisionHandler = boxesInColumn[i].GetComponent<BoxCollisionHandler>();
                    if (collisionHandler == null)
                    {
                        collisionHandler = boxesInColumn[i].gameObject.AddComponent<BoxCollisionHandler>();
                    }
                    collisionHandler.SetColumnController(this);

                    Rigidbody rb = boxesInColumn[i].GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.useGravity = false;
                    }
                }
            }
        }

        private void InitializeStrategy()
        {
            currentStrategy?.Cleanup();

            MovementStrategySettings settings = GetCurrentSettings();

            switch (movementType)
            {
                case MovementType.Linear:
                    currentStrategy = new LinearMovementStrategy();
                    break;
                case MovementType.Circular:
                    currentStrategy = new CircularMovementStrategy();
                    break;
                case MovementType.Equalizer:
                    currentStrategy = new EqualizerMovementStrategy();
                    break;
                case MovementType.Pendulum:
                    currentStrategy = new PendulumMovementStrategy();
                    break;
                case MovementType.MorphingLine:
                    currentStrategy = new MorphingLineMovementStrategy();
                    break;
                case MovementType.FollowPlatform:
                    currentStrategy = new FollowPlatformMovementStrategy();
                    break;
                case MovementType.CirclePlatform:
                    currentStrategy = new CirclePlatformMovementStrategy();
                    break;
            }

            currentStrategy?.Initialize(boxesInColumn, initialPositions, settings);
        }

        private MovementStrategySettings GetCurrentSettings()
        {
            switch (movementType)
            {
                case MovementType.Linear:
                    linearSettings.movementType = MovementType.Linear;
                    return linearSettings;
                case MovementType.Circular:
                    circularSettings.movementType = MovementType.Circular;
                    return circularSettings;
                case MovementType.Equalizer:
                    equalizerSettings.movementType = MovementType.Equalizer;
                    return equalizerSettings;
                case MovementType.Pendulum:
                    pendulumSettings.movementType = MovementType.Pendulum;
                    return pendulumSettings;
                case MovementType.MorphingLine:
                    morphingLineSettings.movementType = MovementType.MorphingLine;
                    return morphingLineSettings;
                case MovementType.FollowPlatform:
                    followPlatformSettings.movementType = MovementType.FollowPlatform;
                    return followPlatformSettings;
                case MovementType.CirclePlatform:
                    CirclePlatformSettings.movementType = MovementType.CirclePlatform;
                    return CirclePlatformSettings;
                default:
                    return linearSettings;
            }
        }

        public void OnBoxHit(Box hitBox)
        {
            currentStrategy?.OnBoxHit(hitBox);
        }

        void OnDestroy()
        {
            currentStrategy?.Cleanup();
        }
    }
}
