using Assets.Scripts.PlatformLogic;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class BoosterDisableInputPlatform : AbstractBooster
    {
        [SerializeField] private InputPointMovement _inputPointMovement;

        public override void StopAction(BoosterEffect boosterEffect)
        {
            if (boosterEffect.IsActive == false)
                return;

            _inputPointMovement.SetActiveInputObject(true);
            boosterEffect.SetActionActive();
        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            _inputPointMovement.SetActiveInputObject(false);
            boosterEffect.SetActionActive();
            PlayTimer(boosterEffect, StopAction);
        }
    }
}