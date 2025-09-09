using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    public class SoundAction : MonoBehaviour
    {
        private const float AudioSourceState = 0;

        [SerializeField] private AudioSource _audioSourceMusic;
        [SerializeField] private AudioSource[] _audioSources;

        private void Awake()
        {
            float currentVolumeMusic = PlayerPrefs.GetFloat(AudioName.Music.ToString());
            _audioSourceMusic.enabled = true ? currentVolumeMusic > AudioSourceState : currentVolumeMusic == AudioSourceState;
            float currentValue = PlayerPrefs.GetFloat(AudioName.Effect.ToString());

            foreach (AudioSource audioSource in _audioSources)
                audioSource.enabled = true ? currentValue > AudioSourceState : currentValue == AudioSourceState;
        }
    }
}