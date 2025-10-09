using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.BoxLogic;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.LocationLogic
{
    public class LocationAnimation : MonoBehaviour
    {
        //private const int HightPosition = 20;
        //private const int MinValue = 0;
        //private const float FirstDuration = 0.5f;
        //private const float SecondDuration = 0.01f;
        //private const float FirstAngle = 180f;
        //private const float SecondAngle = 180f;

        //private Box[] _boxesTransform;
        //private Transform _transform;
        //private Coroutine _coroutine;
        //private float _currentAngle = 0;

        //private void Start()
        //{
        //    _transform = transform;
        //    _boxesTransform = new Box[_transform.childCount];

        //    for (int i = 0; i < _boxesTransform.Length; i++)
        //    {
        //        _transform.GetChild(i).TryGetComponent(out Box box);
        //        _boxesTransform[i] = box;
        //        _boxesTransform[i].SetKinematic(true);
        //    }

        //    if (_coroutine != null) StopCoroutine(_coroutine);

        //    _coroutine = StartCoroutine(Move(ActiveBoxKinematic));
        //}

        //private void ActiveBoxKinematic()
        //{
        //    foreach (var box in _boxesTransform) box.SetKinematic(false);
        //}

        //private IEnumerator Move(Action Stated)
        //{
        //    List<Vector3> positions = new List<Vector3>();
        //    WaitForSeconds _waitForSeconds = new WaitForSeconds(SecondDuration);

        //    for (int i = 0; i < _boxesTransform.Length; i++)
        //    {
        //        positions.Add(_boxesTransform[i].transform.position);
        //        _boxesTransform[i].transform.DOMove(_transform.position + (Vector3.up + Vector3.forward) * HightPosition, MinValue);
        //    }

        //    for (int i = 0; i < _boxesTransform.Length; i++)
        //    {
        //        _currentAngle = _boxesTransform[i].transform.rotation.eulerAngles.y - FirstAngle;
        //        MoveWithRotate(positions[i], i, FirstDuration, _currentAngle);
        //        yield return _waitForSeconds;
        //    }

        //    yield return _waitForSeconds = new WaitForSeconds(FirstDuration);

        //    for (int i = 0; i < _boxesTransform.Length; i++)
        //    {
        //        _currentAngle = _boxesTransform[i].transform.rotation.eulerAngles.y - FirstAngle - SecondAngle;
        //        MoveWithRotate(positions[i] + Vector3.up, i, FirstDuration, _currentAngle);
        //        yield return _waitForSeconds = new WaitForSeconds(SecondDuration);
        //    }

        //    yield return _waitForSeconds = new WaitForSeconds(FirstDuration);

        //    for (int i = 0; i < _boxesTransform.Length; i++)
        //    {
        //        _currentAngle = _boxesTransform[i].transform.rotation.eulerAngles.y - FirstAngle - SecondAngle;
        //        MoveWithRotate(positions[i], i, FirstDuration, _currentAngle);
        //        yield return _waitForSeconds = new WaitForSeconds(SecondDuration);
        //    }

        //    yield return _waitForSeconds = new WaitForSeconds(FirstDuration);
        //    Stated?.Invoke();
        //    StopCoroutine(_coroutine);
        //}

        //private void MoveWithRotate(Vector3 position, int index, float duration, float angleY)
        //{
        //    _boxesTransform[index].transform.DOMove(position, duration);
        //    _boxesTransform[index].transform.DORotate(new Vector3(MinValue, angleY, MinValue), duration, RotateMode.FastBeyond360);
        //}
    }
}