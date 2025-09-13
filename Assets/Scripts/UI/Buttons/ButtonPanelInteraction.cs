using System;
using Assets.Scripts.UI.Panels;
using UnityEngine;

namespace Assets.Scripts.UI.Buttons
{
    public class ButtonPanelInteraction : AbstractButton
    {
        private const int MinTimeScale = 0;
        private const int MaxTimeScale = 1;

        [SerializeField] private Panel _panelOpenPanel;
        [SerializeField] private Panel[] _panelClosePanels = null;
        [SerializeField] private AbstractButton _buttonEnable;
        [SerializeField] private bool _isStateOnOpenPanel = true;
        [SerializeField] private bool _isUseTimeScale = false;
        [SerializeField] private bool _isNeedDisableViewObject = false;

        public event Action<bool> Clicked;

        public bool IsClick { get; private set; } = false;

        protected override void OnClick()
        {
            if (_panelClosePanels != null)
                ClosePanels();

            if (_panelOpenPanel != null)
                _panelOpenPanel.OnMove(_isStateOnOpenPanel);

            if (_buttonEnable != null)
                _buttonEnable.gameObject.SetActive(true);

            HaveTimeScale();
            SwitchObject();
            IsClick = true;
            Clicked?.Invoke(_isStateOnOpenPanel);
        }

        public void SetStartStateButton()
        {
            IsClick = false;
        }

        private void ClosePanels()
        {
            foreach (var panel in _panelClosePanels) panel.OnMove(!_isStateOnOpenPanel);
        }

        private void HaveTimeScale()
        {
            if (_isUseTimeScale == false) return;

            Time.timeScale = MinTimeScale;

            if (_isStateOnOpenPanel == false) Time.timeScale = MaxTimeScale;
        }

        private void SwitchObject()
        {
            if (_isNeedDisableViewObject == false) return;

            gameObject.SetActive(false);
        }
    }
}