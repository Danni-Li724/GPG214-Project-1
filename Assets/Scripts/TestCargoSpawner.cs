using UnityEngine;
using Game.Data;
using Game.Gameplay;

namespace Game.Gameplay
{
    /// <summary>
    /// Test: given a cargo type id, spawn that cargo at a point
    /// </summary>
    public class TestCargoSpawner : MonoBehaviour
    {
        [SerializeField] private CargoDatabaseSO cargoDatabase;
        [SerializeField] private Transform spawnPoint;

        [Header("Optional")]
        [SerializeField] private bool destroyPreviousSpawn = true;

        private GameObject lastSpawned;

        public void SpawnCargoById(int cargoTypeId)
        {
            if (cargoDatabase == null || spawnPoint == null) return;

            CargoTypeSO type = cargoDatabase.GetById(cargoTypeId);
            if (type == null || type.CargoPrefab == null) return;

            if (destroyPreviousSpawn && lastSpawned != null)
            {
                Destroy(lastSpawned);
                lastSpawned = null;
            }

            GameObject go = Instantiate(type.CargoPrefab, spawnPoint.position, Quaternion.identity);
            lastSpawned = go;

            Cargo cargo = go.GetComponent<Cargo>();
            if (cargo != null)
            {
                // cargo.Init(type);
            }
        }

        public void SpawnRandomCargo()
        {
            if (cargoDatabase == null) return;

            CargoTypeSO type = cargoDatabase.GetRandom();
            if (type == null) return;

            SpawnCargoById(type.CargoTypeId);
        }

        public void ClearSpawned()
        {
            if (lastSpawned != null)
            {
                Destroy(lastSpawned);
                lastSpawned = null;
            }
        }
    }
}