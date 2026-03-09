using System;

namespace Game.Core
{
    /// <summary>
    /// holds current session runtime data (not persistent)
    /// </summary>
    [Serializable]
    public class GameSession
    {
        public int TotalPacked;
        public int CorrectPacked;
        public int Mistakes;

        public void ResetSession()
        {
            TotalPacked = 0;
            CorrectPacked = 0;
            Mistakes = 0;
        }
        public void RecordResult(bool success)
        {
            TotalPacked++;

            if (success) CorrectPacked++;
            else Mistakes++;
        }
    }
}

