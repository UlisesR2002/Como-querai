using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int maxHP = 3;
    public int hp;
    public float HpPercentage { get => (float)hp / (float)maxHP; }

    public virtual void GetDamage(int dmg = 1)
    {
        hp -= dmg;
        if (hp < 0)
        {
            OnDead();
        }
    }

    private void Start()
    {
        hp = maxHP;

        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void OnDead();

}
