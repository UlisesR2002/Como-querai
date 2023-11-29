using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    [Header("Camera")]
    [SerializeField] private Rigidbody rb;

    [Header("UI")]
    [SerializeField] private GameObject canvasPause;
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
    [SerializeField] private GunController activeGun;
    [SerializeField] private List<GunController> guns;

    public static PlayerController instance;

    // Start is called before the first frame update
    private void Awake()
    {
        isPaused = false;
        instance = this;
        //Initial state
        Cursor.lockState = CursorLockMode.Locked;
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
        FOV();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !canvasPause.activeSelf;
            canvasPause.SetActive(isPaused);

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


        bool crouch = Input.GetKey(KeyCode.C);
        camAnimator.SetBool("Crouch",crouch);
        collAnimator.SetBool("Crouch", crouch);
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
        if (Input.GetButton("Fire1") && !stair && !isPaused)
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

    private void MoveAndRotate()
    {
        if (Input.GetButton("Jump") && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }

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

        rb.velocity = velocity;
        //transform =(speed * Time.deltaTime * keyboard, Space.Self);

        //rb.AddForce(keyboard);
    }

    public override void OnDead()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stair"))
        {
            stair = true;
            rb.useGravity = false;
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
}
