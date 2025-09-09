using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Sound;
using Assets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Buttons
{
    public class ButtonPlayGame : AbstractButton
    {
        [SerializeField] private ScenesName _scenesName;
        [SerializeField] private PanelFade _panelFade;
        [SerializeField] private SoundMusic _soundMusic;

        public event Action Clicked;

        protected override void OnClick()
        {
            _panelFade.SetActive(false, LoadScene);
            _soundMusic.SetActive(false);
            Clicked?.Invoke();
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(_scenesName.ToString());
        }
    }
}