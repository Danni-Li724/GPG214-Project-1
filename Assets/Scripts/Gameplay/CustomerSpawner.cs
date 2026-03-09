using Game.Core;
using Game.Data;
using UnityEngine;

namespace Game.Gameplay
{
    public class CustomerSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject customerPrefab;

        [SerializeField] private AddressablesAssetProvider assetProvider;
        [SerializeField] private ReactionKeysSO reactionKeys;

        public Customer Spawn(CustomerRequest request, int index)
        {
            Transform point = spawnPoints[index % spawnPoints.Length];
            GameObject go = Instantiate(customerPrefab, point.position, Quaternion.identity);

            Customer c = go.GetComponent<Customer>();
            c.Init(request, assetProvider, reactionKeys);
            return c;
        }
    }
}