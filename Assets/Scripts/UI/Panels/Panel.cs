using System;
using System.Threading.Tasks;
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

        protected async Task MovePanel(bool isActive, Action OnActive = null)
        {
            if (isActive == true) gameObject.SetActive(isActive);

            if (isActive)
            {
                _tween = _rectTransform.DOAnchorPosY(_topPosition, _tweenDurationOpen).SetUpdate(true);
                await _tween.AsyncWaitForCompletion();
            }
            else
            {
                _tween = _rectTransform.DOAnchorPosY(_middlePosition, _tweenDurationClose).SetUpdate(true);
                await _tween.AsyncWaitForCompletion();
            }

            gameObject.SetActive(isActive);

            OnActive?.Invoke();
        }
    }
}