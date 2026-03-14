using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Core
{
    public class AudioLoader : MonoBehaviour
    {
        [SerializeField] private string audioFolderRelative = "Audio";
        private readonly Dictionary<string, AudioClip> clipCache = new Dictionary<string, AudioClip>();

        public void LoadWav(string filename, Action<AudioClip> onLoaded)
        {
            StartCoroutine(LoadWavRoutine(filename, onLoaded));
        }

        private IEnumerator LoadWavRoutine(string filename, Action<AudioClip> onLoaded)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                onLoaded?.Invoke(null);
                yield break;
            }

            if (clipCache.TryGetValue(filename, out AudioClip cached) && cached != null)
            {
                onLoaded?.Invoke(cached);
                yield break;
            }

            string relativePath = Path.Combine(audioFolderRelative, filename);
            string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);

            using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.WAV))
            {
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogWarning($"WAV load failed: {relativePath} | {req.error}");
                    onLoaded?.Invoke(null);
                    yield break;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(req);
                clipCache[filename] = clip;
                onLoaded?.Invoke(clip);
            }
        }
    }
}