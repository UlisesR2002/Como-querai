using TMPro;
using UnityEngine;

public class CompanionScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float minDistance;
    [SerializeField] private float elevationHeight; // Nueva variable para la altura de elevación
    [SerializeField] private float speed;
    [SerializeField] private float rotationspeed;

    public TextMeshPro messageText;

    public string message;
    private bool showMessages = false;

    public AudioClip meowSound;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        messageText.text = message;
        audioSource.PlayOneShot(meowSound);
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;

            if (distance > minDistance)
            {
                // Calcula la posición relativa al jugador con elevación en Y
                Vector3 posicionRelativa = player.position - (direction.normalized * minDistance) + Vector3.up * elevationHeight;

                // Calcula la rotación hacia la posición relativa
                Quaternion rotacion = Quaternion.LookRotation(posicionRelativa - transform.position);

                // Aplica la rotación suave
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * rotationspeed);

                // Mueve al compañero hacia la posición relativa
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            // Restablece el mensaje si el compañero está cerca del jugador
            else
            {
                transform.LookAt(player.position);
                showMessages = false;
            }
        }
    }

    public void ChangeMessage(string message)
    {
        messageText.text = message;
        audioSource.PlayOneShot(meowSound);
    }
}