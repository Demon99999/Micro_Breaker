using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.UI.AdditionalImprovement
{
    public class AdditionalImprovementTemplate : MonoBehaviour
    {
        [SerializeField] private AdditionalImprovementName _additionalImprovementName;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _description;
        [SerializeField] private int _firstPrice;
        [SerializeField] private int _secondprice;
        [SerializeField] private int _thirdPrice;

        public AdditionalImprovementName AdditionalImprovementName => _additionalImprovementName;

        public Sprite Sprite => _sprite;

        public string Description => _description;

        public int FirstPrice => _firstPrice;

        public int SecondPrice => _secondprice;

        public int ThirdPrice => _thirdPrice;
    }
}