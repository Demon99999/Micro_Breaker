using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] private List<Button> _buttons;

        private AudioSource _audioSource;

        private void Awake() => _audioSource = GetComponent<AudioSource>();

        public AudioSource AudioSource => _audioSource;

        private void OnEnable()
        {
            foreach (var button in _buttons) button.onClick.AddListener(OnPlay);
        }

        private void OnDisable()
        {
            foreach (var button in _buttons) button.onClick.RemoveListener(OnPlay);
        }

        public void OnPlay()
        {
            if (_audioSource.enabled == false) return;

            _audioSource.Play();
        }
    }
}