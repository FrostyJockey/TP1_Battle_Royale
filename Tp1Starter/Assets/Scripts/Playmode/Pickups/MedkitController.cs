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

        private MedkitSensorCollision medkitSensorCollision;
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

            medkitSensorCollision = GameObject.FindWithTag(Tags.Ennemy).GetComponentInChildren<MedkitSensorCollision>();
        }

        private void OnEnable()
        {
            medkitSensorCollision.OnMedkitPickup += OnMedkitPickup;
        }

        private void OnDisable()
        {
            medkitSensorCollision.OnMedkitPickup -= OnMedkitPickup;
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

        private void Hide(MedkitController medkit)
        {
            medkit.transform.root.gameObject.SetActive(false);
            AssociatedSpawner.IsSpawnerAvailable = true;
        }

        public void SetAssociatedSpawner(PickupSpawnerController spawner)
        {
            AssociatedSpawner = spawner;
        }
    }
}
