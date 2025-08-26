using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enums;
using Assets.Scripts.UI.AdditionalImprovement;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Buttons
{
    public class ButtonAdditionalImprovement : AbstractButton
    {
        [SerializeField] private AdditionalImprovementName _additionalImprovementName;
        [SerializeField] private Image _image;
        [SerializeField] private Image _imageBuy;
        [SerializeField] private Image _imageSelected;
        [SerializeField] private List<AdditionalImprovementTemplate> _additionalImprovementTemplates;

        public event Action<ButtonAdditionalImprovement> Clicked;
        public event Action<ButtonAdditionalImprovement> Selected;

        public bool IsBuy { get; private set; } = false;

        public bool IsSelect { get; private set; } = false;

        public AdditionalImprovementName AdditionalImprovementName => _additionalImprovementName;

        public AdditionalImprovementTemplate AdditionalImprovementTemplate { get; private set; }

        protected override void InitAwake()
        {
            base.InitAwake();
            AdditionalImprovementTemplate = GetCurrentAdditionalImprovementTemplate();
            _image.sprite = AdditionalImprovementTemplate.Sprite;
            SetState();
        }

        protected override void OnClick()
        {
            if (IsBuy == true)
            {
                SetSelect();
                SetState();
                Selected?.Invoke(this);
                return;
            }

            Clicked?.Invoke(this);
        }

        public void SetBuy(bool isSelect)
        {
            IsSelect = !isSelect;
            IsBuy = !IsBuy;
            SetSelect();
            SetState();
        }

        private void SetSelect()
        {
            IsSelect = !IsSelect;
        }

        private void SetState()
        {
            _imageBuy.gameObject.SetActive(!IsBuy);
            _imageSelected.gameObject.SetActive(IsSelect);
        }

        private AdditionalImprovementTemplate GetCurrentAdditionalImprovementTemplate()
        {
            return _additionalImprovementTemplates.Where(additionalImprovement => additionalImprovement.AdditionalImprovementName == _additionalImprovementName).FirstOrDefault();
        }
    }
}