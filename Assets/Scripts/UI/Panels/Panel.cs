using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    [RequireComponent(typeof(RectTransform))]
    public class Panel : MonoBehaviour
    {
        [SerializeField] private float _topPosition;
        [SerializeField] private float _middlePosition;
        [SerializeField] private float _tweenDurationOpen;
        [SerializeField] private float _tweenDurationClose;
        [SerializeField] private bool _isActiveAtStart = false;

        public event Action Moved;

        private RectTransform _rectTransform;
        private Tween _tween;

        protected RectTransform RectTransform => _rectTransform;

        protected float TopPosition => _topPosition;

        protected float MiddlePosition => _middlePosition;

        protected float TweenDurationOpen => _tweenDurationOpen;

        private void Awake() => InitAwake();

        public virtual void OnMove(bool isAction) => Moved?.Invoke();

        protected virtual void InitAwake()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (_isActiveAtStart == true) gameObject.SetActive(false);
        }

        protected async UniTask MovePanel(bool isActive, Action OnActive = null)
        {

            if (isActive)
                gameObject.SetActive(true);

            float targetY = isActive ? _topPosition : _middlePosition;
            float duration = isActive ? _tweenDurationOpen : _tweenDurationClose;

            await _rectTransform.DOAnchorPosY(targetY, duration)
                .SetUpdate(true)
                .AsyncWaitForCompletion();

            gameObject.SetActive(isActive);
            OnActive?.Invoke();
            //if (isActive == true) gameObject.SetActive(isActive);

            //if (isActive)
            //{
            //    _tween = _rectTransform.DOAnchorPosY(_topPosition, _tweenDurationOpen).SetUpdate(true);
            //    await _tween.AsyncWaitForCompletion();
            //}
            //else
            //{
            //    _tween = _rectTransform.DOAnchorPosY(_middlePosition, _tweenDurationClose).SetUpdate(true);
            //    await _tween.AsyncWaitForCompletion();
            //}

            //gameObject.SetActive(isActive);

            //OnActive?.Invoke();
        }

        //protected IEnumerator MovePanelCoroutine(bool isActive, Action OnActive = null)
        //{
        //    if (isActive == true)
        //        gameObject.SetActive(isActive);

        //    if (isActive)
        //    {
        //        _tween = _rectTransform.DOAnchorPosY(_topPosition, _tweenDurationOpen).SetUpdate(true);
        //        yield return _tween.WaitForCompletion();
        //    }
        //    else
        //    {
        //        _tween = _rectTransform.DOAnchorPosY(_middlePosition, _tweenDurationClose).SetUpdate(true);
        //        yield return _tween.WaitForCompletion();
        //    }

        //    gameObject.SetActive(isActive);
        //    OnActive?.Invoke();
        //}
    }
}