using System;
using Assets.Scripts.PlayerLogic;
using Assets.Scripts.UI.AdditionalImprovement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
    public class PanelBuyAdditionalImprovement : Panel
    {
        [SerializeField] private Panel _backGround;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Button _buttonBuy;
        [SerializeField] private TMP_Text _textPrice;
        [SerializeField] private Image _currentImage;
        [SerializeField] private AdditionalImprovementColection _additionalImprovementColection;
        [SerializeField] private Color _firstColor;

        public event Action<AdditionalImprovement.AdditionalImprovement> Buid;

        private AdditionalImprovement.AdditionalImprovement _additionalImprovement;

        private void OnEnable()
        {
            _buttonBuy.onClick.AddListener(OnBuy);
        }

        private void OnDisable()
        {
            _buttonBuy.onClick.RemoveListener(OnBuy);
        }

        public override async void OnMove(bool isAction)
        {
            _backGround.gameObject.SetActive(isAction);
            base.OnMove(isAction);
            await MovePanel(isAction);
        }

        public void Init(AdditionalImprovement.AdditionalImprovement additionalImprovement)
        {
            _additionalImprovement = additionalImprovement;
            SetAccessBuy();
        }

        private void OnBuy()
        {
            if (_wallet.Coin < _additionalImprovement.Price)
                return;

            _wallet.RemoveCoins(_additionalImprovement.Price);
            _additionalImprovementColection.SaveAdditionalImprovement(_additionalImprovement);
            OnMove(false);
            Buid?.Invoke(_additionalImprovement);
        }

        private void SetAccessBuy()
        {
            _currentImage.sprite = _additionalImprovement.Sprite;
            _textPrice.text = _additionalImprovement.Price.ToString();

            if (_wallet.Coin < _additionalImprovement.Price)
            {
                _buttonBuy.image.color = Color.red;
                return;
            }

            _buttonBuy.image.color = _firstColor;
        }
    }
}