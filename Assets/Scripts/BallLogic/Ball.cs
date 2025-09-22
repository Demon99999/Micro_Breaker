using System;
using Assets.Scripts.Enums;
using Assets.Scripts.PlayerLogic;
using Assets.Scripts.Shops;
using UnityEngine;

namespace Assets.Scripts.BallLogic
{
    [RequireComponent(typeof(Rigidbody), typeof(BallEffect))]
    public class Ball : MonoBehaviour
    {
        private const int MinExtraLive = 0;
        private const int MaxExtraLive = 9;

        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Transform _ballPoint;
        [SerializeField] private ChangeTemplate _changeTemplate;

        public event Action Actived;
        public event Action<int> ExtraLiveChanged;

        private Transform _transform;

        public ChangeTemplate ChangeTemplate => _changeTemplate;

        public Rigidbody Rigidbody { get; private set; }

        public BallEffect BallEffect { get; private set; }

        public int ExtraLive { get; private set; } = MinExtraLive;

        public bool IsActive { get; private set; } = false;

        private void Awake()
        {
            _transform = transform;
            Rigidbody = GetComponent<Rigidbody>();
            BallEffect = GetComponent<BallEffect>();
            Rigidbody.isKinematic = true;
        }

        private void OnEnable()
        {
            _playerInput.MousePressedUp += OnDisconnectParentObject;
        }

        private void OnDisable()
        {
            _playerInput.MousePressedUp -= OnDisconnectParentObject;
        }

        public void AddExtraLive(int extraLive)
        {
            if (extraLive < MinExtraLive && extraLive > MaxExtraLive)
                return;

            ExtraLive = extraLive;
            ExtraLiveChanged?.Invoke(ExtraLive);
        }

        public void GiveLive()
        {
            ExtraLive--;
            ExtraLiveChanged?.Invoke(ExtraLive);
            _changeTemplate.gameObject.SetActive(true);
        }

        public void Die()
        {
            IsActive = false;
            BallEffect.CurrentParticleSystem.Stop();
            _changeTemplate.gameObject.SetActive(false);
            _transform.position = _ballPoint.position;
            Rigidbody.isKinematic = true;
        }

        public void OnDisconnectParentObject()
        {
            if (IsActive)
                return;

            IsActive = true;
            BallEffect.SetParticleSystem(BoosterNames.Default);
            Rigidbody.isKinematic = false;
            Actived?.Invoke();
        }
    }
}