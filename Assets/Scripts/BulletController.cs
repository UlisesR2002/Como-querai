using Assets.Scripts;
using UnityEngine;


public class BulletController : MonoBehaviour
{
    public float damage = 1;
    public bool damagePlayer = true;
    public bool damageEnemy = true;
    public bool damageEntities = true;

    [Header("Audio")]
    public AudioClip hitSoundEnemy;
    public AudioClip hitSoundPlayer;
    public AudioClip hitSoundBox;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        Destroy(gameObject, 2.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (damagePlayer && collision.gameObject.TryGetComponent(out PlayerController p))
        {
            audioSource.PlayOneShot(hitSoundPlayer);
            p.TakeDamage(damage);
            p.isHurt();
            Destroy(gameObject);
            return;
        }
        else if (damagePlayer == false && collision.gameObject.TryGetComponent(out PlayerController _))
        {
            return;    
        }

        if(damageEnemy && collision.gameObject.TryGetComponent(out EnemyController e))
        {
            audioSource.PlayOneShot(hitSoundEnemy);
            e.TakeDamage(damage);
            e.isHurt();
            Destroy(gameObject);
            return;
        }

        if (damageEntities && collision.gameObject.TryGetComponent(out Entity en))
        {
            audioSource.PlayOneShot(hitSoundBox);
            en.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }
}
