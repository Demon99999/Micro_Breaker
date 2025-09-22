using System;
using Assets.Scripts.Envierment;
using Assets.Scripts.Interfaces;
using Assets.Scripts.PlatformLogic;
using Assets.Scripts.Sound;
using UnityEngine;

namespace Assets.Scripts.BallLogic
{
    [RequireComponent(typeof(Ball))]
    public class BallMovement : MonoBehaviour
    {
        private const int Divider = 2;
        private const int MinValue = 0;
        private const int MaxSpeed = 4000;
        private const int Damage = 1;
        private const int CountDistance = 20;
        private const float GravityValue = 0.05f;
        private const float PositionZero = 0f;
        private const float GravityPositionZ = 0.002f;

        [SerializeField] private Platform _platformTargetGravity;
        [SerializeField] private Transform _ballPoint;
        [SerializeField] private BallSound _ballSound;
        [SerializeField] private float _speed;

        public event Action BoxTriggered;

        private Vector3 _currentDirection;
        private Vector3 _lastPlatformPosition;
        private Vector3 _lastVelosity;
        private Vector3 _gravityDirection;
        private Ball _ball;
        private bool _isGravityPlatform = false;
        private bool _isSetSpeed = false;
        private float _startSpeed;

        public Transform Transform { get; private set; }

        public Ball Ball => _ball;

        private void Awake()
        {
            _ball = GetComponent<Ball>();
            Transform = transform;
            _startSpeed = _speed;
        }

        private void FixedUpdate()
        {
            if (_ball.IsActive == false)
            {
                _currentDirection = Vector3.forward;
                FollowToPointPosition();
                return;
            }

            _ball.BallEffect.RotateTarget(_ball.Rigidbody.velocity);
            _lastVelosity = _ball.Rigidbody.velocity;
            _lastPlatformPosition = _platformTargetGravity.transform.position;

            if (_isGravityPlatform == true)
            {
                if (Vector3.Distance(_platformTargetGravity.transform.position, Transform.position) < CountDistance)
                {
                    _gravityDirection = (_platformTargetGravity.transform.position - Transform.position).normalized;
                    _currentDirection += new Vector3(_gravityDirection.x * GravityValue, PositionZero, _gravityDirection.z * GravityPositionZ);
                }
            }

            Move(_currentDirection, _speed);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_ball.IsActive == false)
                return;

            SetSpeed();
            Collision currentCollision = collision;
            Vector3 currentPoint = currentCollision.contacts[MinValue].point;

            if (currentCollision.gameObject.TryGetComponent(out ITrigger trigger))
            {
                trigger.Play(currentPoint);

                if (currentCollision.gameObject.TryGetComponent(out Fence fence))
                {
                    _ballSound.Play(fence.GetClip());

                    if (fence.IsHorizontal == false && fence.IsOpenPortal == false)
                    {
                        _currentDirection = Vector3.Reflect(_lastVelosity, collision.contacts[MinValue].normal).normalized;
                        _currentDirection.z -= GravityValue;
                        return;
                    }

                    if (fence.IsOpenPortal == true)
                    {
                        fence.Relocate(this, currentPoint, _currentDirection);
                        return;
                    }
                }

                _currentDirection = Vector3.Reflect(_lastVelosity, collision.contacts[MinValue].normal).normalized;

                if (currentCollision.gameObject.TryGetComponent(out Platform platform))
                    if (_isGravityPlatform == true)
                        _gravityDirection = Vector3.zero;
            }

            if (currentCollision.gameObject.TryGetComponent(out ISound sound))
                _ballSound.Play(sound.GetClip());

            if (currentCollision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(Damage);
                BoxTriggered?.Invoke();
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            float degreeSpeed = 13;

            if (collision.gameObject.TryGetComponent(out Platform platform))
            {
                _speed += degreeSpeed *= degreeSpeed;
                Vector3 newDirection = _lastPlatformPosition - _platformTargetGravity.transform.position;
                _currentDirection = (_currentDirection - newDirection).normalized;
            }
        }

        public void SetCurrentDirrection(Vector3 direction)
        {
            _currentDirection = direction;
        }

        public void SetGravity()
        {
            _isGravityPlatform = !_isGravityPlatform;
        }

        public void SetMaxSpeed()
        {
            _isSetSpeed = true;
            _speed = MaxSpeed;
        }

        public void SetStandartSpeed()
        {
            _isSetSpeed = false;
            _speed = _startSpeed;
        }

        public void Move(Vector3 direction)
        {
            Move(direction, _speed);
        }

        private void FollowToPointPosition()
        {
            Transform.position = _ballPoint.position;
        }

        private void Move(Vector3 direction, float speed)
        {
            _ball.Rigidbody.velocity = speed * Time.fixedDeltaTime * direction;
        }

        private void SetSpeed()
        {
            if (_isSetSpeed == true)
                return;

            if (_speed < MinValue)
                _speed = _startSpeed;

            if (_speed > _startSpeed)
                _speed -= (_speed - _startSpeed) / Divider;
        }
    }
}