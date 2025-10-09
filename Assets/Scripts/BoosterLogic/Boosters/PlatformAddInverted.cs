using Assets.Scripts.PlatformLogic;
using Assets.Scripts.Shops;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class PlatformAddInverted : AbstractBooster
    {
        private const int MinValue = 0;

        [SerializeField] private PlatformMovement _platformMovement;
        [SerializeField] private PlatformInverted _platformInverted;

        private PlatformMovement _platformMovementClone;

        public bool IsAction { get; private set; } = false;

        public override void StopAction(BoosterEffect boosterEffect)
        {
            if (IsAction == false)
                return;

            _platformMovementClone.gameObject.SetActive(false);
            boosterEffect.SetActionActive();
            IsAction = false;
        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            IsAction = true;

            if (_platformInverted.IsAction == true)
                _platformInverted.StopAction(boosterEffect);

            _platformMovementClone = Instantiate(_platformMovement, Transform);
            _platformMovementClone.EnableInverted();
            _platformMovementClone.transform.GetChild(MinValue).TryGetComponent(out ChangeTemplate changeTemplateClone);
            _platformMovement.transform.GetChild(MinValue).TryGetComponent(out ChangeTemplate changeTemplate);
            changeTemplateClone.EnableCurrentTemplate(changeTemplate.CurrentTemplate.Name, MinValue);
            boosterEffect.SetActionActive();
            PlayTimer(boosterEffect, StopAction);
        }
    }
}