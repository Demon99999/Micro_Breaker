using System;
using UnityEngine;

namespace Assets.Scripts.LocationLogic
{
    public class LocationChooseInput : MonoBehaviour
    {
        [SerializeField] private LocationCreateView _createView;

        public event Action<LocationObject> LocationChoosed;

        private LocationObject _firstLocationObject;
        private LocationObject _lastLocationObject;

        public bool IsActive { get; private set; } = true;

        public void SetFirstLocationObject(Vector3 inputMouse) => _firstLocationObject = TryGetLocation(inputMouse);

        public void SetLastLocationObject(Vector3 inputMouse) => _lastLocationObject = TryGetLocation(inputMouse);

        public void LoadLocation()
        {
            if (_firstLocationObject == null || _lastLocationObject == null) return;

            if (_firstLocationObject.Name != _lastLocationObject.Name) return;

            if (IsActive == true)
            {
                _createView.gameObject.SetActive(true);
                LocationChoosed?.Invoke(_firstLocationObject);
                _firstLocationObject = null;
                _lastLocationObject = null;
            }
        }

        private LocationObject TryGetLocation(Vector3 inputMouse)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputMouse);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                hit.collider.TryGetComponent(out LocationObject locationObject);
                return locationObject;
            }
            return null;
        }
    }
}