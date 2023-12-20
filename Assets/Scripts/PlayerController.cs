using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : Entity
{
    [Header("Camera")]
    [SerializeField] private Rigidbody rb;

    [Header("UI")]
    [SerializeField] private GameObject canvasPause;
    [SerializeField] private GameObject canvasControl;
    [SerializeField] private Image redSplatterImage = null;
    [SerializeField] private Image hurtImage = null;
    public static bool isPaused;

    [Header("Camera")]
    public GameObject cameraObject;
    [SerializeField] private Camera cam;
    [Range(0, 90), SerializeField] private float cameraMaxUpDown;
    [SerializeField] private float cameraSensibility;

    [Header("Animator")]
    [SerializeField] private Animator camAnimator;
    [SerializeField] private Animator collAnimator;

    [Header("FOV")]
    [SerializeField] private float fovSensibility;
    [SerializeField] private float minFOV;
    [SerializeField] private float maxFOV;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool grounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool crouch;
    [SerializeField] private bool stair;
    [SerializeField] private float stairSpeed;

    [Header("Combat")]
    [SerializeField] private bool aim;
    [SerializeField] private GunController activeGun;
    [SerializeField] private List<GunController> guns;

    [Header("Regen")]
    [SerializeField] public int regenRate;
    public static PlayerController instance;

    [Header("Audio")]
    public AudioClip walkSound;
    public AudioClip pauseSound;
    private AudioSource walkAudioSource;
    private AudioSource pauseAudioSource;
    [SerializeField] private float walkVolume = 0.5f;


    // Start is called before the first frame update
    private void Awake()
    {
        isPaused = false;
        instance = this;
        //Initial state
        Cursor.lockState = CursorLockMode.Locked;

        walkAudioSource = gameObject.AddComponent<AudioSource>();
        pauseAudioSource = gameObject.AddComponent<AudioSource>();
        walkAudioSource.volume = walkVolume;
        walkAudioSource.loop = true;
    }

    public override void OnStart()
    {
    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit _, 1.5f, groundLayer);

        MoveAndRotate();
        Shoot();
        ChangeWeapon();
        UpdateHealth();

        if (!aim)
        {
            FOV();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        bool crouch = Input.GetKey(KeyCode.C);
        camAnimator.SetBool("Crouch", crouch);
        collAnimator.SetBool("Crouch", crouch);
    }
    public void Pause()
    {
        pauseAudioSource.PlayOneShot(pauseSound);
        isPaused = !canvasPause.activeSelf;
        canvasPause.SetActive(isPaused);
        canvasControl.SetActive(false);
        if (isPaused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void FOV()
    {
        float oldFOV = cam.fieldOfView;
        oldFOV += -fovSensibility * Input.mouseScrollDelta.y * Time.deltaTime;
        oldFOV = Mathf.Clamp(oldFOV, minFOV, maxFOV);
        cam.fieldOfView = oldFOV;
    }

    private void Shoot()
    {
        if (isPaused)
        {
            aim = activeGun.OnAim(false, cam);
            return;
        }
        if (Input.GetButton("Fire1") && !stair)
        {

            if (Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out RaycastHit i, Mathf.Infinity))
            {
                activeGun.TryShoot(i.point);
            }
            else
            {
                //Si no encontramos nada disparamos como objetivo hacia algun objeto en 100 unidades
                Vector3 where = cameraObject.transform.position + cameraObject.transform.forward * 100f;
                activeGun.TryShoot(where);
            }
        }

        aim = Input.GetButton("Fire2");
        aim = activeGun.OnAim(aim, cam);
    }

    private void ChangeWeapon()
    {
        if (Input.GetButtonDown("Fire3") && !isPaused)
        {
            int index = guns.IndexOf(activeGun);
            index++;
            if (index == guns.Count)
            {
                index = 0;
            }

            activeGun.gameObject.SetActive(false);

            activeGun = guns[index];

            activeGun.gameObject.SetActive(true);
        }
    }
    public void UpdateHealth()
    {
        Color splatterAlpha = redSplatterImage.color;
        splatterAlpha.a = 1 - HpPercentage;
        redSplatterImage.color = splatterAlpha;

        if (hp <= maxHP - 0.01f)
        {
            hp += Time.deltaTime * regenRate;
        }
        else
        {
            hp = maxHP;
        }
    }

    public void isHurt()
    {
        regenRate = 0;
        StartCoroutine(HurtFlash());
        StartCoroutine(WaitRegen());
    }

    private void MoveAndRotate()
    {
        //Camera
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Vector2 mouse = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            //Rotate x
            transform.Rotate(0, mouse.x * cameraSensibility * Time.deltaTime, 0);
            //Rotate y
            float oldAngle = Utilities.AngleOfVector(Utilities.VectorOfAngle(cameraObject.transform.rotation.eulerAngles.x));
            //Debug.Log(oldAngle);
            oldAngle -= mouse.y * Time.deltaTime * cameraSensibility;
            oldAngle = Mathf.Clamp(oldAngle, -cameraMaxUpDown, cameraMaxUpDown);

            cameraObject.transform.localRotation = Quaternion.Euler(oldAngle, 0, 0);

        }


        //Movement
        Vector3 keyboard = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (stair)
        {
            //Si presionamos hacia adelante
            if (keyboard.z > 0)
            {
                //Subimos
                keyboard.y = stairSpeed * Time.deltaTime;
                keyboard.z = 0;

            }
            //Si presionamos hacia abajo y no estamos en el suelo
            else if (keyboard.z < 0 && !grounded)
            {
                keyboard.z = 0;
                keyboard.y = -stairSpeed * Time.deltaTime;
            }
        }

        Vector3 velocity = speed * transform.TransformDirection(keyboard);

        if(!stair)
            velocity.y = rb.velocity.y; // Preserve the y component to maintain gravity effect

        if (Input.GetButton("Jump") && grounded && !stair)
        {
            velocity.y = jumpForce;
        }

        rb.velocity = velocity;
        PlaySound();
    }

    public override void OnDead()
    {
        Time.timeScale = 0.01f;
        TransitionController.transitionController.StartTransition(SceneManager.GetActiveScene().name);
    }

    public void PlaySound()
    {
        if (!isPaused)
        {
            // Reproducir sonido de caminar si el player se está moviendo hacia adelante o hacia atrás y está en el suelo
            if (grounded && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !walkAudioSource.isPlaying)
            {
                walkAudioSource.clip = walkSound;
                walkAudioSource.Play();
            }
            else if (!grounded || (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))
            {
                // Detener el sonido si no está en el suelo o no se está moviendo
                walkAudioSource.Stop();
            }
        }
        else
        {
            // Detener el sonido si el juego está pausado
            walkAudioSource.Stop();
        }

    }

    public void GiveAmmo(string tag, int ammo)
    {
        foreach (var gun in guns)
        {
            if (gun.gunTag == tag)
            {
                gun.ammo += ammo;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stair"))
        {
            stair = true;
            rb.useGravity = false;
        }

        if(other.CompareTag("Ammo"))
        {
            if (other.TryGetComponent(out Ammo a))
            {
                GiveAmmo(a.tag, a.ammo);
                Destroy(a.gameObject);
            }
            else
            {
                Debug.LogError("Municion sin el script de municion!!!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stair"))
        {
            stair = false;
            rb.useGravity = true;
        }
    }

    IEnumerator HurtFlash()
    {
        hurtImage.enabled = true;
        yield return new WaitForSeconds(0.1f);
        hurtImage.enabled = false;
    }

    IEnumerator WaitRegen()
    {
        yield return new WaitForSeconds(5.0f);
        regenRate = 1;
    }
}
