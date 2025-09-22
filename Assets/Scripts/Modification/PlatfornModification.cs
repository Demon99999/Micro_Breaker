using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Modification
{
    public class PlatfornModification : ObjectModification
    {
        private const float FirstUpgradeValue = 0.1f;

        [SerializeField] private ParticleSystem[] _particleSystemsPositove;
        [SerializeField] private ParticleSystem[] _particleSystemsNegative;

        public override void SetNewScale(BoosterNames boosterNames, bool isSetBooster)
        {
            ChangeScale(new Vector3(Transform.localScale.x * GetScaleValue(boosterNames), Transform.localScale.y, Transform.localScale.z));
            PlayParticle(boosterNames);
        }

        public override void SetDefultScale(bool isSetBooster)
        {
            ChangeScale(new Vector3(DefultScaleValue, DefultScaleValue, DefultScaleValue));
        }

        public void SetAdditionalImprovementScale(float scale) =>
            ChangeScale(new Vector3(Transform.localScale.x + (scale * FirstUpgradeValue), Transform.localScale.y, Transform.localScale.z));

        private void PlayParticle(BoosterNames boosterNames)
        {
            ParticleSystem[] currentParticleSystems = GetParticleSystemsState(boosterNames);

            if (currentParticleSystems == null)
                return;

            foreach (var particle in GetParticleSystemsState(boosterNames))
                particle.Play();
        }

        private ParticleSystem[] GetParticleSystemsState(BoosterNames boosterNames)
        {
            if (boosterNames == BoosterNames.Positive)
                return _particleSystemsPositove;

            if (boosterNames == BoosterNames.Negative)
                return _particleSystemsNegative;

            return null;
        }
    }
}