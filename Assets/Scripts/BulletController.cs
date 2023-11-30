using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Entity e))
        {
            e.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
