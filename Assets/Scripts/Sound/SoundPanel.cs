using System.Collections.Generic;
using Assets.Scripts.UI.Panels;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPanel : MonoBehaviour
    {
        [SerializeField] private List<Panel> _panels;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            foreach (var panel in _panels) panel.Moved += OnPlay;
        }

        private void OnDisable()
        {
            foreach (var panel in _panels) panel.Moved -= OnPlay;
        }

        private void OnPlay()
        {
            if (_audioSource.enabled == false)
                return;

            _audioSource.Play();
        }
    }
}