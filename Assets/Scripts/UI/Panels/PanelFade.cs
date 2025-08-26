using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
    [RequireComponent(typeof(CanvasGroup), typeof(Image))]
    public class PanelFade : MonoBehaviour
    {
        private const float MinValueAlpha = 0;
        private const float MaxValueAlpha = 1;

        [SerializeField] private float _duration;

        private CanvasGroup _canvasGroup;
        private Image _image;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _image = GetComponent<Image>();
            SetActive(true);
        }

        public void SetActive(bool isActive, TweenCallback tweenCallback = null)
        {
            if (isActive == false)
            {
                _image.raycastTarget = true;
                _canvasGroup.DOFade(MaxValueAlpha, _duration).OnComplete(tweenCallback);
            }
            else
            {
                _canvasGroup.DOFade(MinValueAlpha, _duration).OnComplete(() => { _image.raycastTarget = false; tweenCallback?.Invoke(); });
            }
        }
    }
}