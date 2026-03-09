using System;

namespace Game.Saving
{
    [Serializable]
    public class PlayerProfileData
    {
        public string PlayerName;
        public int PlayerAge;
        public int IconId;
        public int FavColorId;
    }

    [Serializable]
    public class LastSessionData
    {
        public int TotalPacked;
        public int CorrectPacked;
        public int Mistakes;
        public float SuccessRatePercent;
    }
}