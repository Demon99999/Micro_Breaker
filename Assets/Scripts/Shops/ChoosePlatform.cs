using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Shops
{
    public class ChoosePlatform : MonoBehaviour
    {
        private const int MinValue = 0;
        private const int Element = 1;
        private const int LoopValue = 1;
        private const float DurationMove = 0.5f;
        private const float DurationRotate = 5f;
        private const float RotateY = 360f;
        private const float Scale = 13f;

        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _oldTransform;
        [SerializeField] private Transform _currentTransform;
        [SerializeField] private Transform _nextTransform;

        private readonly List<Template> _platformTemplates = new List<Template>();
        private Tween _rotateTween;
        private int _currentIndex = MinValue;

        public Template CurrentTemplate { get; private set; }

        public void Init(List<Product> products)
        {
            Create(products);
            _currentIndex = products.IndexOf(products.Where(product => product.IsSelected == true).FirstOrDefault());
            SetCurrentPlatform(MinValue);
        }

        private void Create(List<Product> products)
        {
            for (int i = MinValue; i < products.Count; i++)
            {
                _platformTemplates.Add(Instantiate(products[i].Template, _spawnPoint));
                _platformTemplates[i].transform.DOScale(new Vector3(Scale, Scale, Scale), MinValue);
            }
        }

        public void SetCurrentPlatform(int element)
        {
            foreach (var template in _platformTemplates) template.gameObject.SetActive(false);

            CurrentTemplate = _platformTemplates[GetCurrentIndex(element)];
            CurrentTemplate.gameObject.SetActive(true);
            MoveChangePlatform();
        }

        private void MoveChangePlatform()
        {
            if (_currentIndex > MinValue)
                SetPlatformState(_platformTemplates[_currentIndex - Element], _oldTransform);

            SetPlatformState(CurrentTemplate, _currentTransform);
            PlatformRotate();

            if (_currentIndex < _platformTemplates.Count - Element)
                SetPlatformState(_platformTemplates[_currentIndex + Element], _nextTransform);
        }

        private void SetPlatformState(Template template, Transform needTransform)
        {
            template.gameObject.SetActive(true);
            template.transform.DOMove(needTransform.position, DurationMove);
            template.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        private async void PlatformRotate()
        {
            if (_rotateTween != null && _rotateTween.IsPlaying() == true) _rotateTween.Kill();

            await RotateMove();
        }

        public async Task RotateMove()
        {
            _rotateTween = CurrentTemplate.transform.DORotate(new Vector3(MinValue, RotateY, MinValue), DurationRotate, RotateMode.FastBeyond360).
                SetLoops(-LoopValue).SetRelative().SetEase(Ease.Linear);

            Task tween = _rotateTween.AsyncWaitForCompletion();

            await tween;
        }

        private int GetCurrentIndex(int element)
        {
            int newIndex = _currentIndex + element;

            if (newIndex < MinValue) return _currentIndex;

            if (newIndex >= _platformTemplates.Count - Element)
                return _currentIndex = _platformTemplates.Count - Element;

            return _currentIndex = newIndex;
        }
    }
}