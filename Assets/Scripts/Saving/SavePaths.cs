using System.IO;
using UnityEngine;

namespace Game.Saving
{
    public static class SavePaths
    {
        public static string ProfilePath
        {
            get { return Path.Combine(Application.persistentDataPath, "profile.json"); }
        }

        public static string LastSessionPath
        {
            get { return Path.Combine(Application.persistentDataPath, "last_session.json"); }
        }
    }
}