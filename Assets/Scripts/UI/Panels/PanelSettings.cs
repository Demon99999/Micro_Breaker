using Assets.Scripts.UI.Buttons;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    public class PanelSettings : Panel
    {
        private const int MinVolume = 0;

        [SerializeField] private ButtonSettingAudio _buttonSettingMusic;
        [SerializeField] private ButtonSettingAudio _buttonSettingEffect;
        [SerializeField] private Panel _backGround;

        private float _currentVolume;

        private void OnEnable()
        {
            _buttonSettingMusic.Changed += OnSave;
            _buttonSettingEffect.Changed += OnSave;
        }

        private void OnDisable()
        {
            _buttonSettingMusic.Changed -= OnSave;
            _buttonSettingEffect.Changed -= OnSave;
        }

        private void Start()
        {
            LoadAudioSettings(_buttonSettingMusic);
            LoadAudioSettings(_buttonSettingEffect);
        }

        public override async void OnMove(bool isActive)
        {
            _backGround?.gameObject.SetActive(isActive);
            base.OnMove(isActive);
            await MovePanel(isActive);
        }

        private void LoadAudioSettings(ButtonSettingAudio buttonSettingAudio)
        {
            _currentVolume = PlayerPrefs.GetFloat(buttonSettingAudio.AudioName.ToString());
            bool isEnable = _currentVolume == buttonSettingAudio.MaxVolume;
            buttonSettingAudio.Init(isEnable);
        }

        private void OnSave(ButtonSettingAudio buttonSettingAudio)
        {
            _currentVolume = buttonSettingAudio.IsEnable ? buttonSettingAudio.MaxVolume : MinVolume;
            PlayerPrefs.SetFloat(buttonSettingAudio.AudioName.ToString(), _currentVolume);
        }
    }
}