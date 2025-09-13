using System;
using System.Collections.Generic;
using Assets.Scripts.BoosterLogic;
using Assets.Scripts.BoxLogic;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.LocationLogic
{
    public class Location : MonoBehaviour
    {
        [SerializeField] private LocationName _locationName;
        [SerializeField] private BoxContainer _boxContainer;
        [SerializeField] private string _additionaValue = string.Empty;

        public string LocationName => _locationName.ToString();

        public BoxContainer BoxContainer => _boxContainer;

        public string AdditionaValue => _additionaValue;

        public void Init(BoostersContainer boostersContainer, ParticleSystem particleSystem, Action<List<AbstractBooster>> Created)
        {
            _boxContainer.Fill(boostersContainer, particleSystem, Created);
        }
    }
}