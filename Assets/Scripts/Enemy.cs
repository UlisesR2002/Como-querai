using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Material NormalMaterial;
    public Material DamageMaterial;
    public float HP = 5;
    float OriginalHP;
    public float DamageStunTime = 0.2f;
    public float DamageTimer = 0.2f;
    public float HitStopTimer = 0.01f;

    public Slider LifeBar;

    public MeshRenderer Mesh;
    public SkinnedMeshRenderer ModelMesh;

    Animator animator;
    Material[] mats;

    public Animator modelAnimator;
    // Start is called before the first frame update
    void Start()
    {
        OriginalHP = HP;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DamageTimer > 0)
        {
            DamageTimer -= Time.deltaTime;
            if(Time.timeScale<1)
            {
                HitStopTimer -= Time.unscaledDeltaTime;
                if (HitStopTimer < 0)
                {
                    Time.timeScale = 1;
                }
            }
            if (DamageTimer <= 0)
            {
                EndDamageFrame();
            }
            if (HP <= 0)
            {
                Debug.Log("Cut off "+ (DamageStunTime - DamageTimer) / DamageStunTime);
                if (Mesh != null)
                {
                    Mesh.material.SetFloat("_Cutoff", (DamageStunTime - DamageTimer) / DamageStunTime);
                }
                if(ModelMesh!=null)
                {
                    ModelMesh.materials[0].SetFloat("_Cutoff", (DamageStunTime - DamageTimer) / DamageStunTime);
                }
            }
        }
        else if (PlayerMovement.Instance != null)
        {
            Vector3 lookAtPos = PlayerMovement.Instance.transform.position;
            lookAtPos.y = transform.position.y;
            transform.LookAt(lookAtPos);
            transform.Rotate(0, 180, 0);
        }
    }
    public void EndDamageFrame()
    {
        DamageTimer = 0;
        Time.timeScale = 1;
        Mesh.material = NormalMaterial;
        if (ModelMesh != null)
        {
            ModelMesh.material = NormalMaterial;
        }
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void ReceiveDamage(float Damage)
    {
        if (DamageTimer > 0)
        {
            return;
        }
        HP -= Damage;
        LifeBar.value = HP / OriginalHP;
        DamageTimer = DamageStunTime;
        Mesh.material = DamageMaterial;
        if (ModelMesh != null)
        {
            ModelMesh.material = DamageMaterial;
        }
        if (HP > 0) {
            Time.timeScale = 0.2f;
            HitStopTimer = 0.1f;
            animator.SetTrigger("Hit");
            if (modelAnimator != null)
            {
                modelAnimator?.SetTrigger("Hit");
            }
            PlayerMovement.Instance.ActivateCameraAnimation("Shake");
            DamageTimer = DamageStunTime;
        }
        else
        {
            HitStopTimer = 0.4f;
            Time.timeScale = 0.01f;
            animator.SetTrigger("Die");
            PlayerMovement.Instance.ActivateCameraAnimation("Zoom");
            if (modelAnimator != null)
            {
                modelAnimator?.SetTrigger("Hit");
            }
            DamageStunTime = 3f;
            DamageTimer = DamageStunTime;
        }
    }
}
