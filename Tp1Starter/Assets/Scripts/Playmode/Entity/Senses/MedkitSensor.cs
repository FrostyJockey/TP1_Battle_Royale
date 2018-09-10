using System.Collections;
using System.Collections.Generic;
using Playmode.Pickups;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void MedkitSensorEventHandler(MedkitController medkit);

    public class MedkitSensor : MonoBehaviour
    {
        private ICollection<MedkitController> medkitsInSight;

        public event MedkitSensorEventHandler OnMedkitSeen;
        public event MedkitSensorEventHandler OnMedkitSightLost;

        public IEnumerable<MedkitController> MedkitsInSight => medkitsInSight;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            medkitsInSight = new HashSet<MedkitController>();
        }

        private void NotifyMedkitSeen(MedkitController medkit)
        {
            if (OnMedkitSeen != null) OnMedkitSeen(medkit);
        }

        private void NotifyMedkitSightLost(MedkitController medkit)
        {
            if (OnMedkitSightLost != null) OnMedkitSightLost(medkit);
        }

        public void See(MedkitController medkit)
        {
            //BEN_CORRECTION : Risidus de programmation.
            Debug.Log("I see the medkit!");

            medkitsInSight.Add(medkit);

            NotifyMedkitSeen(medkit);
        }

        public void LooseSightOf(MedkitController medkit)
        {
            //BEN_CORRECTION : Risidus de programmation.
            Debug.Log("Where's the medkit??");

            medkitsInSight.Remove(medkit);

            NotifyMedkitSightLost(medkit);
        }
    }
}

