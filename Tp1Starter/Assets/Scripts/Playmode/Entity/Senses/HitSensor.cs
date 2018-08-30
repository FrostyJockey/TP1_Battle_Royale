using Playmode.Bullet;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void HitSensorEventHandler(int hitPoints, BulletController bullet);

    public class HitSensor : MonoBehaviour
    {
        private AudioSource bulletHitSource;
        public event HitSensorEventHandler OnHit;

        private void OnEnable()
        {
            bulletHitSource = GetComponent<AudioSource>();
            bulletHitSource.Stop();
        }

        public void Hit(int hitPoints, BulletController bullet)
        {
            NotifyHit(hitPoints, bullet);
            PlayAudioBodyHit();
        }

        private void NotifyHit(int hitPoints, BulletController bullet)
        {
            if (OnHit != null) OnHit(hitPoints, bullet);
        }

        private void PlayAudioBodyHit()
        {
            bulletHitSource.Play();
        }
    }
}