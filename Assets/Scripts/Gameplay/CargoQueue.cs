using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class CargoQueue : MonoBehaviour
    {
        private readonly Queue<Cargo> queue = new Queue<Cargo>();
        private Cargo activeForPackaging; // the one cargo currently at the packaging station (or heading there)
        private readonly Queue<Cargo> readyForPickup = new Queue<Cargo>(); // cargo that finished packaging and is now waiting at delivery
        
        private Cargo activeAtDelivery;

        public void EnqueueNewCargo(Cargo cargo)
        {
            queue.Enqueue(cargo);
            CargoMover mover = cargo.GetComponent<CargoMover>();
            if (mover != null)
            {
                mover.ArrivedAtWait += OnCargoArrivedAtWaitPoint;
                mover.ArrivedAtPackageWait += OnCargoArrivedAtPackageWaitingPoint;
            }
        }

        public bool HasCargoWaitingForPackaging()
        {
            // true only when the active cargo has reached packaging station and is waiting
            if (activeForPackaging == null) return false;

            CargoMover mover = activeForPackaging.GetComponent<CargoMover>();
            return mover != null && mover.CurrentState == CargoMover.State.WaitingForPackaging;
        }

        public Cargo GetActiveCargoForPackaging()
        {
            return activeForPackaging;
        }

        public void MarkActiveCargoPackaged()
        {
            if (activeForPackaging == null) return;
            CargoMover mover = activeForPackaging.GetComponent<CargoMover>();
            if (mover != null) mover.MarkPackagedAndContinue();
            readyForPickup.Enqueue(activeForPackaging);
            activeForPackaging = null;
            TryReleaseNextToPackaging();
            TryReleaseNextToDelivery();
            
        }

        public bool HasCargoWaitingAtDelivery()
        {
            if (activeAtDelivery == null) return false;
            CargoMover mover = activeAtDelivery.GetComponent<CargoMover>();
            return mover != null && mover.CurrentState == CargoMover.State.WaitingAtHandoff;
        }

        public Cargo TakeCargoReadyForPickup()
        {
            if (!HasCargoWaitingAtDelivery()) return null;
            Cargo cargo = activeAtDelivery;
            activeAtDelivery = null;
            TryReleaseNextToDelivery();
            return cargo;
        }

        private void TryReleaseNextToPackaging()
        {
            if (activeForPackaging != null) return;

            if (queue.Count == 0) return;

            Cargo candidate = queue.Peek();
            CargoMover candidateMover = candidate.GetComponent<CargoMover>();
            if (candidateMover == null) return;

            if (candidateMover.CurrentState != CargoMover.State.WaitingInQueue) return;

            activeForPackaging = queue.Dequeue();

            CargoMover mover = activeForPackaging.GetComponent<CargoMover>();
            if (mover != null)
            {
                mover.ReleaseToPackaging();
            }
        }
        
        private void OnCargoArrivedAtWaitPoint(CargoMover mover)
        {
            TryReleaseNextToPackaging();
        }

        private void OnCargoArrivedAtPackageWaitingPoint(CargoMover mover)
        {
            TryReleaseNextToDelivery();
        }
        private void TryReleaseNextToDelivery()
        {
            if (activeAtDelivery != null) return;

            if (readyForPickup.Count == 0) return;

            Cargo candidate = readyForPickup.Peek();
            CargoMover candidateMover = candidate.GetComponent<CargoMover>();
            if (candidateMover == null) return;

            if (candidateMover.CurrentState != CargoMover.State.WaitingForHandoffQueue) return;

            activeAtDelivery = readyForPickup.Dequeue();

            CargoMover mover = activeAtDelivery.GetComponent<CargoMover>();
            if (mover != null)
            {
                mover.ReleaseToDelivery();
            }
        }
    }
}

