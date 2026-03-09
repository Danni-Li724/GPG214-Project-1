using System.Threading.Tasks;
using Game.Core;
using Game.Data;
using UnityEngine;

namespace Game.Gameplay
{
    public class CargoSpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform packagingPoint;
        [SerializeField] private Transform deliveryPoint;

        [SerializeField] private CargoQueue cargoQueue;
        [SerializeField] private AddressablesAssetProvider assetProvider;

        public async Task<Cargo> SpawnAsync(CargoTypeSO cargoType)
        {
            GameObject go = Instantiate(cargoType.CargoPrefab, spawnPoint.position, Quaternion.identity);
            Cargo cargo = go.GetComponent<Cargo>();
            CargoMover mover = go.GetComponent<CargoMover>();

            await cargo.InitAsync(cargoType, assetProvider);
            mover.Configure(packagingPoint, deliveryPoint);

            cargoQueue.EnqueueNewCargo(cargo);
            return cargo;
        }
    }
}
