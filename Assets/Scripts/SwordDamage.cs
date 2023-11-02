using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public float DamageTimer = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DamageTimer > 0)
        {
            DamageTimer -= Time.deltaTime;
        }
    }
    public void ActivateDamage(float newDamageTime)
    {
        DamageTimer = newDamageTime;
    }
    public void DeactivateDamage()
    {
        DamageTimer = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (DamageTimer > 0)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ReceiveDamage(0.5f);                
            }
        }
    }
}
