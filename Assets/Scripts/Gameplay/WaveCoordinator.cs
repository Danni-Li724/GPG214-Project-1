using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Gameplay
{
    public class WaveCoordinator : MonoBehaviour
    {
        [SerializeField] private CargoDatabaseSO cargoDatabase;
        [SerializeField] private CargoSpawner cargoSpawner;
        [SerializeField] private CustomerSpawner customerSpawner;

        [Header("Wave")]
        [SerializeField] private int waveSize = 4;
        [SerializeField] private float spawnInterval = 1.0f;

        private readonly System.Random randomNumberGeneraor = new System.Random();

        private async void Start()
        {
            await StartWaveAsync(waveSize);
        }
        
        private async Task DelaySeconds(float seconds)
        {
            if (seconds <= 0f) return;
            float end = Time.time + seconds;
            while (Time.time < end)
            {
                await Task.Yield();
            }
        }

        public async Task StartWaveAsync(int count)
        {
            // all customer/cargo requests are matched (same cargo type id for a pair)
            List<CustomerRequest> requests = new List<CustomerRequest>(count);
            List<CargoTypeSO> cargoToSpawn = new List<CargoTypeSO>(count);
            // give result mnager the total count to determine when to show results button
            FindFirstObjectByType<ResultsManager>()?.SetTotalCustomers(count);
            for (int i = 0; i < count; i++)
            {
                CargoTypeSO type = cargoDatabase.GetRandom();
                if (type == null)
                {
                    Debug.LogError("CargoDatabaseSO's database list has nulls");
                    return;
                }

                if (type.CargoPrefab == null)
                {
                    Debug.LogError($"CargoTypeSO '{type.name}' has no prefab");
                    return;
                }
                PackagingType pack = type.RequiredPackaging;
                requests.Add(new CustomerRequest(type.CargoTypeId, pack));
                cargoToSpawn.Add(type);
            }

            // shuffle cargo spawn order so it isn't in the order of customers, that would be too easy
            Shuffle(cargoToSpawn);

            // spawn customers in request order
            for (int i = 0; i < requests.Count; i++)
            {
                customerSpawner.Spawn(requests[i], i);
                await DelaySeconds(spawnInterval);
            }

            // spawn cargo in shuffled order
            for (int i = 0; i < cargoToSpawn.Count; i++)
            {
                await cargoSpawner.SpawnAsync(cargoToSpawn[i]);
                await DelaySeconds(spawnInterval);
            }
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = randomNumberGeneraor.Next(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}