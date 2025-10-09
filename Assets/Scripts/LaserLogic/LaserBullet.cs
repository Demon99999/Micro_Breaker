using Assets.Scripts.BoxLogic;
using UnityEngine;

namespace Assets.Scripts.LaserLogic
{
    [RequireComponent(typeof(Rigidbody))]
    public class LaserBullet : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private Rigidbody _rigidbody;
        private const int Damage = 5;
        private const float Speed = 300f;
        private const int MaxLiveTime = 5;
        private const int MinLiveTime = 0;
        private float _currentLiveTime;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _currentLiveTime = MaxLiveTime;
        }

        private void Update()
        {
            _rigidbody.AddForce(Speed * Time.deltaTime * Vector3.forward, ForceMode.Impulse);
            Die();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Box box))
            {
                _particleSystem.Play();
                box.Play(collision.contacts[0].point);
                box.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
        }

        private void Die()
        {
            _currentLiveTime -= Time.deltaTime;

            if (_currentLiveTime < MinLiveTime)
                gameObject.SetActive(false);
        }
    }
}