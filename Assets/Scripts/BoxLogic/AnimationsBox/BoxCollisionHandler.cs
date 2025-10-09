using Assets.Scripts.BallLogic;
using UnityEngine;

namespace Assets.Scripts.BoxLogic.AnimationsBox
{
    public class BoxCollisionHandler : MonoBehaviour
    {
        private ColumnMovement columnController;

        public void SetColumnController(ColumnMovement controller)
        {
            columnController = controller;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Ball ball))
            {
                columnController?.OnBoxHit(GetComponent<Box>());
            }
        }
    }
}
