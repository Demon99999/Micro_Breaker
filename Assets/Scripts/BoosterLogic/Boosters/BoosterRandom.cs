using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enums;
using Assets.Scripts.LocationLogic;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class BoosterRandom : AbstractBooster
    {
        [SerializeField] private LocationCreate _locationCreate;

        private AbstractBooster _currentBooster;

        private void Start()
        {
            _locationCreate.Created += OnSetCurrenBooster;
        }

        public override void StopAction(BoosterEffect boosterEffect)
        {
            if (boosterEffect == null)
            {
                Debug.LogError("BoosterEffect is null in BoosterRandom.StopAction");
                return;
            }

            if (this == null)
            {
                Debug.LogError("BoosterRandom instance is null");
                return;
            }

            _currentBooster.StopAction(boosterEffect);
        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            _currentBooster.OnStartAction(boosterEffect);
        }

        private void OnSetCurrenBooster(List<AbstractBooster> abstractBoosters)
        {
            List<AbstractBooster> boosters = abstractBoosters.Where(booster => booster.BoosterName == BoosterNames.Positive ||
                                                                    booster.BoosterName == BoosterNames.Negative).ToList();

            Debug.Log(boosters.Count);
            _currentBooster = boosters[Random.Range(0, boosters.Count)];
            _locationCreate.Created -= OnSetCurrenBooster;
        }
    }
}