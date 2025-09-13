using Assets.Scripts.Sound;
using Assets.Scripts.UI.AdditionalImprovement;
using Assets.Scripts.UI.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
    public class PanelAdditionalImprovement : Panel
    {
        private const int FirstMultiplier = 1;
        private const int SecondMultiplier = 3;
        private const int ThirdMultiplier = 9;

        [SerializeField] private Panel _backGround;
        [SerializeField] private PanelBuyAdditionalImprovement _panelBuyUpgrade;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private AdditionalImprovement.AdditionalImprovement _firstUpgrade;
        [SerializeField] private AdditionalImprovement.AdditionalImprovement _secondUpgrade;
        [SerializeField] private AdditionalImprovement.AdditionalImprovement _thirdUpgrate;
        [SerializeField] private SoundButton _soundButton;

        private ButtonAdditionalImprovement _currentButtonUpgrade;

        private void OnEnable()
        {
            _firstUpgrade.Clicked += OnOpenBuyCanUpgrade;
            _secondUpgrade.Clicked += OnOpenBuyCanUpgrade;
            _thirdUpgrate.Clicked += OnOpenBuyCanUpgrade;
            _panelBuyUpgrade.Buid += OnSetAccess;
        }

        private void OnDisable()
        {
            _firstUpgrade.Clicked -= OnOpenBuyCanUpgrade;
            _secondUpgrade.Clicked -= OnOpenBuyCanUpgrade;
            _thirdUpgrate.Clicked -= OnOpenBuyCanUpgrade;
            _panelBuyUpgrade.Buid -= OnSetAccess;
        }

        public void Init(ButtonAdditionalImprovement buttonUpgrade)
        {
            _currentButtonUpgrade = buttonUpgrade;
            AdditionalImprovementTemplate imageUpgrade = _currentButtonUpgrade.AdditionalImprovementTemplate;
            _image.sprite = imageUpgrade.Sprite;
            _text.text = buttonUpgrade.AdditionalImprovementTemplate.Description;

            _firstUpgrade.Init(imageUpgrade.Sprite, imageUpgrade.FirstPrice, FirstMultiplier, imageUpgrade.AdditionalImprovementName);
            _secondUpgrade.Init(imageUpgrade.Sprite, imageUpgrade.SecondPrice, SecondMultiplier, imageUpgrade.AdditionalImprovementName);
            _thirdUpgrate.Init(imageUpgrade.Sprite, imageUpgrade.ThirdPrice, ThirdMultiplier, imageUpgrade.AdditionalImprovementName);
        }

        public override async void OnMove(bool isAction)
        {
            _backGround.gameObject.SetActive(isAction);
            base.OnMove(isAction);
            await MovePanel(isAction);
        }

        private void OnSetAccess(AdditionalImprovement.AdditionalImprovement upgrade)
        {
            upgrade.SetState(false);
            _currentButtonUpgrade.SetBuy(true);
            OnMove(false);
        }

        private void OnOpenBuyCanUpgrade(AdditionalImprovement.AdditionalImprovement upgrade)
        {
            _soundButton.AudioSource.Play();
            _panelBuyUpgrade.gameObject.SetActive(true);
            _panelBuyUpgrade.Init(upgrade);
            _panelBuyUpgrade.OnMove(true);
        }
    }
}