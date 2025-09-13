using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.LocationLogic
{
    public class LocationCreateView : MonoBehaviour
    {
        private const int CountCreateObjects = 20;
        private const int MinValue = 0;
        private const int Duration = 10;
        private const int Angle = 360;
        private const int SetLoops = -1;

        [SerializeField] private Location[] _locations;
        [SerializeField] private LocationChooseInput _chooseInput;
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _spawnCenter;

        private readonly List<GameObject> _gameObjects = new List<GameObject>();
        private readonly List<Vector3> _localPositions = new List<Vector3>();

        private void Awake()
        {
            Create(CountCreateObjects);
            gameObject.SetActive(false);
        }

        private void OnEnable() => _chooseInput.LocationChoosed += OnChoosed;

        private void OnDisable() => _chooseInput.LocationChoosed -= OnChoosed;

        private void Create(int countObject)
        {
            for (int i = 0; i < countObject; i++)
            {
                GameObject currentObject = Instantiate(_gameObject, _spawnCenter);
                currentObject.SetActive(false);
                _gameObjects.Add(currentObject);
                _localPositions.Add(Vector3.zero);
            }
        }

        private void OnChoosed(LocationObject locationObject)
        {
            foreach (var currentObject in _gameObjects)
                currentObject.SetActive(false);

            var newLocation = _locations.Where(location => location.LocationName == locationObject.Name.ToString() &&
                                               location.AdditionaValue == locationObject.AdditionaValue).FirstOrDefault();

            if (newLocation == null)
                return;

            if (newLocation.BoxContainer.transform.childCount > _gameObjects.Count)
                Create(newLocation.BoxContainer.transform.childCount - _gameObjects.Count);

            for (int i = 0; i < newLocation.BoxContainer.transform.childCount; i++)
            {
                _localPositions[i] = newLocation.BoxContainer.transform.GetChild(i).transform.localPosition;
                _gameObjects[i].transform.localPosition = _localPositions[i];
                _gameObjects[i].gameObject.SetActive(true);
            }

            _spawnCenter.DORotate(new Vector3(MinValue, Angle, MinValue), Duration, RotateMode.FastBeyond360)
                .SetLoops(SetLoops).SetRelative().SetEase(Ease.Linear);
        }
    }
}