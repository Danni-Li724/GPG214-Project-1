using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Core
{
    public class AddressablesAssetProvider : MonoBehaviour
    {
        private readonly Dictionary<string, Object> cache = new Dictionary<string, Object>();

        public async Task<T> LoadAsync<T>(string key) where T : Object
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            if (cache.TryGetValue(key, out Object cached))
            {
                return cached as T;
            }

            var handle = Addressables.LoadAssetAsync<T>(key);
            await handle.Task;

            if (handle.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                Debug.LogWarning($"Addressables failed to load key: {key}");
                return null;
            }

            cache[key] = handle.Result;
            return handle.Result;
        }
        
        public void ClearCache()
        {
            cache.Clear();
        }
    }
}