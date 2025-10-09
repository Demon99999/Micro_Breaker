using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LaserLogic
{
    public class LaserGun : MonoBehaviour
    {
        private const float WaitSeconds = 0.5f;

        [SerializeField] private LaserBullet _laserBullet;
        [SerializeField] private int _countBullets;
        [SerializeField] private Transform _firstShootPoints;
        [SerializeField] private Transform _secondShootPoints;
        [SerializeField] private AudioSource _audioSource;

        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds;
        private Transform _currentPointTransform;

        private readonly List<LaserBullet> _laserBullets = new List<LaserBullet>();

        private void Awake()
        {
            Create();
        }

        public void Shooting(Action shotBack)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(EnableBullets(shotBack));
        }

        public void StopShoot()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private void Create()
        {
            _waitForSeconds = new WaitForSeconds(WaitSeconds);
            _currentPointTransform = _secondShootPoints;

            for (int i = 0; i < _countBullets; i++)
            {
                _laserBullets.Add(Instantiate(_laserBullet, GetPosition()));
                _laserBullets[i].gameObject.SetActive(false);
            }
        }

        private IEnumerator EnableBullets(Action shotBack)
        {
            foreach (var laserBullet in _laserBullets)
            {
                laserBullet.gameObject.SetActive(true);
                laserBullet.transform.parent = null;
                _audioSource.Play();
                yield return _waitForSeconds;
            }

            shotBack?.Invoke();
            StopCoroutine(_coroutine);
        }

        private Transform GetPosition()
        {
            if (_currentPointTransform == _secondShootPoints)
                return _currentPointTransform = _firstShootPoints;

            if (_currentPointTransform == _firstShootPoints)
                return _currentPointTransform = _secondShootPoints;

            return _currentPointTransform;
        }
    }
}