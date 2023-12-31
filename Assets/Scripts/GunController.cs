using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("References")]
    public string gunTag;
    [SerializeField] private GameObject[] gunModel;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject pivot;
    [SerializeField] private Animator animator;
    
    [Header("Bullets")]
    [SerializeField] private int bulletDamage;
    [SerializeField] private bool infiniteAmmo;
    public float ammo;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDelay;

    [Header("Aim")]
    [SerializeField] private bool aiming;
    [SerializeField] private GameObject aim;
    [SerializeField] private float aimFOV;

    [Header("Audio")]
    public AudioClip shootMainSound;
    private AudioSource audioSource;

    private GunDelayer delayer;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        delayer = new(bulletDelay);
    }
    private void Update()
    {
        audioSource.volume = VolumeManager.GetVolume();
    }

    public void TryShoot(Vector3 where)
    {
        if (delayer.CanShoot())
        {
            if (ammo <= 0 && !infiniteAmmo)
            {
                return;
            }
            ammo--;

            Vector3 from = pivot.transform.position;

            if (aiming)
            {
                from = PlayerController.instance.cameraObject.transform.position + 
                    PlayerController.instance.cameraObject.transform.forward * 0.8f;
                
            }

            animator.SetTrigger("Fire");

            // Calcula la direcci�n hacia el punto
            Vector3 direction = where - from;

            // Calcula la rotaci�n hacia esa direcci�n
            Quaternion rotation = Quaternion.LookRotation(direction);

            //Recoil
            Vector3 newPlayerAngle = PlayerController.instance.transform.rotation.eulerAngles;
            Vector3 newCameraAngle =  PlayerController.instance.cameraObject.transform.rotation.eulerAngles;

            newPlayerAngle.y += Random.Range(-1f, 1f);
            newCameraAngle.x += Random.Range(-2.5f, -1f);

            PlayerController.instance.transform.rotation = Quaternion.Euler(newPlayerAngle);
            PlayerController.instance.cameraObject.transform.rotation = Quaternion.Euler(newCameraAngle);

            GameObject go = Instantiate(bulletPrefab, from, rotation);
            if (audioSource != null && shootMainSound != null)
            {
                audioSource.PlayOneShot(shootMainSound, 1.0f);
            }

            if (go.TryGetComponent(out Rigidbody r))
            {
                r.AddRelativeForce(Vector3.forward * bulletSpeed,ForceMode.VelocityChange);
            }

            if (go.TryGetComponent(out BulletController b))
            {
                b.damage = bulletDamage;
                b.damagePlayer = false;
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

    public string GetAmmoText()
    {
        if (infiniteAmmo)
        {
            return "Infinite";
        }


        return ammo.ToString();
    }
}
