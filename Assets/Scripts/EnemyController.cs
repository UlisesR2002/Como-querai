using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyController : Entity
    {
        public override void OnDead()
        {
            Destroy(gameObject);
        }

        public override void OnStart()
        {
        }

        public override void OnUpdate()
        {
        }
    }
}
