using UnityEngine;

public abstract class EnemyAI : Entity
{
    public Transform target;
    public float searchRadius;
    public float minDistance;
    public float speed;
    public bool attack = false;
    public Animator animator;

    public override void OnUpdate()
    {
        if (target == null)
        {
            Search();
            return;
        }

        Attack();
    }

    protected void Attack()
    {
        Vector3 vp = target.position;
        Vector3 vm = transform.position;

        if (Vector3.Distance(vp, vm) > minDistance)
        {
            Vector3 newPosition = Vector3.MoveTowards(vm, vp, speed * Time.deltaTime);
            newPosition.y = vm.y;
            transform.position = newPosition;
        }
        else if(!attack)
        {
            animator.SetTrigger("Attack1");
        }

        vp.y = vm.y;
        transform.LookAt(vp);
        //transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
    }


    protected void Search()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                target = hitCollider.transform;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
