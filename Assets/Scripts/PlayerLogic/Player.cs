using Assets.Scripts.BallLogic;
using Assets.Scripts.PlatformLogic;
using UnityEngine;

namespace Assets.Scripts.PlayerLogic
{
    public class Player : MonoBehaviour
    {
        private const int CoinValue = 1;

        [SerializeField] private Wallet _wallet;
        [SerializeField] private BallMovement _ballMovement;
        [SerializeField] private PlatformTrigger _platformTrigger;

        private void OnEnable()
        {
            _ballMovement.BoxTriggered += OnAddCoins;
            _platformTrigger.CoinTriggered += OnAddCoins;
        }

        private void OnDisable()
        {
            _ballMovement.BoxTriggered -= OnAddCoins;
            _platformTrigger.CoinTriggered -= OnAddCoins;
        }

        private void OnAddCoins()
        {
            _wallet.AddCoin(CoinValue);
        }
    }
}