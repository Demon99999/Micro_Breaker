using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.PlatformLogic
{
    [RequireComponent(typeof(Collider))]
    public class Platform : MonoBehaviour, ITrigger, ISound
    {
        [SerializeField] private InputPointMovement _inputPointMovement;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private ParticleSystem _particleExplosion;
        [SerializeField] private Transform _transformTemplateContainer;
        [SerializeField] private AudioSource _audioSourceEffect;
        [SerializeField] private AudioClip _audioClipExplosion;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private float _speedForceBall;

        private Collider _collider;

        private void Start()
        {
            SetStartState();
        }

        public void Play(Vector3 point)
        {
            _particleSystem.transform.position = point;
            _particleSystem.Play();
        }

        public void GiveLive()
        {
            SetActive(true);
        }

        public void Die()
        {
            _particleExplosion.transform.position = transform.position;
            _particleExplosion.Play();
            SetStartState();
            SetActive(false);
            _audioSourceEffect.clip = _audioClipExplosion;

            if (_audioSourceEffect.enabled == false)
                return;

            _audioSourceEffect.Play();
        }

        private void SetActive(bool isActive)
        {
            _collider.enabled = isActive;
            _transformTemplateContainer.gameObject.SetActive(isActive);
            _inputPointMovement.SetActiveInputObject(isActive);
        }

        public AudioClip GetClip()
        {
            return _audioClip;
        }

        private void SetStartState()
        {
            _collider = GetComponent<Collider>();
            transform.position = _startPoint.position;
            _inputPointMovement.transform.position = _startPoint.position;
        }
    }
}