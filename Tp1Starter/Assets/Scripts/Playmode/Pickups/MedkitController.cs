using System;
using System.Collections;
using System.Collections.Generic;
using Playmode.Entity.Senses;
using Playmode.Util.Values;
using UnityEngine;

namespace Playmode.Pickups
{
    public class MedkitController : MonoBehaviour {


        [SerializeField] private int healthValue = 30;

        private PickupSpawnerController pickupSpawnerController;

        private int spawnerNumber;

        public int HealthValue { get { return healthValue; } }

        public PickupSpawnerController AssociatedSpawner
        {
            get { return pickupSpawnerController; }
            set
            {
                if (pickupSpawnerController != value)
                {
                    pickupSpawnerController = value;
                }
            }
        }

        private void Awake()
        {
            ValidateSerialisedFields();
        }

        private void ValidateSerialisedFields()
        {
            if (healthValue < 0)
                throw new ArgumentException("Medkits need a value higher than 0.");
        }

        private void OnMedkitPickup(MedkitController medkit)
        {
            Hide(medkit);
        }

        public void ActivateAssociatedSpawner(MedkitController medkit)
        {
            AssociatedSpawner.IsSpawnerAvailable = true;
            Hide(medkit);
        }

        private void Hide(MedkitController medkit)
        {
            var currentMedkit = medkit.transform.root.gameObject;

            currentMedkit.SetActive(false);
            currentMedkit.GetComponentInChildren<MedkitController>().AssociatedSpawner.IsSpawnerAvailable = true;
        }

        public void SetAssociatedSpawner(PickupSpawnerController spawner)
        {
            AssociatedSpawner = spawner;
        }
    }
}
