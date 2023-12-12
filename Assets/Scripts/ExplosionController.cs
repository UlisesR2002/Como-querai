using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage = 1;
    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        Destroy(collider, 0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController p))
        {
            p.TakeDamage(damage);
            p.isHurt();
            return;
        }

        if (collision.gameObject.TryGetComponent(out EnemyController e))
        {
            e.TakeDamage(damage);
            e.isHurt();
            return;
        }

        if (collision.gameObject.TryGetComponent(out Entity en))
        {
            en.TakeDamage(damage);
            return;
        }
    }
}
