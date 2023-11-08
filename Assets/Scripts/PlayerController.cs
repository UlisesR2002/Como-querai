using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Entity
{
    public static PlayerController Instance;
    
    [Header("References")]
    public Animator playerAnimator;
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
    public bool dashing;
    public float dashSpeed = 10;

    [Header("Attack")]
    public bool blocking;
    public float AttackTimer = 0;
    public float ComboTimer = 0;
    public string comboKey;
    public List<string> combos;

    [Header("Caracteristics")]
    public float maxHealth = 100f;
    public float health;


    // Start is called before the first frame update
    public override void OnStart()
    {
        Instance = this;
        rg = GetComponent<Rigidbody>();
        health = maxHealth;
    }

    // Update is called once per frame

    public override void OnUpdate()
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
        Block();

        if (!swordDamageComponent.DoDamage && !blocking && !dashing && grounded && Input.GetKeyDown(KeyCode.F))
        {
            playerAnimator.SetTrigger("Dash");
        }


        #region Timers

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
        #endregion

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
    }

    private void Block()
    {
        blocking = false;
        //Si no estamos haciendo daño, o haciendo un dash
        if (!swordDamageComponent.DoDamage && !dashing && Input.GetKey(KeyCode.E))
        {
            blocking = true;
        }
        SwordAnimator.SetBool("Block", blocking);
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

        float s = speed;

        if (dashing)
        {
            movement.Set(0, 0, 0);
            movement -= CameraTransform.forward;
            s = dashSpeed;
        }

        movement.Normalize();
        movement *= s * Time.deltaTime;

        Quaternion PrevRot = transform.rotation;

        if (dashing)
        {
            transform.LookAt(transform.position - movement);
        }
        else
        {
            transform.LookAt(transform.position + movement);
        }

        Quaternion ObjRot = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(PrevRot, ObjRot, Time.deltaTime * 1000);

        transform.position += movement;

        if (grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 currentVelocity = rg.velocity;
                currentVelocity.y = jumpForce;
                rg.velocity = currentVelocity;
            }
        }
        else if (rg.velocity.y < 0)
        {
            rg.AddForce(100f * rg.mass * Time.deltaTime * Physics.gravity);
            Debug.Log("Falling Harder");
        }
    }

    public void ActivateCameraAnimation(string animation)
    {
        CameraAnimator.SetTrigger(animation);
    }

    #region Life

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            health -= 10;
        }
    }

    public override void OnDead()
    {
        TransitionController.instance.StartTransition(SceneManager.GetActiveScene().name);
    }

    #endregion

    #region Event Listener
    public void StartDashing()
    {
        dashing = true;
    }
    public void EndDashing()
    {
        dashing = false;
    }
    #endregion
}

