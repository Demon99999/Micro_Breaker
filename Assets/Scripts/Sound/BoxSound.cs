using Assets.Scripts.BoosterLogic;
using Assets.Scripts.BoosterLogic.Boosters;
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
        private bool _isInitialized = false;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _locationCreate.Inited += OnInit;
        }

        private void OnDisable()
        {
            _locationCreate.Inited -= OnInit;

            if (_isInitialized && _boxContainer != null)
            {
                foreach (var box in _boxContainer.Boxes)
                {
                    box.Damaged -= OnPlay;
                }
            }

            if (_boxesFalling != null)
            {
                foreach (var box in _boxesFalling.Boxes)
                {
                    box.Damaged -= OnPlay;
                }
            }

            _isInitialized = false;
        }

        private void OnInit(Location location)
        {
            if (location == null) return;

            _boxContainer = location.BoxContainer;

            // Проверяем все необходимые компоненты
            if (_boxContainer == null || _boxesFalling == null)
            {
                Debug.LogError("BoxSound: Missing required components!");
                return;
            }

            foreach (var box in _boxContainer.Boxes)
            {
                box.Damaged += OnPlay;
            }

            //foreach (var box in _boxesFalling.Boxes)
            //{
            //    box.Damaged += OnPlay;
            //}

            _isInitialized = true;
        }

        private void OnPlay(AudioClip audioClip)
        {
            if (_audioSource == null || _audioSource.enabled == false || audioClip == null)
                return;

            _audioSource.clip = audioClip;
            _audioSource.Play();
        }
    }
}