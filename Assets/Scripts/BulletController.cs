using Assets.Scripts;
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
        if (damagePlayer && collision.gameObject.TryGetComponent(out Entity p)) 
        {
            p.TakeDamage(damage);
        }

        if(collision.gameObject.TryGetComponent(out EnemyController e))
        {
            e.TakeDamage(damage);
            e.GetComponent<MeshRenderer>().material = e.damagematerial;
            Destroy(gameObject);
        }
    }
}
