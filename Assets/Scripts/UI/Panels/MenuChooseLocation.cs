using System.Collections;
using Assets.Scripts.LocationLogic;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
    public class MenuChooseLocation : MonoBehaviour
    {
        private const int PositionZ = 20;
        private const int Priority = 1;

        [SerializeField] private PanelPlayGame _panelPlayGame;
        [SerializeField] private PanelShop _panelShop;
        [SerializeField] private LocationChooseInput _locationChooseInput;
        [SerializeField] private SwipeMove _swipeMove;
        [SerializeField] private CinemachineVirtualCamera _virtualMobileCamera;

        private void OnEnable()
        {
            _panelShop.Activated += OnActive;
            _locationChooseInput.LocationChoosed += OnInit;
            _panelPlayGame.ButtonClose.Clicked += OnActivateControl;
            _panelPlayGame.ButtonPlayGame.Clicked += OnClick;
        }

        private void OnDisable()
        {
            _panelShop.Activated -= OnActive;
            _locationChooseInput.LocationChoosed -= OnInit;
            _panelPlayGame.ButtonClose.Clicked -= OnActivateControl;
            _panelPlayGame.ButtonPlayGame.Clicked -= OnClick;
        }

        private void OnInit(LocationObject locationObject)
        {
            if (IsLocationObjectIdentity(locationObject) == true && _panelPlayGame.IsOpen == true)
            {
                ClosePanel();
                return;
            }

            if (IsLocationObjectIdentity(locationObject) == false && _panelPlayGame.IsOpen == true)
            {
                StartCoroutine(ChangeLocationObjectInPanel(locationObject));
                return;
            }

            _virtualMobileCamera.transform.position = new Vector3(locationObject.transform.position.x,
                                                            _virtualMobileCamera.transform.position.y,
                                                                  locationObject.transform.position.z - PositionZ);
            _panelPlayGame.ButtonClose.SetStartStateButton();
            OpenPanel(locationObject);
        }

        private IEnumerator ChangeLocationObjectInPanel(LocationObject locationObject)
        {
            ClosePanel();
            yield return new WaitForSeconds(0.5f);
            OpenPanel(locationObject);
        }

        private void OpenPanel(LocationObject locationObject)
        {
            _panelPlayGame.OnMove(true);
            _panelPlayGame.Init(locationObject);
            OnActivateControl(true);
        }

        private void ClosePanel()
        {
            _panelPlayGame.OnMove(false);
            OnActivateControl(false);
        }

        private bool IsLocationObjectIdentity(LocationObject locationObject)
        {
            if (_panelPlayGame.LocationObject != null &&
                _panelPlayGame.LocationObject.Name == locationObject.Name &&
                _panelPlayGame.LocationObject.AdditionaValue == locationObject.AdditionaValue)
                return true;

            return false;
        }

        private void OnActive(bool isActive)
        {
            OnActivateControl(isActive);

            _panelPlayGame.OnMove(false);

            if (_panelShop.IsActive == false && _panelPlayGame.IsInit == true && _panelPlayGame.ButtonClose.IsClick == false)
            {
                _panelPlayGame.OnMove(true);
                OnActivateControl(true);
            }
        }

        private void OnActivateControl(bool isActive) => _swipeMove.enabled = !isActive;

        private void OnClick()
        {
            _virtualMobileCamera.gameObject.SetActive(true);
            _virtualMobileCamera.Priority = Priority;
        }
    }
}