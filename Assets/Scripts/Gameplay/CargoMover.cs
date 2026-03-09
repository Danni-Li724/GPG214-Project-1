using UnityEngine;

namespace Game.Gameplay
{
    public class CargoMover : MonoBehaviour
    {
        public enum State
        {
            ToPackaging,
            WaitingForPackaging,
            ToDelivery,
            WaitingAtDelivery,
            Carried,
            Done
        }

        [SerializeField] private float moveSpeed = 2f;

        private Transform packagingPoint;
        private Transform deliveryPoint;

        public State CurrentState { get; private set; }

        public void Configure(Transform packaging, Transform delivery)
        {
            packagingPoint = packaging;
            deliveryPoint = delivery;
            CurrentState = State.ToPackaging;
        }

        private void Update()
        {
            if (CurrentState == State.ToPackaging)
            {
                MoveTowards(packagingPoint, State.WaitingForPackaging);
            }
            else if (CurrentState == State.ToDelivery)
            {
                MoveTowards(deliveryPoint, State.WaitingAtDelivery);
            }
        }

        private void MoveTowards(Transform target, State arriveState)
        {
            if (target == null) return;

            Vector3 to = target.position - transform.position;
            if (to.sqrMagnitude <= 0.01f)
            {
                transform.position = target.position;
                CurrentState = arriveState;
                return;
            }

            Vector3 step = to.normalized * moveSpeed * Time.deltaTime;
            transform.position += step;
        }

        public void MarkPackagedAndContinue()
        {
            if (CurrentState == State.WaitingForPackaging)
            {
                CurrentState = State.ToDelivery;
            }
        }

        public void MarkPickedUp()
        {
            CurrentState = State.Carried;
        }

        public void MarkDelivered()
        {
            CurrentState = State.Done;
        }
    }
}