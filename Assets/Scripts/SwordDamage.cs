using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int damage;
    public bool DoDamage;
    public List<Enemy> enemiesHitted;

    public void ActivateDamage(int damage)
    {
        enemiesHitted.Clear();
        this.damage = damage;
        DoDamage = true;
    }
    public void DeactivateDamage()
    {
        enemiesHitted.Clear();
        DoDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (DoDamage && other.gameObject.TryGetComponent(out Enemy e) && !enemiesHitted.Contains(e))
        {
            e.ReceiveDamage(damage);                
            enemiesHitted.Add(e);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (DoDamage && other.gameObject.TryGetComponent(out Enemy e) && !enemiesHitted.Contains(e))
        {
            e.ReceiveDamage(damage);
            enemiesHitted.Add(e);
        }
    }
}
