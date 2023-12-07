using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyController : Entity
    {
        public EnemyType enemyType;
        private Transform player;
        public Rigidbody bullet;
        public Transform gunpivot;
        public GameObject explosion;
        bool isShoot = false;
        public Material normalmaterial;
        public Material damagematerial;

        public override void OnDead()
        {
            Destroy(gameObject);
        }

        public override void OnStart()
        {
            player = PlayerController.instance.transform;
            switch(enemyType)
            {
                case EnemyType.StayAndShoot:
                    StartCoroutine(shoot());
                    break;

                case EnemyType.ChaseAndExplode:

                    break;

                case EnemyType.EscapeAndShoot:
                    StartCoroutine(shoot());
                    break;
            }
            
        }

        public override void OnUpdate()
        {
            RaycastHit hit;
            Vector3 distance = this.transform.position - player.position;
            distance.y = 0;
            if (Physics.Linecast(this.transform.position, player.transform.position, out hit, -1))
            {
                
                if(hit.transform.CompareTag("Player"))
                {
                    switch (enemyType)
                    {
                        case EnemyType.StayAndShoot:

                            if (distance.magnitude > 10)
                            {
                                isShoot = false;
                            }
                            else
                            {
                                isShoot = true;
                                Vector3 lookAtDirection = Vector3.ProjectOnPlane(player.position - transform.position, Vector3.up);
                                this.transform.rotation = Quaternion.LookRotation(lookAtDirection);
                            }
                            break;

                        case EnemyType.ChaseAndExplode:
                            // Define una distancia límite para detonar la explosión
                            float explosionDistanceLimit = 2f;

                            if (distance.magnitude < explosionDistanceLimit)
                            {

                                float yDifference = Mathf.Abs(player.position.y - transform.position.y);
                                float yDifferenceLimit = 2f; 

                                if (yDifference < yDifferenceLimit)
                                {
                                    Instantiate(explosion, this.transform.position, Quaternion.identity);
                                    OnDead();
                                }
                                else
                                {
                                    Vector3 lookAtDirection = Vector3.ProjectOnPlane(player.position - transform.position, Vector3.up);
                                    this.transform.rotation = Quaternion.LookRotation(lookAtDirection);
                                }
                            }
                            else
                            {
                                this.transform.Translate(Vector3.forward * 4f * Time.deltaTime);
                                Vector3 lookAtDirection = Vector3.ProjectOnPlane(player.position - transform.position, Vector3.up);
                                this.transform.rotation = Quaternion.LookRotation(lookAtDirection);
                            }
                            break;

                        case EnemyType.EscapeAndShoot:
                            if (distance.magnitude > 10)
                            {
                                isShoot = true;
                                Vector3 lookAtDirection = Vector3.ProjectOnPlane(player.position - transform.position, Vector3.up);
                                this.transform.rotation = Quaternion.LookRotation(lookAtDirection);
                            }
                            else
                            {
                                isShoot = false;
                                this.transform.Translate(Vector3.back * 10f * Time.deltaTime);
                                Vector3 lookAtDirection = Vector3.ProjectOnPlane(player.position - transform.position, Vector3.up);
                                this.transform.rotation = Quaternion.LookRotation(lookAtDirection);
                            }
                            break;
                    }
                }
            }
        }

        IEnumerator shoot()
        {
            yield return new WaitForSeconds(1);

            if (isShoot)
            {
                // Genera pequeños cambios aleatorios en la dirección del disparo
                Vector3 randomOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.05f, 0.1f), Random.Range(-0.1f, 0.1f));

                // Calcula la dirección del disparo con el offset aleatorio
                Vector3 shootDirection = (player.position - gunpivot.position).normalized + randomOffset;

                // Instancia el proyectil con la dirección modificada
                Rigidbody clone = Instantiate(bullet, gunpivot.transform.position, Quaternion.identity);
                clone.velocity = shootDirection * 15000 * Time.deltaTime;
            }

            StartCoroutine(shoot());
        }

        public enum EnemyType
        {
            StayAndShoot,
            ChaseAndExplode,
            EscapeAndShoot
        }
    }
}
