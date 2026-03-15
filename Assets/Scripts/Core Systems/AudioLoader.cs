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
                Debug.LogWarning("AudioLoader received an empty filename.");
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
            // explicit file check before load as required from brief:
            if (!File.Exists(fullPath)) 
            {
                Debug.LogWarning($"audio file missing. falling back to null/default. Path: {fullPath}");
                onLoaded?.Invoke(null);
                yield break;
            }
            string url = "file://" + fullPath;
            using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            // using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.WAV))
            {
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    // corrupted/invalid audio now fails with error log
                    Debug.LogWarning($"audio load failed or file was corrupt: {relativePath} | {req.error}");
                    onLoaded?.Invoke(null);
                    yield break;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(req);

                if (clip == null)
                {
                    Debug.LogWarning($"Audio clip content was null after loading. File may be invalid: {relativePath}");
                    onLoaded?.Invoke(null);
                    yield break;
                }

                clipCache[filename] = clip;
                onLoaded?.Invoke(clip);
            }
        }
    }
} 