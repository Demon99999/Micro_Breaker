using Assets.Scripts.Envierment;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class BoosterPortal : AbstractBooster
    {
        [SerializeField] private Portal _portal;
        [SerializeField] private BorderCollisionWithLoss _triggerLoss;

        public override void StopAction(BoosterEffect boosterEffect)
        {
            if (boosterEffect.IsActive == false) return;

            _portal.Open(false);
            _triggerLoss.Collider.enabled = true;
            boosterEffect.SetActionActive();
        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            _portal.Open(true);
            _triggerLoss.Collider.enabled = false;
            boosterEffect.SetActionActive();
            PlayTimer(boosterEffect, StopAction);
        }
    }
}