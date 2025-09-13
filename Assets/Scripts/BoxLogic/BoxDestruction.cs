using UnityEngine;

namespace Assets.Scripts.BoxLogic
{
    [RequireComponent(typeof(Box), typeof(BoxCollider), typeof(Rigidbody))]
    public class BoxDestruction : MonoBehaviour
    {
        [SerializeField] private Transform _boxDestructionTemplate;

        private Rigidbody _rigidbody;
        private BoxCollider _boxCollider;
        private Box _box;

        private void Awake()
        {
            _box = GetComponent<Box>();
            _boxCollider = GetComponent<BoxCollider>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _box.Died += OnDied;
        }

        private void OnDisable()
        {
            _box.Died -= OnDied;
        }

        private void OnDied()
        {
            _boxCollider.isTrigger = true;
            _rigidbody.isKinematic = true;
            _box.DisableBoxTemplate();
            _boxDestructionTemplate.gameObject.SetActive(true);
        }
    }
}