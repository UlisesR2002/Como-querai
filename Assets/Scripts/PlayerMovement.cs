using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float rotSpeed = 100;
    public float jumpForce = 200;

    public float ComboTimer = 0;
    public float AirComboTimer = 0;
    public Vector3 savedVelocity;

    public Animator SwordAnimator;
    public Collider checkGroundCollider;
    public List<Collider> groundColliders = new List<Collider>();

    public SwordDamage swordDamageComponent;

    Rigidbody rg;

    public static PlayerMovement Instance;

    public Animator CameraAnimator;
    public Transform CameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        rg = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            //transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
            movement += CameraTransform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //transform.Rotate(0, -rotSpeed * Time.deltaTime, 0);
            movement -= CameraTransform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
            movement += CameraTransform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement -= CameraTransform.forward;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        movement.y = 0;
        movement.Normalize();
        movement *= speed * Time.deltaTime;
        Quaternion PrevRot = transform.rotation;
        transform.LookAt(transform.position + movement);
        Quaternion ObjRot = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(PrevRot, ObjRot, Time.deltaTime * 1000);
        
        transform.position += movement;
        if (ComboTimer >= 0)
        {
            ComboTimer -= Time.deltaTime;
        }
        if (AirComboTimer >= 0)
        {
            AirComboTimer -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (ComboTimer <= 0)
            {
                SwordAnimator.SetTrigger("Attack1");
                ComboTimer = AirComboTimer = 0.5f;
                savedVelocity = rg.velocity;
                swordDamageComponent.ActivateDamage(0.3f);
            }
            else
            {
                SwordAnimator.SetTrigger("Attack2");
                AirComboTimer = 0.3f;
                swordDamageComponent.ActivateDamage(0.2f);
            }
        }
        bool canJump = false;
        foreach (Collider groundCollider in groundColliders)
        {
            if (groundCollider!=null && checkGroundCollider.bounds.Intersects(groundCollider.bounds))
            {
                canJump = true;
                break;
            }
        }
        if (canJump) {         
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 currentVelocity = rg.velocity;
                currentVelocity.y = jumpForce;
                rg.velocity = currentVelocity;
            }
        }
        else
        {
            /*if (AirComboTimer >= 0)
            {
                rg.velocity = Vector3.zero;
            }
            else if (savedVelocity.sqrMagnitude > 0)
            {
                rg.velocity = savedVelocity;
                savedVelocity = Vector3.zero;
            }
            else */
            if(rg.velocity.y < 0)
            {
                rg.AddForce(100f*rg.mass *Time.deltaTime* Physics.gravity);
                Debug.Log("Falling Harder");
            }
        }
    }

    public void ActivateCameraAnimation(string animation)
    {
        CameraAnimator.SetTrigger(animation);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!groundColliders.Contains(collision.collider))
        {
            groundColliders.Add(collision.collider);
        }
        
    }
}
