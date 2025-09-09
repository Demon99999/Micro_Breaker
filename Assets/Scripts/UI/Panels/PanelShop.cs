using System;
using Assets.Scripts.UI.Buttons;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    public class PanelShop : Panel
    {
        [SerializeField] private ButtonPanelInteraction _buttonClose;
        [SerializeField] private Panel _backGroundPanel;

        public event Action<bool> Activated;

        public bool IsActive { get; private set; } = false;

        public ButtonPanelInteraction ButtenClose => _buttonClose;

        private void OnEnable()
        {
            _buttonClose.Clicked += OnMove;
        }

        private void OnDisable()
        {
            _buttonClose.Clicked -= OnMove;
        }

        public override void OnMove(bool isActive)
        {
            base.OnMove(isActive);
            OnMovePanel(isActive);
        }

        public void OnMovePanel(bool isOpen)
        {
            _buttonClose.gameObject.SetActive(isOpen);
            _backGroundPanel.gameObject.SetActive(isOpen);

            RectTransform.DOAnchorPosX(MiddlePosition, TweenDurationOpen);

            if (isOpen == false)
            {
                RectTransform.DOAnchorPosX(TopPosition, TweenDurationOpen);
            }

            IsActive = isOpen;
            Activated?.Invoke(isOpen);
        }
    }
}