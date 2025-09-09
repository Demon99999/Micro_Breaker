using System;
using Assets.Scripts.PlayerLogic;
using Assets.Scripts.UI.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shops
{
    public class PanelProduct : Panel
    {
        [SerializeField] private Button _buttonPay;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private TMP_Text _textNameTemplate;
        [SerializeField] private TMP_Text _textPrice;
        [SerializeField] private Panel _backGround;
        [SerializeField] private Image _imageTemplate;
        [SerializeField] private Image _imageButtonBuy;
        [SerializeField] private Color _startColor;

        public event Action Bought;

        private Product _product;

        private void OnEnable()
        {
            _buttonPay.onClick.AddListener(OnListener);
        }

        private void OnDisable()
        {
            _buttonPay.onClick.RemoveListener(OnListener);
        }

        public override async void OnMove(bool isAction)
        {
            base.OnMove(isAction);
            await MovePanel(isAction);
            _backGround.gameObject.SetActive(isAction);
        }

        public void Init(Product product)
        {
            _product = product;
            _imageTemplate.sprite = _product.Template.Sprite;
            _textNameTemplate.text = _product.Name;
            AssignAccess();
        }

        private void AssignAccess()
        {
            _textPrice.text = _product.Price.ToString();

            if (_wallet.Coin < _product.Price)
            {
                _buttonPay.enabled = false;
                SetColor(Color.red);
                return;
            }

            SetColor(_startColor);
            _buttonPay.enabled = true;
        }

        private void OnListener()
        {
            _wallet.RemoveCoins(_product.Price);
            _product.Buy();
            Bought?.Invoke();
            OnMove(false);
        }

        private void SetColor(Color color)
        {
            _buttonPay.image.color = color;
            _imageButtonBuy.color = color;
            _textPrice.color = color;
        }
    }
}