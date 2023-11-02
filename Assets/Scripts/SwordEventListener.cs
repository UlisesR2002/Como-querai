using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEventListener : MonoBehaviour
{
    public SwordDamage sword;
    public Rigidbody fatherRigidbody;
    public float impulse = 5f;
    // Start is called before the first frame update
    public void StartDamage(float time)
    {
        sword.ActivateDamage(time);
    }

    // Update is called once per frame
    void StopDamage()
    {
        sword.DeactivateDamage();
    }
    void AddJump()
    {
        fatherRigidbody.velocity += Vector3.up * impulse;
    }
}
