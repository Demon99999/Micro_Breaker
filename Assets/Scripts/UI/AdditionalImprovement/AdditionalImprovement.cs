using System;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.AdditionalImprovement
{
    [RequireComponent(typeof(AdditionalImprovementView))]
    public class AdditionalImprovement : MonoBehaviour
    {
        public event Action<AdditionalImprovement> Clicked;

        private AdditionalImprovementView _additionalImprovementView;

        public Sprite Sprite { get; private set; }

        public int Price { get; private set; }

        public int Count { get; private set; }

        public AdditionalImprovementName AdditionalImprovementName { get; private set; }

        public Button Botton => _additionalImprovementView.Button;

        private void Awake() => _additionalImprovementView = GetComponent<AdditionalImprovementView>();

        private void OnEnable() => _additionalImprovementView.Button.onClick.AddListener(OnClick);

        private void OnDisable() => _additionalImprovementView.Button.onClick.RemoveListener(OnClick);

        public void Init(Sprite sprite, int price, int count, AdditionalImprovementName additionalImprovementName)
        {
            Sprite = sprite;
            Price = price;
            Count = count;
            AdditionalImprovementName = additionalImprovementName;
            _additionalImprovementView.Init(Sprite, Price, Count);
        }

        public void SetState(bool isEnable) => _additionalImprovementView.SetState(isEnable);

        private void OnClick() => Clicked?.Invoke(this);
    }
}