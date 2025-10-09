using System;
using Assets.Scripts.BoosterLogic;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.PlatformLogic
{
    public class PlatformTrigger : MonoBehaviour
    {
        private const int MinCount = 0;

        [SerializeField] private AudioSource _audioSourceBoosterEffect;
        [SerializeField] private ParticleSystem _particleSystemDefult;
        [SerializeField] private ParticleSystem _particleSystemNegative;
        [SerializeField] private ParticleSystem _particleSystemPositiv;

        public event Action CoinTriggered;

        private ParticleSystem _particleSystem;
        private bool _isEnableCollision = true;

        private void OnCollisionEnter(Collision collider)
        {
            if (collider.gameObject.TryGetComponent(out BoosterEffect booster))
            {
                if (_isEnableCollision == false)
                {
                    booster.gameObject.SetActive(false);
                    return;
                }

                if (booster.IsCoin == true)
                    CoinTriggered?.Invoke();

                _audioSourceBoosterEffect.Play();
                _particleSystem = GetParticleSystem(booster);
                _particleSystem.transform.position = collider.contacts[MinCount].point;
                _particleSystem.Play();
                booster.PlayAction();
                booster.gameObject.SetActive(false);
            }
        }

        public void ChangeStateCollision()
        {
            _isEnableCollision = !_isEnableCollision;
        }

        private ParticleSystem GetParticleSystem(BoosterEffect boosterEffect)
        {
            if (boosterEffect.BoosterName == BoosterNames.Default)
                return _particleSystemDefult;

            if (boosterEffect.BoosterName == BoosterNames.Negative)
                return _particleSystemNegative;

            return _particleSystemPositiv;
        }
    }
}