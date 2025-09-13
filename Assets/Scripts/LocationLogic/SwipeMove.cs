using Assets.Scripts.PlatformLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.LocationLogic
{
    [RequireComponent(typeof(Rigidbody))]
    public class SwipeMove : MonoBehaviour
    {
        private const float ClampX = 60f;
        private const float ClampYMin = -1500;
        private const float ClampYMax = 10;
        private const float MinSpeedValue = 0.5f;

        [SerializeField] private LocationChooseInput _inputLocation;
        [SerializeField] private float _speedSwipe;

        private readonly Restrictor _restrict = new Restrictor();
        private Rigidbody _rigidBody;
        private Transform _transform;
        private Vector3 _startPosition;
        private Vector3 _currentPosition;
        private bool _isDragging = false;
        private float _speed = MinSpeedValue;
        private float _timer = 0;
        private float _delay = 0.2f;

        private void Awake()
        {
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void OnMouseDown()
        {
            if (IsInputUI()) return;

            _startPosition = Input.mousePosition;
            _inputLocation.SetFirstLocationObject(Input.mousePosition);
        }

        private void OnMouseDrag()
        {
            _timer += Time.deltaTime;

            if (_timer >= _delay) _isDragging = true;

            _currentPosition = Input.mousePosition;
            var direction = (_currentPosition - _startPosition).magnitude;
            _speed = direction * MinSpeedValue;
        }

        private void OnMouseUp()
        {
            _isDragging = false;
            _speed = MinSpeedValue;
            _timer = 0;

            if (_isDragging == true) return;

            _inputLocation.SetLastLocationObject(Input.mousePosition);
            _inputLocation.LoadLocation();
        }

        private void Update()
        {
            Drag();
            _restrict.RestrictMove(_transform, ClampX, ClampYMin, ClampYMax);
        }

        private void Drag()
        {
            if (_isDragging)
            {
                Vector3 direction = (_currentPosition - _startPosition).normalized;
                Vector3 newDirection = new Vector3(direction.x, _rigidBody.velocity.y, direction.y);
                _rigidBody.velocity = newDirection * _speed;
            }
        }

        private bool IsInputUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}