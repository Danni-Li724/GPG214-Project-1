using System.IO;
using UnityEngine;

namespace Game.Saving
{
    /// <summary>
    /// Does one thing: read/write text files
    /// </summary>
    public static class JsonFileWriter
    {
        public static void Write(string path, string json)
        {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(path, json);
        }

        public static bool TryRead(string path, out string json)
        {
            json = string.Empty;

            if (!File.Exists(path))
                return false;

            json = File.ReadAllText(path);
            return true;
        }
    }
}

