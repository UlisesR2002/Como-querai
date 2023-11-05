using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public bool DoDamage;
    public List<Enemy> enemiesHitted;

    public void ActivateDamage()
    {
        enemiesHitted.Clear();
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
            e.ReceiveDamage(1);                
            enemiesHitted.Add(e);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (DoDamage && other.gameObject.TryGetComponent(out Enemy e) && !enemiesHitted.Contains(e))
        {
            e.ReceiveDamage(1);
            enemiesHitted.Add(e);
        }
    }
}
