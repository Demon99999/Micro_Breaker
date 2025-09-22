using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.BallLogic
{
    public class BallEffect : MonoBehaviour, IEffect
    {
        [SerializeField] private ParticleSystem _effectDefult;
        [SerializeField] private ParticleSystem _effectNegayive;
        [SerializeField] private ParticleSystem _effectPositive;

        public ParticleSystem CurrentParticleSystem { get; private set; }

        private void Awake()
        {
            _effectDefult.Stop();
            _effectNegayive.Stop();
            _effectPositive.Stop();
            CurrentParticleSystem = _effectDefult;
        }

        public void Play(Vector3 point) { }

        public void RotateTarget(Vector3 direction) => transform.LookAt(-direction);

        public void SetParticleSystem(BoosterNames boosterNames)
        {
            CurrentParticleSystem.Stop();

            if (boosterNames == BoosterNames.Default)
                CurrentParticleSystem = _effectDefult;

            if (boosterNames == BoosterNames.Negative)
                CurrentParticleSystem = _effectNegayive;

            if (boosterNames == BoosterNames.Positive)
                CurrentParticleSystem = _effectPositive;

            CurrentParticleSystem.Play();
        }
    }
}