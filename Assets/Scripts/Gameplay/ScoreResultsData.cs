using System;
using System.Xml.Serialization;

namespace Game.Gameplay
{
    [Serializable]
    [XmlRoot("ScoreResults")]
    public class ScoreResultsData
    {
        public int WrongCargoCount;
        public int RightCargoWrongPackagingCount;
        public int PerfectDeliveryCount;
    }
}