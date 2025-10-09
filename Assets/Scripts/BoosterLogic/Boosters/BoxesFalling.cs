using Assets.Scripts.BoxLogic;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class BoxesFalling : AbstractBooster
    {
        [SerializeField] private Transform _pathBoxesContainer;
        [SerializeField] private ParticleSystem _particleSystem;

        public Box[] Boxes { get; private set; }

        public bool IsActive { get; private set; } = false;

        private void Start() => Fill();
        
        public override void StopAction(BoosterEffect _)
        {

        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            IsActive = true;
            _pathBoxesContainer.gameObject.SetActive(true);
            boosterEffect.SetActionActive();
        }

        private void Fill()
        {
            Boxes = new Box[_pathBoxesContainer.childCount];

            for (int i = 0; i < Boxes.Length; i++)
            {
                _pathBoxesContainer.GetChild(i).TryGetComponent(out Box box);
                Boxes[i] = box;
                Boxes[i].InitEffect(_particleSystem);
            }
        }
    }
}