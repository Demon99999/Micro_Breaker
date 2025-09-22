using System.Collections;
using Assets.Scripts.BoosterLogic;
using Assets.Scripts.BoxLogic;
using Assets.Scripts.LocationLogic;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.CameraLogic
{
    public class CameraShake : MonoBehaviour
    {
        private const float MaxDelta = 25f;
        private const float NewPositionX = 20;
        private const float MinValue = 0;
        private const float ShakeTimer = 0.3f;
        private const float Intensety = 1.5f;

        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private LocationCreate _locationCreate;
        [SerializeField] private BoxesFalling _boxesFalling;

        private Coroutine _coroutineMove;
        private Coroutine _coroutineShake;
        private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
        private CinemachineTransposer _transposer;

        private void Awake()
        {
            _multiChannelPerlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
        }

        private void OnEnable()
        {
            _locationCreate.Inited += onInit =>
            {
                foreach (Box box in _locationCreate.CurrentLocation.BoxContainer.Boxes) box.Died += OnShake;

                foreach (Box box in _boxesFalling.Boxes) box.Died += OnShake;
            };
        }

        private void OnDisable()
        {
            _locationCreate.Inited += onInit =>
            {
                foreach (Box box in _locationCreate.CurrentLocation.BoxContainer.Boxes)
                    box.Died -= OnShake;

                foreach (Box box in _boxesFalling.Boxes)
                    box.Died -= OnShake;
            };
        }

        public void OnShake()
        {
            if (_coroutineShake != null)
                StopCoroutine(_coroutineShake);

            _coroutineShake = StartCoroutine(PlayShake());
        }

        public void Destabilize()
        {
            if (_coroutineMove != null)
                StopCoroutine(_coroutineMove);

            _coroutineMove = StartCoroutine(Move());
        }

        public void Stabilize()
        {
            _transposer.m_FollowOffset.x = 0f;
        }

        private IEnumerator PlayShake()
        {
            _multiChannelPerlin.m_AmplitudeGain = Intensety;
            yield return new WaitForSeconds(ShakeTimer);
            _multiChannelPerlin.m_AmplitudeGain = MinValue;
        }

        private IEnumerator Move()
        {
            while (_transposer.m_FollowOffset.x != NewPositionX)
            {
                _transposer.m_FollowOffset.x = GetPositionX(NewPositionX);
                yield return null;
            }

            while (_transposer.m_FollowOffset.x != -NewPositionX)
            {
                _transposer.m_FollowOffset.x = GetPositionX(-NewPositionX);
                yield return null;
            }

            while (_transposer.m_FollowOffset.x != NewPositionX)
            {
                _transposer.m_FollowOffset.x = GetPositionX(NewPositionX);
                yield return null;
            }

            while (_transposer.m_FollowOffset.x != MinValue)
            {
                _transposer.m_FollowOffset.x = GetPositionX(MinValue);
                yield return null;
            }

            StopCoroutine(_coroutineMove);
        }

        private float GetPositionX(float newPositionX)
        {
            return Mathf.MoveTowards(_transposer.m_FollowOffset.x, newPositionX, MaxDelta * Time.deltaTime);
        }
    }
}