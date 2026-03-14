using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Game.Gameplay;

namespace Game.Saving
{
    public static class MyXmlSerializer
    {
        public static void Save(string path, ScoreResultsData data)
        {
            try
            {
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(ScoreResultsData));
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    serializer.Serialize(stream, data);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"xml save failed: {e.Message}");
            }
        }

        public static ScoreResultsData LoadOrDefault(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return new ScoreResultsData();
                }

                XmlSerializer serializer = new XmlSerializer(typeof(ScoreResultsData));
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    return serializer.Deserialize(stream) as ScoreResultsData ?? new ScoreResultsData();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"xml load failed: {e.Message}");
                return new ScoreResultsData();
            }
        }
    }
}