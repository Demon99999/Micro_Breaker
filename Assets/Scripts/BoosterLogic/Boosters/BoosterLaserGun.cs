using Assets.Scripts.LaserLogic;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class BoosterLaserGun : AbstractBooster
    {
        [SerializeField] private LaserGun _laserGun;

        public override void StopAction(BoosterEffect boosterEffect)
        {
            if (boosterEffect.IsActive == false)
                return;

            _laserGun.StopShoot();
            boosterEffect.SetActionActive();
        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            _laserGun.Shooting(() => { StopAction(boosterEffect); });
            boosterEffect.SetActionActive();
        }
    }
}