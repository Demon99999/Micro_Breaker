using Assets.Scripts.LocationLogic;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class BoosterDisableDestruction : AbstractBooster
    {
        [SerializeField] private LocationCreate _locationCreate;
        [SerializeField] private BoxesFalling _boxesFalling;

        public override void StopAction(BoosterEffect boosterEffect)
        {
            if (boosterEffect.IsActive == false)
                return;

            ActiveCanDestructionBoxsFalling();
            _locationCreate.ActiveCanDestructionBoxs();
            boosterEffect.SetActionActive();
        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            ActiveCanDestructionBoxsFalling();
            _locationCreate.ActiveCanDestructionBoxs();
            boosterEffect.SetActionActive();
            PlayTimer(boosterEffect, StopAction);
        }

        private void ActiveCanDestructionBoxsFalling()
        {
            foreach (var box in _boxesFalling.Boxes)
                box.SetCanDestructuin();
        }
    }
}