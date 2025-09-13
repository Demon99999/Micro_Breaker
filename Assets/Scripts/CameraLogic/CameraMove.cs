using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LocationLogic;
using UnityEngine;

namespace Assets.Scripts.CameraLogic
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private ContainerLocationObjects _containerLocationObjects;
        [SerializeField] private Transform _transformPoint;
        [SerializeField] private SwipeMove _swipeMove;

        private void OnEnable()
        {
            _containerLocationObjects.Filled += OnSetDistance;
        }

        private void OnDisable()
        {
            _containerLocationObjects.Filled -= OnSetDistance;
        }

        private void OnSetDistance(List<LocationObject> locationObject)
        {
            var distance = _swipeMove.transform.position.z - locationObject.First().transform.position.z
                           + _transformPoint.transform.position.z;

            _swipeMove.transform.position = new Vector3(_swipeMove.transform.position.x,
                _swipeMove.transform.position.y, distance);
        }
    }
}