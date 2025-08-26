using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Shops
{
    public class SwipePanel : MonoBehaviour
    {
        private const int NextElement = 1;

        public event Action<int> Swiped;

        private Vector3 _startInputPosition;
        private Vector3 _currentInputPosition;
        private bool _wasPressed = false;

        private void Update()
        {
            if (HaveInputOnPanel() == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _wasPressed = true;
                    _startInputPosition = Input.mousePosition;
                }

                if (Input.GetMouseButton(0)) _currentInputPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0) && _wasPressed == true)
            {
                float distance = _currentInputPosition.x - _startInputPosition.x;
                int currentSwipeElement = 0;

                if (-150 < distance && distance < 150) return;

                if (distance < -150) currentSwipeElement = NextElement;
                else if (distance > 150) currentSwipeElement = -NextElement;

                Swiped?.Invoke(currentSwipeElement);
                _wasPressed = false;
            }
        }

        private bool HaveInputOnPanel()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            var result = raycastResults.Where(ray => ray.gameObject.TryGetComponent(out SwipePanel swipePanel)).FirstOrDefault();

            if (result.gameObject != null)
            {
                return true;
            }
            
            return false;
        }
    }
}