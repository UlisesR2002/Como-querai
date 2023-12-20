using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyController : Entity
    {
        [Header("Type Enemy")]
        public EnemyType enemyType;
        public float stayDistance = 15f;
        public float chaseDistance = 4f;
        public float escapeDistance = 10;


        [Header("Game Object")]
        private Transform player;
        public Rigidbody bullet;
        public Transform gunpivot;

        [Header("Particles")]
        public GameObject deadparticles;
        public GameObject explosion;
        bool isShoot = false;

        [Header("Materials")]
        public Material normalmaterial;
        public Material damagematerial;

        [Header("Audio")]
        public AudioClip shootSound;
        private AudioSource audioSource;

        public override void OnDead()
        {
            switch (enemyType)
            {
                case EnemyType.StayAndShoot:
                    Instantiate(deadparticles, this.transform.position, Quaternion.identity);
                    break;

                case EnemyType.ChaseAndExplode:
                    Instantiate(explosion, this.transform.position, Quaternion.identity);
                    break;

                case EnemyType.EscapeAndShoot:
                    Instantiate(deadparticles, this.transform.position, Quaternion.identity);
                    break;
            }


            Destroy(gameObject);
        }

        public override void OnStart()
        {
            player = PlayerController.instance.transform;
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            switch (enemyType)
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
            audioSource.volume = VolumeManager.GetVolume();

            Vector3 distance = this.gunpivot.position - player.position;
            distance.y = 0;
            if (Physics.Linecast(this.gunpivot.position, player.transform.position, out RaycastHit hit, -1))
            {
                            Debug.Log("DISTANCE TO PLAYER " + distance.magnitude);
                if (hit.transform.CompareTag("Player"))
                {
                    switch (enemyType)
                    {
                        case EnemyType.StayAndShoot:


                            if (distance.magnitude > stayDistance)
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
                            if (distance.magnitude < chaseDistance)
                            {

                                float yDifference = Mathf.Abs(player.position.y - transform.position.y);
                                float yDifferenceLimit = 4f;

                                if (yDifference < yDifferenceLimit)
                                {

                                    //Instantiate(explosion, this.transform.position, Quaternion.identity);
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
                                this.transform.Translate(4f * Time.deltaTime * Vector3.forward);
                                Vector3 lookAtDirection = Vector3.ProjectOnPlane(player.position - transform.position, Vector3.up);
                                this.transform.rotation = Quaternion.LookRotation(lookAtDirection);
                            }
                            break;

                        case EnemyType.EscapeAndShoot:
                            if (distance.magnitude > escapeDistance)
                            {
                                isShoot = false;
                                Vector3 lookAtDirection = Vector3.ProjectOnPlane(player.position - transform.position, Vector3.up);
                                this.transform.rotation = Quaternion.LookRotation(lookAtDirection);
                            }
                            else
                            {
                                isShoot = true;
                                this.transform.Translate(Vector3.back * 10f * Time.deltaTime);
                                Vector3 lookAtDirection = Vector3.ProjectOnPlane(player.position - transform.position, Vector3.up);
                                this.transform.rotation = Quaternion.LookRotation(lookAtDirection);
                            }
                            break;
                    }
                }
            }
        }
        public void isHurt()
        {
            StartCoroutine(ChangeMaterialForDuration(0.1f));
        }

        IEnumerator shoot()
        {
            yield return new WaitForSeconds(1);

            if (isShoot)
            {
                // Genera pequeños cambios aleatorios en la dirección del disparo
                Vector3 randomOffset = new Vector3(Random.Range(-0.07f, 0.07f), Random.Range(-0.05f, 0.07f), Random.Range(-0.07f, 0.07f));

                // Calcula la dirección del disparo con el offset aleatorio
                Vector3 shootDirection = (player.position - gunpivot.position).normalized + randomOffset;

                // Instancia el proyectil con la dirección modificada
                Rigidbody clone = Instantiate(bullet, gunpivot.transform.position, Quaternion.identity);
                clone.velocity = shootDirection * 15000 * Time.deltaTime;

                if (audioSource != null && shootSound != null)
                {
                    audioSource.PlayOneShot(shootSound, 2.0f);
                }
            }

            StartCoroutine(shoot());
        }

        IEnumerator ChangeMaterialForDuration(float duration)
        {
            GetComponent<MeshRenderer>().material = damagematerial;
            yield return new WaitForSeconds(duration);
            if (hp >= 0)
            {
                GetComponent<MeshRenderer>().material = normalmaterial;
            }

        }

        public enum EnemyType
        {
            StayAndShoot,
            ChaseAndExplode,
            EscapeAndShoot
        }


        private void OnDrawGizmosSelected()
        {
            float radius = 0f;
            switch (enemyType)
            {
                case EnemyType.StayAndShoot:
                    radius = stayDistance;
                    break;
                case EnemyType.ChaseAndExplode:
                    radius = chaseDistance;
                    break;
                case EnemyType.EscapeAndShoot:
                    radius = escapeDistance;
                    break;
                default:
                    break;
            }

            Gizmos.DrawWireSphere(transform.position, radius);

            if(player != null)
                Gizmos.DrawLine(gunpivot.position, player.transform.position);
        }

        
    }
}
