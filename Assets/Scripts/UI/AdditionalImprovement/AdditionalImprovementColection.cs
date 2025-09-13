using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.SaveLoadServices;
using Assets.Scripts.UI.Buttons;
using Assets.Scripts.UI.Panels;
using UnityEngine;

namespace Assets.Scripts.UI.AdditionalImprovement
{
    public class AdditionalImprovementColection : MonoBehaviour
    {
        private const int SelectMaxValue = 1;
        private const int MinValue = 0;

        [SerializeField] private List<ButtonAdditionalImprovement> _buttonAdditionalImprovements;
        [SerializeField] private PanelAdditionalImprovement _panelAdditionalImprovement;
        [SerializeField] private SaveService _saveService;

        private List<AdditionalImprovementValue> _additionalImprovementValues = new List<AdditionalImprovementValue>();

        private void OnEnable()
        {
            _saveService.Loaded += OnLoadAdditionalImprovement;

            foreach (var upgrade in _buttonAdditionalImprovements)
            {
                upgrade.Clicked += OnClick;
                upgrade.Selected += OnSaveSelect;
            }
        }

        private void OnDisable()
        {
            _saveService.Loaded -= OnLoadAdditionalImprovement;

            foreach (var upgrade in _buttonAdditionalImprovements)
            {
                upgrade.Clicked -= OnClick;
                upgrade.Selected -= OnSaveSelect;
            }
        }

        public void SaveAdditionalImprovement(AdditionalImprovement additionalImprovement)
        {
            _additionalImprovementValues.Add(new AdditionalImprovementValue(additionalImprovement.AdditionalImprovementName.ToString(), additionalImprovement.Count, SelectMaxValue));
            _saveService.SaveAdditionalImprovementValues(_additionalImprovementValues);
        }

        private void OnLoadAdditionalImprovement()
        {
            _additionalImprovementValues = _saveService.GetAdditionalImprovementValues();
            SetStateButtonAdditionalImprovement();
        }

        private void SetStateButtonAdditionalImprovement()
        {
            if (_additionalImprovementValues.Count <= MinValue) return;

            foreach (var additionalImprovementValue in _additionalImprovementValues)
            {
                ButtonAdditionalImprovement buttonAdditionalImprovement = _buttonAdditionalImprovements.Where(button 
                    => button.AdditionalImprovementName.ToString() == additionalImprovementValue.AdditionalImprovementName).FirstOrDefault();

                buttonAdditionalImprovement.SetBuy(additionalImprovementValue.IsSelect);
            }
        }

        private void OnClick(ButtonAdditionalImprovement buttonAdditionalImprovement)
        {
            OnSaveSelect(buttonAdditionalImprovement);
            _panelAdditionalImprovement.gameObject.SetActive(true);
            _panelAdditionalImprovement.Init(buttonAdditionalImprovement);
            _panelAdditionalImprovement.OnMove(true);
        }

        private void OnSaveSelect(ButtonAdditionalImprovement buttonAdditionalImprovement)
        {
            if (buttonAdditionalImprovement.IsBuy == false) return;

            AdditionalImprovementValue additionalImprovementValue = _additionalImprovementValues.Where(additionalImprovement => additionalImprovement.AdditionalImprovementName == buttonAdditionalImprovement.AdditionalImprovementName.ToString()).FirstOrDefault();
            additionalImprovementValue.SetSelect(buttonAdditionalImprovement.IsSelect);
            _saveService.SaveAdditionalImprovementValues(_additionalImprovementValues);
        }
    }
}