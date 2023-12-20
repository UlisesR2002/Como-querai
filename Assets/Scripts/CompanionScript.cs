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

    private bool mensajeMostrado = false;

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

                // Si el mensaje a�n no se ha mostrado, mu�stralo
                if (!mensajeMostrado)
                {
                    if (messageText != null)
                    {
                        messageText.text = "Here is a clue for you";
                        mensajeMostrado = true;
                    }
                }
            }
            // Restablece el mensaje si el compa�ero est� cerca del jugador
            else
            {
                mensajeMostrado = false;
            }
        }
    }
}