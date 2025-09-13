using UnityEngine;

namespace Assets.Scripts.PlatformLogic
{
    public class Restrictor
    {
        public void RestrictMove(Transform transform, float clampX, float clampYMin, float clampYMax)
        {
            float positionX = Mathf.Clamp(transform.position.x, -clampX, clampX);
            float positionY = Mathf.Clamp(transform.position.z, clampYMin, clampYMax);
            Vector3 clampPosition = new Vector3(positionX, transform.position.y, positionY);
            transform.position = clampPosition;
        }
    }
}