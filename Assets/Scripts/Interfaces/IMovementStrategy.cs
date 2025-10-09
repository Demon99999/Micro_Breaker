using Assets.Scripts.BoxLogic;
using Assets.Scripts.BoxLogic.AnimationsBox;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IMovementStrategy
    {
        void Initialize(Box[] boxes, Vector3[] initialPositions, MovementStrategySettings settings);
        void Update();
        void OnBoxHit(Box hitBox);
        void EnableGravityForBox(Box box);
        void EnableGravityForAllBoxes();
        void Cleanup();
    }
}
