using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ViewIndicationPlatform : MonoBehaviour
    {
        [SerializeField] private Transform _path;
        [SerializeField] private ImageIndicator _imageIndicator;

        private ImageIndicator[] _imageIndicators;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _imageIndicators = new ImageIndicator[_path.childCount];

            for (int i = 0; i < _imageIndicators.Length; i++)
            {
                ImageIndicator imageIndicator = Instantiate(_imageIndicator, _transform);
                _imageIndicators[i] = imageIndicator;
                _imageIndicators[i].SetAction(false);
            }
        }
    }
}