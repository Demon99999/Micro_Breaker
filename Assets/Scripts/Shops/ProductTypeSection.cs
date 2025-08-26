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

        public void AddProduct(Product product) => _products.Add(product);

        public void Init(SaveService saveService)
        {
            
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
           
        }

        private void OnSaveProductsName()
        {
            
        }
    }
}