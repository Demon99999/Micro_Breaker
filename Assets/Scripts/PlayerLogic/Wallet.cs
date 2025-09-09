using System;
using UnityEngine;

namespace Assets.Scripts.PlayerLogic
{
    public class Wallet : MonoBehaviour
    {
        private const int MinCoins = 0;

        public event Action<int> Changed;

        private int _coins = 0;

        public int Coin => _coins;

        public void Init(int coins)
        {
            _coins = coins;

            Changed?.Invoke(_coins);
        }

        public void AddCoin(int coins)
        {
            if (coins < MinCoins)
                return;

            _coins += coins;

            Changed?.Invoke(_coins);
        }

        public void RemoveCoins(int coins)
        {
            if (_coins < coins)
                return;

            _coins -= coins;

            Changed?.Invoke(_coins);
        }
    }
}