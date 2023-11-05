using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    [Header("References")]
    public Animator SwordAnimator;
    public SwordDamage swordDamageComponent;
    public Animator CameraAnimator;
    public Transform CameraTransform;

    [Header("Movement")]
    public float speed = 10;
    public float rotSpeed = 100;
    public float jumpForce = 200;
    public LayerMask groundLayer;
    public bool grounded;
    private Rigidbody rg;

    [Header("Attack")]
    public bool blocking;
    public float AttackTimer = 0;
    public float ComboTimer = 0;
    public string comboKey;
    public List<string> combos;



    // Start is called before the first frame update
    void Start()
    {
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

        grounded = false;

        if (Physics.Raycast(transform.position, Vector3.down, 0.7f, groundLayer))
        {
            grounded = true;    
        }

        Movement();

        blocking = false;
        if (Input.GetKey(KeyCode.E))
        {
            blocking = true;
        }

        SwordAnimator.SetBool("Block", blocking);

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

        if (grounded) 
        {         
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 currentVelocity = rg.velocity;
                currentVelocity.y = jumpForce;
                rg.velocity = currentVelocity;
            }
        }
        else if(rg.velocity.y < 0)
        {
            rg.AddForce(100f*rg.mass *Time.deltaTime* Physics.gravity);
            Debug.Log("Falling Harder"); 
        }
    }

    void Attack(string AttackID)
    {
        if (blocking)
        {
            return;    
        }

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

        SwordAnimator.SetTrigger(comboKey);
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
}

