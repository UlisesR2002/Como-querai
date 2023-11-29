using UnityEngine;
using UnityEngine.Playables;
using static UnityEngine.GraphicsBuffer;

public class GunController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject pivot;
    [SerializeField] private bool infiniteAmmo;
    [SerializeField] private float bullets;
    [SerializeField] private float s;

    [SerializeField] private float bulletDelay;
    [SerializeField] private Animator animator;
    private GunDelayer delayer;

    // Start is called before the first frame update
    void Start()
    {
        delayer = new(bulletDelay);
    }

    public void TryShoot(Vector3 where)
    {
        if (delayer.CanShoot())
        {
            if (bullets <= 0 && !infiniteAmmo)
            {
                return;
            }

            animator.SetTrigger("Fire");

            // Calcula la dirección hacia el punto
            Vector3 direction = where - pivot.transform.position;

            // Calcula la rotación hacia esa dirección
            Quaternion rotation = Quaternion.LookRotation(direction);

            GameObject go = Instantiate(bulletPrefab,pivot.transform.position, rotation);

            if (go.TryGetComponent(out Rigidbody r))
            {
                r.AddRelativeForce(Vector3.forward * s,ForceMode.VelocityChange);
            }
        }
    }
}
