using System;
using Assets.Scripts.LocationLogic;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class Counter : MonoBehaviour
    {
        private const int Seconds = 60;

        [SerializeField] private LocationCreate _locationCreate;

        public event Action<string> Winned;

        private float _time = 0;

        public Location CurrentLocation { get; private set; }

        public int CountLiveBoxs { get; private set; }

        private void OnEnable()
        {
            _locationCreate.Inited += OnInit;
            _locationCreate.Inited += OnInit =>
            {
                foreach (var box in CurrentLocation.BoxContainer.Boxes)
                    box.Died += OnSetDestroyBox;
            };
        }

        private void OnDisable()
        {
            _locationCreate.Inited -= OnInit;
            _locationCreate.Inited -= OnInit =>
            {
                foreach (var box in CurrentLocation.BoxContainer.Boxes)
                    box.Died -= OnSetDestroyBox;
            };
        }

        public void UpdateTime()
        {
            _time += Time.deltaTime;
        }

        private void OnInit(Location currentLocation)
        {
            CurrentLocation = currentLocation;
        }

        private string GetResultTime()
        {
            return $"{(int)_time / Seconds}:{(int)_time}";
        }

        private void OnSetDestroyBox()
        {
            foreach (var box in CurrentLocation.BoxContainer.Boxes)
                box.Rigidbody.WakeUp();

            CountLiveBoxs++;

            if (CountLiveBoxs == CurrentLocation.BoxContainer.Boxes.Length)
                Winned?.Invoke(GetResultTime());
        }
    }
}