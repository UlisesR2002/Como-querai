using Assets.Scripts;
using UnityEngine;


public class BulletController : MonoBehaviour
{
    public float damage = 1;
    public bool damagePlayer = true;
    public bool damageEnemy = true;
    public bool damageEntities = true;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (damagePlayer && collision.gameObject.TryGetComponent(out PlayerController p))
        {
            p.TakeDamage(damage);
            p.isHurt();
            Destroy(gameObject);
            return;
        }
        else if (damagePlayer == false && collision.gameObject.TryGetComponent(out PlayerController _))
        {
            return;    
        }

        if(damageEnemy && collision.gameObject.TryGetComponent(out EnemyController e))
        {
            e.TakeDamage(damage);
            e.isHurt();
            Destroy(gameObject);
            return;
        }

        if (damageEntities && collision.gameObject.TryGetComponent(out Entity en))
        {
            en.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }
}
