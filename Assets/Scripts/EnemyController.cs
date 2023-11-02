using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float searchRadius;
    public float minDistance;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Search();
            return;
        }

        Attack();
    }


    void Attack()
    {
        Vector3 vp = target.position;
        Vector3 vm = transform.position;

        if (Vector3.Distance(vp, vm) > minDistance)
        {
            Vector3 newPosition = Vector3.MoveTowards(vm, vp, speed * Time.deltaTime);
            newPosition.y = vm.y;
            transform.position = newPosition;
        }
        vp.y = vm.y;
        transform.LookAt(vp);
        //transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);

    }


    void Search()
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
