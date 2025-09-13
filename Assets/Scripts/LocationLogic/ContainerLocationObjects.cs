using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.SaveLoadServices;
using UnityEngine;

namespace Assets.Scripts.LocationLogic
{
    public class ContainerLocationObjects : MonoBehaviour
    {
        [SerializeField] private SaveService _saveService;

        public event Action<List<LocationObject>> Filled;

        private Transform _transform;

        private LocationObject[] _locationObjects;

        private void Awake() => _transform = transform;

        private void OnEnable() => _saveService.Loaded += OnFill;

        private void OnDisable() => _saveService.Loaded -= OnFill;

        private void OnFill()
        {
            _locationObjects = new LocationObject[_transform.childCount];

            for (int i = 0; i < _locationObjects.Length; i++)
            {
                _transform.GetChild(i).TryGetComponent(out LocationObject locationObject);
                _locationObjects[i] = locationObject;
            }

            if (_saveService.LevelDatas == null) return;

            List<LocationObject> newLocationObjects = new List<LocationObject>();

            for (int i = 0; i < _saveService.LevelDatas.Length; i++)
            {
                newLocationObjects.Add(_locationObjects.Where(location => location.Name.ToString() == _saveService.LevelDatas[i].LocationName &&
                                                              location.AdditionaValue == _saveService.LevelDatas[i].AdditionaValue).FirstOrDefault());

                newLocationObjects[i].Init(_saveService.LevelDatas[i]);
            }

            if (_saveService.LevelDatas.Length == _locationObjects.Length)
                return;

            var dublicatLocationObjects = _locationObjects.Where(location => location.Name == _locationObjects[newLocationObjects.Count].Name).ToList();

            foreach (var dublicatLocationObject in dublicatLocationObjects)
            {
                dublicatLocationObject.SetActive();
            }

            Filled?.Invoke(dublicatLocationObjects);
        }
    }
}