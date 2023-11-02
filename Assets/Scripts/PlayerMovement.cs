using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float rotSpeed = 100;
    public float jumpForce = 200;

    public float AttackTimer = 0;
    public float ComboTimer = 0;


    public Animator SwordAnimator;
    public Collider checkGroundCollider;
    public List<Collider> groundColliders = new List<Collider>();

    public SwordDamage swordDamageComponent;

    Rigidbody rg;

    public static PlayerMovement Instance;

    public Animator CameraAnimator;
    public Transform CameraTransform;

    public string comboKey;
    public List<string> combos;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Instance = this;
        rg = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        Movement();


        if (ComboTimer >= 0)
        {
            ComboTimer -= Time.deltaTime;
        }

        if (AttackTimer >= 0)
        {
            AttackTimer -= Time.deltaTime;
        }

        if (ComboTimer < 0)
        {
            comboKey = "";
        }

        if (AttackTimer <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack("1");
            }

            if (Input.GetMouseButtonDown(1))
            {
                Attack("2");
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

    void Attack(string AttackID)
    {
        comboKey += AttackID;

        //Si esta ruta tiene combo
        bool comboRoute = false;

        foreach (string combo in combos)
        {
            //Suponemos que este combo si tiene ruta
            bool thisCombo = true;

            for (int i = 0; i < comboKey.Length; i++)
            {
                if (combo[i] != comboKey[i])
                {
                    //Si no es asi, pasamos al siguente
                    thisCombo = false;
                    break;
                }
            }

            //si este combo funciona, nos vale, nos salimos
            if (thisCombo)
            {
                comboRoute = true;
                break;
            }
        }

        //Si no tiene combo
        if (!comboRoute)
        {
            //quitamos este movimiento y nos vamos
            comboKey = comboKey.Substring(0, comboKey.Length - 1);
            return;
        }

        //Si este es el ultimo ataque del combo
        if (comboKey.Length >= 3)
        {
            //No se puede atacar por x segundos
            AttackTimer = 0.6f;
        }


        ComboTimer = 0.5f;

        if (ComboTimer <= 0)
        {
            SwordAnimator.SetTrigger("Attack1");
            swordDamageComponent.ActivateDamage(0.3f);
        }
        else
        {
            SwordAnimator.SetTrigger("Attack2");
            swordDamageComponent.ActivateDamage(0.2f);
        }
    }

    void Movement()
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

        movement.y = 0;
        movement.Normalize();
        movement *= speed * Time.deltaTime;
        Quaternion PrevRot = transform.rotation;
        transform.LookAt(transform.position + movement);
        Quaternion ObjRot = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(PrevRot, ObjRot, Time.deltaTime * 1000);

        transform.position += movement;
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

