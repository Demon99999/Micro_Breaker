using Assets.Scripts.PlatformLogic;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class BoosterPlatformFollowForBall : AbstractBooster
    {
        [SerializeField] private PlatformTrigger _platformTrigger;
        [SerializeField] private PlatformMovement _platformMovement;
        [SerializeField] private BoostersContainer _boostersContainer;

        public override void StopAction(BoosterEffect boosterEffect)
        {
            if (boosterEffect.IsActive == false)
                return;

            SetAction(boosterEffect);
        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            _boostersContainer.Reset();
            SetAction(boosterEffect);
            PlayTimer(boosterEffect, StopAction);
        }

        private void SetAction(BoosterEffect boosterEffect)
        {
            _platformTrigger.ChangeStateCollision();
            _platformMovement.ChangeTarget();
            boosterEffect.SetActionActive();
        }
    }
}