using Assets.Scripts.BallLogic;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.BoosterLogic.Boosters
{
    public class BoosterAddBalls : AbstractBooster
    {
        private const int Count = 2;
        private const float RandomValue = 1;
        private const int Speed = 1000;
        private const int MinValue = 0;

        [SerializeField] private BallMovement _ballMovement;
        [SerializeField] private Transform _createPoint;

        private BallMovement[] _ballMovements;

        public override void StopAction(BoosterEffect boosterEffect)
        {
            if (boosterEffect.IsActive == false)
                return;

            boosterEffect.SetActionActive();
            _ballMovement.Ball.BallEffect.SetParticleSystem(BoosterNames.Default);

            foreach (var ball in _ballMovements) ball.gameObject.SetActive(false);
        }

        public override void OnStartAction(BoosterEffect boosterEffect)
        {
            boosterEffect.SetActionActive();
            _ballMovements = new BallMovement[Count];
            _ballMovement.Ball.BallEffect.SetParticleSystem(BoosterName);

            for (int i = 0; i < _ballMovements.Length; i++)
            {
                _ballMovements[i] = Instantiate(_ballMovement, Transform);
                _ballMovements[i].Ball.OnDisconnectParentObject();
                _ballMovements[i].Ball.BallEffect.SetParticleSystem(BoosterName);
                _ballMovements[i].Ball.ChangeTemplate.EnableCurrentTemplate(_ballMovement.Ball.ChangeTemplate.CurrentTemplate.Name, MinValue);
                _ballMovements[i].SetCurrentDirrection(new Vector3(Random.Range(-RandomValue, RandomValue), transform.position.y, RandomValue));
            }
        }
    }
}