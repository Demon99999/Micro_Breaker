using UnityEngine;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class BallSound : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Play(AudioClip audioClip)
        {
            if (_audioSource.enabled == false)
                return;

            _audioSource.clip = audioClip;
            _audioSource.Play();
        }
    }
}