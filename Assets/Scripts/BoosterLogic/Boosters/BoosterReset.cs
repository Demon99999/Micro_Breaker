using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class BoosterReset : AbstractBooster
    {
        [SerializeField] private BoostersContainer _boostersContainer;

        public override void StopAction(BoosterEffect _)
        {

        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            foreach (var booster in _boostersContainer.AbstractBoosters)
            {
                if (booster.BoosterEffect.IsActive == true)
                    booster.StopAction(booster.BoosterEffect);
            }
        }
    }
}