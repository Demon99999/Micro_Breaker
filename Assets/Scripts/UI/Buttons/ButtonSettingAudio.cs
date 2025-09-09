using System;
using Assets.Scripts.Enums;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Buttons
{
    public class ButtonSettingAudio : AbstractButton
    {
        private const string StateEnable = "On";
        private const string StateDisable = "Off";

        [SerializeField] private TMP_Text _text;
        [SerializeField] private AudioName _audioName;
        [SerializeField] private AudioSource[] _audioSources;
        [SerializeField] private float _maxVolume;

        public event Action<ButtonSettingAudio> Changed;

        public bool IsEnable;

        public AudioName AudioName => _audioName;

        public AudioSource[] AudioSources => _audioSources;

        public float MaxVolume => _maxVolume;

        protected override void OnClick()
        {
            IsEnable = !IsEnable;
            SetParameters(IsEnable);
            Changed?.Invoke(this);
        }

        public void Init(bool isEnable)
        {
            IsEnable = isEnable;
            SetParameters(isEnable);
        }

        public void SetParameters(bool isEnable)
        {
            foreach (var audioSource in _audioSources)
            {
                audioSource.enabled = isEnable;
                _text.text = isEnable ? StateEnable : StateDisable;
            }
        }
    }
}