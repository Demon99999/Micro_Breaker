using Assets.Scripts.BallLogic;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class BoosterChangeSpeed : AbstractBooster
    {
        [SerializeField] private BallMovement _ballMovement;
        [SerializeField] private BallEffect _ballEffect;

        public override void StopAction(BoosterEffect boosterEffect)
        {
            if (boosterEffect.IsActive == false)
                return;

            _ballMovement.SetStandartSpeed();
            _ballEffect.SetParticleSystem(BoosterNames.Default);
            boosterEffect.SetActionActive();
        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            _ballMovement.SetMaxSpeed();
            _ballEffect.SetParticleSystem(BoosterNames.Negative);
            boosterEffect.SetActionActive();
            PlayTimer(boosterEffect, StopAction);
        }
    }
}