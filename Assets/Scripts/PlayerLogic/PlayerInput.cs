using System;
using Assets.Scripts.Envierment;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.PlayerLogic
{
    public class PlayerInput : MonoBehaviour
    {
        private const int MinValue = 0;
        private const string AxsisX = "Mouse X";
        private const string AxsisY = "Mouse Y";

        public event Action<Vector3, Vector3> MousePressed;
        public event Action<bool> MouseUped;
        public event Action MousePressedUp;

        private bool _isInputPlatform;

        public bool IsControl { get; private set; } = false;

        private void Update()
        {
            if (IsControl == false)
                return;

            if (IsMouseOverUI())
                return;

            if (Input.GetMouseButton(MinValue))
            {
                MouseUped?.Invoke(Input.GetMouseButton(MinValue));
                MousePressed?.Invoke(GetPosition(), GetRaycastPoint());
            }
            else
            {
                MouseUped?.Invoke(Input.GetMouseButtonUp(MinValue));
            }

            if (Input.GetMouseButtonUp(MinValue) && _isInputPlatform == true)
            {
                MousePressedUp?.Invoke();
            }
        }

        public void SetControl()
        {
            IsControl = !IsControl;
        }

        private Vector3 GetRaycastPoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _isInputPlatform = hit.collider.gameObject.TryGetComponent(out Ground ground);
                return hit.point;
            }

            return Vector3.zero;
        }

        private bool IsMouseOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private Vector3 GetPosition()
        {
            float positionX = Input.GetAxis(AxsisX);
            float positionY = Input.GetAxis(AxsisY);
            return new Vector3(positionX, MinValue, positionY);
        }
    }
}