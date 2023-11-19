using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private Camera cam;

    [Range(0, 90), SerializeField] private float cameraMaxUpDown;
    [SerializeField] private float cameraSensibility;
    [SerializeField] private float fovSensibility;

    [SerializeField] private float minFOV;
    [SerializeField] private float maxFOV;


    [SerializeField] private float speed;
    public static PlayerController instance;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        //Initial state
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnStart(){}

    // Update is called once per frame
    public override void OnUpdate()
    {
        MoveAndRotate();
        Shoot();

        float oldFOV = cam.fieldOfView;
        oldFOV += -fovSensibility * Input.mouseScrollDelta.y * Time.deltaTime;
        oldFOV = Mathf.Clamp(oldFOV, minFOV, maxFOV);
        cam.fieldOfView = oldFOV;
    }

    private void Shoot()
    {
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Shoot!");

            //Question for timer
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit i, Mathf.Infinity))
            {
                if (i.transform.TryGetComponent(out PlayerController p))
                {
                    Debug.Log("MySelf???");
                }

                if (i.transform.TryGetComponent(out EnemyController e))
                {
                    e.TakeDamage(1);
                }
            }
        }
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
        transform.Translate(speed * Time.deltaTime * keyboard, Space.Self);
    }

    public override void OnDead()
    {
        Destroy(gameObject);
    }
}
