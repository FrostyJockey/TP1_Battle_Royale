using System.Collections;
using System.Collections.Generic;
using Playmode.Pickups;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void MedkitSensorCollisionEventHandler(MedkitController medkit);

    public class MedkitSensorCollision : MonoBehaviour
    {
        public event MedkitSensorCollisionEventHandler OnMedkitPickup;

        private void NotifyMedkitPickup(MedkitController medkit)
        {
            if (OnMedkitPickup != null) OnMedkitPickup(medkit);
        }

        public void Pickup(MedkitController medkit)
        {
            NotifyMedkitPickup(medkit);
        }
    }
}


