using Game.Core;
using Game.Data;
using UnityEngine;

namespace Game.Gameplay
{
    public class CustomerSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject customerPrefab;
        [SerializeField] private ScoreManager scoreManager;

        [Header("Streaming")]
        [SerializeField] private AddressablesAssetProvider assetProvider;
        [SerializeField] private ReactionKeysSO reactionKeys;

        [Header("Customer Visuals")]
        [SerializeField] private CustomerAppearanceDatabaseSO appearanceDatabase;
        [SerializeField] private RequestIconDatabaseSO requestIconDatabase;

        private readonly System.Random rng = new System.Random();

        public async void Spawn(CustomerRequest request, int index)
        {
            Transform point = spawnPoints[index % spawnPoints.Length];
            GameObject go = Instantiate(customerPrefab, point.position, Quaternion.identity);

            Customer customer = go.GetComponent<Customer>();

            string appearanceKey = appearanceDatabase != null
                ? appearanceDatabase.GetRandomKey(rng)
                : null;

            await customer.InitAsync(
                request,
                appearanceKey,
                assetProvider,
                reactionKeys,
                requestIconDatabase,
                scoreManager
            );
        }
    }
}