using System.IO;
using Game.Saving;
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
        
        public void SaveScoresToXml()
        {
            ScoreResultsData data = new ScoreResultsData();
            data.WrongCargoCount = WrongCargoCount;
            data.RightCargoWrongPackagingCount = RightCargoWrongPackagingCount;
            data.PerfectDeliveryCount = PerfectDeliveryCount;

            MyXmlSerializer.Save(ScorePaths.ScoreXmlPath, data);
        }
    }
    
    public static class ScorePaths
    {
        public static string ScoreXmlPath =>
            Path.Combine(Application.persistentDataPath, "scores.xml");
    }
}
