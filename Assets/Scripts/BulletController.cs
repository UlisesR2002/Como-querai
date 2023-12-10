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
        if (damagePlayer && collision.gameObject.TryGetComponent(out Entity p)) 
        {
            p.TakeDamage(damage);
        }

        if(collision.gameObject.TryGetComponent(out EnemyController e))
        {
            e.TakeDamage(damage);
            StartCoroutine(ChangeMaterialForDuration(e, e.damagematerial, 0.1f));

        }
    }

    IEnumerator ChangeMaterialForDuration(EnemyController enemy, Material newMaterial, float duration)
    {
        enemy.GetComponent<MeshRenderer>().material = newMaterial;
        yield return new WaitForSeconds(duration);
        if (enemy != null)
        {
            enemy.GetComponent<MeshRenderer>().material = enemy.normalmaterial;
        }
        Destroy(gameObject);
    }
}
