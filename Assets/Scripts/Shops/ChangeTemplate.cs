using System.Linq;
using Assets.Scripts.Enums;
using Assets.Scripts.Modification;
using UnityEngine;

namespace Assets.Scripts.Shops
{
    public class ChangeTemplate : MonoBehaviour
    {
        private const int MinValue = 0;
        private const int MaxValue = 1;

        [SerializeField] private ObjectModification _objectModification;

        private Transform _transform;
        private Template[] _templates;

        public Template CurrentTemplate { get; private set; }

        private void Awake()
        {
            Fill();
        }

        public void EnableCurrentTemplate(string name, int value)
        {
            bool isCanScale = true ? value == MaxValue : value == MinValue;

            CurrentTemplate = _templates.Where(template => template.Name == name).FirstOrDefault();

            if (name == string.Empty)
            {
                CurrentTemplate = _templates[MinValue];
            }

            CurrentTemplate.gameObject.SetActive(true);

            if (isCanScale == false) return;

            _objectModification.SetNewScale(BoosterNames.Positive, false);
        }

        private void Fill()
        {
            _transform = transform;
            _templates = new Template[_transform.childCount];

            for (int i = 0; i < _templates.Length; i++)
            {
                _transform.GetChild(i).TryGetComponent(out Template template);
                _templates[i] = template;
                _templates[i].gameObject.SetActive(false);
            }
        }
    }
}