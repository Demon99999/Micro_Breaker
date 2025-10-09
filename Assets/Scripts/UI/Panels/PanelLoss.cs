using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.Sound;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
    public class PanelLoss : Panel
    {
        [SerializeField] private Button _buttonTryAgain;
        [SerializeField] private Button _buttonExit;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private TMP_Text _bricksSmashed;
        [SerializeField] private PromotionalVideo _promotionalVideo;
        [SerializeField] private SoundMusic _soundMusic;

        public event Action<string> Clicked;

        private void OnEnable()
        {
            //_buttonTryAgain.onClick.AddListener(() => { _promotionalVideo.ShowInterstitialAd(); });
            _buttonExit.onClick.AddListener(() => { OnClickRestart(ScenesName.MainMenu.ToString()); });
            //_promotionalVideo.ClosedCallBack += () => { OnClickRestart(ScenesName.Game.ToString()); };

            _buttonTryAgain.onClick.AddListener(() => { OnClickRestart(ScenesName.Game.ToString()); });
        }

        private void OnDisable()
        {
            //_buttonTryAgain.onClick.RemoveListener(() => { _promotionalVideo.ShowInterstitialAd(); });
            _buttonExit.onClick.RemoveListener(() => { OnClickRestart(ScenesName.MainMenu.ToString()); });
            //_promotionalVideo.ClosedCallBack -= () => { OnClickRestart(ScenesName.Game.ToString()); };

            _buttonTryAgain.onClick.RemoveListener(() => { OnClickRestart(ScenesName.Game.ToString()); });
        }


        public override async void OnMove(bool isActive)
        {
            base.OnMove(isActive);
            await MovePanel(isActive);
        }

        //public override void OnMove(bool isActive)
        //{
        //    base.OnMove(isActive);
        //    StartCoroutine(MovePanelCoroutine(isActive));
        //}

        public void Fill(string bricksSmashed, string score)
        {
            _bricksSmashed.text = bricksSmashed;
            _score.text = score;
        }

        private void OnClickRestart(string sceneName)
        {
            OnMove(false);
            _soundMusic.SetActive(false);
            Clicked?.Invoke(sceneName);
        }
    }
}