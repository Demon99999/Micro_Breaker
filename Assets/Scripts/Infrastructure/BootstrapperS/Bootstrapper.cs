using Assets.Scripts.PlayerLogic;
using Assets.Scripts.SaveLoadServices;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.BootstrapperS
{
    [RequireComponent(typeof(SaveService), typeof(Wallet))]
    public abstract class Bootstrapper : MonoBehaviour
    {
        protected SaveService SaveService { get; private set; }

        protected Wallet Wallet { get; private set; }

        private void Awake()
        {
            SaveService = GetComponent<SaveService>();
            Wallet = GetComponent<Wallet>();
        }

        private void OnEnable()
        {
            SaveService.Loaded += OnInit;
        }

        private void OnDisable()
        {
            SaveService.Loaded -= OnInit;
        }

        protected abstract void OnInit();
    }
}
