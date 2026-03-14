using System.Threading.Tasks;
using Game.Core;
using Game.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Gameplay
{
    public class CargoSpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform packagingPoint;
        [SerializeField] private Transform packageWaitPoint;
        [SerializeField] private Transform waitPoint;
        [SerializeField] private Transform deliveryPoint;

        [SerializeField] private CargoQueue cargoQueue;
        [SerializeField] private AddressablesAssetProvider assetProvider;

        [SerializeField] private CargoSfxDatabaseSO cargoSfxDatabase;
        [SerializeField] private AudioLoader audioLoader;
        [SerializeField] private SfxPlayer sfxPlayer;
        public async Task<Cargo> SpawnAsync(CargoTypeSO cargoType)
        {
            GameObject go = Instantiate(cargoType.CargoPrefab, spawnPoint.position, Quaternion.identity);
            Cargo cargo = go.GetComponent<Cargo>();
            CargoMover mover = go.GetComponent<CargoMover>();
            await cargo.InitAsync(cargoType, assetProvider);
            // streams audio
            string spawnFile = (cargoSfxDatabase != null) ? cargoSfxDatabase.GetFilename(cargoType.CargoTypeId) : null;
            if (!string.IsNullOrWhiteSpace(spawnFile) && audioLoader != null && sfxPlayer != null)
            {
                audioLoader.LoadWav(spawnFile + ".wav", clip =>
                {
                    sfxPlayer.PlayOneShot(clip, 1f, 1f);
                });
            }
            mover.Configure(waitPoint, packagingPoint, packageWaitPoint, deliveryPoint);
            cargoQueue.EnqueueNewCargo(cargo);
            return cargo;
        }
    }
}
