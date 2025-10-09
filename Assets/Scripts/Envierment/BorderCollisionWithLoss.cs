using System;
using System.Collections;
using Assets.Scripts.BallLogic;
using Assets.Scripts.BoosterLogic;
using Assets.Scripts.PlatformLogic;
using UnityEngine;

namespace Assets.Scripts.Envierment
{
    [RequireComponent(typeof(Collider))]
    public class BorderCollisionWithLoss : MonoBehaviour
    {
        private const float WaitSeconds = 1.5f;
        private const float MinExtraLive = 0;

        [SerializeField] private Platform _platform;
        [SerializeField] private Ball _ball;
        [SerializeField] private BoostersContainer _boostersContainer;
        [SerializeField] private ParticleSystem _particleSystem;

        public event Action Lost;

        private Collider _collider;
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds;

        public Collider Collider => _collider;

        private void Awake()
        {
            _waitForSeconds = new WaitForSeconds(WaitSeconds);
            _collider = GetComponent<Collider>();

            if (_boostersContainer == null)
                _boostersContainer = FindObjectOfType<BoostersContainer>(true);

            if (_platform == null)
                _platform = FindObjectOfType<Platform>(true);

            if (_ball == null)
                _ball = FindObjectOfType<Ball>(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out AbstractBooster booster))
                booster.gameObject.SetActive(false);

            if (other.TryGetComponent(out Ball ball))
            {
                if (ball.name == _ball.name)
                {
                    SafeResetBoosters();

                    ball.Die();
                    _platform.Die();
                    TryGiveExtraLive();
                    return;
                }

                ball.gameObject.SetActive(false);
            }
        }

        private void SafeResetBoosters()
        {
            try
            {
                if (_boostersContainer != null && _boostersContainer.gameObject.activeInHierarchy)
                {
                    _boostersContainer.Reset();
                }
                else
                {
                    Debug.LogWarning("BoostersContainer not available, searching...");
                    _boostersContainer = FindObjectOfType<BoostersContainer>(true);
                    if (_boostersContainer != null)
                    {
                        _boostersContainer.Reset();
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Exception in SafeResetBoosters: {e}");
            }
        }

        private void TryGiveExtraLive()
        {
            if (_ball == null || _ball.ExtraLive <= MinExtraLive)
            {
                Lost?.Invoke();
                return;
            }

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(TakeExtraLive());
        }

        private IEnumerator TakeExtraLive()
        {
            if (_particleSystem != null)
                _particleSystem.Play();

            yield return _waitForSeconds;

            if (_platform != null && _ball != null)
            {
                _platform.GiveLive();
                _ball.GiveLive();
            }

            if (_particleSystem != null)
                _particleSystem.Stop();

            _coroutine = null;
        }
    }
}