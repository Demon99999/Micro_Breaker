using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.AdditionalImprovement
{
    public class AdditionalImprovementView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Color _disableColor;

        private Color _oldColor;

        public Button Button => _button;

        private void Awake() => _oldColor = _button.image.color;

        public void Init(Sprite sprite, int price, int count)
        {
            SetState(true);
            _oldColor = _button.image.color;
            _image.sprite = sprite;
            _priceText.text = price.ToString();
            _levelText.text = count.ToString();
        }

        public void SetState(bool isEnable)
        {
            _button.image.color = _oldColor;

            if (isEnable == false) _button.image.color = _disableColor;

            _button.enabled = isEnable;
        }
    }
}