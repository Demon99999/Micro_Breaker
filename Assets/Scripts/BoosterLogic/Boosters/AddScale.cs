using Assets.Scripts.Enums;
using Assets.Scripts.Modification;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class AddScale : AbstractBooster
    {
        [SerializeField] private BallModificationm _ballModification;
        [SerializeField] private PlatfornModification _playformModification;

        public override void StopAction(BoosterEffect boosterEffect)
        {
            if (boosterEffect.IsActive == false)
                return;

            _playformModification.SetDefultScale(true);
            _ballModification.SetDefultScale(true);
            boosterEffect.SetActionActive();
        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            if (boosterEffect.ObjectsName == ObjectsName.Platform)
                _playformModification.SetNewScale(boosterEffect.BoosterName, true);

            if (boosterEffect.ObjectsName == ObjectsName.Ball)
                _ballModification.SetNewScale(boosterEffect.BoosterName, true);

            boosterEffect.SetActionActive();
        }
    }
}