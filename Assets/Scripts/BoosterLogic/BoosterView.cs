using Assets.Scripts.Enums;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BoosterLogic
{
    public class BoosterView : MonoBehaviour
    {
        private const float MinFadeValue = 0;
        private const float Duration = 4;

        [SerializeField] private Image _image;
        [SerializeField] private Color _colorPositive;
        [SerializeField] private Color _colorNegative;
        [SerializeField] private Color _colorDefult;

        public void Init(Sprite sprite, BoosterNames boosterNames)
        {
            _image.sprite = sprite;
            SetColor(boosterNames);
            _image.DOFade(MinFadeValue, Duration).OnComplete(Die);
        }

        private void Die() => Destroy(gameObject);

        private void SetColor(BoosterNames boosterNames)
        {
            if (boosterNames == BoosterNames.Positive)
                _image.color = _colorPositive;

            if (boosterNames == BoosterNames.Negative)
                _image.color = _colorNegative;

            if (boosterNames == BoosterNames.Default)
                _image.color = _colorDefult;
        }
    }
}