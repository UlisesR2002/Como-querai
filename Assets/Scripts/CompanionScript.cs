using TMPro;
using UnityEngine;

public class CompanionScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float minDistance;
    [SerializeField] private float elevationHeight; // Nueva variable para la altura de elevaci�n
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
                // Calcula la posici�n relativa al jugador con elevaci�n en Y
                Vector3 posicionRelativa = player.position - (direction.normalized * minDistance) + Vector3.up * elevationHeight;

                // Calcula la rotaci�n hacia la posici�n relativa
                Quaternion rotacion = Quaternion.LookRotation(posicionRelativa - transform.position);

                // Aplica la rotaci�n suave
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * rotationspeed);

                // Mueve al compa�ero hacia la posici�n relativa
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            // Restablece el mensaje si el compa�ero est� cerca del jugador
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