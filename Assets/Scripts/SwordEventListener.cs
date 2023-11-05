using UnityEngine;

public class SwordEventListener : MonoBehaviour
{
    public SwordDamage sword;
    public Rigidbody fatherRigidbody;
    public float impulse = 5f;

    // Start is called before the first frame update
    public void StartDamage()
    {
        //Debug.Log("Damage listener");
        sword.ActivateDamage();
    }

    // Update is called once per frame
    public void StopDamage()
    {
        //Debug.Log("Stop Damage listener");
        sword.DeactivateDamage();
    }

    public void AddJump()
    {
        fatherRigidbody.velocity += Vector3.up * impulse;
    }
}
