using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Modification
{
    public abstract class ObjectModification : MonoBehaviour
    {
        private const float NegativeScaleValue = 0.8f;
        private const float PositiveScaleValue = 1.2f;

        public float DefultScaleValue { get; private set; }

        public Transform Transform { get; private set; }

        private void Awake()
        {
            Transform = transform;
            DefultScaleValue = Transform.localScale.x;
        }

        public abstract void SetNewScale(BoosterNames boosterNames, bool isSetBooster);

        public abstract void SetDefultScale(bool isSetBooster);

        protected float GetScaleValue(BoosterNames boosterNames)
        {
            if (boosterNames == BoosterNames.Negative) return NegativeScaleValue;

            return PositiveScaleValue;
        }

        protected void ChangeScale(Vector3 scale) => Transform.localScale = scale;
    }
}