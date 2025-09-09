using Assets.Scripts.Enums;
using Assets.Scripts.Sound;
using Assets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Buttons
{
    public class ButtonPlay : AbstractButton
    {
        [SerializeField] private ScenesName _scenesName;
        [SerializeField] private PanelFade _panelFade;
        [SerializeField] private SoundMusic _soundMusic;

        protected override void OnClick()
        {
            _panelFade.SetActive(false, OnLoadScene);
            _soundMusic.SetActive(false);
        }

        private void OnLoadScene() => SceneManager.LoadScene(_scenesName.ToString());
    }
}