using System;
using Assets.Scripts.BallLogic;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Envierment
{
    public class Fence : MonoBehaviour, ITrigger, ISound
    {
        [SerializeField] private ParticleSystem _particleTrigger;
        [SerializeField] private ParticleSystem _particlePortal;
        [SerializeField] private float _speed;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private AudioClip _audioClipPortal;
        [SerializeField] private Transform _portalPoint;
        [SerializeField] private Fence _parallelFence;
        [SerializeField] private bool _isHorizontal;
        [SerializeField] private bool _isOpenPortal = false;

        public event Action<BallMovement, Vector3, Vector3, string> PortalMoved;

        private ParticleSystem _particleSystem;

        public bool IsOpenPortal => _isOpenPortal;

        public Transform PortalPoint => _portalPoint;

        public Fence ParallelFence => _parallelFence;

        public bool IsHorizontal => _isHorizontal;

        public AudioClip GetClip()
        {
            if (_isOpenPortal)
                return _audioClipPortal;

            return _audioClip;
        }

        public void Relocate(BallMovement ballMovement, Vector3 point, Vector3 direction)
        {
            PortalMoved?.Invoke(ballMovement, point, direction, this.name);
        }

        public void Play(Vector3 point)
        {
            if (_isOpenPortal)
                _particleSystem = _particlePortal;
            else
                _particleSystem = _particleTrigger;

            _particleSystem.transform.position = point;
            _particleSystem.Play();
        }

        public void ActivePortal(bool isPortal) => _isOpenPortal = isPortal;
    }
}