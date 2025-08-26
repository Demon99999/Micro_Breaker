using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shops
{
    [RequireComponent(typeof(Button))]
    public class ProductBallView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private Button _buttonBuy;
        [SerializeField] private Image _imageChoosed;
        [SerializeField] private Image _imageBlock;
        [SerializeField] private Image _imageTemplate;

        private AudioSource _audioSourceButton;
        private PanelProduct _panelProduct;
        private Product _product;
        private Button _buttonChoose;

        public Button ButtonBuy => _buttonBuy;

        private void Awake() => _buttonChoose = GetComponent<Button>();

        private void OnEnable()
        {
            _buttonChoose.onClick.AddListener(OnClick);
            _buttonBuy.onClick.AddListener(OnOpenBuyPanel);
        }

        private void OnDisable()
        {
            _buttonChoose.onClick.RemoveListener(OnClick);
            _buttonBuy.onClick.RemoveListener(OnOpenBuyPanel);
        }

        public void Init(PanelProduct panelProduct, Product product, AudioSource audioSourceButton)
        {
            _product = product;
            _name.text = product.Name;
            _price.text = product.Price.ToString();
            _imageTemplate.sprite = product.Template.Sprite;
            _panelProduct = panelProduct;
            _audioSourceButton = audioSourceButton;
        }

        public void SetState()
        {
            _imageBlock.gameObject.SetActive(!_product.IsBuy);
            _buttonBuy.gameObject.SetActive(!_product.IsBuy);

            if (_product.Price == 0) _buttonBuy.gameObject.SetActive(false);

            _imageChoosed.gameObject.SetActive(_product.IsSelected);
            _buttonChoose.enabled = _product.IsBuy;
        }

        private void OnClick() => _product.Select();

        private void OnOpenBuyPanel()
        {
            _panelProduct.OnMove(true);
            _panelProduct.Init(_product);
            _audioSourceButton.Play();
        }
    }
}