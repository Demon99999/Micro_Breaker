using Assets.Scripts.PlatformLogic;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class PlatformInverted : AbstractBooster
    {
        [SerializeField] private PlatformMovement _platformMovement;
        [SerializeField] private PlatformAddInverted _platformAddInverted;

        public bool IsAction { get; private set; } = false;

        public override void StopAction(BoosterEffect boosterEffect)
        {
            if (IsAction == false)
                return;

            _platformMovement.EnableInverted();
            boosterEffect.SetActionActive();
            IsAction = false;
        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            IsAction = true;

            if (_platformAddInverted.IsAction == true)
                _platformAddInverted.StopAction(boosterEffect);

            _platformMovement.EnableInverted();
            boosterEffect.SetActionActive();
            PlayTimer(boosterEffect, StopAction);
        }
    }
}