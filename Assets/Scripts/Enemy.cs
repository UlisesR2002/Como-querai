using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    public Material NormalMaterial;
    public Material DamageMaterial;
    public float DamageStunTime = 0.2f;
    public float DamageTimer = 0.2f;
    public float HitStopTimer = 0.01f;

    public Slider LifeBar;

    public MeshRenderer Mesh;
    public SkinnedMeshRenderer ModelMesh;

    Animator animator;
    Material[] mats;

    public Animator modelAnimator;

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnUpdate()
    {
        if (DamageTimer > 0)
        {
            DamageTimer -= Time.deltaTime;
            if (Time.timeScale < 1)
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
            if (hp <= 0)
            {
                Debug.Log("Cut off " + (DamageStunTime - DamageTimer) / DamageStunTime);
                if (Mesh != null)
                {
                    Mesh.material.SetFloat("_Cutoff", (DamageStunTime - DamageTimer) / DamageStunTime);
                }
                if (ModelMesh != null)
                {
                    ModelMesh.materials[0].SetFloat("_Cutoff", (DamageStunTime - DamageTimer) / DamageStunTime);
                }
            }
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
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu("Recibir 1 de daño")]
    public void Damage1()
    {
        ReceiveDamage(1);
    }

    public void ReceiveDamage(int Damage)
    {
        if (DamageTimer > 0)
        {
            return;
        }

        GetDamage(Damage);

        LifeBar.value = HpPercentage;
        DamageTimer = DamageStunTime;
        Mesh.material = DamageMaterial;

        if (ModelMesh != null)
        {
            ModelMesh.material = DamageMaterial;
        }

        if (hp > 0) 
        {
            Time.timeScale = 0.2f;
            HitStopTimer = 0.1f;
            animator.SetTrigger("Hit");
            if (modelAnimator != null)
            {
                modelAnimator?.SetTrigger("Hit");
            }
            PlayerController.Instance.ActivateCameraAnimation("Shake");
            DamageTimer = DamageStunTime;
        }
    }


    public override void OnDead()
    {
        HitStopTimer = 0.4f;
        Time.timeScale = 0.01f;
        animator.SetTrigger("Die");
        PlayerController.Instance.ActivateCameraAnimation("Zoom");
        if (modelAnimator != null)
        {
            modelAnimator?.SetTrigger("Hit");
        }
        DamageStunTime = 3f;
        DamageTimer = DamageStunTime;
    }
}
