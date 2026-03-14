using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.UI
{
    public class SpawnBundleObject : MonoBehaviour
    {
        [SerializeField] private string bundleFileName = "cutecubes";
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Transform spawnedParent;

        private AssetBundle bundle;
        private string[] prefabNames;

        public void SpawnOneFromBundle()
        {
            StartCoroutine(EnsureBundleThenSpawnOne());
        }

        private IEnumerator EnsureBundleThenSpawnOne()
        {
            if (bundle == null)
            {
                yield return LoadBundleFromStreamingAssets();
            }

            if (bundle == null)
            {
                Debug.LogWarning("bundle not loaded");
                yield break;
            }

            if (prefabNames == null || prefabNames.Length == 0)
            {
                prefabNames = bundle.GetAllAssetNames();
            }

            if (prefabNames == null || prefabNames.Length == 0)
            {
                Debug.LogWarning("bundle name doesn't match");
                yield break;
            }

            string assetName = prefabNames[Random.Range(0, prefabNames.Length)];

            AssetBundleRequest req = bundle.LoadAssetAsync<GameObject>(assetName);
            yield return req;

            GameObject prefab = req.asset as GameObject;
            Vector3 pos = transform.position;
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
                if (sp != null) pos = sp.position;
                pos.z = 0f;
            }
            Instantiate(prefab, pos, Quaternion.identity, spawnedParent);
        }

        private IEnumerator LoadBundleFromStreamingAssets()
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, bundleFileName);

            using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(fullPath))
            {
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogWarning($"failed to load bundle '{bundleFileName}' at '{fullPath}': {req.error}");
                    yield break;
                }

                bundle = DownloadHandlerAssetBundle.GetContent(req);
                if (bundle == null)
                {
                    Debug.LogWarning("downloaded bundle is null");
                    yield break;
                }

                prefabNames = bundle.GetAllAssetNames();
                Debug.Log($"loaded bundle '{bundleFileName}' with {prefabNames.Length} assets");
            }
        }

        private void OnDestroy()
        {
            if (bundle != null)
            {
                bundle.Unload(false);
                bundle = null;
            }
        }
    }
}