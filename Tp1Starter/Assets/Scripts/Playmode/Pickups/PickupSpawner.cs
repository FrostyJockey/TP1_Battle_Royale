﻿using Playmode.Util.Values;
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
            var nextPickable = (int)UnityEngine.Random.Range((float)PickupTypes.Medkit, (float)PickupTypes.Uzi+1);

            var nextSpawnPoint = 0;

            int spawnerAvailableCount = 0;

            while (true && spawnerAvailableCount < transform.childCount)
            {
                nextSpawnPoint = UnityEngine.Random.Range(0, transform.childCount);

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
    }
}