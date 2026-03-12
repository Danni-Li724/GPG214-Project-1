using UnityEngine;

namespace Game.Gameplay
{
    public class CargoMover : MonoBehaviour
    {
        public enum State
        {
            ToWaitPoint,
            WaitingInQueue,
            ToPackaging,
            WaitingForPackaging,
            ToPackageWaitingPoint,
            WaitingForHandoffQueue,
            ToDeliveryPoint,
            WaitingAtHandoff,
            Carried,
            Done
        }

        [SerializeField] private float moveSpeed = 2f;

        private Transform waitPoint;
        private Transform packagingPoint;
        private Transform packageWaitingPoint;
        private Transform deliveryPoint;

        public State CurrentState { get; private set; }

        public System.Action<CargoMover> ArrivedAtWait;
        public System.Action<CargoMover> ArrivedAtPackageWait;

        public void Configure(
            Transform wait,
            Transform packaging,
            Transform packageWait,
            Transform delivery
        )
        {
            waitPoint = wait;
            packagingPoint = packaging;
            packageWaitingPoint = packageWait;
            deliveryPoint = delivery;

            CurrentState = State.ToWaitPoint;
        }

        private void Update()
        {
            if (CurrentState == State.ToWaitPoint)
            {
                MoveTowards(waitPoint, State.WaitingInQueue, onArrive: () => ArrivedAtWait?.Invoke(this));
            }
            else if (CurrentState == State.ToPackaging)
            {
                MoveTowards(packagingPoint, State.WaitingForPackaging);
            }
            else if (CurrentState == State.ToPackageWaitingPoint)
            {
                MoveTowards(packageWaitingPoint, State.WaitingForHandoffQueue, onArrive: () => ArrivedAtPackageWait?.Invoke(this));
            }
            else if (CurrentState == State.ToDeliveryPoint)
            {
                MoveTowards(deliveryPoint, State.WaitingAtHandoff);
            }
        }

        private void MoveTowards(Transform target, State arriveState, System.Action onArrive = null)
        {
            if (target == null) return;

            Vector3 to = target.position - transform.position;

            if (to.sqrMagnitude <= 0.01f)
            {
                transform.position = target.position;
                CurrentState = arriveState;
                onArrive?.Invoke();
                return;
            }

            transform.position += to.normalized * moveSpeed * Time.deltaTime;
        }

        // called by queue controller when this cargo becomes the "active" one
        public void ReleaseToPackaging()
        {
            // if hasn't arrived yet, go straight to packaging instead of waiting
            if (CurrentState == State.ToWaitPoint || CurrentState == State.WaitingInQueue)
            {
                CurrentState = State.ToPackaging;
            }
        }

        // called after player picks packaging option
        public void ReleaseToDelivery()
        {
            if (CurrentState == State.WaitingForHandoffQueue)
            {
                CurrentState = State.ToDeliveryPoint;
            }
        }
        
        public void MarkPackagedAndContinue()
        {
            if (CurrentState == State.WaitingForPackaging)
            {
                CurrentState = State.ToPackageWaitingPoint;
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