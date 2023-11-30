using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] gunModel;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject pivot;
    [SerializeField] private Animator animator;
    
    [Header("Bullets")]
    [SerializeField] private int bulletDamage;
    [SerializeField] private bool infiniteAmmo;
    [SerializeField] private float bullets;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDelay;

    [Header("Aim")]
    [SerializeField] private bool aiming;
    [SerializeField] private GameObject aim;
    [SerializeField] private float aimFOV;

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

            Vector3 from = pivot.transform.position;

            if (aiming)
            {
                from = PlayerController.instance.cameraObject.transform.position + 
                    PlayerController.instance.cameraObject.transform.forward * 0.8f;
                
            }

            animator.SetTrigger("Fire");

            // Calcula la dirección hacia el punto
            Vector3 direction = where - from;

            // Calcula la rotación hacia esa dirección
            Quaternion rotation = Quaternion.LookRotation(direction);

            GameObject go = Instantiate(bulletPrefab, from, rotation);

            if (go.TryGetComponent(out Rigidbody r))
            {
                r.AddRelativeForce(Vector3.forward * bulletSpeed,ForceMode.VelocityChange);
            }

            if (go.TryGetComponent(out BulletController b))
            {
                b.damage = bulletDamage;
            }
        }
    }

    public bool OnAim(bool active, Camera playerCamera)
    {
        if (aim != null)
        { 
            aim.SetActive(active);

            foreach (GameObject g in gunModel) 
            {
                g.SetActive(!active);
            }

            if (active)
            { 
                playerCamera.fieldOfView = aimFOV;
            }

            aiming = active;
            return active;
        }

        return false;
    }
}
