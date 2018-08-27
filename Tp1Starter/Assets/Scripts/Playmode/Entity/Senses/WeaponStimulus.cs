using System.Collections;
using System.Collections.Generic;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public class WeaponStimulus : MonoBehaviour
    {
        private WeaponController weapon;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            weapon = transform.root.GetComponentInChildren<WeaponController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<WeaponSensor>()?.See(weapon);

            if (other.gameObject.name == "Body")
            {
                other.GetComponent<WeaponSensorCollision>()?.Pickup(weapon);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            other.GetComponent<WeaponSensor>()?.LooseSightOf(weapon);
        }
    }
}
