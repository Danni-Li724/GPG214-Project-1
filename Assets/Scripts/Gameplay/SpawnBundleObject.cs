using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Game.UI;
using kf;

namespace Game.Gameplay
{
    /// <summary>
    /// This script demonstrates asset bundle loading
    /// NOTE: all the lines that are marked between "//---optimization---//" bars are changes I have made
    /// on top of existing code in an effort to optimize using the circular buffer :). Before that, my initial code
    /// just spawns bundle objects endlessly...
    /// </summary>
    public class SpawnBundleObject : MonoBehaviour
    {
        [SerializeField] private string bundleFileName = "cutecubes";
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Transform spawnedParent;
        
        //---optimization---//
        [Header("Pooling (using Greg's Circular Buffer)")]
        [SerializeField] private int poolCapacity = 10;
        [SerializeField] private bool destroyOverwritten = true;
        private CircularBuffer<GameObject> pool;
        //---optimization---//

        private AssetBundle bundle;
        private string[] prefabNames;
        
        //---optimization---//
        private void Awake()
        {
            // CircularBuffer now caps the amount of spawned bundle objects
            pool = new CircularBuffer<GameObject>(poolCapacity);
        }
        //---optimization---//

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
                // filters to select only prefabs so runtime loading is safer
                prefabNames = bundle.GetAllAssetNames()
                    .Where(n => n.EndsWith(".prefab"))
                    .ToArray();
            }

            if (prefabNames == null || prefabNames.Length == 0)
            {
                Debug.LogWarning("no prefab assets found in bundle. Make sure prefabs were assigned to bundle 'cutecubes' before building");
                yield break;
            }

            string assetName = prefabNames[Random.Range(0, prefabNames.Length)];

            AssetBundleRequest req = bundle.LoadAssetAsync<GameObject>(assetName);
            yield return req;

            GameObject prefab = req.asset as GameObject;
            if (prefab == null)
            {
                // error handling: if the bundle contains anything unexpected or corrupt - logs the problem and exits safely
                Debug.LogWarning($"buddle object '{assetName}' is not a valid prefab");
                yield break;
            }

            Vector3 pos = transform.position;
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
                if (sp != null)
                {
                    pos = sp.position;
                }
            }
            // force z because it was previously not showing in play view
            pos.z = 0f;
            // the oldest object is removed before a new one is pushed when pool is full
            
            //---optimization---//
            if (pool.Full())
            {
                GameObject oldest = pool.Front();
                CleanupOld(oldest);
            }
            GameObject instance = Instantiate(prefab, pos, Quaternion.identity, spawnedParent);
            pool.Push(instance);
        }
         
        private void CleanupOld(GameObject old)
        {
            if (old == null) return;

            if (destroyOverwritten)
            {
                Destroy(old);
            }
            else
            {
                old.SetActive(false);
            }
        }
        //---optimization---//

        private IEnumerator LoadBundleFromStreamingAssets()
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, bundleFileName);

            using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(fullPath))
            {
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    // explicit corrupt/missing/invalid bundle handling required from brief
                    Debug.LogWarning($"failed to load bundle '{bundleFileName}' at '{fullPath}': {req.error}");
                    yield break;
                }

                bundle = DownloadHandlerAssetBundle.GetContent(req);
                if (bundle == null)
                {
                    // more error handling
                    Debug.LogWarning("downloaded bundle is null");
                    yield break;
                }

                prefabNames = bundle.GetAllAssetNames();
                Debug.Log($"loaded bundle '{bundleFileName}' with {prefabNames.Length} assets");
            }
            
            Debug.Log($"Loaded bundle '{bundleFileName}'"); // dynamic asset load error handling which ensures players
            //loads correct bundles
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