using System.Collections;
using System.Collections.Generic;
using Playmode.Pickups;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public class MedkitStimulus : MonoBehaviour
    {
        private MedkitController medkit;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            medkit = transform.GetComponentInChildren<MedkitController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<MedkitSensor>()?.See(medkit);

            if (other.gameObject.name == "Body")
            {
                other.GetComponent<MedkitSensorCollision>()?.Pickup(medkit);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            other.GetComponent<MedkitSensor>()?.LooseSightOf(medkit);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //collision.gameObject.GetComponent<MedkitSensorCollision>()?.Pickup(medkit);
        }
    }
}
