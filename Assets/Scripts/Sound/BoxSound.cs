using Assets.Scripts.BoosterLogic;
using Assets.Scripts.BoxLogic;
using Assets.Scripts.LocationLogic;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class BoxSound : MonoBehaviour
    {
        [SerializeField] private LocationCreate _locationCreate;
        [SerializeField] private BoxesFalling _boxesFalling;

        private AudioSource _audioSource;
        private BoxContainer _boxContainer;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _locationCreate.Inited += OnInit;
            _locationCreate.Inited += OnInit =>
            {
                foreach (var box in _boxContainer.Boxes) box.Damaged += OnPlay;
                foreach (var box in _boxesFalling.Boxes) box.Damaged += OnPlay;
            };
        }

        private void OnDisable()
        {
            _locationCreate.Inited -= OnInit;
            _locationCreate.Inited -= OnInit =>
            {
                foreach (var box in _boxContainer.Boxes) box.Damaged -= OnPlay;
                foreach (var box in _boxesFalling.Boxes) box.Damaged -= OnPlay;
            };
        }

        private void OnInit(Location location)
        {
            _boxContainer = location.BoxContainer;
        }

        private void OnPlay(AudioClip audioClip)
        {
            if (_audioSource.enabled == false)
                return;

            _audioSource.clip = audioClip;
            _audioSource.Play();
        }
    }
}