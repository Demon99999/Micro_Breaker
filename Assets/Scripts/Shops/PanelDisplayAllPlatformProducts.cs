using System.Collections.Generic;
using System.Linq;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shops
{
    [RequireComponent(typeof(ProductTypeSection), typeof(SwipePanel))]
    public class PanelDisplayAllPlatformProducts : MonoBehaviour
    {
        [SerializeField] private ChoosePlatform _choosePlatform;
        [SerializeField] private Button _buttonBuy;
        [SerializeField] private Button _buttonSelect;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private TMP_Text _choosed;
        [SerializeField] private Image _imageBlock;

        private SwipePanel _swipePanel;
        private PanelProduct _panelProduct;
        private ProductTypeSection _productTypeSection;
        private Product _currentProduct;
        private List<Product> _products = new();

        private void Awake()
        {
            _swipePanel = GetComponent<SwipePanel>();
            _productTypeSection = GetComponent<ProductTypeSection>();
        }

        private void OnEnable()
        {
            _productTypeSection.Inited += OnCreate;
            _productTypeSection.Buyed += OnSetStateProduct;
            _swipePanel.Swiped += OnSetCurrentProduct;
            _buttonBuy.onClick.AddListener(OnOpenBuyPanel);
            _buttonSelect.onClick.AddListener(OnChoose);
        }

        private void OnDisable()
        {
            _productTypeSection.Inited -= OnCreate;
            _productTypeSection.Buyed -= OnSetStateProduct;
            _swipePanel.Swiped -= OnSetCurrentProduct;
            _buttonBuy.onClick.RemoveListener(OnOpenBuyPanel);
            _buttonSelect.onClick.RemoveListener(OnChoose);
        }

        private void OnChoose()
        {
            _currentProduct.Select();
            OnSetStateProduct();
        }

        public void OnCreate(List<Product> products, PanelProduct panelProduct)
        {
            _products = products;
            _panelProduct = panelProduct;
            _choosePlatform.Init(products);
            _currentProduct = GetCurrentProduct();
            OnSetStateProduct();
        }

        private Product GetCurrentProduct()
        {
            return _products.Where(product => product.Name == _choosePlatform.CurrentTemplate.Name).FirstOrDefault();
        }

        private void OnSetCurrentProduct(int element)
        {
            _choosePlatform.SetCurrentPlatform(element);
            _currentProduct = GetCurrentProduct();
            OnSetStateProduct();
        }

        private void OnSetStateProduct()
        {
            _name.text = _currentProduct.Name.ToString();
            _price.text = _currentProduct.Price.ToString();
            _buttonBuy.gameObject.SetActive(!_currentProduct.IsBuy);
            _buttonSelect.gameObject.SetActive(_currentProduct.IsBuy);
            _buttonSelect.gameObject.SetActive(!_currentProduct.IsSelected && _currentProduct.IsBuy);
            _choosed.gameObject.SetActive(_currentProduct.IsSelected);
            _imageBlock.gameObject.SetActive(!_currentProduct.IsBuy);
        }

        private void OnOpenBuyPanel()
        {
            _panelProduct.Init(_currentProduct);
            _panelProduct.OnMove(true);
        }
    }
}