using Playmode.Util.Values;
using Playmode.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playmode.Pickups
{
    public class PickupSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject pickupPrefabMedkit;
        [SerializeField] private GameObject pickupPrefabShotgun;
        [SerializeField] private GameObject pickupPrefabUzi;
        [SerializeField] private float pickupSpawnDelay = 10f;

        private int spawnerAvailableCount;

        private int nextSpawnPoint;

        private void OnEnable()
        {
            StartCoroutine(PickupSpawnCoroutine());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator PickupSpawnCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(pickupSpawnDelay);
                SpawnRandomPickables();
            }
        }

        private void SpawnRandomPickables()
        {
            var nextPickable = CreateRandomNumber();

            InitValues();

            while (true && spawnerAvailableCount < transform.childCount)
            {
                nextSpawnPoint = NextRandomSpawnpointNumber();

                var spawner = CheckSpawnerAvailability(nextSpawnPoint);

                if (spawner is PickupSpawnerController)
                {
                    SpawnPickable(transform.GetChild(nextSpawnPoint).position, (PickupTypes)nextPickable, spawner);
                    break;
                }
                else
                {
                    spawnerAvailableCount++;
                }
            }
        }

        private void SpawnPickable(Vector3 position, PickupTypes pickupType, PickupSpawnerController spawner)
        {
            switch (pickupType)
            {
                case PickupTypes.Medkit:
                    var medkit = Instantiate(pickupPrefabMedkit, position, Quaternion.identity);
                    medkit.gameObject.GetComponentInChildren<MedkitController>().SetAssociatedSpawner(spawner);
                    break;

                case PickupTypes.Shotgun:
                    var weaponShotgun = Instantiate(pickupPrefabShotgun, position, Quaternion.identity);
                    weaponShotgun.gameObject.GetComponentInChildren<WeaponController>().SetAssociatedSpawner(spawner);
                    break;

                case PickupTypes.Uzi:
                    var weaponUzi = Instantiate(pickupPrefabUzi, position, Quaternion.identity);
                    weaponUzi.gameObject.GetComponentInChildren<WeaponController>().SetAssociatedSpawner(spawner);
                    break;
            }
        }

        private PickupSpawnerController CheckSpawnerAvailability(int spawnerToCheck)
        {
            var child = transform.GetChild(spawnerToCheck);

            if (child.GetComponent<PickupSpawnerController>().IsSpawnerAvailable)
            {
                child.GetComponent<PickupSpawnerController>().IsSpawnerAvailable = false;
                return child.GetComponent<PickupSpawnerController>();
            }
            return null;
        }

        private int CreateRandomNumber()
        {
            int randomNumber = (int)UnityEngine.Random.Range((float)PickupTypes.Medkit, (float)PickupTypes.Uzi + 1);
            return randomNumber;
        }

        private int NextRandomSpawnpointNumber()
        {
            return UnityEngine.Random.Range(0, transform.childCount);
        }

        private void InitValues()
        {
            spawnerAvailableCount = 0;
            nextSpawnPoint = 0;
        }
    }
}
