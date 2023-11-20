using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyController : Entity
    {
        public EnemyType enemyType;
        public Transform player;
        public Rigidbody bullet;
        public Transform gun;
        public GameObject explosion;
        bool isShoot = false;

        public override void OnDead()
        {
            Destroy(gameObject);
        }

        public override void OnStart()
        {
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
                                this.transform.LookAt(player.transform);
                            }
                            break;

                        case EnemyType.ChaseAndExplode:
                            if (distance.magnitude > 2)
                            {
                                this.transform.Translate(Vector3.forward * 4f * Time.deltaTime);
                                this.transform.LookAt(player.transform);
                            }
                            else
                            {
                                Instantiate(explosion, this.transform.position, Quaternion.identity);
                                OnDead();
                            }
                            break;

                        case EnemyType.EscapeAndShoot:
                            if (distance.magnitude > 10)
                            {
                                isShoot = true;
                                this.transform.LookAt(player.transform);
                            }
                            else
                            {
                                isShoot = false;
                                this.transform.Translate(Vector3.back * 10f * Time.deltaTime);
                                this.transform.LookAt(player.transform);
                            }
                            break;
                    }
                }
            }
        }

        IEnumerator shoot()
        {
            yield return new WaitForSeconds (1);
            if (isShoot)
            {
                Rigidbody clone;
                clone = (Rigidbody)Instantiate(bullet, gun.transform.position, Quaternion.identity);
                clone.velocity = gun.TransformDirection(Vector3.forward * 1000 * Time.deltaTime);
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
