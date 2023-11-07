using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyDamageListener : MonoBehaviour
    {
        public int damage;
        public bool DoDamage;

        public void StartDamage(int damage)
        {
            this.damage = damage;
            DoDamage = true;
        }

        public void EndDamage()
        {
            DoDamage = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController p))
            {
                p.GetDamage(damage);
                DoDamage = false;
            }
        }
    }
}
