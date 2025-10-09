using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.BallLogic;
using Assets.Scripts.BoosterLogic;
using Assets.Scripts.Envierment;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.PlatformLogic;
using Assets.Scripts.PlayerLogic;
using Assets.Scripts.SaveLoadServices;
using Assets.Scripts.UI.Buttons;
using Assets.Scripts.UI.Panels;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class GameState : MonoBehaviour
    {
        private const int Priority = 2;
        private const int DurationWin = 3;
        private const int DurationLoss = 1;
        private const int PassedValue = 1;

        [SerializeField] private Counter _counter;
        [SerializeField] private BorderCollisionWithLoss _triggerLoss;
        [SerializeField] private PanelWin _panelWin;
        [SerializeField] private PanelLoss _panelLoss;
        [SerializeField] private CinemachineVirtualCamera _virtualEndCamera;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private BallMovement _ballMovement;
        [SerializeField] private InputPointMovement _inputPointMovement;
        [SerializeField] private SaveService _saveService;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private PanelFade _panelFade;
        [SerializeField] private BoostersContainer _boostersContainer;
        [SerializeField] private ButtonPanelInteraction _buttonPanelInteraction;

        private WaitForSeconds _waitForSeconds;
        private Coroutine _coroutine;
        private bool _isResetting = false;

        private void Awake()
        {
            if (_boostersContainer == null)
            {
                _boostersContainer = FindObjectOfType<BoostersContainer>(true);
            }
        }

        private void OnEnable()
        {
            _counter.Winned += OnOpenPanelWin;
            _triggerLoss.Lost += OnOpenPanelLoss;
            _panelWin.Clicked += OnLoadScene;
            _panelLoss.Clicked += OnLoadScene;
        }

        private void OnDisable()
        {
            _counter.Winned -= OnOpenPanelWin;
            _triggerLoss.Lost -= OnOpenPanelLoss;
            _panelWin.Clicked -= OnLoadScene;
            _panelLoss.Clicked -= OnLoadScene;
        }

        private void Start()
        {
            _waitForSeconds = new WaitForSeconds(DurationWin);
        }

        private void OnOpenPanelWin(string time)
        {
            _virtualEndCamera.Priority = Priority;

            _waitForSeconds = new WaitForSeconds(DurationWin);

            PlayOpen(_panelWin, _waitForSeconds, () =>
            {
                _panelWin.Fill(time, _counter.CountLiveBoxs.ToString(), _wallet.Coin.ToString());
                SaveGameProgress();
            });

            SafeResetAndDisableBoosters();
        }

        private void OnOpenPanelLoss()
        {
            _waitForSeconds = new WaitForSeconds(DurationLoss);

            PlayOpen(_panelLoss, _waitForSeconds, () =>
            {
                _panelLoss.Fill(_counter.CountLiveBoxs.ToString(), _wallet.Coin.ToString());
            });

            SafeResetAndDisableBoosters();
        }

        private void SafeResetAndDisableBoosters()
        {
            if (_isResetting) return;

            _isResetting = true;

            try
            {
                if (_boostersContainer != null)
                {
                    if (_boostersContainer.gameObject.activeInHierarchy)
                    {
                        _boostersContainer.Reset();
                    }

                    StartCoroutine(DisableBoostersWithDelay());
                }
                else
                {
                    Debug.LogWarning("BoostersContainer is null in GameState");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Exception in SafeResetAndDisableBoosters: {e}");
            }
            finally
            {
                _isResetting = false;
            }
        }

        private IEnumerator DisableBoostersWithDelay()
        {
            yield return new WaitForEndOfFrame();

            if (_boostersContainer != null)
            {
                _boostersContainer.gameObject.SetActive(false);
            }
        }

        private void PlayOpen(Panel panel, WaitForSeconds waitForSeconds, Action action = null)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(MovePanel(panel, waitForSeconds, action));
        }

        private IEnumerator MovePanel(Panel panel, WaitForSeconds waitForSeconds, Action action = null)
        {
            SafeDisableComponents();

            _playerInput.SetControl();
            yield return waitForSeconds;

            panel.OnMove(true);
            action?.Invoke();
        }

        private void SafeDisableComponents()
        {
            try
            {
                if (_buttonPanelInteraction != null)
                    _buttonPanelInteraction.gameObject.SetActive(false);

                if (_inputPointMovement != null)
                    _inputPointMovement.gameObject.SetActive(false);

                if (_ballMovement != null)
                    _ballMovement.gameObject.SetActive(false);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Exception in SafeDisableComponents: {e}");
            }
        }

        private void SaveGameProgress()
        {
            _saveService.SaveCoins(_saveService.Coins + _wallet.Coin);

            if (_saveService.LevelData.Passed == PassedValue)
                return;

            List<LevelData> LevelData = _saveService.LevelDatas.ToList();

            LevelData.Add(new LevelData()
            {
                LocationName = _saveService.LevelData.LocationName,
                AdditionaValue = _saveService.LevelData.AdditionaValue,
                Active = PassedValue,
                Passed = PassedValue
            });

            _saveService.SaveLevelDatas(LevelData);
        }

        private void OnLoadScene(string sceneName)
        {
            _panelFade.SetActive(false, () => { SceneManager.LoadScene(sceneName); });
        }
    }
}