using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.BoxLogic
{
    public class BoxTemplate : MonoBehaviour
    {
        [SerializeField] private BoosterNames _boosterNames;

        public BoosterNames BoosterNames => _boosterNames;
    }
}