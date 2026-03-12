using UnityEngine;

namespace Game.Gameplay
{
    public class ScoreManager : MonoBehaviour
    {
        public  int WrongCargoCount { get; private set; }                 // gave wrong cargo regardless of packaging
        public int RightCargoWrongPackagingCount { get; private set; }   // right cargo, wrong packaging
        public int PerfectDeliveryCount { get; private set; }            // both right

        public void RecordWrongCargo()
        {
            WrongCargoCount++;
        }

        public void RecordRightCargoWrongPackaging()
        {
            RightCargoWrongPackagingCount++;
        }

        public void RecordPerfectDelivery()
        {
            PerfectDeliveryCount++;
        }

        public void ResetScores()
        {
            WrongCargoCount = 0;
            RightCargoWrongPackagingCount = 0;
            PerfectDeliveryCount = 0;
        }
    }
}