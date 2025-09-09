using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enums;
using Assets.Scripts.PlayerLogic;
using Assets.Scripts.SaveLoadServices;
using Assets.Scripts.Shops;
using Assets.Scripts.UI.Panels;
using UnityEngine;

namespace Shop
{
    public class ProductTypeSection : Panel
    {
        [SerializeField] private ObjectsName _objectsName;
        [SerializeField] private PanelProduct _panelProduct;
        [SerializeField] private Wallet _wallet;

        public event Action<List<Product>, PanelProduct> Inited;
        public event Action Selected;
        public event Action Buyed;

        private SaveService _saveService;
        private List<string> _accessProductNames = new List<string>();
        private readonly List<Product> _products = new List<Product>();

        public Product CurrentProduct { get; private set; }

        private void OnEnable()
        {
            _panelProduct.Bought += OnSaveProductsName;

            foreach (var product in _products) product.Selected += SetCurrentProduct;
        }

        private void OnDisable()
        {
            _panelProduct.Bought -= OnSaveProductsName;

            foreach (var product in _products)
                product.Selected -= SetCurrentProduct;
        }

        public override void OnMove(bool isActive) => this.gameObject.SetActive(isActive);

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public void Init(SaveService saveService)
        {
            _saveService = saveService;
            CurrentProduct = _products.Where(product => product.Name == _saveService.GetCurrentProduct(_objectsName)).FirstOrDefault();

            if (CurrentProduct == null)
            {
                CurrentProduct = _products.First();
                _saveService.SaveCurrentProduct(_objectsName, CurrentProduct.Name);
                _accessProductNames.Add(CurrentProduct.Name);
                _saveService.SaveArrayProducts(_objectsName, _accessProductNames.ToArray());
            }

            OpenAccess(_saveService.GetArrayProducts(_objectsName));
            CurrentProduct.Buy();
            SetCurrentProduct(CurrentProduct);
            Inited?.Invoke(_products, _panelProduct);
            gameObject.SetActive(false);
        }

        private void OpenAccess(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                Product product = _products.Where(p => p.Name == names[i]).FirstOrDefault();
                product.Buy();
            }
        }

        private void SetCurrentProduct(Product productSet)
        {
            foreach (var product in _products)
                product.SetStatusOfTheSelected(false);

            productSet.SetStatusOfTheSelected(true);
            CurrentProduct = productSet;
            _saveService.SaveCurrentProduct(_objectsName, CurrentProduct.Name);
            Selected?.Invoke();
        }

        private void OnSaveProductsName()
        {
            _accessProductNames = _products.Where(product => product.IsBuy == true).Select(product => product.Name).ToList();
            _saveService.SaveArrayProducts(_objectsName, _accessProductNames.ToArray());
            _saveService.SaveCoins(_wallet.Coin);

            Buyed?.Invoke();
        }
    }
}