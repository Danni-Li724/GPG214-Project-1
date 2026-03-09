using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Data;
using UnityEngine;

namespace Game.Gameplay
{
    public class WaveCoordinator : MonoBehaviour
    {
        [SerializeField] private CargoDatabaseSO cargoDatabase;
        [SerializeField] private CargoSpawner cargoSpawner;
        [SerializeField] private CustomerSpawner customerSpawner;

        [Header("Wave")]
        [SerializeField] private int waveSize = 4;

        private readonly System.Random randomNumberGeneraor = new System.Random();

        private async void Start()
        {
            await StartWaveAsync(waveSize);
        }

        public async Task StartWaveAsync(int count)
        {
            // all customer/cargo requests are matched (same cargo type id for a pair)
            List<CustomerRequest> requests = new List<CustomerRequest>(count);
            List<CargoTypeSO> cargoToSpawn = new List<CargoTypeSO>(count);

            for (int i = 0; i < count; i++)
            {
                CargoTypeSO type = cargoDatabase.GetRandom();
                PackagingType pack = (PackagingType)randomNumberGeneraor.Next(0, 4);

                requests.Add(new CustomerRequest(type.CargoTypeId, pack));
                cargoToSpawn.Add(type);
            }

            // shuffle cargo spawn order so it isn't in the order of customers, that would be too easy
            Shuffle(cargoToSpawn);

            // spawn customers in request order
            for (int i = 0; i < requests.Count; i++)
            {
                customerSpawner.Spawn(requests[i], i);
            }

            // spawn cargo in shuffled order
            for (int i = 0; i < cargoToSpawn.Count; i++)
            {
                await cargoSpawner.SpawnAsync(cargoToSpawn[i]);
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