using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class CargoQueue : MonoBehaviour
    {
        private readonly Queue<Cargo> waitingAtPackaging = new Queue<Cargo>();
        private readonly Queue<Cargo> waitingAtDelivery = new Queue<Cargo>();

        public void EnqueueNewCargo(Cargo cargo)
        {
            waitingAtPackaging.Enqueue(cargo);
        }

        public bool HasCargoWaitingForPackaging()
        {
            return waitingAtPackaging.Count > 0;
        }

        public Cargo PeekWaitingCargo()
        {
            if (waitingAtPackaging.Count == 0) return null;
            return waitingAtPackaging.Peek();
        }

        public void MarkFrontCargoPackaged()
        {
            if (waitingAtPackaging.Count == 0) return;

            Cargo cargo = waitingAtPackaging.Dequeue();
            var mover = cargo.GetComponent<CargoMover>();
            if (mover != null) mover.MarkPackagedAndContinue();

            waitingAtDelivery.Enqueue(cargo);
        }

        public bool HasCargoWaitingAtDelivery()
        {
            // only cargo that actually reached the delivery point is gonna be pickable
            // for now at least, might change in project 2
            return waitingAtDelivery.Count > 0;
        }

        public Cargo TakeCargoReadyForPickup()
        {
            if (waitingAtDelivery.Count == 0) return null;
            return waitingAtDelivery.Dequeue();
        }
    }
}
