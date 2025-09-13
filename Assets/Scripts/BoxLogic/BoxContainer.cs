using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.BoosterLogic;
using UnityEngine;

namespace Assets.Scripts.BoxLogic
{
    public class BoxContainer : MonoBehaviour
    {
        private const float ParticleScale = 2;

        private ParticleSystem _particleSystem;
        private Transform _transform;
        private Box[] _boxes;

        public Box[] Boxes => _boxes;

        public void Fill(BoostersContainer boosterContainer, ParticleSystem particleSystem, Action<List<AbstractBooster>> Created)
        {
            _particleSystem = Instantiate(particleSystem, _transform);
            _particleSystem.transform.localScale = new Vector3(ParticleScale, ParticleScale, ParticleScale);
            _transform = transform;
            _boxes = new Box[_transform.childCount];
            boosterContainer.Fill();

            for (int i = 0; i < _boxes.Length; i++)
            {
                if (_transform.GetChild(i).TryGetComponent(out Box box))
                {
                    BoosterEffect booster = boosterContainer.GetRandomBoosters(box.BoosterName);
                    _boxes[i] = box;
                    _boxes[i].transform.rotation = box.transform.rotation;

                    if (booster == null)
                        _boxes[i].InitEffect(_particleSystem);
                    else
                        _boxes[i].InitBooster(booster, _particleSystem);
                }
            }

            var boosters = boosterContainer.AbstractBoosters.Where(booster => booster.BoosterEffect.IsCreated == false).ToList();
            Created?.Invoke(boosters);
        }
    }
}