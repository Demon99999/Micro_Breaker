using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class BackGroundStartScene : MonoBehaviour
    {
        private const float MinRandomValue = 0.1f;
        private const float MaxRandomValue = 1;
        private const float Duration = 1.0f;
        private const float Delay = 0.01f;

        private Transform _transform;
        private Transform[] _transformsBox;
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(Delay);

        private void Start()
        {
            Fill();
        }
        
        private void Fill()
        {
            _transform = transform;
            _transformsBox = new Transform[_transform.childCount];

            for (int i = 0; i < _transformsBox.Length; i++)
            {
                _transformsBox[i] = _transform.GetChild(i);
            }

            MoveCoroutine();
        }

        private void MoveCoroutine()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            float randomX;

            for (int i = 0; i < _transformsBox.Length; i++)
            {
                randomX = Random.Range(MinRandomValue, MaxRandomValue);
                _transformsBox[i].DOLocalMoveX(randomX, Duration).SetEase(Ease.InOutSine).SetLoops(-(int)Duration, LoopType.Yoyo);
                yield return _waitForSeconds;
            }

            StopCoroutine(_coroutine);
        }
    }
}