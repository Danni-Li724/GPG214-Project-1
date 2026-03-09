using System.IO;
using UnityEngine;

public class FileSystems : MonoBehaviour
{
   private void Start()
   {
      FileDirectories();
      CreateDirectories();
      CreateDirectories();
   }
   public void FileDirectories()
   {
      string[] directories = Directory.GetDirectories(Application.dataPath, "*", SearchOption.AllDirectories);
      string[] allFiles = Directory.GetFiles(Application.dataPath);
      for (int i = 0; i < allFiles.Length; i++)
         Debug.Log(allFiles[i]);
      string assetsPath = Application.dataPath;
      Debug.Log("asset path: " + assetsPath);
      string persistentDataPath = Application.persistentDataPath;
      Debug.Log("persistent data path: " + persistentDataPath);
      string temporaryCachePath = Application.temporaryCachePath;
      Debug.Log("temporary cache path: " + temporaryCachePath);
      string editorPath = "Assets/Editor";
      Debug.Log("editor path: " + editorPath);
      string resourcesPath = "Assets/Resources";
      Debug.Log("resources path: " + resourcesPath);
      GameObject cargoObj = Resources.Load<GameObject>("cargo.Prefab");
   }

   public void CreateDirectories()
   {
      string streamingAssetsPath = Application.streamingAssetsPath;
      if (Directory.Exists(streamingAssetsPath))
         Debug.Log("asset path: " + streamingAssetsPath);
      else
      // path.combine = accessing this directory and create the folder in there
         Directory.CreateDirectory(Path.Combine(Application.dataPath, "StreamingAssets"));
   }
   
   public void CreatePlayerFolders()
   {
      string streamingAssetsPath = Application.streamingAssetsPath;
      if (!Directory.Exists(streamingAssetsPath))
      {
         Directory.CreateDirectory(Path.Combine(streamingAssetsPath, "PlayerFolder"));
         if (File.Exists(Path.Combine(streamingAssetsPath, "PlayerFolder", "Player.txt")))
            Debug.Log(" already has Player.txt");
         else File.CreateText(Path.Combine(streamingAssetsPath, "PlayerFolder", "Player.txt"));
         Debug.Log("created Player.txt");
      }
   }
}
