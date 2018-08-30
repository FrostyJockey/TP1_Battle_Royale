using Playmode.Bullet;
using System;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public class HitStimulus : MonoBehaviour
    {
        [SerializeField] private int bulletHitPoint = 0;

        public int BulletDamage
        {
            get { return bulletHitPoint; }
            set
            {
                if (bulletHitPoint != value)
                {
                    bulletHitPoint = value;
                }
            }
        }

        private void Awake()
        {
            ValidateSerializeFields();
        }

        private void ValidateSerializeFields()
        {
            if (BulletDamage < 0)
                throw new ArgumentException("Hit points can't be less than 0.");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<Entity.Senses.HitSensor>()?.Hit(BulletDamage, transform.root.GetComponentInChildren<BulletController>());
        }
    }
}