using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public abstract class AbstractButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            InitAwake();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        protected virtual void InitAwake()
        {
            _button = GetComponent<Button>();
        }

        protected abstract void OnClick();
    }
}