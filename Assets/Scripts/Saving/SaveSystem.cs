using UnityEngine;
using Game.Core;

namespace Game.Saving
{
    /// <summary>
    /// convert data models to/from JSON and store them
    /// </summary>
    public class SaveSystem : MonoBehaviour
    {
        public bool HasProfile()
        {
            return System.IO.File.Exists(SavePaths.ProfilePath);
        }

        public void SaveProfile(PlayerProfileData profile)
        {
            var json = JsonUtility.ToJson(profile, true);
            JsonFileWriter.Write(SavePaths.ProfilePath, json);
        }

        public PlayerProfileData LoadProfileOrDefault()
        {
            string json;
            if (JsonFileWriter.TryRead(SavePaths.ProfilePath, out json))
            {
                var data = JsonUtility.FromJson<PlayerProfileData>(json);
                if (data != null) return data;
            }
            // default to fallback on
            return new PlayerProfileData
            {
                PlayerName = "Player",
                PlayerAge = 18,
                IconId = 0,
                FavColorId = 0
            };
        }

        public void SaveLastSession(GameSession session)
        {
            float rate = 0f;
            if (session.TotalPacked > 0)
            {
                rate = (float)session.CorrectPacked / (float)session.TotalPacked;
            }

            var last = new LastSessionData
            {
                TotalPacked = session.TotalPacked,
                CorrectPacked = session.CorrectPacked,
                Mistakes = session.Mistakes,
                SuccessRatePercent = rate * 100f
            };

            var json = JsonUtility.ToJson(last, true);
            JsonFileWriter.Write(SavePaths.LastSessionPath, json);
        }

        public LastSessionData LoadLastSessionOrDefault()
        {
            string json;
            if (JsonFileWriter.TryRead(SavePaths.LastSessionPath, out json))
            {
                var data = JsonUtility.FromJson<LastSessionData>(json);
                if (data != null) return data;
            }

            return new LastSessionData
            {
                TotalPacked = 0,
                CorrectPacked = 0,
                Mistakes = 0,
                SuccessRatePercent = 0f
            };
        }
    }
}
