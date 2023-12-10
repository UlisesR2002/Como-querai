using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage = 1;
    public bool damagePlayer = true;

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
        }

        if(collision.gameObject.TryGetComponent(out EnemyController e))
        {
            e.TakeDamage(damage);
            e.isHurt();
            Destroy(gameObject);

        }
    }
}
