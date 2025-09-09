using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.SaveLoadServices;
using Assets.Scripts.UI.Buttons;
using Assets.Scripts.UI.Panels;
using Shop;
using UnityEngine;

namespace Assets.Scripts.Shops
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private Template[] _templates;
        [SerializeField] private ProductTypeSection _panelCreateBallProducts;
        [SerializeField] private ProductTypeSection _panelCreatePlatformProducts;
        [SerializeField] private SaveService _saveService;
        [SerializeField] private PanelShop _panelShop;
        [SerializeField] private ButtonPanelInteraction[] _buttonPanelInteractions;

        private readonly List<Product> _products = new List<Product>();

        private void OnEnable()
        {
            foreach (var buttonPanelInteraction in _buttonPanelInteractions)
                buttonPanelInteraction.Clicked += OnPlaySound;

            _saveService.Loaded += OnCreate;
        }

        private void OnDisable()
        {
            foreach (var buttonPanelInteraction in _buttonPanelInteractions)
                buttonPanelInteraction.Clicked -= OnPlaySound;

            _saveService.Loaded -= OnCreate;
        }

        private void OnPlaySound(bool isAction)
        {
            _panelShop.OnMove(isAction);
        }

        private void OnCreate()
        {
            _templates = _templates.OrderBy(template => template.Price).ToArray();

            foreach (var template in _templates)
                _products.Add(new Product(template));

            foreach (var product in _products)
            {
                if (product.Template.ObjectsName == Enums.ObjectsName.Ball)
                    _panelCreateBallProducts.AddProduct(product);

                if (product.Template.ObjectsName == Enums.ObjectsName.Platform)
                    _panelCreatePlatformProducts.AddProduct(product);
            }

            _panelCreateBallProducts.Init(_saveService);
            _panelCreatePlatformProducts.Init(_saveService);
        }
    }
}